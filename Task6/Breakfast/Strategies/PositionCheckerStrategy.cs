using System;
using System.Collections.Generic;

namespace Task6
{
    interface IPositionCheckerStrategy
    {
        public bool CheckPositionByCondition((Condition conditions, List<(string propertyName, string neededValue)> propAndValues) conditionInfoArr, CircleList<Person> checkedArr);
    }
    abstract class PositionCheckerStrategyBase : IPositionCheckerStrategy
    {
        protected int _logicalIndex;
        protected CircleList<Person> _checkedArr;
        protected (Condition conditions, List<(string propertyName, string neededValue)> propAndValues) _conditionInfoArr;

        public bool CheckPositionByCondition((Condition conditions, List<(string propertyName, string neededValue)> propAndValues) conditionInfoArr, CircleList<Person> checkedArr)
        {
            _checkedArr = checkedArr ?? throw new ArgumentNullException(nameof(conditionInfoArr));
            _conditionInfoArr = conditionInfoArr;
            _logicalIndex = GetLogicalIndex();

            return CheckPosition();
        }
        public abstract bool CheckPosition();

        protected bool IsCorrectPosition(int distanceBetweenLogicalIndex = 0, int propAndValuesIndex = 0)
        {
            return _checkedArr[_logicalIndex + distanceBetweenLogicalIndex].FirstName == _conditionInfoArr.propAndValues[propAndValuesIndex].neededValue ||
                   _checkedArr[_logicalIndex + distanceBetweenLogicalIndex].LastName == _conditionInfoArr.propAndValues[propAndValuesIndex].neededValue;
        }
        private int GetLogicalIndex()
        {
            for (int i = 0; i < _checkedArr.Count; i++)
            {
                if (_checkedArr[i].FirstName == _conditionInfoArr.propAndValues[0].neededValue || _checkedArr[i].LastName == _conditionInfoArr.propAndValues[0].neededValue)
                    return i;
            }
            return -1;
        }
    }
    class BetweenPositionCheckerStrategy : PositionCheckerStrategyBase
    {
        public override bool CheckPosition() => (IsCorrectPosition(-1, 1) || IsCorrectPosition(-1, 2)) && (IsCorrectPosition(1, 1) || IsCorrectPosition(1, 2));
    }
    class RightPositionCheckerStrategy : PositionCheckerStrategyBase
    {
        public override bool CheckPosition() => IsCorrectPosition(-1, 1);
    }
    class LeftPositionCheckerStrategy : PositionCheckerStrategyBase
    {
        public override bool CheckPosition() => IsCorrectPosition(1, 1);
    }
    class LastNameEqualPositionCheckerStrategy : PositionCheckerStrategyBase
    {
        public override bool CheckPosition() => _logicalIndex != -1; // Этот класс вообще не нужен и эта проверка тоже. Есть 2 варианта для даного решения: _logicalIndex != -1 ИЛИ IsCorrectPosition()
    }
}
