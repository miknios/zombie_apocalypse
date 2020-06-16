using Unity.Entities;
using Zenject;

namespace DefaultNamespace
{
	public class WorldInitialization : IInitializable
	{
		[Inject] private DiContainer diContainer;
		
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