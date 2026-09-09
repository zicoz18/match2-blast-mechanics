using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Items;
using Game.Managers;
using UnityEngine;

namespace Game.Core.ItemBase
{
    public class ItemFactory : MonoBehaviour, IProvidable
    {
        [SerializeField] private ItemBase ItemBasePrefab;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Item CreateItem(ItemType itemType, Transform parent)
        {
            if (itemType == ItemType.None)
            {
                return null;
            }

            var itemBase = Instantiate(ItemBasePrefab, Vector3.zero, Quaternion.identity, parent);

            Item item = null;
            switch (itemType)
            {
                case ItemType.GreenCube:
                    item = CreateCubeItem(itemBase, MatchType.Green, itemType);
                    break;
                case ItemType.YellowCube:
                    item = CreateCubeItem(itemBase, MatchType.Yellow, itemType);
                    break;
                case ItemType.BlueCube:
                    item = CreateCubeItem(itemBase, MatchType.Blue, itemType);
                    break;
                case ItemType.RedCube:
                    item = CreateCubeItem(itemBase, MatchType.Red, itemType);
                    break;
                case ItemType.Crate:
                    item = CreateCrateItem(itemBase, itemType);
                    break;
                case ItemType.Balloon:
                    item = CreateBalloonItem(itemBase, itemType);
                    break;
                case ItemType.GreenBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Green, itemType);
                    break;
                case ItemType.YellowBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Yellow, itemType);
                    break;
                case ItemType.BlueBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Blue, itemType);
                    break;
                case ItemType.RedBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Red, itemType);
                    break;
                case ItemType.VerticalRocket:
                    item = CreateVerticalRocketItem(itemBase, itemType);
                    break;
                case ItemType.HorizontalRocket:
                    item = CreateHorizontalRocketItem(itemBase, itemType);
                    break;
                case ItemType.Bomb:
                    item = CreateBombItem(itemBase, itemType);
                    break;
                default:
                    Debug.LogWarning("Can not create item: " + itemType);
                    break;
            }

            return item;
        }

        public Item CreateItemAtCell(ItemType itemType, Transform parent, Cell cell)
        {
            var item = CreateItem(itemType, parent);
            if (item == null) return null;
            cell.Item = item;
            item.transform.position = cell.transform.position;
            return item;
        }

        private Item CreateCubeItem(ItemBase itemBase, MatchType matchType, ItemType itemType)
        {
            var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
            cubeItem.PrepareCubeItem(itemBase, matchType, itemType);

            return cubeItem;
        }

        private Item CreateCrateItem(ItemBase itemBase, ItemType itemType)
        {
            var crateItem = itemBase.gameObject.AddComponent<CrateItem>();
            crateItem.PrepareCrateItem(itemBase, itemType);
            return crateItem;
        }

        private Item CreateBalloonItem(ItemBase itemBase, ItemType itemType)
        {
            var balloonItem = itemBase.gameObject.AddComponent<BalloonItem>();
            balloonItem.PrepareBalloonItem(itemBase, itemType);
            return balloonItem;
        }

        private Item CreateColorBalloonItem(ItemBase itemBase, MatchType matchType, ItemType itemType)
        {
            var colorBalloonItem = itemBase.gameObject.AddComponent<ColorBalloonItem>();
            colorBalloonItem.PrepareColorBalloonItem(itemBase, matchType, itemType);
            return colorBalloonItem;
        }

        private Item CreateBombItem(ItemBase itemBase, ItemType itemType)
        {
            var bombItem = itemBase.gameObject.AddComponent<BombItem>();
            bombItem.PrepareSpecialItem(itemBase, itemType);
            return bombItem;
        }

        private Item CreateVerticalRocketItem(ItemBase itemBase, ItemType itemType)
        {
            var verticalRocketItem = itemBase.gameObject.AddComponent<VerticalRocketItem>();
            verticalRocketItem.PrepareSpecialItem(itemBase, itemType);
            return verticalRocketItem;
        }

        private Item CreateHorizontalRocketItem(ItemBase itemBase, ItemType itemType)
        {
            var horizontalRocketItem = itemBase.gameObject.AddComponent<HorizontalRocketItem>();
            horizontalRocketItem.PrepareSpecialItem(itemBase, itemType);
            return horizontalRocketItem;
        }
    }
}