using DG.Tweening;
using ECS_Logic.Weapons;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
	public class SpellCooldownIndicator : MonoBehaviour
	{
		[SerializeField] private SpellType spellType = SpellType.FireStrike;
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
			if (spellFiredSignal.SpellType != spellType)
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
			KillTweens();

			iconImageTransform
				.DOScale(1.2f, 0.1f)
				.SetLoops(2, LoopType.Yoyo);

			iconImage
				.DOColor(Color.green, 0.3f)
				.SetLoops(2, LoopType.Yoyo);
		}

		private void KillTweens()
		{
			iconImageTransform.DOKill(true);
			iconImage.DOKill(true);
		}

		private void SetTextToCurrentTime(float i)
		{
			cooldownLabel.SetText($"{i:0.0}s");
		}

		private void OnDestroy()
		{
			KillTweens();
		}
	}
}