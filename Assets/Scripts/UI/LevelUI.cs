using TMPro;
using UnityEngine;
using Game.Managers;

namespace UI
{
	public class LevelUI : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI movesRemainingText;

		private LevelProgressManager _levelProgress;
		private int _shownMovesRemaining = -1;

		private void Start()
		{
			_levelProgress = ServiceProvider.GetLevelProgressManager;
		}

		private void Update()
		{
			if (_levelProgress == null) return;

			int movesRemaining = _levelProgress.GetMovesRemaining();
			if (movesRemaining == _shownMovesRemaining) return;

			_shownMovesRemaining = movesRemaining;
			movesRemainingText.text = movesRemaining.ToString();
		}
	}
}
