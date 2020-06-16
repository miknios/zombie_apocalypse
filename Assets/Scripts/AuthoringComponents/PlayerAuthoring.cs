using DefaultNamespace;
using ECS_Logic.Common.Health.Components;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace ECS_Logic.Common.Collision.Components
{
	public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[Inject] private PlayerConfig playerConfig = null;
		
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponent<PlayerTag>(entity);
			dstManager.AddComponentData(entity, new HealthPoints {Value = playerConfig.HealthPoints});
			dstManager.AddBuffer<DamageToApplyBufferElement>(entity);
		}
	}
}