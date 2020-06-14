using Unity.Entities;

namespace ECS_Logic.Common.Health.Components
{
	public struct HealthPoints : IComponentData
	{
		public int Value;
	}
}