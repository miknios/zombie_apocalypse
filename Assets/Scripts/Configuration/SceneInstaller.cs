using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace DefaultNamespace
{
	public class SceneInstaller : MonoInstaller
	{
		[SerializeField] private DifficultyConfig difficultyConfig = null;
		
		public override void InstallBindings()
		{
			BindSignals();
			Initialization();
			BindScriptableObjects();
		}

		private void BindScriptableObjects()
		{
			Container.BindInstance(difficultyConfig.PlayerConfig);
			Container.BindInstance(difficultyConfig.SpawnerConfig);
		}

		private void Initialization()
		{
			Container.Bind<IInitializable>().To<WorldInitialization>().AsSingle();
		}

		private void BindSignals()
		{
			SignalBusInstaller.Install(Container);
			Container.DeclareSignal<KilledEnemySignal>();
		}
	}
}