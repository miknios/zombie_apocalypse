using Unity.Entities;

namespace ECS_Logic.Weapons.Components
{
	[GenerateAuthoringComponent]
	public struct Projectile : IComponentData
	{
		public int Damage;
	}
}