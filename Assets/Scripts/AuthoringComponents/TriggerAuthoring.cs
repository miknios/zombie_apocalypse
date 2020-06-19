using ECS_Logic.Collision;
using ECS_Logic.Collision.Components;
using ECS_Logic.EffectTriggers.Components;
using Unity.Entities;
using UnityEngine;

namespace AuthoringComponents
{
	public class TriggerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private float radius = 0.5f;
		[SerializeField] private CollisionLayer collisionLayer = CollisionLayer.Enemy;
		[SerializeField] private EffectType effectType = EffectType.Damage;
		[SerializeField] private float value = 70;
		[SerializeField] private bool isPenetrative = false;

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new TriggerArea
			{
				CollisionLayer = collisionLayer,
				Radius = radius
			});

			dstManager.AddComponentData(entity, new EffectTrigger
			{
				Value = value,
				EffectType = effectType
			});

			dstManager.AddBuffer<TriggerCollisionBufferElement>(entity);
			dstManager.AddBuffer<AlreadyCollidedBufferElement>(entity);

			if (isPenetrative)
				dstManager.AddComponent<Penetratrive>(entity);
		}
	}
}