using DefaultNamespace;
using ECS_Logic.Common.Health.Components;
using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Common.Health.Systems
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	public class DamageApplySystem : SystemBase
	{
		protected override void OnUpdate()
		{
			Entities
				.ForEach((ref HealthPoints healthPoints, ref DynamicBuffer<DamageToApplyBufferElement> damageBuffer) =>
				{
					int damage = 0;
					for (int i = 0; i < damageBuffer.Length; i++)
					{
						damage += damageBuffer[i].Value;
					}
					damageBuffer.Clear();

					healthPoints.Value = math.max(healthPoints.Value - damage, 0);
				})
				.ScheduleParallel();
		}
	}
}