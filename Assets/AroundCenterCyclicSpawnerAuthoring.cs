using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.ECS_Logic.Common.Components;
using DefaultNamespace.ECS_Logic.Timer.Components;
using DefaultNamespace.ECS_Logic.Timer.Components.TimerTypes;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
[AddComponentMenu("AuthoringComponents/AroundCenterCyclicSpawner")]
public class AroundCenterCyclicSpawnerAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private float spawnDistance = 25;
	[SerializeField] private int spawnCount = 3;
	[SerializeField] private float interval = 0.3f;

	public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
	{
		referencedPrefabs.Add(enemyPrefab);
	}
	public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
	{
		dstManager.AddComponentData(entity, new AroundCenterCyclicSpawnerComponent
		{
			EntityToSpawn = conversionSystem.GetPrimaryEntity(enemyPrefab),
			SpawnCount = spawnCount,
			SpawnDistance = spawnDistance
		});

		var timerEntity = dstManager.CreateEntity();
		dstManager.AddComponentData(timerEntity, new TimerComponent
		{
			AutoRestart = true,
			InitialTime = interval,
			CurrentTime =  interval,
			Owner = entity
		});
		dstManager.AddComponent<AroundCenterCyclicSpawnerTimerComponent>(timerEntity);
		dstManager.AddComponent<EnabledComponent>(timerEntity);
	}
}