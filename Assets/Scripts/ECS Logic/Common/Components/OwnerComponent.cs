using Unity.Entities;

namespace ECS_Logic.Common.Components
{
	public class OwnerComponent : IComponentData
	{
		public Entity Value;
	}
}