using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.LevelBase;
using UnityEngine;

namespace Game.Levels
{
    public class LevelData_2 : LevelData
    {
        public int Level_2_MoveLimit = 20;
        public Goal[] Level_2_Goals = { new Goal(GoalType.Balloon, 10) };


        private const int PercentRange = 100;
        private const int CubeProbabilityPercent = 90;
        public override ItemType GetNextFillItemType()
        {
            // With %90 chance get cube, with 10% get balloon
            int randomness = Random.Range(0, PercentRange);
            if (randomness < CubeProbabilityPercent)
            {
                return GetRandomCubeItemType();
            }
            else
            {
                return ItemType.Balloon;
            }
        }

        public override void Initialize()
        {
            GridData = new ItemType[Board.Rows, Board.Cols];

            for (var y = 0; y < Board.Rows; y++)
            {
                for (var x = 0; x < Board.Cols; x++)
                {
                    if (GridData[x, y] != ItemType.None) continue;
                    if (x == y || x + y == 8)
                    {
                        GridData[x, y] = ItemType.Balloon;
                    }
                    else
                    {
                        GridData[x, y] = GetRandomCubeItemType();
                    }
                }
            }

            MoveLimit = Level_2_MoveLimit;
            Goals = Level_2_Goals;
        }
    }
}