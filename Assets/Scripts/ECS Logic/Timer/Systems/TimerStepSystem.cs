﻿using DefaultNamespace.ECS_Logic.Common.Components;
using DefaultNamespace.ECS_Logic.Timer.Components;
using ECS_Logic;
using Unity.Entities;
using Unity.Mathematics;

namespace DefaultNamespace
{
	[UpdateInGroup(typeof(ApplySelfContainedDataSystemGroup))]
	public class TimerStepSystem : SystemBase
	{
		protected override void OnUpdate()
		{
			float deltaTime = Time.DeltaTime;

			Entities
				.ForEach((int entityInQueryIndex, ref Timer timerComponent, 
					in Enabled enabledComponent) =>
				{
					float newCurrentTime = timerComponent.CurrentTime - deltaTime;
					newCurrentTime = math.max(0, newCurrentTime);
					timerComponent.CurrentTime = newCurrentTime;
				})
				.ScheduleParallel();
		}
	}
}