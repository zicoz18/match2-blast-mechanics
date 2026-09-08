using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class CubeItem : Item
    {
        private MatchType _matchType;

        public void PrepareCubeItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            Prepare(itemBase, ServiceProvider.GetImageLibrary.GetSpriteForCubeItem(matchType));
        }

        public override MatchType GetMatchType()
        {
            return _matchType;
        }

        public override void OnDeath()
        {
            ServiceProvider.GetParticleManager.PlayCubeParticle(this);
            base.OnDeath();
        }

        public override void SetHint(int groupCount)
        {
            SpecialType specialType = Board.GetSpecialTypeForMatchCount(groupCount);
            UpdateSprite(ServiceProvider.GetImageLibrary.GetSpriteForCubeItem(GetMatchType(), specialType));
        }

    }
}
