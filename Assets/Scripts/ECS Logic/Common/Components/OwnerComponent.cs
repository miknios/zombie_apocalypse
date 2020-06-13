using Unity.Entities;

namespace DefaultNamespace.ECS_Logic.Common.Components
{
	public class OwnerComponent : IComponentData
	{
		public Entity Value;
	}
}