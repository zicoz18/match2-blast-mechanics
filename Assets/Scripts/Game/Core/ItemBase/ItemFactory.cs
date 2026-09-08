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
                    item = CreateCubeItem(itemBase, MatchType.Green);
                    break;
                case ItemType.YellowCube:
                    item = CreateCubeItem(itemBase, MatchType.Yellow);
                    break;
                case ItemType.BlueCube:
                    item = CreateCubeItem(itemBase, MatchType.Blue);
                    break;
                case ItemType.RedCube:
                    item = CreateCubeItem(itemBase, MatchType.Red);
                    break;
                case ItemType.Crate:
                    item = CreateCrateItem(itemBase);
                    break;
                case ItemType.Balloon:
                    item = CreateBalloonItem(itemBase);
                    break;
                case ItemType.GreenBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Green);
                    break;
                case ItemType.YellowBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Yellow);
                    break;
                case ItemType.BlueBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Blue);
                    break;
                case ItemType.RedBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.Red);
                    break;
                case ItemType.VerticalRocket:
                    item = CreateVerticalRocketItem(itemBase);
                    break;
                case ItemType.HorizontalRocket:
                    item = CreateHorizontalRocketItem(itemBase);
                    break;
                case ItemType.Bomb:
                    item = CreateBombItem(itemBase);
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

        private Item CreateCubeItem(ItemBase itemBase, MatchType matchType)
        {
            var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
            cubeItem.PrepareCubeItem(itemBase, matchType);

            return cubeItem;
        }

        private Item CreateCrateItem(ItemBase itemBase)
        {
            var crateItem = itemBase.gameObject.AddComponent<CrateItem>();
            crateItem.PrepareCrateItem(itemBase);
            return crateItem;
        }

        // TODO: Think about a cleaner way to do this
        private Item CreateBalloonItem(ItemBase itemBase)
        {
            var balloonItem = itemBase.gameObject.AddComponent<BalloonItem>();
            balloonItem.PrepareBalloonItem(itemBase);
            return balloonItem;
        }

        private Item CreateColorBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            var colorBalloonItem = itemBase.gameObject.AddComponent<ColorBalloonItem>();
            colorBalloonItem.PrepareColorBalloonItem(itemBase, matchType);
            return colorBalloonItem;
        }

        private Item CreateBombItem(ItemBase itemBase)
        {
            var bombItem = itemBase.gameObject.AddComponent<BombItem>();
            bombItem.PrepareSpecialItem(itemBase);
            return bombItem;
        }

        private Item CreateVerticalRocketItem(ItemBase itemBase)
        {
            var verticalRocketItem = itemBase.gameObject.AddComponent<VerticalRocketItem>();
            verticalRocketItem.PrepareSpecialItem(itemBase);
            return verticalRocketItem;
        }

        private Item CreateHorizontalRocketItem(ItemBase itemBase)
        {
            var horizontalRocketItem = itemBase.gameObject.AddComponent<HorizontalRocketItem>();
            horizontalRocketItem.PrepareSpecialItem(itemBase);
            return horizontalRocketItem;
        }
    }
}