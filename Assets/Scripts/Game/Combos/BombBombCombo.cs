using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.ComboBase;

namespace Game.Combos
{
    public class BombBombCombo : Combo
    {
        private const int BlastRadius = 3;

        public BombBombCombo(List<Cell> cells) : base(cells) { }

        protected override List<Cell> GetComboBlastArea(Cell tappedCell)
        {
            return tappedCell.Board.GetSquareArea(tappedCell, BlastRadius);
        }
    }
}
