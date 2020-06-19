using Configuration;
using Zenject;

namespace Installers
{
	public class MenuInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IInitializable>().To<WorldDisposer>().AsSingle();
		}
	}
}