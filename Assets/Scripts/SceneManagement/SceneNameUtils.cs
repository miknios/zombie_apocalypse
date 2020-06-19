using System;

namespace SceneManagement
{
	public static class SceneNameUtils
	{
		public enum Scene
		{
			Gameplay,
			Menu
		}

		private const string GAMEPLAY_SCENE = "GameplayScene";
		private const string MENU_SCENE = "MainMenuScene";

		public static string SceneNameForEnum(Scene scene)
		{
			switch (scene)
			{
				case Scene.Gameplay:
					return GAMEPLAY_SCENE;
				case Scene.Menu:
					return MENU_SCENE;
				default:
					throw new Exception("Scene not found in SceneNameUtils!");
			}
		}
	}
}