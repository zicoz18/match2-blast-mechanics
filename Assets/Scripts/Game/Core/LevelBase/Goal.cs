using Game.Core.Enums;
using UnityEngine;

namespace Game.Core.LevelBase
{
    public class Goal
    {
        private GoalType Type;
        private int Count;

        public Goal(GoalType type, int count)
        {
            Type = type;
            Count = count;
        }

        public bool IsComplete()
        {
            return Count == 0;
        }

        public bool TryDecrementCount(GoalType _type)
        {
            bool isDecremented = false;
            if (IsSameType(_type))
            {
                isDecremented = TryDecrementCount();
            }
            return isDecremented;
        }

        public bool IsSameType(GoalType _type)
        {
            return Type == _type;
        }

        private bool TryDecrementCount()
        {
            if (Count > 0)
            {
                Count--;
                return true;
            }
            return false;
        }
    }
}
