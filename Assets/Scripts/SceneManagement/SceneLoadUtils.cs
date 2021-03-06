﻿using Unity.Entities;
using UnityEngine.SceneManagement;

namespace SceneManagement
{
	public static class SceneLoadUtils
	{
		public static void LoadGameplayScene()
		{
			DefaultWorldInitialization.Initialize("Default World", false);
			SceneManager.LoadScene(SceneNameUtils.SceneNameForEnum(SceneNameUtils.Scene.Gameplay));
		}
	}
}