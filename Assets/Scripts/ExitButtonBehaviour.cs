using UnityEngine;

namespace DefaultNamespace
{
	public class ExitButtonBehaviour : ButtonBehaviour
	{
		protected override void OnButtonClick() => Application.Quit();
	}
}