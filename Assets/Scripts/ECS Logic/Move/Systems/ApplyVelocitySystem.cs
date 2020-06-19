using Configuration;
using ECS_Logic.Move.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace ECS_Logic.Move.Systems
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	public class ApplyVelocitySystem : SystemBase
	{
		[BurstCompile]
		struct ApplyVelocityJob : IJobChunk
		{
			public float DeltaTime;
			public ArchetypeChunkComponentType<Translation> TranslationType;
			[ReadOnly] public ArchetypeChunkComponentType<CurrentVelocity> VelocityComponentType;
			[ReadOnly] public ArchetypeChunkComponentType<VelocityMultiplier> VelocityMultiplierComponentType;

			public void Execute(ArchetypeChunk chunk, int chunkIndex, int firstEntityIndex)
			{
				var chunkTranslations = chunk.GetNativeArray(TranslationType);
				var chunkVelocityComponents = chunk.GetNativeArray(VelocityComponentType);
				var chunkVelocityMultiplierComponents = chunk.GetNativeArray(VelocityMultiplierComponentType);

				bool chunkWithMultiplier = chunk.Has(VelocityMultiplierComponentType);

				for (int i = 0; i < chunk.Count; i++)
				{
					var translation = chunkTranslations[i];
					var velocity = chunkVelocityComponents[i];

					float multiplier = chunkWithMultiplier ? chunkVelocityMultiplierComponents[i].Value : 1;
					chunkTranslations[i] = new Translation
					{
						Value = translation.Value + velocity.Value * multiplier * DeltaTime
					};
				}
			}
		}

		private EntityQuery entityQuery;

		protected override void OnCreate()
		{
			entityQuery = GetEntityQuery(
				ComponentType.ReadWrite<Translation>(),
				ComponentType.ReadOnly<CurrentVelocity>());
		}

		protected override void OnUpdate()
		{
			var job = new ApplyVelocityJob
			{
				DeltaTime = Time.DeltaTime,
				TranslationType = GetArchetypeChunkComponentType<Translation>(),
				VelocityComponentType = GetArchetypeChunkComponentType<CurrentVelocity>(),
				VelocityMultiplierComponentType = GetArchetypeChunkComponentType<VelocityMultiplier>()
			};

			Dependency = job.ScheduleParallel(entityQuery, Dependency);
		}
	}
}