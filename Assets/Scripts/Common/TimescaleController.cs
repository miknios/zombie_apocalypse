using Signals;
using UnityEngine;
using Zenject;

public class TimescaleController : MonoBehaviour
{
	[Inject] private SignalBus signalBus = null;

	private void Awake()
	{
		signalBus.Subscribe<GameOverSignal>(() => SetTimescaleToZero());
	}

	private void SetTimescaleToZero()
	{
		Time.timeScale = 0;
	}
}