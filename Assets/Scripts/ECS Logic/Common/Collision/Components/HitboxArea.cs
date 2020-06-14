using Unity.Entities;

namespace ECS_Logic.Common.Collision.Components
{
	[GenerateAuthoringComponent]
	public struct HitboxArea : IComponentData
	{
		public float Radius;
		public CollisionLayer CollisionLayer;
	}
}