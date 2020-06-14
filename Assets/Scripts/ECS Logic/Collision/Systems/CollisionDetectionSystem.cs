using ECS_Logic.Common.Collision.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace DefaultNamespace
{
	public class CollisionDetectionSystem : SystemBase
	{
		private EntityQuery hitboxAreaQuery;

		protected override void OnCreate()
		{
			hitboxAreaQuery = GetEntityQuery(
				ComponentType.ReadOnly(typeof(HitboxArea)),
				ComponentType.ReadOnly(typeof(Translation)));
		}

		protected override void OnUpdate()
		{
			if(hitboxAreaQuery.IsEmptyIgnoreFilter)
				return;
			
			var hitboxEntities = hitboxAreaQuery.ToEntityArray(Allocator.TempJob);

			Entities
				.ForEach(
					(int entityInQueryIndex, ref DynamicBuffer<TriggerCollisionBufferElement> triggerCollisionBuffer, 
						in TriggerArea triggerArea, in Translation translation) =>
					{
						for (int i = 0; i < hitboxEntities.Length; i++)
						{
							Entity hitboxEntity = hitboxEntities[i];
							HitboxArea hitboxArea = GetComponent<HitboxArea>(hitboxEntity);
							if (hitboxArea.CollisionLayer != triggerArea.CollisionLayer)
								continue;

							float3 hitboxPosition = GetComponent<Translation>(hitboxEntity).Value;
							float hitboxRadius = hitboxArea.Radius;
							if (!CirclesAreIntersecting(hitboxPosition, hitboxRadius, translation.Value,
								triggerArea.Radius))
								continue;

							bool isPresentInBuffer = false;
							for (int j = 0; j < triggerCollisionBuffer.Length; j++)
							{
								if (triggerCollisionBuffer[j] == hitboxEntity)
									isPresentInBuffer = true;
							}
							if(isPresentInBuffer)
								continue;
							
							triggerCollisionBuffer.Add(hitboxEntities[i]);
						}
					})
				.WithDeallocateOnJobCompletion(hitboxEntities)
				.ScheduleParallel();
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