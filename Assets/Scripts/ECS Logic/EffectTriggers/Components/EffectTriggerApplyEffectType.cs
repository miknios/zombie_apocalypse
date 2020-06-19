using System;
using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Components
{
	public struct EffectTriggerApplyEffectType : ISharedComponentData, IEquatable<object>
	{
		public EffectType EffectType;

		private bool Equals(EffectTriggerApplyEffectType other)
		{
			return EffectType == other.EffectType;
		}

		public override bool Equals(object obj)
		{
			return obj is EffectTriggerApplyEffectType other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (int) EffectType;
		}
	}
}