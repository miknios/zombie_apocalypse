using ECS_Logic;
using ECS_Logic.Common.Collision.Components;
using ECS_Logic.Common.Health.Components;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateBefore(typeof(CollisionDependentSystemGroup))]
	public class EnemyTriggerSystem : SystemBase
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
				.WithAll<EnemyTag>()
				.ForEach((int entityInQueryIndex, Entity enemyEntity,
					in DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer) =>
				{
					if (collisionBuffer.Length == 0)
						return;

					commandBuffer.DestroyEntity(entityInQueryIndex, enemyEntity);
					Entity hitEntity = collisionBuffer[0].HitboxEntity;
					
					commandBuffer.AppendToBuffer(entityInQueryIndex, hitEntity, new DamageToApplyBufferElement{Value = 50});
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}