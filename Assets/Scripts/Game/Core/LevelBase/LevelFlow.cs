using System.Collections;
using Game.Core.Enums;
using Game.Managers;
using UI;
using UnityEngine;

namespace Game.Core.LevelBase
{
	public class LevelFlow : MonoBehaviour
	{
		private const float ShowResultDelaySeconds = 0.8f;

		[SerializeField] private ResultPanel resultPanel;

		private LevelProgressManager _levelProgress;

		private void Start()
		{
			_levelProgress = ServiceProvider.GetLevelProgressManager;
			_levelProgress.OnLevelPlayStateChanged += HandleLevelPlayStateChanged;
		}

		private void OnDestroy()
		{
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

			StartCoroutine(ShowResultAfterDelay(args.LevelPlayState));
		}

		private IEnumerator ShowResultAfterDelay(LevelPlayState state)
		{
			yield return new WaitForSeconds(ShowResultDelaySeconds);
			resultPanel.Show(state);
		}
	}
}
