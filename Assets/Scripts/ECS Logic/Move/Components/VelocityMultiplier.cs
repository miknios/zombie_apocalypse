using Unity.Entities;

namespace ECS_Logic.Move.Components
{
	public struct VelocityMultiplier : IComponentData
	{
		public float Value;
		public Entity Source;
	}
}