using UnityEngine.Rendering;
using Zenject;

namespace DefaultNamespace
{
	public class SceneInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			BindSignals();
			Container.Bind<IInitializable>().To<WorldInitialization>().AsSingle();
		}

		private void BindSignals()
		{
			SignalBusInstaller.Install(Container);
			Container.DeclareSignal<KilledEnemySignal>();
		}
	}
}