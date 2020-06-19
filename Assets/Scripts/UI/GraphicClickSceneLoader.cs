using SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
	public class GraphicClickSceneLoader : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] private SceneNameUtils.Scene scene = SceneNameUtils.Scene.Gameplay;

		public void OnPointerDown(PointerEventData eventData)
		{
			SceneManager.LoadScene(SceneNameUtils.SceneNameForEnum(scene));
		}
	}
}