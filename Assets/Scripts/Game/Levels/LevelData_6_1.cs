using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.LevelBase;

namespace Game.Levels
{
    public class LevelData_6_1 : LevelData
    {
        public int Level_6_1_MoveLimit = 20;
        public Goal[] Level_6_1_Goals = { new Goal(GoalType.AnyRocket, 5), new Goal(GoalType.Bomb, 4) };

        public override ItemType GetNextFillItemType()
        {
            return GetRandomCubeItemType();
        }

        public override void Initialize()
        {
            GridData = new ItemType[Board.Rows, Board.Cols];

            GridData[4, 7] = ItemType.HorizontalRocket;
            GridData[4, 6] = ItemType.Bomb;

            GridData[1, 2] = ItemType.VerticalRocket;
            GridData[2, 2] = ItemType.VerticalRocket;

            GridData[6, 1] = ItemType.Bomb;
            GridData[7, 1] = ItemType.Bomb;

            for (var y = 0; y < Board.Rows; y++)
            {
                for (var x = 0; x < Board.Cols; x++)
                {
                    if (GridData[x, y] != ItemType.None) continue;
                    GridData[x, y] = GetRandomCubeItemType();
                }
            }

            MoveLimit = Level_6_1_MoveLimit;
            Goals = Level_6_1_Goals;
        }
    }
}