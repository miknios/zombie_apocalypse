using DefaultNamespace;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Unity.Mathematics
{
	public class RotatePlayerTowardsCursorSystem : SystemBase
	{
		private EntityQuery playerQuery;
		private EntityCommandBufferSystem commandBufferSystem;
		
		protected override void OnCreate()
		{
			base.OnCreate();
			
			playerQuery = GetEntityQuery(typeof(PlayerTagComponent), typeof(Rotation));
			commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			NativeArray<Entity> playerEntities = playerQuery.ToEntityArray(Allocator.TempJob);

			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();
			Entities
				.WithChangeFilter<CursorWorldPositionComponent>()
				.ForEach((int entityInQueryIndex, in CursorWorldPositionComponent cursorPosition) =>
				{
					for (int i = 0; i < playerEntities.Length; i++)
					{
						float3 newForward = cursorPosition.Value;
						newForward.y = 0;
						quaternion newRotationValue = quaternion.LookRotation(newForward, new float3(0, 1, 0));
						Rotation newRotation = new Rotation
						{
							Value = newRotationValue
						};
						
						commandBuffer.SetComponent(entityInQueryIndex, playerEntities[i], newRotation);
					}
				})
				.WithDeallocateOnJobCompletion(playerEntities)
				.ScheduleParallel();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}