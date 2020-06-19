using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
	public class GraphicClickSceneLoader : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] private SceneNameUtils.Scene scene;
		
		public void OnPointerDown(PointerEventData eventData)
		{
			SceneManager.LoadScene(SceneNameUtils.SceneNameForEnum(scene));
		}
	}
}