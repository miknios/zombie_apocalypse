using Signals;
using UnityEngine;
using Zenject;

namespace Common
{
	public class GameOverObjectActivator : MonoBehaviour
	{
		[Inject]
		public void ConstructWithInjection(SignalBus signalBus)
		{
			signalBus.Subscribe<GameOverSignal>(() => OnGameOver());
			gameObject.SetActive(false);
		}

		private void OnGameOver() => gameObject.SetActive(true);
	}
}