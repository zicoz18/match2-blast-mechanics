using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.LevelBase;
using Game.Managers;

namespace UI
{
	public class GoalDisplay : MonoBehaviour
	{
		private const string CompletedGlyph = "√";

		private static readonly Color CompletedTint = new Color(0.6f, 0.6f, 0.6f, 1f);

		[SerializeField] private Image iconImage;
		[SerializeField] private TextMeshProUGUI countText;

		private Color _defaultTint;
		private int _shownCount = -1;

		private void Awake()
		{
			_defaultTint = iconImage.color;
		}

		public void Bind(Goal goal)
		{
			iconImage.sprite = ServiceProvider.GetImageLibrary.GetSpriteForGoalType(goal.Type);
			Refresh(goal);
		}

		public void Refresh(Goal goal)
		{
			if (goal.Count == _shownCount) return;

			_shownCount = goal.Count;

			if (!goal.IsComplete())
			{
				iconImage.color = _defaultTint;
				countText.text = goal.Count.ToString();
				return;
			}

			iconImage.color = CompletedTint;
			countText.text = CompletedGlyph;
		}
	}
}
