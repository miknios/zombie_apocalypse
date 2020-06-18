using Signals;
using UI;
using Zenject;

public class KilledEnemiesLabelController : EnemyCountLabelBase
{
	[Inject] private SignalBus signalBus = null;
	
	protected override void OnAwake()
	{
		signalBus.Subscribe<KilledEnemySignal>(s => UpdateLabel(s.EnemiesKilledCount));
	}

	protected override string GetTextForCount(int count) => $"Killed enemies: {count}";
}