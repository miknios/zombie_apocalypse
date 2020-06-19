using ECS_Logic.Common.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;
using UnityEngine;

namespace AuthoringComponents
{
	public class AutoDestroyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
	{
		[SerializeField] private float destroyAfterSeconds = 0.625f;

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