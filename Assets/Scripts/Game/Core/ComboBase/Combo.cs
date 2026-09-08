
using System.Collections.Generic;
using Game.Core.BoardBase;

namespace Game.Core.ComboBase
{
    public abstract class Combo
    {
        private List<Cell> _matchingCells;
        protected abstract List<Cell> GetComboBlastArea(Cell tappedCell);

        public Combo(List<Cell> cells)
        {
            _matchingCells = cells;
        }

        public void ExecuteCombo(Cell tappedCell)
        {
            List<Cell> comboBlastArea = GetComboBlastArea(tappedCell);
            // First Damage() each SpecialItem (not activate or anything)
            foreach (Cell cell in _matchingCells)
            {
                if (cell.HasItem()) cell.Item.Damage();
            }
            // Then blast the items in the comboBlastArea
            foreach (var cell in comboBlastArea)
            {
                if (cell.HasItem()) cell.Item.TryBlastExecute();
            }
        }
    }
}