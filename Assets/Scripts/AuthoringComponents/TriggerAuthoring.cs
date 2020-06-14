using Unity.Entities;
using UnityEngine;

namespace ECS_Logic.Common.Collision.Components
{
	public class TriggerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private float radius;
		[SerializeField] private CollisionLayer collisionLayer;
		
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