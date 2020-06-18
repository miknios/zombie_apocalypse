﻿using AuthoringComponents;
using DG.Tweening;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SpellCooldownIndicator : MonoBehaviour
{
	[SerializeField] private SpellType spellType = SpellType.FireStrike;
	[SerializeField] private float timerTimeStep = 0.1f;
	[SerializeField] private TMP_Text cooldownLabel = null;
	[SerializeField] private Image iconImage = null;

	private RectTransform iconImageTransform;

	[Inject]
	public void ConstructWithInjection(SignalBus signalBus)
	{
		iconImageTransform = iconImage.transform as RectTransform;
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
		DOTween
			.To(SetTextToCurrentTime, cooldown, 0, cooldown)
			.OnComplete(IndicateSpellAvailable);
	}

	private void IndicateSpellAvailable()
	{
		ClearText();

		iconImageTransform.DOKill(true);
		iconImageTransform
			.DOScale(1.2f, 0.1f)
			.SetLoops(2, LoopType.Yoyo);

		iconImage.DOKill(true);
		iconImage
			.DOColor(Color.green, 0.3f)
			.SetLoops(2, LoopType.Yoyo);
	}

	private void SetTextToCurrentTime(float i)
	{
		cooldownLabel.SetText($"{i:0.0}s");
	}
}