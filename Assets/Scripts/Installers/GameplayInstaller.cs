using Configuration;
using Zenject;

namespace Installers
{
	public class GameplayInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IInitializable>().To<GameplayWorldInitialization>().AsSingle();
		}
	}
}