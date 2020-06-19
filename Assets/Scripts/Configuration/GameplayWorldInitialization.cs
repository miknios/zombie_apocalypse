using Unity.Entities;
using Zenject;

namespace DefaultNamespace
{
	public class GameplayWorldInitialization : IInitializable
	{
		[Inject] private DiContainer diContainer = null;
		
		public void Initialize()
		{
			InjectIntoSystems();
		}

		private void InjectIntoSystems()
		{
			var systems = World.DefaultGameObjectInjectionWorld.Systems;
			foreach (var systemBase in systems)
			{
				diContainer.Inject(systemBase);
			}
		}
	}
}