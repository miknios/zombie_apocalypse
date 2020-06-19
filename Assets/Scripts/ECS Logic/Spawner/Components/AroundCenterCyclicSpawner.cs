using Unity.Entities;

namespace ECS_Logic.Spawner.Components
{
	public struct AroundCenterCyclicSpawner : IComponentData
	{
		public float SpawnDistance;
		public int SpawnCount;
		public Entity EntityToSpawn;
	}
}