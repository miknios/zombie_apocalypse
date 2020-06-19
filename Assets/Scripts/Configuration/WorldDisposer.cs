using Unity.Entities;
using Zenject;

namespace Configuration
{
	public class WorldDisposer : IInitializable
	{
		public void Initialize()
		{
			World.DisposeAllWorlds();
		}
	}
}