using DefaultNamespace;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class EcsCursorWorldPositionSynchronizer : MonoBehaviour
{
	[SerializeField] private Camera mainCamera = null;
	private EntityManager entityManager;
	private Entity cursorEntity;
	private readonly RaycastHit[] raycastHits = new RaycastHit[1];

	private void Awake()
	{
		entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
		cursorEntity = entityManager.CreateEntity(typeof(CursorWorldPosition));
		Synchronize();
	}

	private void LateUpdate()
	{
		Synchronize();
	}

	private void Synchronize()
	{
		float3 newPosition = new float3();
		CalculateCursorWorldPosition(ref newPosition);
		var component = new CursorWorldPosition
		{
			Value = newPosition
		};
		
		entityManager.SetComponentData(cursorEntity, component);
	}

	private void CalculateCursorWorldPosition(ref float3 position)
	{
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		int hitCount =
			Physics.RaycastNonAlloc(ray, raycastHits, Mathf.Infinity, 1 << UnityLayer.MOUSE_RAYCAST_COLLISION);

		if (hitCount == 0)
			return;

		position = raycastHits[0].point;
	}
}