using DefaultNamespace.ECS_Logic.Common.Components;
using DefaultNamespace.ECS_Logic.Timer.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public class TimerStepSystem : SystemBase
	{
		protected override void OnUpdate()
		{
			float deltaTime = Time.DeltaTime;

			Entities
				.ForEach((int entityInQueryIndex, ref TimerComponent timerComponent, 
					in EnabledComponent enabledComponent) =>
				{
					float newCurrentTime = timerComponent.CurrentTime - deltaTime;
					newCurrentTime = math.max(0, newCurrentTime);
					timerComponent.CurrentTime = newCurrentTime;
				})
				.ScheduleParallel();
		}
	}
}