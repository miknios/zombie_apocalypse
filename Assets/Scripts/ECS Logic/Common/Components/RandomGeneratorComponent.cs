using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace.ECS_Logic.Common.Components
{
	public struct RandomGeneratorComponent : IComponentData
	{
		public Random Value;
	}
}