using Unity.Entities;
using Unity.Mathematics;

namespace ECS_Logic.Components
{
	public struct CursorWorldPosition : IComponentData
	{
		public float3 Value;
	}
}