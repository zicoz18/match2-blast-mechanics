using System.Collections.Generic;
using Game.Core.BoardBase;
using UnityEngine;

namespace Game.Mechanics
{
	public class HintManager : MonoBehaviour
	{
		[SerializeField] private Board Board;
		private readonly MatchFinder _matchFinder = new MatchFinder();
		private readonly bool[,] _alreadyMatchedCells = new bool[Board.Cols, Board.Rows];
		private readonly List<Cell> _groupBuffer = new List<Cell>();

		private void Update()
		{
			ProcessHints();
		}

		private void ProcessHints()
		{
			ClearAlreadyMatchedCells();

			for (int y = 0; y < Board.Rows; y++)
			{
				for (int x = 0; x < Board.Cols; x++)
				{
					Cell cell = Board.Cells[x, y];
					if (!cell.HasItem()) continue;
					if (_alreadyMatchedCells[cell.X, cell.Y]) continue;

					_matchFinder.FindMatches(cell, cell.Item.GetMatchType(), _groupBuffer);
					if (_groupBuffer.Count == 0) continue;

					ApplyHintsToGroup();
				}
			}
		}

		private void ApplyHintsToGroup()
		{
			int groupSize = _groupBuffer.Count;

			for (int i = 0; i < groupSize; i++)
			{
				Cell cell = _groupBuffer[i];
				_alreadyMatchedCells[cell.X, cell.Y] = true;
				cell.Item.SetHint(groupSize);
			}
		}

		private void ClearAlreadyMatchedCells()
		{
			for (int x = 0; x < _alreadyMatchedCells.GetLength(0); x++)
			{
				for (int y = 0; y < _alreadyMatchedCells.GetLength(1); y++)
				{
					_alreadyMatchedCells[x, y] = false;
				}
			}
		}
	}
}
