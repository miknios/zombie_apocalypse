using ECS_Logic;
using ECS_Logic.Common.Collision.Components;
using ECS_Logic.Common.Health.Components;
using Unity.Entities;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(CollisionDependentSystemGroup))]
	public class EnemyTriggerSystem : SystemBase
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
				.WithAll<EnemyTag>()
				.ForEach((int entityInQueryIndex, Entity enemyEntity,
					in DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer) =>
				{
					if (collisionBuffer.Length == 0)
						return;

					destroyCommandBuffer.DestroyEntity(entityInQueryIndex, enemyEntity);
					Entity hitEntity = collisionBuffer[0].HitboxEntity;
					
					// TODO: change from hard coded value to some config cached and captured to job one
					appendCommandBuffer.AppendToBuffer(entityInQueryIndex, hitEntity, 
						new DamageToApplyBufferElement{Value = 50});
				})
				.ScheduleParallel();
			
			appendCommandBufferSystem.AddJobHandleForProducer(Dependency);
			destroyCommandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}