using DefaultNamespace;
using DefaultNamespace.ECS_Logic.Common.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ECS_Logic.Systems
{
	[UpdateInGroup(typeof(ContinousWorkProducerSystemGroup))]
	public class AroundCenterCyclicSpawnerSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
         		{
         			commandBufferSystem = World.DefaultGameObjectInjectionWorld
         				.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
         		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
			
			Entities
				.WithAll<Timeout, AroundCenterCyclicSpawnerTimerComponent>()
				.ForEach((Entity entity, int entityInQueryIndex, in Timer timerComponent) =>
				{
					Entity spawnerEntity = timerComponent.Owner;
					var spawner = GetComponent<AroundCenterCyclicSpawner>(spawnerEntity);
					var randomGeneratorComponent = GetComponent<RandomGeneratorComponent>(spawnerEntity);
					var randomGenerator = randomGeneratorComponent.Value;
					for (int i = 0; i < spawner.SpawnCount; i++)
					{
						SpawnEntity(ref commandBuffer, ref spawner, ref randomGenerator, entityInQueryIndex);
					}

					randomGeneratorComponent.Value = randomGenerator;
					commandBuffer.SetComponent(entityInQueryIndex, spawnerEntity, randomGeneratorComponent);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}

		private static void SpawnEntity(ref EntityCommandBuffer.Concurrent commandBuffer, 
			ref AroundCenterCyclicSpawner spawner, ref Random randomGenerator, int entityInQueryIndex)
		{
			var spawnedEntity = commandBuffer.Instantiate(entityInQueryIndex, spawner.EntityToSpawn);
			float randomAngle = randomGenerator.NextFloat() * math.PI * 2;

			float3 newPosition = GetRandomPosition(randomAngle, ref spawner);

			Translation translation = new Translation {Value = newPosition};
			commandBuffer.SetComponent(entityInQueryIndex, spawnedEntity, translation);

			Color color = Color.HSVToRGB(randomGenerator.NextFloat(), 0.6f, 0.6f);
			float4 colorValue = new float4(color.r, color.g, color.b, color.a);
			MaterialColor materialColor = new MaterialColor{Value = colorValue};
			commandBuffer.AddComponent(entityInQueryIndex, spawnedEntity, materialColor);
		}

		private static float3 GetRandomPosition(float randomAngle, ref AroundCenterCyclicSpawner spawner)
		{
			return new float3
			{
				x = math.cos(randomAngle) * spawner.SpawnDistance,
				y = 0,
				z = math.sin(randomAngle) * spawner.SpawnDistance
			};
		}
	}
}