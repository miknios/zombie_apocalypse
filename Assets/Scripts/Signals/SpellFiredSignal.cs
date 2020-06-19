using ECS_Logic.Weapons;

namespace Signals
{
	public struct SpellFiredSignal
	{
		public SpellType SpellType;
		public float Cooldown;
	}
}