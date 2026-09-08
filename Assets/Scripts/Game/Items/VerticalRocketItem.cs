using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class VerticalRocketItem : SpecialItem
    {

        protected override List<Cell> GetBlastArea()
        {
            return Cell.Board.GetColumn(Cell);

        }

        protected override Sprite GetSprite()
        {
            return ServiceProvider.GetImageLibrary.VerticalRocketSprite;
        }
    }
}