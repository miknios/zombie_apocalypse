using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
	public class DifficultyConfirmButtonBehaviour : ButtonBehaviour
	{
		[SerializeField] private DifficultySetting difficultySetting;

		protected override void OnButtonClick()
		{
			PlayerPrefs.SetInt(PlayerPrefsKey.DIFFICULTY_SETTING, (int)difficultySetting);
			PlayerPrefs.Save();
			SceneManager.LoadScene(SceneNameUtils.SceneNameForEnum(SceneNameUtils.Scene.Gameplay));
		}
	}
}