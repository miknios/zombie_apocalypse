using Unity.Entities;

namespace ECS_Logic.Timers.Components
{
	public struct Timer : IComponentData
	{
		public float InitialTime;
		public float CurrentTime;
		public bool AutoRestart;
		public Entity Owner;
	}
}