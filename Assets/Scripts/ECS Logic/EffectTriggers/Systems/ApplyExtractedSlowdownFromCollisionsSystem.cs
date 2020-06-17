using ECS_Logic.Common.Health.Systems;
using ECS_Logic.Common.Move.Components;
using ECS_Logic.EffectTriggers.Components;
using Unity.Entities;

namespace ECS_Logic
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	[UpdateBefore(typeof(DamageApplySystem))]
	public class ApplyExtractedSlowdownFromCollisionsSystem : SystemBase
	{
		private EntityCommandBufferSystem destroyCommandBufferSystem;
		private EntityManager entityManager;

		protected override void OnCreate()
		{
			destroyCommandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
			entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		}

		protected override void OnUpdate()
		{
			var destroyCommandBuffer = destroyCommandBufferSystem.CreateCommandBuffer();

			Entities
				.WithoutBurst()
				.WithSharedComponentFilter(new EffectTriggerApplyEffectType {EffectType = EffectType.Slowdown})
				.ForEach((int entityInQueryIndex, Entity entity, in EffectTriggerApplyData effectTriggerApplyData) =>
				{
					if (entityManager.Exists(effectTriggerApplyData.Target))
					{
						entityManager.SetComponentData(effectTriggerApplyData.Target,
							new VelocityMultiplier {Value = effectTriggerApplyData.Value});
					}

					destroyCommandBuffer.DestroyEntity(entity);
				})
				.Run();

			destroyCommandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}