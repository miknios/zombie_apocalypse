using DefaultNamespace.ECS_Logic.Timer.Components;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
	public class TimerAutoRestartSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			base.OnCreate();

			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

			Entities
				.WithAll<Timeout>()
				.ForEach((Entity entity, int entityInQueryIndex, ref Timer timerComponent) =>
				{
					if(!timerComponent.AutoRestart)
						return;

					timerComponent.CurrentTime = timerComponent.InitialTime;
					commandBuffer.RemoveComponent<Timeout>(entityInQueryIndex, entity);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}