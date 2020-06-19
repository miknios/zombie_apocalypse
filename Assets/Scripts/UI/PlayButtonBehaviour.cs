using UnityEngine;

namespace UI
{
	public class PlayButtonBehaviour : ButtonBehaviour
	{
		[SerializeField] private GameObject difficultyChoosePanel = null;

		protected override void OnButtonClick()
		{
			difficultyChoosePanel.SetActive(true);
		}
	}
}