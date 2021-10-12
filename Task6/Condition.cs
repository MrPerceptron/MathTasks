using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Task6
{
    public class Condition : IComparable<Condition>
    {
        public enum Position
        {
            Left = 1,
            Between = 2,
            Right = 3,
            LastNameEqual = 4
        }
        public Position ItemPosition { get; set; }

        public Expression<Func<Person, bool>>[] AllConditions { get; set; }
        public Condition(params Expression<Func<Person, bool>>[] conditions)
        {
            AllConditions = new Expression<Func<Person, bool>>[conditions.Length];

            for (int i = 0; i < conditions.Length; i++)
                AllConditions[i] = conditions[i];
        }

        private Dictionary<Position, int> _conditionPriority = new()
        {
            { Position.Left, 0 },
            { Position.Right, 0 },
            { Position.Between, 1 },
            { Position.LastNameEqual, 2 }
        };
        public int CompareTo(Condition other)
        {
            int currentPriority = _conditionPriority[this.ItemPosition];
            int otherPriority = _conditionPriority[other.ItemPosition];

            if (currentPriority < otherPriority)
                return -1;

            return currentPriority > otherPriority ? 1 : 0;
        }
    }
}
