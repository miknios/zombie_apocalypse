using DefaultNamespace;
using DefaultNamespace.ECS_Logic.Common.Components;
using DefaultNamespace.ECS_Logic.Timer.Components;
using DefaultNamespace.ECS_Logic.Timer.Components.TimerTypes;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS_Logic.Systems
{
	[UpdateBefore(typeof(TimerAutoRestartSystem))]
	public class AroundCenterCyclicSpawnerSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			base.OnCreate();

			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
			
			Entities
				.WithAll<TimeOutComponent, AroundCenterCyclicSpawnerTimerComponent>()
				.ForEach((Entity entity, int nativeThreadIndex, int entityInQueryIndex, in TimerComponent timerComponent) =>
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