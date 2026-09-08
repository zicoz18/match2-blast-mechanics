using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;

namespace Game.Items
{
    public class BalloonItem : Item
    {
        public void PrepareBalloonItem(ItemBase itemBase)
        {
            Prepare(itemBase, ServiceProvider.GetImageLibrary.BalloonSprite);
        }

        public override void TryNeighbourExecute(MatchType _)
        {
            Damage();
        }
    }
}