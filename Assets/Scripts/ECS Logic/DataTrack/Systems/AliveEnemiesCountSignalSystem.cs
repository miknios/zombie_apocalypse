using ECS_Logic.TagComponents;
using Signals;
using Unity.Entities;
using Zenject;

namespace ECS_Logic.DataTrack.Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public class AliveEnemiesCountSignalSystem : SystemBase
	{
		[Inject] private SignalBus signalBus = null;
		private EntityQuery query;
		private int previousCount;

		protected override void OnCreate()
		{
			previousCount = 0;
			query = GetEntityQuery(ComponentType.ReadOnly<EnemyTag>());
		}

		protected override void OnUpdate()
		{
			int currentCount = query.CalculateEntityCount();
			if (currentCount == previousCount)
				return;

			signalBus.Fire(new AliveEnemiesSignal {AliveEnemiesCount = currentCount});
			previousCount = currentCount;
		}
	}
}