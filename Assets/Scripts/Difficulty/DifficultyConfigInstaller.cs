﻿using Common;
using Configs;
using UnityEngine;
using Zenject;

namespace Difficulty
{
	[CreateAssetMenu(fileName = "DifficultyConfigInstaller",
		menuName = "ZombieApocalypse/Configs/DifficultyConfigInstaller")]
	public class DifficultyConfigInstaller : ScriptableObjectInstaller
	{
		[SerializeField] private DifficultyConfig easyConfig = null;
		[SerializeField] private DifficultyConfig mediumConfig = null;
		[SerializeField] private DifficultyConfig hardConfig = null;

		public override void InstallBindings()
		{
			DifficultyConfig difficultyConfig = GetSavedDifficultyConfig();
			Container.BindInstance(difficultyConfig.PlayerConfig);
			Container.BindInstance(difficultyConfig.SpawnerConfig);
		}

		private DifficultyConfig GetSavedDifficultyConfig()
		{
			var savedSetting = GetSavedDifficultySetting();
			return GetDifficultyConfigForSetting(savedSetting);
		}

		private DifficultySetting GetSavedDifficultySetting()
		{
			if (!PlayerPrefs.HasKey(PlayerPrefsKey.DIFFICULTY_SETTING))
				return DifficultySetting.Medium;

			return (DifficultySetting) PlayerPrefs.GetInt(PlayerPrefsKey.DIFFICULTY_SETTING);
		}

		private DifficultyConfig GetDifficultyConfigForSetting(DifficultySetting difficultySetting)
		{
			switch (difficultySetting)
			{
				case DifficultySetting.Easy:
					return easyConfig;
				case DifficultySetting.Medium:
					return mediumConfig;
				case DifficultySetting.Hard:
					return hardConfig;
				default:
					return mediumConfig;
			}
		}
	}
}