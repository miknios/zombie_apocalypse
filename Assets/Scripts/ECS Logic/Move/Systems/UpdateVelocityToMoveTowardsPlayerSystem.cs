using Configuration;
using ECS_Logic.Move.Components;
using ECS_Logic.TagComponents;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS_Logic.Move.Systems
{
	[UpdateInGroup(typeof(UpdateSimulationDataSystemGroup))]
	public class UpdateVelocityToMoveTowardsPlayerSystem : SystemBase
	{
		private EntityQuery playerEntityQuery;
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			playerEntityQuery = GetEntityQuery(ComponentType.ReadOnly<PlayerTag>());
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			Entity playerEntity = playerEntityQuery.GetSingletonEntity();
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

			Entities
				.WithAll<MoveTowardsPlayer>()
				.WithNone<CurrentVelocity>()
				.ForEach((Entity entity, int entityInQueryIndex) =>
				{
					commandBuffer.AddComponent<CurrentVelocity>(entityInQueryIndex, entity);
				})
				.ScheduleParallel();

			commandBufferSystem.AddJobHandleForProducer(Dependency);

			Entities
				.WithAll<MoveTowardsPlayer>()
				.ForEach((ref CurrentVelocity currentVelocity, in BaseSpeed baseSpeedComponent,
					in Translation translation) =>
				{
					float3 playerPosition = GetComponent<Translation>(playerEntity).Value;
					float3 currentPosition = translation.Value;
					float3 direction = math.normalize(playerPosition - currentPosition);
					currentVelocity.Value = direction * baseSpeedComponent.Value;
				})
				.ScheduleParallel();
		}
	}
}