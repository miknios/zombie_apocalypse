using Signals;
using Zenject;

namespace DefaultNamespace
{
	public class ProjectInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindSignals();
			
			Container.Bind<IInitializable>().To<GameplayWorldInitialization>().AsSingle();
		}

		private void BindSignals()
		{
			SignalBusInstaller.Install(Container);
			Container.DeclareSignal<KilledEnemySignal>().OptionalSubscriber();
			Container.DeclareSignal<AliveEnemiesSignal>().OptionalSubscriber();
			Container.DeclareSignal<GameOverSignal>().OptionalSubscriber();
			Container.DeclareSignal<SpellFiredSignal>().OptionalSubscriber();
		}
	}
}