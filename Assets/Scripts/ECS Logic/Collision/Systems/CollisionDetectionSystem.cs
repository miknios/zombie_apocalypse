using ECS_Logic;
using ECS_Logic.Common.Collision.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(CollisionDetectionSystemGroup))]
	public class CollisionDetectionSystem : SystemBase
	{
		struct HitboxCollisionDetectionData
		{
			public float3 Position;
			public float Radius;
			public CollisionLayer CollisionLayer;
			public Entity Entity;
		}

		private EntityQuery hitboxQuery;
		private EntityQuery triggerQuery;

		protected override void OnUpdate()
		{
			int hitboxCount = hitboxQuery.CalculateEntityCount();
			NativeMultiHashMap<uint, HitboxCollisionDetectionData> hitboxDataForCellHash =
				new NativeMultiHashMap<uint, HitboxCollisionDetectionData>(hitboxCount, Allocator.TempJob);
			var parallelHashMap = hitboxDataForCellHash.AsParallelWriter();

			var hitboxHashMapJobHandle = Entities
				.WithStoreEntityQueryInField(ref hitboxQuery)
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
				.ScheduleParallel(Dependency);

			int triggerCount = triggerQuery.CalculateEntityCount();
			NativeArray<uint> triggerCellHashArray = new NativeArray<uint>(triggerCount, Allocator.TempJob);

			var triggerArrayJobHandle = Entities
				.WithStoreEntityQueryInField(ref triggerQuery)
				.ForEach((int entityInQueryIndex, in TriggerArea triggerArea, in LocalToWorld localToWorld) =>
				{
					triggerCellHashArray[entityInQueryIndex] = GetCellHash(localToWorld.Position);
				})
				.ScheduleParallel(Dependency);

			var combinedJobHandles = JobHandle.CombineDependencies(hitboxHashMapJobHandle, triggerArrayJobHandle);

			var collisionDetectionJobHandle = Entities
				.WithReadOnly(triggerCellHashArray)
				.WithReadOnly(hitboxDataForCellHash)
				.ForEach((int entityInQueryIndex, ref DynamicBuffer<TriggerCollisionBufferElement> collisionBuffer, 
					in TriggerArea triggerArea, in LocalToWorld localToWorld) =>
				{
					var triggerCellHash = triggerCellHashArray[entityInQueryIndex];
					var hitboxDataForCell = hitboxDataForCellHash.GetValuesForKey(triggerCellHash);
					while (hitboxDataForCell.MoveNext())
					{
						var hitboxData = hitboxDataForCell.Current;
						
						if(triggerArea.CollisionLayer != hitboxData.CollisionLayer)
							continue;

						if(!CirclesAreIntersecting(localToWorld.Position, triggerArea.Radius, 
							hitboxData.Position, hitboxData.Radius))
							continue;

						collisionBuffer.Add(new TriggerCollisionBufferElement {HitboxEntity = hitboxData.Entity});
					}
				})
				.ScheduleParallel(combinedJobHandles);

			Dependency = collisionDetectionJobHandle;
			var disposeJobHandle = triggerCellHashArray.Dispose(Dependency);
			disposeJobHandle = JobHandle.CombineDependencies(disposeJobHandle, hitboxDataForCellHash.Dispose(Dependency));
			Dependency = disposeJobHandle;
		}

		private static uint GetCellHash(float3 position)
		{
			float cellSize = 40;
			return math.hash(math.floor((position.xy - new float2(cellSize)) / cellSize));
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