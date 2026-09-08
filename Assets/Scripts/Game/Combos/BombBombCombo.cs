using System.Collections.Generic;
using System.Linq;
using Game.Core.BoardBase;
using Game.Core.ComboBase;

namespace Game.Combos
{
    public class BombBombCombo : Combo
    {
        public BombBombCombo(List<Cell> cells) : base(cells) { }

        protected override List<Cell> GetComboBlastArea(Cell tappedCell)
        {
            IEnumerable<Cell> blastAreaCells = new List<Cell>();

            List<Cell> neighboursIncludingDiagonals = tappedCell.GetNeighboursIncludingDiagonals();
            foreach (Cell cell in neighboursIncludingDiagonals)
            {
                blastAreaCells = blastAreaCells.Union(cell.GetNeighboursIncludingDiagonals());
            }

            return blastAreaCells.ToList();
        }
    }
}