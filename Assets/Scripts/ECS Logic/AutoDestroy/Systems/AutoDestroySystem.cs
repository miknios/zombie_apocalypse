using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;

namespace ECS_Logic.AutoDestruction.Systems
{
	public class AutoDestroySystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;
		
		protected override void OnCreate()
		{
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}
		
		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
			
			Entities
				.WithAll<Timeout, AutoDestroyTimer>()
				.ForEach((Entity entity, int entityInQueryIndex) =>
				{
					commandBuffer.DestroyEntity(entityInQueryIndex, entity);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}