using Unity.Entities;

namespace ECS_Logic.TagComponents
{
	[GenerateAuthoringComponent]
	public struct Projectile : IComponentData
	{
		public int Damage;
	}
}