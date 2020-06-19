using UnityEngine;

public class InitiallyDeactivatedObjectsDeactivator : MonoBehaviour
{
	private void Awake()
	{
		var objectsToDeactivate = GetComponentsInChildren<InitiallyDeactivated>();
		foreach (var objectToDeactivate in objectsToDeactivate)
		{
			objectToDeactivate.gameObject.SetActive(false);
		}
	}
}