using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	[RequireComponent(typeof(Button))]
	public abstract class ButtonBehaviour : MonoBehaviour
	{
		private void Awake()
		{
			Button button = GetComponent<Button>();
			button.onClick.AddListener(OnButtonClick);
		}

		protected abstract void OnButtonClick();
	}
}