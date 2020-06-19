using System.Collections.Generic;
using Configs;
using ECS_Logic.Common.Components;
using ECS_Logic.Spawner.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace AuthoringComponents
{
	[RequiresEntityConversion]
	[AddComponentMenu("AuthoringComponents/AroundCenterCyclicSpawnser")]
	public class AroundCenterCyclicSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity,
		IDeclareReferencedPrefabs
	{
		[Inject] private SpawnerConfig spawnerConfig = null;

		public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
		{
			referencedPrefabs.Add(spawnerConfig.EnemyPrefab);
		}

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			SetupSpawnerEntity(entity, dstManager, conversionSystem);
			SetupSpawnerTimerEntity(entity, dstManager);
		}

		private void SetupSpawnerEntity(Entity entity, EntityManager dstManager,
			GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new AroundCenterCyclicSpawner
			{
				EntityToSpawn = conversionSystem.GetPrimaryEntity(spawnerConfig.EnemyPrefab),
				SpawnCount = spawnerConfig.SpawnCount,
				SpawnDistance = spawnerConfig.DistanceFromPlayer
			});

			Random random = new Random();
			dstManager.AddComponentData(entity,
				new RandomGeneratorComponent {Value = new Unity.Mathematics.Random((uint) random.Next())});
		}

		private void SetupSpawnerTimerEntity(Entity entity, EntityManager dstManager)
		{
			var timerEntity = dstManager.CreateEntity();
			dstManager.AddComponentData(timerEntity, new Timer
			{
				AutoRestart = true,
				InitialTime = spawnerConfig.SpawnInterval,
				CurrentTime = spawnerConfig.SpawnInterval,
				Owner = entity
			});
			dstManager.AddComponent<AroundCenterCyclicSpawnerTimerComponent>(timerEntity);
			dstManager.AddComponent<Enabled>(timerEntity);
		}
	}
}