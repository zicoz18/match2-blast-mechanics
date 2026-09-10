using Game.Core.Enums;
using Game.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Game.Core.LevelBase
{
	public class LevelFlow : MonoBehaviour
	{
		private const float ReturnDelaySeconds = 1.2f;

		private LevelProgressManager _levelProgress;

		private void Start()
		{
			_levelProgress = ServiceProvider.GetLevelProgressManager;
			_levelProgress.OnLevelPlayStateChanged += HandleLevelPlayStateChanged;
		}

		private void OnDestroy()
		{
			// Genuinely reachable: OnDestroy can run without Start ever having, so this is
			// not the same unreachable check the other call sites had.
			if (_levelProgress == null) return;
			_levelProgress.OnLevelPlayStateChanged -= HandleLevelPlayStateChanged;
		}

		private void HandleLevelPlayStateChanged(
			object sender, LevelProgressManager.OnLevelPlayStateChangedEventArgs args)
		{
			if (args.LevelPlayState == LevelPlayState.Playing) return;

			if (args.LevelPlayState == LevelPlayState.Completed)
			{
				LevelProgress.Advance();
			}

			Invoke(nameof(ReturnToMainScene), ReturnDelaySeconds);
		}

		private void ReturnToMainScene()
		{
			SceneManager.LoadScene(SceneNames.Main);
		}
	}
}
