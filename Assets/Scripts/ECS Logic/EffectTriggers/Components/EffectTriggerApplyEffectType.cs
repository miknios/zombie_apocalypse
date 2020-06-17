using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Components
{
	public struct EffectTriggerApplyEffectType : ISharedComponentData
	{
		public EffectType EffectType;
	}
}