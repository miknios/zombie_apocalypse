using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Common.Move.Components
{
	[GenerateAuthoringComponent]
	public struct CurrentVelocity : IComponentData
	{
		public float3 Value;
	}
}