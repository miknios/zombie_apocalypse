using UnityEngine;

namespace UI
{
	public class ExitButtonBehaviour : ButtonBehaviour
	{
		protected override void OnButtonClick() => Application.Quit();
	}
}