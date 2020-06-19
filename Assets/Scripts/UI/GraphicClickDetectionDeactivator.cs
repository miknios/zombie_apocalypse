using UnityEngine;
using UnityEngine.EventSystems;

namespace DefaultNamespace
{
	public class GraphicClickDetectionDeactivator : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] private GameObject objectToDeactivate;
		
		public void OnPointerDown(PointerEventData eventData)
		{
			objectToDeactivate.SetActive(false);
		}
	}
}