using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
	public class GraphicClickDetectionDeactivator : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] private GameObject objectToDeactivate = null;

		public void OnPointerDown(PointerEventData eventData)
		{
			objectToDeactivate.SetActive(false);
		}
	}
}