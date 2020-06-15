using DefaultNamespace.ECS_Logic.Common.Components;
using DefaultNamespace.ECS_Logic.Timer.Components;
using ECS_Logic.AutoDestruction.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS_Logic.Common.Collision.Components
{
	public class AutoDestroyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private float destroyAfterSeconds;
		
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new AutoDestroyTimer());
			dstManager.AddComponentData(entity, new Timer
			{
				AutoRestart = false,
				CurrentTime = destroyAfterSeconds,
				InitialTime = destroyAfterSeconds,
				Owner = entity
			});
			dstManager.AddComponentData(entity, new Enabled());
		}
	}
}