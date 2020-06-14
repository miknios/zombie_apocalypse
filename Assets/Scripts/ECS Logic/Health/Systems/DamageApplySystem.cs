using DefaultNamespace;
using ECS_Logic.Common.Health.Components;
using Unity.Entities;

namespace ECS_Logic.Common.Health.Systems
{
	[UpdateAfter(typeof(EnemyTriggerSystem))]
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

					healthPoints.Value -= damage;
				})
				.ScheduleParallel();
		}
	}
}