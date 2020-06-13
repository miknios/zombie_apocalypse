using Unity.Entities;

namespace DefaultNamespace.ECS_Logic.Timer.Components
{
	public struct TimerComponent : IComponentData
	{
		public float InitialTime;
		public float CurrentTime;
		public bool AutoRestart;
		public Entity Owner;
	}
}