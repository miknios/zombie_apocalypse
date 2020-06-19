using ECS_Configuration;
using ECS_Logic.Common.Components;
using ECS_Logic.Timers.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Timers.Systems
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	public class TimerStepSystem : SystemBase
	{
		protected override void OnUpdate()
		{
			float deltaTime = Time.DeltaTime;

			Entities
				.ForEach((int entityInQueryIndex, ref Timer timerComponent,
					in Enabled enabledComponent) =>
				{
					float newCurrentTime = timerComponent.CurrentTime - deltaTime;
					newCurrentTime = math.max(0, newCurrentTime);
					timerComponent.CurrentTime = newCurrentTime;
				})
				.ScheduleParallel();
		}
	}
}