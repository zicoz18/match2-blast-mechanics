using Game.Core.Enums;
using Game.Core.LevelBase;
using UnityEngine;


namespace Game.Managers
{
    public class LevelProgressManager : IProvidable
    {
        private Goal[] _goals;
        private int _movesRemaining;

        public void Prepare(Goal[] goals, int moveLimit)
        {
            SetGoals(goals);
            SetMovesRemaining(moveLimit);
        }

        public int GetMovesRemaining()
        {
            return _movesRemaining;
        }

        public Goal[] GetGoals()
        {
            return _goals;
        }

        private void SetGoals(Goal[] goals)
        {
            _goals = goals;
        }

        private void SetMovesRemaining(int movesRemaining)
        {
            _movesRemaining = movesRemaining;
        }

        public bool TryDecrementMovesRemaining()
        {
            bool isDecremented = false;
            if (_movesRemaining > 0)
            {
                SetMovesRemaining(_movesRemaining - 1);
                isDecremented = true;
            }
            return isDecremented;
        }

        public void ItemTypeDestroyed(ItemType itemType)
        {
            GoalType[] goalTypesDestroyed = GetGoalTypesForItemType(itemType);
            for (int i = 0; i < goalTypesDestroyed.Length; i++)
            {
                GoalType goalTypeDestroyed = goalTypesDestroyed[i];
                for (int j = 0; j < _goals.Length; j++)
                {
                    Goal currentGoal = _goals[j];
                    currentGoal.TryDecrementCount(goalTypeDestroyed);
                }
            }
        }

        // Would have preferred to have a map and make sure that each itemType has a corresponding GoalType
        // Yet, there is a simmilar logic for ItemType with ItemFactory, so I will not introduce a new method and will just use the already introduced method inside the codebase
        public GoalType[] GetGoalTypesForItemType(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.None:
                    return System.Array.Empty<GoalType>();
                case ItemType.GreenCube:
                    return GetCubeGoalTypes(GoalType.GreenCube);
                case ItemType.YellowCube:
                    return GetCubeGoalTypes(GoalType.YellowCube);
                case ItemType.BlueCube:
                    return GetCubeGoalTypes(GoalType.BlueCube);
                case ItemType.RedCube:
                    return GetCubeGoalTypes(GoalType.RedCube);
                case ItemType.Balloon:
                    return new[] { GoalType.Balloon };
                case ItemType.GreenBalloon:
                    return GetColoredBalloonGoalTypes(GoalType.GreenBalloon);
                case ItemType.YellowBalloon:
                    return GetColoredBalloonGoalTypes(GoalType.YellowBalloon);
                case ItemType.BlueBalloon:
                    return GetColoredBalloonGoalTypes(GoalType.BlueBalloon);
                case ItemType.RedBalloon:
                    return GetColoredBalloonGoalTypes(GoalType.RedBalloon);
                case ItemType.Crate:
                    return new[] { GoalType.Crate };
                case ItemType.Bomb:
                    return new[] { GoalType.Bomb };
                case ItemType.VerticalRocket:
                    return GetRocketBalloonGoalTypes(GoalType.VerticalRocket);
                case ItemType.HorizontalRocket:
                    return GetRocketBalloonGoalTypes(GoalType.HorizontalRocket);
                default:
                    Debug.LogWarning("Can not get goal types for item type: " + itemType);
                    return System.Array.Empty<GoalType>();
            }
        }

        private GoalType[] GetCubeGoalTypes(GoalType cubeColoredGoalType)
        {
            return new[] { cubeColoredGoalType, GoalType.Cube };
        }

        private GoalType[] GetColoredBalloonGoalTypes(GoalType balloonColoredGoalType)
        {
            return new[] { balloonColoredGoalType, GoalType.Balloon, GoalType.ColoredBalloon };
        }

        private GoalType[] GetRocketBalloonGoalTypes(GoalType directedRocketGoalType)
        {
            return new[] { directedRocketGoalType, GoalType.AnyRocket };
        }
    }
}