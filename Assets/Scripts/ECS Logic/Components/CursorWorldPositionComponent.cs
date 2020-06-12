using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace
{
	public struct CursorWorldPositionComponent : IComponentData
	{
		public float3 Value;
	}
}