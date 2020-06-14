using Unity.Entities;

namespace ECS_Logic.Common.Health.Components
{
	[GenerateAuthoringComponent]
	public struct HealthPoints : IComponentData
	{
		public float Value;
	}
}