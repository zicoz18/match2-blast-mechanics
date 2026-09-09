using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.LevelBase;

namespace Game.Levels
{
    public class LevelData_0 : LevelData
    {
        public int Level_0_MoveLimit = 20;
        public Goal[] Level_0_Goals = { new Goal(GoalType.GreenCube, 10) };

        public override ItemType GetNextFillItemType()
        {
            return GetRandomCubeItemType();
        }

        public override void Initialize()
        {
            GridData = new ItemType[Board.Rows, Board.Cols];

            for (var y = 0; y < Board.Rows; y++)
            {
                for (var x = 0; x < Board.Cols; x++)
                {
                    GridData[x, y] = GetRandomCubeItemType();
                }
            }
            MoveLimit = Level_0_MoveLimit;
            Goals = Level_0_Goals;
        }
    }
}