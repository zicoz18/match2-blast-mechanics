using System;
using System.Collections.Generic;
using Game.Core.Enums;
using Game.Managers;
using Game.Mechanics;
using UnityEngine;

namespace Game.Core.BoardBase
{
	public class Board : MonoBehaviour
	{
		public const int Rows = 9;
		public const int Cols = 9;

		public const int MinimumMatchCount = 2;
		public const int MinimumComboCount = 2;
		public const int MatchCountToCreateRocket = 5;
		public const int MatchCountToCreateBomb = 7;
		private static readonly ItemType[] RocketItemTypeArray = new[] { ItemType.VerticalRocket, ItemType.HorizontalRocket };

		public Transform CellsParent;
		public Transform ItemsParent;
		public Transform ParticlesParent;

		[SerializeField] private Cell CellPrefab;

		public readonly Cell[,] Cells = new Cell[Cols, Rows];

		private readonly MatchFinder _matchFinder = new MatchFinder();
		private readonly MatchNeighbourFinder _matchNeighbourFinder = new MatchNeighbourFinder();

		private bool _canBeTapped = true;

		public int ChangeVersion { get; private set; }
		private void MarkChange()
		{
			ChangeVersion++;
		}

		private void ConsumeMove()
		{
			ServiceProvider.GetLevelProgressManager.TryDecrementMovesRemaining();
		}

		public void Prepare()
		{
			CreateCells();
			PrepareCells();
		}

		private void CreateCells()
		{
			for (var y = 0; y < Rows; y++)
			{
				for (var x = 0; x < Cols; x++)
				{
					var cell = Instantiate(CellPrefab, Vector3.zero, Quaternion.identity, CellsParent);
					Cells[x, y] = cell;
				}
			}
		}

		private void PrepareCells()
		{
			for (var y = 0; y < Rows; y++)
			{
				for (var x = 0; x < Cols; x++)
				{
					Cells[x, y].Prepare(x, y, this);
				}
			}
		}

		public void CellTapped(Cell cell)
		{
			if (!_canBeTapped) return;
			if (IsMoving()) return;
			if (cell == null) return;

			if (!cell.HasItem()) return;
			if (cell.Item.GetCanActivate())
			{
				List<Cell> matchingCells = FindMatchingCellsToExplode(cell);
				if (matchingCells.Count < MinimumComboCount)
				{
					cell.Item.Activate();
				}
				else
				{
					ExecuteCombo(matchingCells, cell);
				}
			}
			else
			{
				MatchType matchType = cell.Item.GetMatchType();
				List<Cell> matchingCells = FindMatchingCellsToExplode(cell);

				if (matchingCells.Count < MinimumMatchCount) return;
				List<Cell> neighbourCellsToMatchingCells = FindNeighbourCellsToMatchingCells(matchingCells);
				ApplyEffectToMatchingCells(matchingCells);
				ApplyEffectToNeighbourCells(neighbourCellsToMatchingCells, matchType);
				ApplyEffectToTappedCell(cell, matchingCells.Count);
			}
			MarkChange();
			ConsumeMove();
		}

		private void ExecuteCombo(List<Cell> matchingCells, Cell tappedCell)
		{
			ComboManager.ExecuteCombo(matchingCells, tappedCell);
		}

		private List<Cell> FindMatchingCellsToExplode(Cell cell)
		{
			return _matchFinder.FindMatches(cell, cell.Item.GetMatchType());
		}

		private List<Cell> FindNeighbourCellsToMatchingCells(List<Cell> matchingCells)
		{
			return _matchNeighbourFinder.FindMatchNeighbours(matchingCells);
		}

		private void ApplyEffectToMatchingCells(List<Cell> matchingCells)
		{
			for (var i = 0; i < matchingCells.Count; i++)
			{
				var matchingCell = matchingCells[i];
				var item = matchingCell.Item;
				if (item != null) item.TryMatchExecute();
			}
		}

		private void ApplyEffectToNeighbourCells(List<Cell> neighbourCellsToMatchingCells, MatchType matchType)
		{
			for (var i = 0; i < neighbourCellsToMatchingCells.Count; i++)
			{
				var neighbourCell = neighbourCellsToMatchingCells[i];
				var item = neighbourCell.Item;
				if (item != null) item.TryNeighbourExecute(matchType);
			}
		}

		protected static ItemType GetRandomRocketTypeFromArray()
		{
			return RocketItemTypeArray[UnityEngine.Random.Range(0, RocketItemTypeArray.Length)];
		}

		public static SpecialType GetSpecialTypeForMatchCount(int matchCount)
		{
			if (matchCount >= MatchCountToCreateBomb) return SpecialType.Bomb;
			if (matchCount >= MatchCountToCreateRocket) return SpecialType.Rocket;
			return SpecialType.None;
		}

		private void ApplyEffectToTappedCell(Cell cell, int matchCount)
		{
			switch (GetSpecialTypeForMatchCount(matchCount))
			{
				case SpecialType.Bomb:
					ServiceProvider.GetItemFactory.CreateItemAtCell(ItemType.Bomb, ItemsParent, cell);
					break;
				case SpecialType.Rocket:
					ServiceProvider.GetItemFactory.CreateItemAtCell(GetRandomRocketTypeFromArray(), ItemsParent, cell);
					break;
			}
		}

		public Cell GetNeighbourWithDirection(Cell cell, Direction direction)
		{
			var x = cell.X;
			var y = cell.Y;
			switch (direction)
			{
				case Direction.None:
					break;
				case Direction.Up:
					y += 1;
					break;
				case Direction.UpRight:
					y += 1;
					x += 1;
					break;
				case Direction.Right:
					x += 1;
					break;
				case Direction.DownRight:
					y -= 1;
					x += 1;
					break;
				case Direction.Down:
					y -= 1;
					break;
				case Direction.DownLeft:
					y -= 1;
					x -= 1;
					break;
				case Direction.Left:
					x -= 1;
					break;
				case Direction.UpLeft:
					y += 1;
					x -= 1;
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", direction, null);
			}

			if (x >= Cols || x < 0 || y >= Rows || y < 0) return null;

			return Cells[x, y];
		}

		public List<Cell> GetSquareArea(Cell centre, int radius)
		{
			List<Cell> area = new List<Cell>();

			for (int x = centre.X - radius; x <= centre.X + radius; x++)
			{
				for (int y = centre.Y - radius; y <= centre.Y + radius; y++)
				{
					if (x < 0 || x >= Cols || y < 0 || y >= Rows) continue;
					area.Add(Cells[x, y]);
				}
			}

			return area;
		}

		public List<Cell> GetRow(Cell cell)
		{
			int y = cell.Y;
			List<Cell> row = new List<Cell>();
			for (int x = 0; x < Cols; x++)
			{
				row.Add(Cells[x, y]);
			}
			return row;
		}

		public List<Cell> GetColumn(Cell cell)
		{
			int x = cell.X;
			List<Cell> column = new List<Cell>();
			for (int y = 0; y < Rows; y++)
			{
				column.Add(Cells[x, y]);
			}
			return column;
		}

		public bool IsMoving()
		{
			for (var y = 0; y < Rows; y++)
			{
				for (var x = 0; x < Cols; x++)
				{
					Cell cell = Cells[x, y];
					if (!cell.HasItem()) continue;
					if (cell.Item.IsFalling()) return true;
				}
			}
			return false;
		}

		public void SetCanBeTapped(bool canBeTapped)
		{
			_canBeTapped = canBeTapped;
		}
	}
}
