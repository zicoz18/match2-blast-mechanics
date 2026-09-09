using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class CrateItem : Item
    {
        private Sprite[] _sprites;

        public void PrepareCrateItem(ItemBase itemBase, ItemType itemType)
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;
            _sprites = new[] { imageLibrary.CrateLayer1Sprite, imageLibrary.CrateLayer2Sprite };

            Health = _sprites.Length;
            Prepare(itemBase, GetSpriteForHealth(Health), itemType);
            CanFall = false;
        }

        public override void TryNeighbourExecute(MatchType _)
        {
            Damage();
        }

        public override void OnNonLethalDamage()
        {
            UpdateSprite(GetSpriteForHealth(Health));
        }

        private Sprite GetSpriteForHealth(int health)
        {
            // Since there is no sprite for health 0, we gotta decrement to get the real sprite's index
            return _sprites[health - 1];
        }
    }
}
