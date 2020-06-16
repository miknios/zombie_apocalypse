using DefaultNamespace;
using ECS_Logic.Common.Health.Components;
using Signals;
using Unity.Entities;
using Zenject;

namespace ECS_Logic.GameOver.Systems
{
	public class FireGameOverSignalSystem : SystemBase
	{
		[Inject] private SignalBus signalBus = null;
		
		protected override void OnUpdate()
		{
			Entities
				.WithoutBurst()
				.WithAll<PlayerTag>()
				.WithChangeFilter<HealthDepleted>()
				.ForEach((in HealthDepleted healthDepleted) =>
				{
					signalBus.Fire(new GameOverSignal{AliveTime = (int)Time.ElapsedTime});
				})
				.Run();
		}
	}
}