using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace
{
	public struct CursorWorldPosition : IComponentData
	{
		public float3 Value;
	}
}