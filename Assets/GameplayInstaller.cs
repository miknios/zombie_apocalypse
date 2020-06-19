using DefaultNamespace;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
	public override void InstallBindings()
	{
		Container.Bind<IInitializable>().To<GameplayWorldInitialization>().AsSingle();
	}
}