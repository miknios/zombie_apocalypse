using System.Collections;
using AuthoringComponents;
using Signals;
using TMPro;
using UnityEngine;
using Zenject;

public class SpellCooldownIndicator : MonoBehaviour
{
	[SerializeField] private SpellType spellType = SpellType.FireStrike;
	[SerializeField] private float timerTimeStep = 0.1f;
	[SerializeField] private TMP_Text cooldownLabel = null;

	private Coroutine currentCoroutine = null;

	[Inject]
	public void ConstructWithInjection(SignalBus signalBus)
	{
		ClearText();
		signalBus.Subscribe<SpellFiredSignal>(ProcessSignal);
	}

	private void ClearText()
	{
		cooldownLabel.SetText("");
	}

	private void ProcessSignal(SpellFiredSignal spellFiredSignal)
	{
		if(spellFiredSignal.SpellType != spellType)
			return;

		AnimateCooldown(spellFiredSignal.Cooldown);
	}

	private void AnimateCooldown(float cooldown)
	{
		if(currentCoroutine != null)
			StopCoroutine(currentCoroutine);
		
		currentCoroutine = StartCoroutine(AnimateLabel(cooldown));
	}

	private IEnumerator AnimateLabel(float cooldown)
	{
		for (float i = cooldown; i > 0; i -= timerTimeStep)
		{
			SetTextToCurrentTime(i);
			yield return new WaitForSecondsRealtime(timerTimeStep);
		}

		ClearText();
		currentCoroutine = null;
	}

	private void SetTextToCurrentTime(float i)
	{
		cooldownLabel.SetText($"{i.ToString("0.0")}s");
	}
}