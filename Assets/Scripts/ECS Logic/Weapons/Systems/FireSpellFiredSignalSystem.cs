using ECS_Logic.Timers.Components;
using ECS_Logic.Weapons.Components;
using Signals;
using Unity.Entities;
using Zenject;

namespace ECS_Logic.Weapons.Systems
{
	[UpdateInGroup(typeof(PresentationSystemGroup))]
	public class FireSpellFiredSignalSystem : SystemBase
	{
		[Inject] private SignalBus signalBus = null;
		
		protected override void OnUpdate()
		{
			Entities
				.WithoutBurst()
				.WithNone<Timeout>()
				.ForEach((in Spell spell, in Timer timer) =>
				{
					if(timer.CurrentTime != timer.InitialTime)
						return;
					
					signalBus.Fire(new SpellFiredSignal
					{
						SpellType = spell.SpellType,
						Cooldown = timer.CurrentTime
					});
				})
				.Run();
		}
	}
}