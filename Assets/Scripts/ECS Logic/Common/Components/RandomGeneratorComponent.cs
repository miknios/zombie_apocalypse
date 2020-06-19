using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Common.Components
{
	public struct RandomGeneratorComponent : IComponentData
	{
		public Random Value;
	}
}