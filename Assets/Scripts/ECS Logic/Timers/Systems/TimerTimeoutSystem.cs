using ECS_Logic;
using ECS_Logic.Timers.Components;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	[UpdateAfter(typeof(TimerStepSystem))]
	public class TimerTimeoutSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			base.OnCreate();
			
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
			
			Entities
				.WithChangeFilter<Timer>()
				.ForEach((Entity entity, int entityInQueryIndex, in Timer timerComponent) =>
				{
					if(timerComponent.CurrentTime == 0)
						commandBuffer.AddComponent<Timeout>(entityInQueryIndex, entity);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}