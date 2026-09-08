using System.Collections.Generic;
using System.Linq;
using Game.Core.BoardBase;
using Game.Core.ComboBase;

namespace Game.Combos
{
    public class RocketRocketCombo : Combo
    {
        public RocketRocketCombo(List<Cell> cells) : base(cells) { }

        protected override List<Cell> GetComboBlastArea(Cell tappedCell)
        {
            List<Cell> cellsRow = tappedCell.Board.GetRow(tappedCell);
            List<Cell> cellsColumn = tappedCell.Board.GetColumn(tappedCell);
            return cellsRow.Union(cellsColumn).ToList();
        }
    }
}