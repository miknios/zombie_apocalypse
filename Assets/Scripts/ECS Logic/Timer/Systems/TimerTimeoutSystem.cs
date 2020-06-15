using DefaultNamespace.ECS_Logic.Timer.Components;
using ECS_Logic;
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
				.WithChangeFilter<TimerComponent>()
				.ForEach((Entity entity, int entityInQueryIndex, in TimerComponent timerComponent) =>
				{
					if(timerComponent.CurrentTime == 0)
						commandBuffer.AddComponent<TimeOutComponent>(entityInQueryIndex, entity);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}