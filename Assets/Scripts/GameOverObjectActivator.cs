using Signals;
using UnityEngine;
using Zenject;

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