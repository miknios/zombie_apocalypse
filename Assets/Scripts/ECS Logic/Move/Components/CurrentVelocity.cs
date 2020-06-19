using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Move.Components
{
	[GenerateAuthoringComponent]
	public struct CurrentVelocity : IComponentData
	{
		public float3 Value;
	}
}