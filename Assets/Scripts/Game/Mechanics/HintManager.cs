using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Managers;
using UnityEngine;

namespace Game.Mechanics
{
	public class HintManager : MonoBehaviour
	{
		[SerializeField] private Board Board;
		private readonly MatchFinder _matchFinder = new MatchFinder();

		private readonly bool[,] _alreadyMatchedCells = new bool[Board.Rows, Board.Cols];
		private int _lastSeenVersion = -1;

		// Update is called once per frame
		private void Update()
		{
			if (Board.IsMoving()) return;
			if (_lastSeenVersion == Board.ChangeVersion) return;
			_lastSeenVersion = Board.ChangeVersion;
			ProcessHints();
		}

		private void ClearAlreadyMatchedCells()
		{
			for (var x = 0; x < _alreadyMatchedCells.GetLength(0); x++)
			{
				for (var y = 0; y < _alreadyMatchedCells.GetLength(1); y++)
				{
					_alreadyMatchedCells[x, y] = false;
				}
			}
		}

		private void ProcessHints()
		{
			ClearAlreadyMatchedCells();
			int matchCountToDefineAGroup = 1;
			List<List<Cell>> matchingGroups = FindMatchesWithAtLeastMatchCount(matchCountToDefineAGroup);
			foreach (List<Cell> matchingGroup in matchingGroups)
			{
				foreach (Cell cell in matchingGroup)
				{
					cell.Item.SetHint(matchingGroup.Count);
				}
			}
		}


		private List<List<Cell>> FindMatchesWithAtLeastMatchCount(int matchCount)
		{
			List<List<Cell>> matchingGroups = new List<List<Cell>>();
			for (var y = 0; y < Board.Rows; y++)
			{
				for (var x = 0; x < Board.Cols; x++)
				{
					var cell = Board.Cells[x, y];
					if (!cell.HasItem()) continue;
					if (_alreadyMatchedCells[cell.X, cell.Y]) continue;
					List<Cell> matchedCells = _matchFinder.FindMatches(cell, cell.Item.GetMatchType());
					foreach (Cell matchedCell in matchedCells)
					{
						_alreadyMatchedCells[matchedCell.X, matchedCell.Y] = true;
					}
					if (matchedCells.Count >= matchCount)
					{
						matchingGroups.Add(matchedCells);
					}
				}
			}
			return matchingGroups;
		}
	}
}
