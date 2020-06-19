using Unity.Entities;
using Zenject;

namespace ECS_Configuration
{
	public class WorldDisposer : IInitializable
	{
		public void Initialize()
		{
			World.DisposeAllWorlds();
		}
	}
}