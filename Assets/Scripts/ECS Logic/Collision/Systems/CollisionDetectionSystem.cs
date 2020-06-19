using ECS_Configuration;
using ECS_Logic.Collision.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECS_Logic.Collision.Systems
{
	// Collision detection system is based on idea of splitting world space into grid.
	// Because of this we can check overlapping entities only inside a single cell instead of checking every single trigger entity with every single hitbox entity.
	[UpdateInGroup(typeof(CollisionDetectionSystemGroup))]
	public class CollisionDetectionSystem : SystemBase
	{
		private struct HitboxCollisionDetectionData
		{
			public float3 Position;
			public float Radius;
			public CollisionLayer CollisionLayer;
			public Entity Entity;
		}

		// TODO: extract this value somewhere else?
		private const float CELL_SIZE = 2;

		private EntityQuery hitboxQuery;
		private EntityQuery triggerQuery;

		protected override void OnUpdate()
		{
			int hitboxCount = hitboxQuery.CalculateEntityCount();
			var hitboxDataForCellHash =
				new NativeMultiHashMap<uint, HitboxCollisionDetectionData>(hitboxCount, Allocator.TempJob);
			var parallelHashMap = hitboxDataForCellHash.AsParallelWriter();

			// Populating hitbox hashmap with  data for later collision parallel processing. Hashmap key is hash value generated from grid cell position.
			var hitboxHashMapJobHandle = Entities
				.ForEach((Entity entity, in HitboxArea hitboxArea, in LocalToWorld localToWorld) =>
				{
					parallelHashMap.Add(GetCellHash(localToWorld.Position), new HitboxCollisionDetectionData
					{
						CollisionLayer = hitboxArea.CollisionLayer,
						Entity = entity,
						Position = localToWorld.Position,
						Radius = hitboxArea.Radius
					});
				})
				.WithStoreEntityQueryInField(ref hitboxQuery)
				.ScheduleParallel(Dependency);

			int triggerCount = triggerQuery.CalculateEntityCount();
			NativeArray<uint> triggerCellHashArray = new NativeArray<uint>(triggerCount, Allocator.TempJob);

			// Populating array where we assign each trigger to cell it's in for later collision parallel processing.
			var triggerArrayJobHandle = Entities
				.ForEach((int entityInQueryIndex, in TriggerArea triggerArea, in LocalToWorld localToWorld) =>
				{
					triggerCellHashArray[entityInQueryIndex] = GetCellHash(localToWorld.Position);
				})
				.WithStoreEntityQueryInField(ref triggerQuery)
				.ScheduleParallel(Dependency);

			// Combining job dependencies for next job, because it needs both collections filled before starting.
			var combinedJobHandles = JobHandle.CombineDependencies(hitboxHashMapJobHandle, triggerArrayJobHandle);

			// Iterating through all triggers checking intersections foreach hitbox inside cell it's in.
			// Adding hitbox entity reference to trigger collision buffer if intersected.
			var collisionDetectionJobHandle = Entities
				.ForEach((int entityInQueryIndex, ref DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer,
					in TriggerArea triggerArea, in LocalToWorld localToWorld) =>
				{
					var triggerCellHash = triggerCellHashArray[entityInQueryIndex];
					CheckIntersectionsWithHitboxesInCell(ref hitboxDataForCellHash, ref collisionBuffer,
						triggerCellHash, triggerArea, localToWorld);
				})
				.WithReadOnly(triggerCellHashArray)
				.WithReadOnly(hitboxDataForCellHash)
				.ScheduleParallel(combinedJobHandles);

			// Disposing temporary native collections with job handles, because jobs needs to finish before disposing them.
			Dependency = collisionDetectionJobHandle;
			var disposeJobHandle = triggerCellHashArray.Dispose(Dependency);
			disposeJobHandle =
				JobHandle.CombineDependencies(disposeJobHandle, hitboxDataForCellHash.Dispose(Dependency));

			// Returning combined job handle dependency to system.
			Dependency = disposeJobHandle;
		}

		private static void CheckIntersectionsWithHitboxesInCell(
			ref NativeMultiHashMap<uint, HitboxCollisionDetectionData> hitboxDataForCellHash,
			ref DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer, uint triggerCellHash,
			TriggerArea triggerArea, LocalToWorld localToWorld)
		{
			var hitboxDataForCell = hitboxDataForCellHash.GetValuesForKey(triggerCellHash);
			while (hitboxDataForCell.MoveNext())
			{
				ProcessHitboxDataAndAddIntersectionsToBuffer(ref collisionBuffer, ref hitboxDataForCell, triggerArea,
					localToWorld);
			}
		}

		private static void ProcessHitboxDataAndAddIntersectionsToBuffer(
			ref DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer,
			ref NativeMultiHashMap<uint, HitboxCollisionDetectionData>.Enumerator hitboxDataForCell,
			TriggerArea triggerArea, LocalToWorld localToWorld)
		{
			var hitboxData = hitboxDataForCell.Current;
			if (triggerArea.CollisionLayer != hitboxData.CollisionLayer)
				return;

			if (!CirclesAreIntersecting(localToWorld.Position, triggerArea.Radius,
				hitboxData.Position, hitboxData.Radius))
				return;

			collisionBuffer.Add(new TriggerCollisionBufferElement {HitboxEntity = hitboxData.Entity});
		}

		// Generates cell hash for entity's position. It's offsetted by half cell, because in our case it's more optimal for cell to be in the center where the player is.
		private static uint GetCellHash(float3 position)
		{
			float2 cellCoordinates = math.floor((position.xy - new float2(CELL_SIZE) / 2) / CELL_SIZE);
			return math.hash(cellCoordinates);
		}

		private static bool CirclesAreIntersecting(float3 center1, float radius1, float3 center2, float radius2)
		{
			float dx = center1.x - center2.x;
			float dy = center1.z - center2.z;
			float distance = math.sqrt(dx * dx + dy * dy);

			return distance < radius1 + radius2;
		}
	}
}