using Unity.Entities;
using Zenject;

namespace DefaultNamespace
{
	public class WorldDisposer : IInitializable
	{
		public void Initialize()
		{
			World.DisposeAllWorlds();
		}
	}
}