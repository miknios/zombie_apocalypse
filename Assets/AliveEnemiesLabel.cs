using Signals;
using UI;
using Zenject;

public class AliveEnemiesLabel : EnemyCountLabelBase
{
	[Inject] private SignalBus signalBus;

	protected override void OnAwake()
	{
		signalBus.Subscribe<AliveEnemiesSignal>(s => UpdateLabel(s.AliveEnemiesCount));
	}

	protected override string GetTextForCount(int count) => $"Alive enemies: {count}";
}