using Configs;
using ECS_Logic.Health.Components;
using ECS_Logic.TagComponents;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace EntityAuthoringComponents
{
	[AddComponentMenu("AuthoringComponents/Player")]
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