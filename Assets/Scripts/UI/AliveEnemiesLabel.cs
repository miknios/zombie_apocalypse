using Signals;
using Zenject;

namespace UI
{
	public class AliveEnemiesLabel : EnemyCountLabelBase
	{
		[Inject] private SignalBus signalBus = null;

		protected override void OnAwake()
		{
			signalBus.Subscribe<AliveEnemiesSignal>(s => UpdateLabel(s.AliveEnemiesCount));
		}

		protected override string GetTextForCount(int count) => $"Alive enemies: {count}";
	}
}