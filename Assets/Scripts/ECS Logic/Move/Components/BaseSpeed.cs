using Unity.Entities;

namespace ECS_Logic.Move.Components
{
	[GenerateAuthoringComponent]
	public struct BaseSpeed : IComponentData
	{
		public float Value;
	}
}