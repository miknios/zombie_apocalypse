using Unity.Entities;

namespace DefaultNamespace
{
	public struct AroundCenterCyclicSpawner : IComponentData
	{
		public float SpawnDistance;
		public int SpawnCount;
		public Entity EntityToSpawn;
	}
}