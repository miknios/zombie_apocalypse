using DefaultNamespace.ECS_Logic.Timer.Components;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(LateSimulationSystemGroup))]
	public class TimerAutoRestartSystem : SystemBase
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
				.WithAll<TimeOutComponent>()
				.ForEach((Entity entity, int entityInQueryIndex, ref TimerComponent timerComponent) =>
				{
					if(!timerComponent.AutoRestart)
						return;

					timerComponent.CurrentTime = timerComponent.InitialTime;
					commandBuffer.RemoveComponent<TimeOutComponent>(entityInQueryIndex, entity);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}