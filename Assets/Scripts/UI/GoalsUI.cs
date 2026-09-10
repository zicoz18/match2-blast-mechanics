using System.Collections.Generic;
using UnityEngine;
using Game.Core.LevelBase;
using Game.Managers;

namespace UI
{
	public class GoalsUI : MonoBehaviour
	{
		[SerializeField] private GoalDisplay goalDisplayPrefab;

		private LevelProgressManager _levelProgress;
		private readonly List<GoalDisplay> _displays = new List<GoalDisplay>();

		private void Start()
		{
			_levelProgress = ServiceProvider.GetLevelProgressManager;
			if (_levelProgress == null) return;

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
		}

		private void Update()
		{
			if (_levelProgress == null) return;

			Goal[] goals = _levelProgress.GetGoals();
			for (int i = 0; i < _displays.Count; i++)
			{
				_displays[i].Refresh(goals[i]);
			}
		}
	}
}
