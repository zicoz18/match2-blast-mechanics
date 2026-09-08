
using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.ComboBase;
using Game.Core.Enums;
using UnityEngine;

namespace Game.Combos
{
    public static class ComboFactory
    {
        public static Combo CreateCombo(ComboType comboType, List<Cell> matchingCells)
        {
            if (comboType == ComboType.None)
            {
                return null;
            }

            Combo combo = null;
            switch (comboType)
            {
                case ComboType.BombBomb:
                    combo = new BombBombCombo(matchingCells);
                    break;
                case ComboType.RocketBomb:
                    combo = new RocketBombCombo(matchingCells);
                    break;
                case ComboType.RocketRocket:
                    combo = new RocketRocketCombo(matchingCells);
                    break;
                default:
                    Debug.LogWarning("Can not create combo: " + comboType);
                    break;
            }

            return combo;
        }
    }
}