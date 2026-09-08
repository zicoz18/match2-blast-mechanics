using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.ComboBase;
using Game.Combos;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Items;

namespace Game.Mechanics
{
    public static class ComboManager
    {
        private const int BombBombComboRequiredBombCount = 2;
        private const int RocketRocketComboRequiredRocketCount = 2;
        private const int BombRocketComboRequiredBothCount = 1;

        private static ComboType GetComboType(List<Cell> matchingCells)
        {
            int rocketCount = 0;
            int bombCount = 0;
            foreach (Cell cell in matchingCells)
            {
                if (!cell.HasItem()) continue;
                Item cellItem = cell.Item;
                if (cellItem is VerticalRocketItem || cellItem is HorizontalRocketItem)
                {
                    rocketCount++;
                }
                else if (cellItem is BombItem)
                {
                    bombCount++;
                }
            }
            if (bombCount >= BombBombComboRequiredBombCount)
            {
                return ComboType.BombBomb;
            }
            else if (bombCount >= BombRocketComboRequiredBothCount && rocketCount >= BombRocketComboRequiredBothCount)
            {
                return ComboType.RocketBomb;
            }
            else if (rocketCount >= RocketRocketComboRequiredRocketCount)
            {
                return ComboType.RocketRocket;
            }
            else
            {
                return ComboType.None;
            }
        }

        public static void ExecuteCombo(List<Cell> matchingCells, Cell tappedCell)
        {
            if (!tappedCell.HasItem()) return;
            ComboType comboType = GetComboType(matchingCells);
            Combo combo = ComboFactory.CreateCombo(comboType, matchingCells);
            if (combo != null)
            {
                combo.ExecuteCombo(tappedCell);
            }
        }
    }
}
