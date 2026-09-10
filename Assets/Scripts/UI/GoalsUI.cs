using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Core.LevelBase;
using Game.Managers;

namespace UI
{
	public class GoalsUI : MonoBehaviour
	{
		private const float GoalDisplayDesignSize = 140f;

		private const float HeightFraction = 0.7f;

		private const float SpacingFraction = 0.2f;

		[SerializeField] private GoalDisplay goalDisplayPrefab;

		private LevelProgressManager _levelProgress;
		private readonly List<GoalDisplay> _displays = new List<GoalDisplay>();
		private RectTransform _rectTransform;
		private HorizontalLayoutGroup _layoutGroup;
		private float _scaledForHeight = -1f;

		private void Awake()
		{
			_rectTransform = (RectTransform)transform;
			_layoutGroup = GetComponent<HorizontalLayoutGroup>();
		}

		private void Start()
		{
			_levelProgress = ServiceProvider.GetLevelProgressManager;
			CreateDisplays(_levelProgress.GetGoals());
		}

		// One display per declared goal, so a level with one, two or five all work without
		// the UI being told how many to expect. The HorizontalLayoutGroup spaces them.
		private void CreateDisplays(Goal[] goals)
		{
			foreach (Goal goal in goals)
			{
				GoalDisplay display = Instantiate(goalDisplayPrefab, transform);
				display.Bind(goal);
				_displays.Add(display);
			}

			_scaledForHeight = -1f;
		}

		private void Update()
		{
			ScaleDisplaysToPanel();

			Goal[] goals = _levelProgress.GetGoals();
			for (int i = 0; i < _displays.Count; i++)
			{
				_displays[i].Refresh(goals[i]);
			}
		}

		private void ScaleDisplaysToPanel()
		{
			float height = _rectTransform.rect.height;
			if (Mathf.Approximately(height, _scaledForHeight)) return;

			_scaledForHeight = height;
			float scale = height * HeightFraction / GoalDisplayDesignSize;

			for (int i = 0; i < _displays.Count; i++)
			{
				_displays[i].transform.localScale = new Vector3(scale, scale, 1f);
			}

			_layoutGroup.spacing = height * SpacingFraction;
		}
	}
}
