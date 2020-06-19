using Signals;
using UnityEngine;
using Zenject;

namespace Common
{
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

		private void OnDestroy()
		{
			RestoreInitialTimescale();
		}

		private void RestoreInitialTimescale()
		{
			Time.timeScale = 1;
		}
	}
}