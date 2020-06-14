using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace ECS_Logic.Common.Collision.Components
{
	public class GunAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
	{
		[SerializeField] private GameObject projectilePrefab;
		[SerializeField] private KeyCode keyToTrigger;

		public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
		{
			referencedPrefabs.Add(projectilePrefab);
		}
		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			// TODO: create gun entity with parameters and projectile prefab entity 
			// then think how to assign controls for shooting - set system field, keep keycode as component or maybe blob asset
			// maybe it's not important right now so let's try setting system field and doing if(input is pressed) -> run shoot job
			
		}
	}
}