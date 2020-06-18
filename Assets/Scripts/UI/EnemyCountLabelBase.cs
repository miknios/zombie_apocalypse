using TMPro;
using UnityEngine;

namespace UI
{
	[RequireComponent(typeof(TMP_Text))]
	public abstract class EnemyCountLabelBase : MonoBehaviour
	{
		private TMP_Text text;

		private void Awake()
		{
			text = GetComponent<TMP_Text>();
			OnAwake();
		}

		protected virtual void OnAwake() { }

		protected void UpdateLabel(int count)
		{
			text.SetText(GetTextForCount(count));
		}

		protected abstract string GetTextForCount(int count);
	}
}