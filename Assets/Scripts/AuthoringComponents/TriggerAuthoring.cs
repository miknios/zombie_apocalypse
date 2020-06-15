using Unity.Entities;
using UnityEngine;

namespace ECS_Logic.Common.Collision.Components
{
	public class TriggerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private float radius = 0.5f;
		[SerializeField] private CollisionLayer collisionLayer = CollisionLayer.Enemy;
		
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new TriggerArea
			{
				CollisionLayer = collisionLayer,
				Radius = radius
			});

			dstManager.AddBuffer<TriggerCollisionBufferElement>(entity);
		}
	}
}