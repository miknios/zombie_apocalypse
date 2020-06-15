using Unity.Entities;

namespace ECS_Logic
{
	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
	[UpdateBefore(typeof(EndInitializationEntityCommandBufferSystem))]
	public class ApplySelfContainedDataSystemGroup : ComponentSystemGroup { }
	
	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
	[UpdateAfter(typeof(ApplySelfContainedDataSystemGroup))]
	public class ContinousWorkProducerSystemGroup : ComponentSystemGroup { }

	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
	[UpdateAfter(typeof(BeginSimulationEntityCommandBufferSystem))]
	public class UpdateSimulationDataSystemGroup : ComponentSystemGroup { }

	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
	[UpdateAfter(typeof(UpdateSimulationDataSystemGroup))]
	public class ContextWideDataDependentSystemGroup : ComponentSystemGroup { }
	
	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderFirst = true)]
	public class CollisionDetectionSystemGroup : ComponentSystemGroup { }
	
	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(LateSimulationSystemGroup), OrderLast = true)]
	[UpdateAfter(typeof(CollisionDetectionSystemGroup))]
	public class CollisionDependentSystemGroup : ComponentSystemGroup { }
	
	[UnityEngine.ExecuteAlways]
	[UpdateInGroup(typeof(CollisionDependentSystemGroup), OrderLast = true)]
	public class CollisionDataAppendEntityCommandBufferSystem : EntityCommandBufferSystem { }
}