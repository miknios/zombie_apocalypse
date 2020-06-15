using ECS_Logic.DataTrack;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
	public class EcsEventsPropagator : MonoBehaviour
	{
		private IKilledEnemiesCountListener[] killedEnemiesCountListeners;
		
		private void Awake()
		{
			killedEnemiesCountListeners = GetComponentsInChildren<IKilledEnemiesCountListener>();
			var eventInvokeSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<KilledEnemiesCountEventInvokeSystem>();
			eventInvokeSystem.KilledEnemiesCountChanged += OnKilledEnemiesCountChanged;
		}

		private void OnKilledEnemiesCountChanged(int newKilledEnemiesCount)
		{
			foreach (var killedEnemiesCountListener in killedEnemiesCountListeners)
			{
				killedEnemiesCountListener.OnKilledEnemiesCountChanged(newKilledEnemiesCount);
			}
		}
	}
}