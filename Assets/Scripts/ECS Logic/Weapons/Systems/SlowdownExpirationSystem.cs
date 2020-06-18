using ECS_Logic.Common.Move.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;

namespace ECS_Logic.Weapons.Systems
{
	public class SlowdownExpirationSystem : SystemBase
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
				.WithAll<Timeout, SlowdownDurationTimer>()
				.ForEach((Entity entity, int entityInQueryIndex, in Timer timer) =>
				{
					commandBuffer.DestroyEntity(entityInQueryIndex, entity);
					
					Entity slowedEntity = timer.Owner;
					if(!HasComponent<VelocityMultiplier>(slowedEntity))
						return;
					
					var multiplierSource = GetComponent<VelocityMultiplier>(slowedEntity).Source;
					if(multiplierSource != entity)
						return;
					
					commandBuffer.RemoveComponent<VelocityMultiplier>(entityInQueryIndex, slowedEntity);
				})
				.ScheduleParallel();
		}
	}
}