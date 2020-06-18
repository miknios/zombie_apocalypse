using AuthoringComponents;
using Unity.Entities;

namespace ECS_Logic.Weapons.Components
{
	public struct Spell : IComponentData
	{
		public SpellType SpellType;
	}
}