using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class KilledEnemiesLabelController : MonoBehaviour, IKilledEnemiesCountListener
{
	private TMP_Text text;

	private void Awake()
	{
		text = GetComponent<TMP_Text>();
	}

	public void OnKilledEnemiesCountChanged(int newKilledEnemiesCount)
	{
		UpdateLabel(newKilledEnemiesCount);
	}

	private void UpdateLabel(int killedEnemiesCount)
	{
		text.SetText($"Killed enemies: {killedEnemiesCount}");
	}
}