﻿using ECS_Configuration;
using ECS_Logic.Collision.Components;
using ECS_Logic.EffectTriggers.Components;
using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Systems
{
	[UpdateInGroup(typeof(CollisionDependentSystemGroup))]
	public class ExtractEffectsFromCollisionsSystem : SystemBase
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

			// Iterate through all non-penetrative triggers.
			// Destroy trigger with collisions and create entity with extracted effect value for later apply.
			Entities
				.WithNone<Penetratrive>()
				.WithChangeFilter<TriggerCollisionBufferElement>()
				.WithoutBurst()
				.ForEach((int entityInQueryIndex, Entity entity, in EffectTrigger effectTrigger,
					in DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer) =>
				{
					if (collisionBuffer.Length == 0)
						return;
					
					commandBuffer.DestroyEntity(entityInQueryIndex, entity);

					var effectApplyData = new EffectTriggerApplyData
					{
						Value = effectTrigger.Value,
						Target = collisionBuffer[0].HitboxEntity
					};
					var effectApplyDataEntity = commandBuffer.CreateEntity(entityInQueryIndex);
					
					commandBuffer.AddComponent(entityInQueryIndex, effectApplyDataEntity, effectApplyData);
					commandBuffer.AddSharedComponent(entityInQueryIndex, effectApplyDataEntity,
						new EffectTriggerApplyEffectType {EffectType = effectTrigger.EffectType});
				})
				.ScheduleParallel();

			// Iterate through all penetrative triggers.
			// Create entities with extracted effect values for later apply and add hitbox entity reference to trigger already collided buffer.
			Entities
				.WithAll<Penetratrive>()
				.WithChangeFilter<TriggerCollisionBufferElement>()
				.WithoutBurst()
				.ForEach((int entityInQueryIndex, Entity entity,
					ref DynamicBuffer<AlreadyCollidedBufferElement> alreadyCollidedBuffer,
					in DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer, in EffectTrigger effectTrigger) =>
				{
					if (collisionBuffer.Length == 0)
						return;

					for (int i = 0; i < collisionBuffer.Length; i++)
					{
						var collisionEntity = collisionBuffer[i].HitboxEntity;
						bool alreadyCollided = false;
						for (int j = 0; j < alreadyCollidedBuffer.Length; j++)
						{
							var alreadyCollidedEntity = alreadyCollidedBuffer[j].HitboxEntity;
							if (collisionEntity == alreadyCollidedEntity)
								alreadyCollided = true;
						}

						if (alreadyCollided)
							continue;

						var effectApplyData = new EffectTriggerApplyData
						{
							Value = effectTrigger.Value,
							Target = collisionEntity
						};
						var effectApplyDataEntity = commandBuffer.CreateEntity(entityInQueryIndex);
						commandBuffer.AddComponent(entityInQueryIndex, effectApplyDataEntity, effectApplyData);
						commandBuffer.AddSharedComponent(entityInQueryIndex, effectApplyDataEntity,
							new EffectTriggerApplyEffectType {EffectType = effectTrigger.EffectType});
						alreadyCollidedBuffer.Add(collisionEntity);
					}
				})
				.ScheduleParallel();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}