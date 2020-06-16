using Signals;
using TMPro;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TMP_Text))]
public class GameOverAliveTimeLabel : MonoBehaviour
{
    private TMP_Text text;
    
    [Inject]
    public void ConstructWithInjection(SignalBus signalBus)
    {
        text = GetComponent<TMP_Text>();
        signalBus.Subscribe<GameOverSignal>(s => SetLabelText(s.AliveTime));
    }

    private void SetLabelText(int aliveTime)
    {
        text.SetText($"You were alive for {aliveTime} seconds!");
    }
}
