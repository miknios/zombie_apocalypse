using Unity.Entities;

namespace ECS_Logic.Health.Components
{
	public struct DamageToApplyBufferElement : IBufferElementData
	{
		public int Value;

		public static implicit operator int(DamageToApplyBufferElement e)
		{
			return e.Value;
		}

		public static implicit operator DamageToApplyBufferElement(int e)
		{
			return new DamageToApplyBufferElement() {Value = e};
		}
	}
}