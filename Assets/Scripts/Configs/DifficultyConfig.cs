using UnityEngine;

[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "ZombieApocalypse/Configs/DifficultyConfig")]
public class DifficultyConfig : ScriptableObject
{
	[SerializeField] private SpawnerConfig spawnerConfig = null;
	[SerializeField] private PlayerConfig playerConfig = null;
	
	public SpawnerConfig SpawnerConfig => spawnerConfig;
	public PlayerConfig PlayerConfig => playerConfig;
}