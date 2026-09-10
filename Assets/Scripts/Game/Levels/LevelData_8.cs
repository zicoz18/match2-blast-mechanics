using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.LevelBase;

namespace Game.Levels
{
    public class LevelData_8 : LevelData
    {
        public int Level_8_MoveLimit = 20;
        public Goal[] Level_8_Goals = { new Goal(GoalType.Crate, 4), new Goal(GoalType.Bomb, 3) };

        private readonly ItemType[] _itemArray =
        {
            ItemType.GreenCube,
            ItemType.YellowCube,
            ItemType.BlueCube,
            ItemType.RedCube,
            ItemType.Balloon
        };

        public override ItemType GetNextFillItemType()
        {
            return GetRandomItemType();
        }

        public override void Initialize()
        {
            GridData = new ItemType[Board.Rows, Board.Cols];

            GridData[1, 1] = ItemType.Crate;
            GridData[1, 7] = ItemType.Crate;

            GridData[7, 1] = ItemType.Crate;
            GridData[7, 7] = ItemType.Crate;

            GridData[4, 3] = ItemType.Bomb;
            GridData[4, 4] = ItemType.Bomb;
            GridData[4, 5] = ItemType.Bomb;

            for (var y = 0; y < Board.Rows; y++)
            {
                for (var x = 0; x < Board.Cols; x++)
                {
                    if (GridData[x, y] != ItemType.None) continue;
                    GridData[x, y] = GetRandomItemType();
                }
            }

            MoveLimit = Level_8_MoveLimit;
            Goals = Level_8_Goals;
        }

        private ItemType GetRandomItemType()
        {
            return GetRandomItemTypeFromArray(_itemArray);
        }
    }
}