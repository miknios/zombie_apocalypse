using Configuration;
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
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
			entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer();

			Entities
				.WithoutBurst()
				.WithSharedComponentFilter(new EffectTriggerApplyEffectType {EffectType = EffectType.Slowdown})
				.ForEach((Entity entity, in EffectTriggerApplyData effectTriggerApplyData) =>
				{
					commandBuffer.DestroyEntity(entity);
					Entity targetEntity = effectTriggerApplyData.Target;
					float value = effectTriggerApplyData.Value;
					if (!entityManager.Exists(targetEntity))
					{
						commandBuffer.DestroyEntity(entity);
						return;
					}


					if (HasComponent<VelocityMultiplier>(targetEntity))
					{
						var slowedBy = GetComponent<VelocityMultiplier>(targetEntity).Source;
						if (HasComponent<SlowdownDurationTimer>(slowedBy))
						{
							var refreshedTimer = GetComponent<Timer>(slowedBy);
							refreshedTimer.CurrentTime = refreshedTimer.InitialTime;
							commandBuffer.SetComponent(slowedBy, refreshedTimer);
							return;
						}

						commandBuffer.DestroyEntity(slowedBy);

						entityManager.SetComponentData(targetEntity,
							CreateTimerAndGetVelocityMultiplier(ref commandBuffer, targetEntity, value));
						return;
					}

					commandBuffer.AddComponent(targetEntity,
						CreateTimerAndGetVelocityMultiplier(ref commandBuffer, targetEntity, value));
				})
				.Run();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
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