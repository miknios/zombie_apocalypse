using ECS_Logic.Common.Move.Components;
using ECS_Logic.Weapons.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace ECS_Logic.Weapons.Systems
{
	[UpdateInGroup(typeof(ContinousWorkProducerSystemGroup))]
	public class GunShootSystem : SystemBase
	{
		private EntityCommandBufferSystem commandBufferSystem;

		protected override void OnCreate()
		{
			commandBufferSystem = World.DefaultGameObjectInjectionWorld
				.GetOrCreateSystem<EndInitializationEntityCommandBufferSystem>();
		}

		protected override void OnUpdate()
		{
			var commandBuffer = commandBufferSystem.CreateCommandBuffer();
			
			Entities
				.ForEach((in Gun gun, in LocalToWorld localToWorld) =>
				{
					if (!Input.GetKeyDown(gun.KeyCode)) 
						return;
					
					Entity projectileEntity = commandBuffer.Instantiate(gun.ProjectileEntity);
					commandBuffer.SetComponent(projectileEntity, new Translation{Value = localToWorld.Position});

					float3 velocity = localToWorld.Forward * gun.ProjectileSpeed;
					commandBuffer.AddComponent(projectileEntity, new CurrentVelocity{Value = velocity});
				})
				.Run();
			
			commandBufferSystem.AddJobHandleForProducer(Dependency);
		}
	}
}