using Unity.Entities;

namespace DefaultNamespace
{
	public struct AroundCenterCyclicSpawnerComponent : IComponentData
	{
		public float SpawnDistance;
		public int SpawnCount;
		public Entity EntityToSpawn;
	}
}