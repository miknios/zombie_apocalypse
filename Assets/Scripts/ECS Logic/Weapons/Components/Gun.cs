using Unity.Entities;
using UnityEngine;

namespace ECS_Logic.Weapons.Components
{
	public struct Gun : IComponentData
	{
		public Entity ProjectileEntity;
		public float ProjectileSpeed;
		public KeyCode KeyCode;
	}
}