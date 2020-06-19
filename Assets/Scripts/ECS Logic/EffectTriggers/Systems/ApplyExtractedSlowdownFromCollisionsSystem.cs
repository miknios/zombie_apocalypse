using ECS_Configuration;
using ECS_Logic.Common.Components;
using ECS_Logic.EffectTriggers.Components;
using ECS_Logic.Health.Systems;
using ECS_Logic.Move.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Timers.Components.TimerTypes;
using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Systems
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	[UpdateBefore(typeof(DamageApplySystem))]
	public class ApplyExtractedSlowdownFromCollisionsSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;
		private EntityManager entityManager;

		protected override void OnCreate()
		{
			var world = World.DefaultGameObjectInjectionWorld;

			commandBufferSystem = world.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
			entityManager = world.EntityManager;
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer();

			// Iterate through slowdown trigger apply data.
			// Add slowdown with timer to target or refresh timer.
			// If there is velocity multiplier already applied on target entity and its source is not slowdown -> override it.
			Entities
				.WithoutBurst()
				.WithSharedComponentFilter(new EffectTriggerApplyEffectType {EffectType = EffectType.Slowdown})
				.ForEach((Entity entity, in EffectTriggerApplyData effectTriggerApplyData) =>
				{
					commandBuffer.DestroyEntity(entity);

					Entity targetEntity = effectTriggerApplyData.Target;
					if (!entityManager.Exists(targetEntity))
					{
						commandBuffer.DestroyEntity(entity);
						return;
					}

					float slowdownValue = effectTriggerApplyData.Value;
					if (HasComponent<VelocityMultiplier>(targetEntity))
					{
						var currentSlowdownSource = GetComponent<VelocityMultiplier>(targetEntity).Source;
						if (HasComponent<SlowdownDurationTimer>(currentSlowdownSource))
						{
							RefreshTimer(ref commandBuffer, currentSlowdownSource);
							return;
						}

						UpdateSlowdownAndChangeSourceToNewTimer(ref commandBuffer, currentSlowdownSource,
							targetEntity, slowdownValue);
						return;
					}

					AddSlowdownWithTimer(ref commandBuffer, targetEntity, slowdownValue);
				})
				.Run();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}

		private void RefreshTimer(ref EntityCommandBuffer commandBuffer, Entity currentSlowdownSource)
		{
			var refreshedTimer = GetComponent<Timer>(currentSlowdownSource);
			refreshedTimer.CurrentTime = refreshedTimer.InitialTime;
			commandBuffer.SetComponent(currentSlowdownSource, refreshedTimer);
		}

		private void UpdateSlowdownAndChangeSourceToNewTimer(ref EntityCommandBuffer commandBuffer,
			Entity currentSlowdownSource, Entity targetEntity, float slowdownValue)
		{
			commandBuffer.DestroyEntity(currentSlowdownSource);
			var velocityMultiplier =
				CreateTimerAndGetVelocityMultiplier(ref commandBuffer, targetEntity, slowdownValue);
			entityManager.SetComponentData(targetEntity, velocityMultiplier);
		}

		private static void AddSlowdownWithTimer(ref EntityCommandBuffer commandBuffer, Entity targetEntity,
			float slowdownValue)
		{
			var velocityMultiplier =
				CreateTimerAndGetVelocityMultiplier(ref commandBuffer, targetEntity, slowdownValue);
			commandBuffer.AddComponent(targetEntity, velocityMultiplier);
		}

		private static VelocityMultiplier CreateTimerAndGetVelocityMultiplier(
			ref EntityCommandBuffer commandBuffer, Entity target, float slowdownValue)
		{
			Entity timerEntity = commandBuffer.CreateEntity();
			commandBuffer.AddComponent<SlowdownDurationTimer>(timerEntity);
			commandBuffer.AddComponent(timerEntity, new Timer
			{
				AutoRestart = false,
				CurrentTime = 5,
				InitialTime = 5,
				Owner = target
			});
			commandBuffer.AddComponent<Enabled>(timerEntity);

			var multiplier = new VelocityMultiplier
			{
				Value = slowdownValue,
				Source = timerEntity
			};

			return multiplier;
		}
	}
}