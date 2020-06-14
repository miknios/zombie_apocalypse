using Unity.Entities;

namespace ECS_Logic.Common.Health.Components
{
	public struct DamageToApplyBufferElement : IBufferElementData
	{
		public int Value;
	}
}