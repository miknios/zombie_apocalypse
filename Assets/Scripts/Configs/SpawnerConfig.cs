using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfig", menuName = "ZombieApocalypse/Configs/SpawnerConfig")]
public class SpawnerConfig : ScriptableObject
{
	[SerializeField] private GameObject enemyPrefab = null;
	[SerializeField] private float distanceFromPlayer = 25;
	[SerializeField] private int spawnCount = 6;
	[SerializeField] private float spawnInterval = 0.1f;

	public GameObject EnemyPrefab => enemyPrefab;
	public float DistanceFromPlayer => distanceFromPlayer;
	public int SpawnCount => spawnCount;
	public float SpawnInterval => spawnInterval;
}