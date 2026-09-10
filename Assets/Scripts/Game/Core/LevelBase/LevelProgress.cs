using System;
using Game.Core.Enums;
using UnityEngine;

namespace Game.Core.LevelBase
{
	public static class LevelProgress
	{
		private const string CurrentLevelKey = "CurrentLevel";

		private static readonly int LevelCount = Enum.GetValues(typeof(LevelName)).Length;

		public static LevelName Current
		{
			get
			{
				int stored = PlayerPrefs.GetInt(CurrentLevelKey, 0);

				return IsValidIndex(stored) ? (LevelName)stored : LevelName.Level0;
			}
		}

		public static void Advance()
		{
			Set((LevelName)(((int)Current + 1) % LevelCount));
		}

		public static void Set(LevelName level)
		{
			PlayerPrefs.SetInt(CurrentLevelKey, (int)level);

			PlayerPrefs.Save();
		}

		private static bool IsValidIndex(int index)
		{
			return index >= 0 && index < LevelCount;
		}
	}
}
