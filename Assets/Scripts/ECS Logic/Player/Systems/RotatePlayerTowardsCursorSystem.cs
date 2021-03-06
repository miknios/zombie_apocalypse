﻿using ECS_Configuration;
using ECS_Logic.Components;
using ECS_Logic.TagComponents;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS_Logic.Player.Systems
{
	[UpdateInGroup(typeof(ContextWideDataDependentSystemGroup))]
	public class RotatePlayerTowardsCursorSystem : SystemBase
	{
		private EntityQuery playerQuery;
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			playerQuery = GetEntityQuery(typeof(PlayerTag), typeof(Rotation));
			commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			if (playerQuery.IsEmptyIgnoreFilter)
				return;

			Entity playerEntity = playerQuery.GetSingletonEntity();
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

			Entities
				.WithChangeFilter<CursorWorldPosition>()
				.ForEach((int entityInQueryIndex, in CursorWorldPosition cursorPosition) =>
				{
					var rotation = GetNewPlayerRotation(in cursorPosition);
					commandBuffer.SetComponent(entityInQueryIndex, playerEntity, rotation);
				})
				.ScheduleParallel();

			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}

		private static Rotation GetNewPlayerRotation(in CursorWorldPosition cursorPosition)
		{
			float3 playerForward = cursorPosition.Value;
			playerForward.y = 0;
			Rotation rotation = new Rotation
			{
				Value = quaternion.LookRotation(playerForward, new float3(0, 1, 0))
			};

			return rotation;
		}
	}
}