using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class ColorBalloonItem : Item
    {
        private MatchType _matchType;
        public void PrepareColorBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            Prepare(itemBase, ServiceProvider.GetImageLibrary.GetSpriteForColorBalloonItem(matchType));
        }

        public override void TryNeighbourExecute(MatchType matchType)
        {
            if (matchType == _matchType)
            {
                Damage();
            }
        }
    }
}