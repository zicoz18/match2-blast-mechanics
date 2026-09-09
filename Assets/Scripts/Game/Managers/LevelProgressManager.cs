using Game.Core.Enums;
using Game.Core.LevelBase;

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
            return;
        }
    }
}