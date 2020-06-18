﻿using DefaultNamespace;
using ECS_Logic.Common.Health.Components;
using ECS_Logic.DataTrack.Components;
using Signals;
using Unity.Collections;
using Unity.Entities;
using Zenject;

namespace ECS_Logic.DataTrack.Systems
{
	[UpdateInGroup(typeof(LateSimulationSystemGroup))]
	public class KilledEnemiesCountSignalSystem : SystemBase
	{
		private NativeArray<int> sumArray;
		private EntityQuery killedEnemiesCountQuery;
		private EntityCommandBufferSystem commandBufferSystem;
		[Inject] private SignalBus signalBus = null;

		protected override void OnCreate()
		{
			var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
			entityManager.CreateEntity(typeof(KilledEnemiesCount));
			
			sumArray = new NativeArray<int>(1, Allocator.Persistent);
			killedEnemiesCountQuery = GetEntityQuery(ComponentType.ReadWrite<KilledEnemiesCount>());
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var array = sumArray;
			array[0] = 0;

			var jobHandle = Entities
				.WithAll<EnemyTag, HealthDepleted>()
				.ForEach((Entity entity) =>
				{
					array[0]++;
				})
				.Schedule(Dependency);

			var currentKilledEnemiesEntity = killedEnemiesCountQuery.GetSingletonEntity();
			var commandBuffer = commandBufferSystem.CreateCommandBuffer();
			var finalJobHandle = Job
				.WithCode(() =>
				{
					int currentKilledEnemiesCount = GetComponent<KilledEnemiesCount>(currentKilledEnemiesEntity).Value;
					int newCurrentKilledEnemiesCount = currentKilledEnemiesCount + array[0];
					commandBuffer.SetComponent(currentKilledEnemiesEntity, new KilledEnemiesCount
					{
						Value = newCurrentKilledEnemiesCount
					});
				})
				.Schedule(jobHandle);

			Dependency = finalJobHandle;
			commandBufferSystem.AddJobHandleForProducer(Dependency);
			
			Entities
				.WithoutBurst()
				.WithChangeFilter<KilledEnemiesCount>()
				.ForEach((in KilledEnemiesCount killedEnemiesCount) =>
				{
					signalBus.Fire(new KilledEnemySignal{EnemiesKilledCount = killedEnemiesCount.Value});
				})
				.Run();
		}

		protected override void OnDestroy()
		{
			sumArray.Dispose();
		}
	}
}