using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "ZombieApocalypse/Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
	[SerializeField] private int healthPoints = 40000;

	public int HealthPoints => healthPoints;
}