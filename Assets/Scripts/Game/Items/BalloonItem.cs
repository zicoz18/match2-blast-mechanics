using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;

namespace Game.Items
{
    public class BalloonItem : Item
    {
        public void PrepareBalloonItem(ItemBase itemBase, ItemType itemType)
        {
            Prepare(itemBase, ServiceProvider.GetImageLibrary.BalloonSprite, itemType);
        }

        public override void TryNeighbourExecute(MatchType _)
        {
            Damage();
        }
    }
}