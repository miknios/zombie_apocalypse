using Unity.Entities;

namespace ECS_Logic.Collision.Components
{
	public struct AlreadyCollidedBufferElement : IBufferElementData
	{
		public Entity HitboxEntity;

		public static implicit operator Entity(AlreadyCollidedBufferElement e)
		{
			return e.HitboxEntity;
		}

		public static implicit operator AlreadyCollidedBufferElement(Entity e)
		{
			return new AlreadyCollidedBufferElement() {HitboxEntity = e};
		}
	}
}