using System.Collections.Generic;
using ECS_Logic.Common.Components;
using ECS_Logic.Timers.Components;
using ECS_Logic.Weapons;
using ECS_Logic.Weapons.Components;
using Unity.Entities;
using UnityEngine;

namespace EntityAuthoringComponents
{
	[AddComponentMenu("AuthoringComponents/Gun")]
	public class GunAuthoring : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
	{
		[SerializeField] private GameObject projectilePrefab = null;
		[SerializeField] private KeyCode keyToTrigger = KeyCode.Mouse0;
		[SerializeField] private float projectileSpeed = 40;
		[SerializeField] private float cooldownTime = 0;
		[SerializeField] private bool isSpell = false;
		[SerializeField] private SpellType spellType = SpellType.FireStrike;

		public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
		{
			referencedPrefabs.Add(projectilePrefab);
		}

		public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
		{
			dstManager.AddComponentData(entity, new Gun
			{
				KeyCode = keyToTrigger,
				ProjectileEntity = conversionSystem.GetPrimaryEntity(projectilePrefab),
				ProjectileSpeed = projectileSpeed
			});

			dstManager.AddComponentData(entity, new Timer
			{
				AutoRestart = false,
				CurrentTime = cooldownTime,
				InitialTime = cooldownTime,
				Owner = entity
			});
			dstManager.AddComponent<Enabled>(entity);

			if (cooldownTime == 0)
				dstManager.AddComponent<Timeout>(entity);

			if (isSpell)
				dstManager.AddComponentData(entity, new Spell {SpellType = spellType});
		}
	}
}