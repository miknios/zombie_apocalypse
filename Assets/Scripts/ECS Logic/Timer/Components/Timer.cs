using Unity.Entities;

namespace DefaultNamespace.ECS_Logic.Timer.Components
{
	public struct Timer : IComponentData
	{
		public float InitialTime;
		public float CurrentTime;
		public bool AutoRestart;
		public Entity Owner;
	}
}