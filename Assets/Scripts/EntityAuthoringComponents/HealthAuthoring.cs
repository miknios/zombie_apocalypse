using ECS_Logic.Health.Components;
using Unity.Entities;
using UnityEngine;

namespace EntityAuthoringComponents
{
	[AddComponentMenu("AuthoringComponents/Health")]
	public class HealthAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private int healthPoints = 100;

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new HealthPoints {Value = healthPoints});
			dstManager.AddBuffer<DamageToApplyBufferElement>(entity);
		}
	}
}