﻿using DefaultNamespace.ECS_Logic.Timer.Components;
using Unity.Entities;

namespace DefaultNamespace
{
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