using Unity.Entities;

namespace ECS_Logic.Common.Move.Components
{
	[GenerateAuthoringComponent]
	public struct BaseSpeed : IComponentData
	{
		public float Value;
	}
}