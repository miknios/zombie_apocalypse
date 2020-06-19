using Unity.Entities;

namespace ECS_Logic.Collision.Components
{
	public struct TriggerCollisionBufferElement : IBufferElementData
	{
		public Entity HitboxEntity;

		public static implicit operator Entity(TriggerCollisionBufferElement e)
		{
			return e.HitboxEntity;
		}

		public static implicit operator TriggerCollisionBufferElement(Entity e)
		{
			return new TriggerCollisionBufferElement() {HitboxEntity = e};
		}
	}
}