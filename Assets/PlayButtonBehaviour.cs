using DefaultNamespace;
using UnityEngine;

public class PlayButtonBehaviour : ButtonBehaviour
{
	[SerializeField] private GameObject difficultyChoosePanel;
	
	protected override void OnButtonClick()
	{
		difficultyChoosePanel.SetActive(true);
	}
}