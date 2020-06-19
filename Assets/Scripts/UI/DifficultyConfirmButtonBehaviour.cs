using Common;
using Difficulty;
using SceneManagement;
using UnityEngine;

namespace UI
{
	public class DifficultyConfirmButtonBehaviour : ButtonBehaviour
	{
		[SerializeField] private DifficultySetting difficultySetting = DifficultySetting.Medium;

		protected override void OnButtonClick()
		{
			PlayerPrefs.SetInt(PlayerPrefsKey.DIFFICULTY_SETTING, (int) difficultySetting);
			PlayerPrefs.Save();

			SceneLoadUtils.LoadGameplayScene();
		}
	}
}