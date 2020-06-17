using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Components
{
	public struct EffectTriggerApplyData : IComponentData
	{
		public Entity Target;
		public float Value;
	}
}