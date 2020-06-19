using Unity.Entities;

namespace ECS_Logic.Collision.Components
{
	public struct TriggerArea : IComponentData
	{
		public float Radius;
		public CollisionLayer CollisionLayer;
	}
}