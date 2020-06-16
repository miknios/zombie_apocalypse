using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class KilledEnemiesLabelController : MonoBehaviour
{
	[Inject] private SignalBus signalBus = null;
	private TMP_Text text;
	
	private void Awake()
	{
		signalBus.Subscribe<KilledEnemySignal>(s => UpdateLabel(s.EnemiesKilledCount));
		text = GetComponent<TMP_Text>();
	}

	private void UpdateLabel(int killedEnemiesCount)
	{
		text.SetText($"Killed enemies: {killedEnemiesCount}");
	}
}