using System;
using Game.Core.Enums;
using Game.Core.LevelBase;
using Game.Core.BoardBase;
using UnityEngine;


namespace Game.Managers
{
    public class LevelProgressManager : IProvidable
    {
        private Board _board;
        private Goal[] _goals;
        private int _movesRemaining;

        private int _lastSeenVersion = -1;

        public LevelPlayState LevelPlayState { get; private set; }

        public event EventHandler<OnLevelPlayStateChangedEventArgs> OnLevelPlayStateChanged;
        public class OnLevelPlayStateChangedEventArgs : EventArgs
        {
            public LevelPlayState LevelPlayState;
        }


        public void CheckProgression()
        {
            if (!ShouldCheckProgression()) return;
            _lastSeenVersion = _board.ChangeVersion;
            bool isLevelCompleted = IsLevelCompleted();
            bool hasMovesRemaining = HasMovesRemaining();

            if (isLevelCompleted)
            {
                UpdateLevelPlayState(LevelPlayState.Completed);
            }
            else if (!hasMovesRemaining)
            {
                UpdateLevelPlayState(LevelPlayState.Failed);
            }
            if (isLevelCompleted || !hasMovesRemaining)
            {
                _board.SetCanBeTapped(false);
            }
        }

        private bool ShouldCheckProgression()
        {
            if (!IsPlaying()) return false;
            if (_board.IsMoving()) return false;
            if (_lastSeenVersion == _board.ChangeVersion) return false;
            return true;
        }

        public bool IsPlaying()
        {
            return LevelPlayState.Playing == LevelPlayState;
        }

        public void Prepare(Goal[] goals, int moveLimit, Board board)
        {
            SetMovesRemaining(moveLimit);
            _goals = goals;
            _board = board;
            _board.SetCanBeTapped(true);
            _lastSeenVersion = _board.ChangeVersion;
            UpdateLevelPlayState(LevelPlayState.Playing);
        }

        private void UpdateLevelPlayState(LevelPlayState newState)
        {
            LevelPlayState = newState;
            OnLevelPlayStateChanged?.Invoke(this, new OnLevelPlayStateChangedEventArgs { LevelPlayState = newState });
        }

        public bool HasMovesRemaining()
        {
            return _movesRemaining > 0;
        }

        public int GetMovesRemaining()
        {
            return _movesRemaining;
        }

        public Goal[] GetGoals()
        {
            return _goals;
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

        private bool IsLevelCompleted()
        {
            for (int i = 0; i < _goals.Length; i++)
            {
                Goal goal = _goals[i];
                bool isGoalComplete = goal.IsComplete();
                if (!isGoalComplete) return false;
            }
            return true;
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
            return new[] { directedRocketGoalType, GoalType.Rocket };
        }
    }
}