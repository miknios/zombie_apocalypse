using Configuration;
using ECS_Logic.EffectTriggers.Components;
using ECS_Logic.Health.Components;
using ECS_Logic.Health.Systems;
using Unity.Entities;

namespace ECS_Logic.EffectTriggers.Systems
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	[UpdateBefore(typeof(DamageApplySystem))]
	public class ApplyExtractedDamageFromCollisionsSystem : SystemBase
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
				.WithSharedComponentFilter(new EffectTriggerApplyEffectType {EffectType = EffectType.Damage})
				.ForEach((int entityInQueryIndex, Entity entity, in EffectTriggerApplyData effectTriggerApplyData) =>
				{
					commandBuffer.DestroyEntity(entity);

					if (!entityManager.Exists(effectTriggerApplyData.Target))
						return;

					var buffer = entityManager.GetBuffer<DamageToApplyBufferElement>(effectTriggerApplyData.Target);
					buffer.Add((int) effectTriggerApplyData.Value);
				})
				.Run();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}