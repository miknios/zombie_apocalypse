using Configuration;
using ECS_Logic.Health.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Health.Systems
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	public class DamageApplySystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

			Entities
				.ForEach((Entity entity, int entityInQueryIndex,
					ref HealthPoints healthPoints, ref DynamicBuffer<DamageToApplyBufferElement> damageBuffer) =>
				{
					int damage = 0;
					for (int i = 0; i < damageBuffer.Length; i++)
					{
						damage += damageBuffer[i].Value;
					}

					damageBuffer.Clear();

					healthPoints.Value = math.max(healthPoints.Value - damage, 0);

					if (healthPoints.Value == 0)
						commandBuffer.AddComponent<HealthDepleted>(entityInQueryIndex, entity);
				})
				.ScheduleParallel();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}