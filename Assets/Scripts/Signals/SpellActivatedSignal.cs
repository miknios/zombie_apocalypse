using ECS_Logic.Weapons;

namespace Signals
{
	public struct SpellActivatedSignal
	{
		public SpellType SpellType;
		public float Cooldown;
	}
}