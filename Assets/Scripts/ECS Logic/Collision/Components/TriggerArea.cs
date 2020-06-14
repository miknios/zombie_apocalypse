using Unity.Entities;

namespace ECS_Logic.Common.Collision.Components
{
	public struct TriggerArea : IComponentData
	{
		public float Radius;
		public CollisionLayer CollisionLayer;
	}
}