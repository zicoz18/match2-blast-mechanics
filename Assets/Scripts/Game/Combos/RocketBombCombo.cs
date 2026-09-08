using System;
using System.Collections.Generic;
using System.Linq;
using Game.Core.BoardBase;
using Game.Core.ComboBase;
using Game.Core.Enums;

namespace Game.Combos
{
    public class RocketBombCombo : Combo
    {
        public RocketBombCombo(List<Cell> cells) : base(cells) { }

        protected override List<Cell> GetComboBlastArea(Cell tappedCell)
        {
            IEnumerable<Cell> blastAreaCells = new List<Cell>();

            blastAreaCells = AddLineOfCells(tappedCell, Direction.None, tappedCell.Board.GetRow, blastAreaCells);
            blastAreaCells = AddLineOfCells(tappedCell, Direction.Up, tappedCell.Board.GetRow, blastAreaCells);
            blastAreaCells = AddLineOfCells(tappedCell, Direction.Down, tappedCell.Board.GetRow, blastAreaCells);

            blastAreaCells = AddLineOfCells(tappedCell, Direction.None, tappedCell.Board.GetColumn, blastAreaCells);
            blastAreaCells = AddLineOfCells(tappedCell, Direction.Right, tappedCell.Board.GetColumn, blastAreaCells);
            blastAreaCells = AddLineOfCells(tappedCell, Direction.Left, tappedCell.Board.GetColumn, blastAreaCells);

            return blastAreaCells.ToList();
        }

        private IEnumerable<Cell> AddLineOfCells(Cell tappedCell, Direction direction, Func<Cell, List<Cell>> getLine, IEnumerable<Cell> blastAreaCells)
        {
            Cell cellOnDirection = tappedCell.Board.GetNeighbourWithDirection(tappedCell, direction);
            if (cellOnDirection != null)
            {
                List<Cell> cellDirectionsRowOrColumn;
                cellDirectionsRowOrColumn = getLine(cellOnDirection);
                blastAreaCells = blastAreaCells.Union(cellDirectionsRowOrColumn);
            }
            return blastAreaCells;
        }
    }
}
