using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class BombItem : SpecialItem
    {

        protected override List<Cell> GetBlastArea()
        {
            return Cell.GetNeighboursIncludingDiagonals();
        }

        protected override Sprite GetSprite()
        {
            return ServiceProvider.GetImageLibrary.BombSprite;
        }
    }
}