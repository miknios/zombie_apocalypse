using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.ECS_Logic.Common.Components;
using DefaultNamespace.ECS_Logic.Timer.Components;
using DefaultNamespace.ECS_Logic.Timer.Components.TimerTypes;
using Unity.Entities;
using UnityEngine;
using Random = System.Random;

[RequiresEntityConversion]
[AddComponentMenu("AuthoringComponents/AroundCenterCyclicSpawner")]
public class AroundCenterCyclicSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	[SerializeField] private GameObject enemyPrefab = null;
	[SerializeField] private float spawnDistance = 25;
	[SerializeField] private int spawnCount = 3;
	[SerializeField] private float interval = 0.3f;

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		referencedPrefabs.Add(enemyPrefab);
	}
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		SetupSpawnerEntity(entity, dstManager, conversionSystem);
		SetupSpawnerTimerEntity(entity, dstManager);
	}

	private void SetupSpawnerEntity(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponentData(entity, new AroundCenterCyclicSpawner
		{
			EntityToSpawn = conversionSystem.GetPrimaryEntity(enemyPrefab),
			SpawnCount = spawnCount,
			SpawnDistance = spawnDistance
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
			InitialTime = interval,
			CurrentTime = interval,
			Owner = entity
		});
		dstManager.AddComponent<AroundCenterCyclicSpawnerTimerComponent>(timerEntity);
		dstManager.AddComponent<Enabled>(timerEntity);
	}
}