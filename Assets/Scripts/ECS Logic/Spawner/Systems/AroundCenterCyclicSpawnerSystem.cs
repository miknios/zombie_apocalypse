using ECS_Configuration;
using ECS_Logic.Common.Components;
using ECS_Logic.Spawner.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace ECS_Logic.Spawner.Systems
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
			ApplyRandomPositionOnCircle(ref commandBuffer, ref spawner, ref randomGenerator, entityInQueryIndex,
				spawnedEntity);
			ApplyRandomColor(ref commandBuffer, ref randomGenerator, entityInQueryIndex, spawnedEntity);
		}

		private static void ApplyRandomPositionOnCircle(ref EntityCommandBuffer.Concurrent commandBuffer,
			ref AroundCenterCyclicSpawner spawner, ref Random randomGenerator, int entityInQueryIndex,
			Entity spawnedEntity)
		{
			float3 randomPosition = GetRandomPositionOnCircle(ref randomGenerator, spawner.SpawnDistance);
			Translation translation = new Translation {Value = randomPosition};
			commandBuffer.SetComponent(entityInQueryIndex, spawnedEntity, translation);
		}

		private static float3 GetRandomPositionOnCircle(ref Random randomGenerator, float radius)
		{
			float randomAngle = randomGenerator.NextFloat() * math.PI * 2;
			return new float3
			{
				x = math.cos(randomAngle) * radius,
				y = 0,
				z = math.sin(randomAngle) * radius
			};
		}

		private static void ApplyRandomColor(ref EntityCommandBuffer.Concurrent commandBuffer,
			ref Random randomGenerator, int entityInQueryIndex, Entity spawnedEntity)
		{
			Color color = Color.HSVToRGB(randomGenerator.NextFloat(), 0.6f, 0.6f);
			float4 colorValue = new float4(color.r, color.g, color.b, color.a);
			MaterialColor materialColor = new MaterialColor {Value = colorValue};
			commandBuffer.AddComponent(entityInQueryIndex, spawnedEntity, materialColor);
		}
	}
}