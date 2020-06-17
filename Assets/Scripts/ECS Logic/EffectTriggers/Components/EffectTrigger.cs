using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Components
{
	public struct EffectTrigger : IComponentData
	{
		public EffectType EffectType;
		public float Value;
	}
}