using ECS_Logic;
using ECS_Logic.Common.Collision.Components;
using ECS_Logic.Common.Health.Components;
using ECS_Logic.TagComponents;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateBefore(typeof(CollisionDependentSystemGroup))]
	public class ProjectileTriggerSystem : SystemBase
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
				.ForEach((int entityInQueryIndex, Entity projectileEntity, in Projectile projectile,
					in DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer) =>
				{
					if (collisionBuffer.Length == 0)
						return;

					commandBuffer.DestroyEntity(entityInQueryIndex, projectileEntity);
					Entity hitEntity = collisionBuffer[0].HitboxEntity;
					
					commandBuffer.AppendToBuffer(entityInQueryIndex, hitEntity, 
						new DamageToApplyBufferElement {Value = projectile.Damage});
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}