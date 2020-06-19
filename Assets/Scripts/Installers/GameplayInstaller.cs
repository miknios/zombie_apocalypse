using ECS_Configuration;
using Signals;
using Unity.Entities;
using Zenject;

namespace Installers
{
	public class GameplayInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IInitializable>().To<WorldInjector>().AsSingle();
			BindSignals();
		}

		private void BindSignals()
		{
			SignalBusInstaller.Install(Container);
			Container.DeclareSignal<KilledEnemySignal>().OptionalSubscriber();
			Container.DeclareSignal<AliveEnemiesSignal>().OptionalSubscriber();
			Container.DeclareSignal<GameOverSignal>().OptionalSubscriber();
			Container.DeclareSignal<SpellActivatedSignal>().OptionalSubscriber();
		}
	}
}