using DefaultNamespace;
using DefaultNamespace.ECS_Logic.Timer.Components;
using Unity.Entities;

namespace Unity.Mathematics
{
	[UpdateBefore(typeof(TimerAutoRestartSystem))]
	public class AroundCenterCyclicSpawnerSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			base.OnCreate();

			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent();

			Entities
				.WithAll<TimeOutComponent>()
				.ForEach((Entity entity, int entityInQueryIndex, in TimerComponent timerComponent) =>
				{
					var spawner = GetComponent<AroundCenterCyclicSpawnerComponent>(timerComponent.Owner);
					commandBuffer.Instantiate(entityInQueryIndex, spawner.EntityToSpawn);
				})
				.ScheduleParallel();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}