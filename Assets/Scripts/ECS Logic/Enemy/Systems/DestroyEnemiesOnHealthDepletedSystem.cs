using DefaultNamespace;
using ECS_Logic.Common.Health.Components;
using Unity.Entities;

namespace ECS_Logic.Enemy.Systems
{
	[UpdateInGroup(typeof(LateSimulationSystemGroup))]
	public class DestroyEnemiesOnHealthDepletedSystem : SystemBase
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
				.WithAll<HealthDepleted, EnemyTag>()
				.ForEach((Entity entity, int entityInQueryIndex) =>
				{
					commandBuffer.DestroyEntity(entityInQueryIndex, entity);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}