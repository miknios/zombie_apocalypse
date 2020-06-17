using ECS_Logic.Common.Health.Components;
using ECS_Logic.Common.Health.Systems;
using ECS_Logic.EffectTriggers.Components;
using Unity.Entities;

namespace ECS_Logic
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	[UpdateBefore(typeof(DamageApplySystem))]
	public class ApplyExtractedDamageFromCollisionsSystem : SystemBase
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
				.WithSharedComponentFilter(new EffectTriggerApplyEffectType {EffectType = EffectType.Damage})
				.ForEach((int entityInQueryIndex, Entity entity, in EffectTriggerApplyData effectTriggerApplyData) =>
				{
					if (entityManager.Exists(effectTriggerApplyData.Target))
					{
						var buffer = entityManager.GetBuffer<DamageToApplyBufferElement>(effectTriggerApplyData.Target);
						buffer.Add((int) effectTriggerApplyData.Value);
					}

					destroyCommandBuffer.DestroyEntity(entity);
				})
				.Run();

			destroyCommandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}