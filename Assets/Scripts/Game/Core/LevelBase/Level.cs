using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Managers;
using Game.Mechanics;
using UnityEngine;

namespace Game.Core.LevelBase
{
	public class Level : MonoBehaviour
	{
#if UNITY_EDITOR
		[Header("Editor only. Lets you open LevelScene directly on a chosen level")]
		[SerializeField] private bool useOverrideLevel;
		[SerializeField] private LevelName overrideLevel;
#endif

		private LevelName CurrentLevel
		{
			get
			{
#if UNITY_EDITOR
				if (useOverrideLevel) return overrideLevel;
#endif
				return LevelProgress.Current;
			}
		}

		[SerializeField] private Board Board;
		[SerializeField] private FallAndFillManager FallAndFillManager;

		private LevelData _levelData;
		private LevelProgressManager _levelProgressManager;

		private void Awake()
		{
			PrepareBoard();
			PrepareLevel();
			PrepareGoals();
			StartFalls();
		}

		private void PrepareBoard()
		{
			Board.Prepare();
		}

		private void Update()
		{
			_levelProgressManager.CheckProgression();
		}

		private void PrepareLevel()
		{
			if (_levelData == null) _levelData = LevelDataFactory.CreateLevelData(CurrentLevel);

			for (var y = 0; y < _levelData.GridData.GetLength(0); y++)
			{
				for (var x = 0; x < _levelData.GridData.GetLength(1); x++)
				{
					var cell = Board.Cells[x, y];

					var itemType = _levelData.GridData[x, y];
					ServiceProvider.GetItemFactory.CreateItemAtCell(itemType, Board.ItemsParent, cell);
				}
			}
		}

		private void PrepareGoals()
		{
			if (_levelData == null) _levelData = LevelDataFactory.CreateLevelData(CurrentLevel);
			if (_levelProgressManager == null)
			{
				_levelProgressManager = new LevelProgressManager();
				ServiceProvider.Register(_levelProgressManager);
			}
			_levelProgressManager.Prepare(_levelData.Goals, _levelData.MoveLimit, Board);
		}

		private void StartFalls()
		{
			FallAndFillManager.Init(Board, _levelData);
			FallAndFillManager.StartFalls();
		}
	}
}
