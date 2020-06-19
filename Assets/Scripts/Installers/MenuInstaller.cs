using Zenject;

namespace DefaultNamespace
{
	public class MenuInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.Bind<IInitializable>().To<WorldDisposer>().AsSingle();
		}
	}
}