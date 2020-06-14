using ECS_Logic.Common.Health.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS_Logic.Common.Collision.Components
{
	public class HealthAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private int healthPoints;
		
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new HealthPoints {Value = healthPoints});
			dstManager.AddBuffer<DamageToApplyBufferElement>(entity);
		}
	}
}