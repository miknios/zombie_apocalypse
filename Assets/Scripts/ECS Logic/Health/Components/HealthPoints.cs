using Unity.Entities;

namespace ECS_Logic.Health.Components
{
	public struct HealthPoints : IComponentData
	{
		public int Value;
	}
}