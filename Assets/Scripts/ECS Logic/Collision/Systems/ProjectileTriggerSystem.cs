using ECS_Logic;
using ECS_Logic.Common.Collision.Components;
using ECS_Logic.Common.Health.Components;
using ECS_Logic.TagComponents;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(CollisionDependentSystemGroup))]
	public class ProjectileTriggerSystem : SystemBase
	{
		private EntityCommandBufferSystem appendCommandBufferSystem;
		private EntityCommandBufferSystem destroyCommandBufferSystem;

		protected override void OnCreate()
		{
			appendCommandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<CollisionDataAppendEntityCommandBufferSystem>();
			destroyCommandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
			GetEntityQuery(ComponentType.ReadWrite<HitboxArea>());
		}

		protected override void OnUpdate()
		{
			var appendCommandBuffer = appendCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
			var destroyCommandBuffer = destroyCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

			Entities
				.ForEach((int entityInQueryIndex, Entity projectileEntity, in Projectile projectile,
					in DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer) =>
				{
					if (collisionBuffer.Length == 0)
						return;

					destroyCommandBuffer.DestroyEntity(entityInQueryIndex, projectileEntity);
					Entity hitEntity = collisionBuffer[0].HitboxEntity;
					
					appendCommandBuffer.AppendToBuffer(entityInQueryIndex, hitEntity, 
						new DamageToApplyBufferElement {Value = projectile.Damage});
				})
				.ScheduleParallel();
			
			appendCommandBufferSystem.AddJobHandleForProducer(Dependency);
			destroyCommandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}