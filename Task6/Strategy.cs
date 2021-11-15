using System;
using System.Collections.Generic;
using System.Linq;

namespace Task6
{
    interface IStrategy
    {
        bool SetName(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr);
    }
    internal abstract class StrategyBase : IStrategy
    {
        protected int _index = 0;
        public bool SetName(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            if (tempArr is null)
                throw new ArgumentNullException(nameof(tempArr));

            if (conditionInfoArr is null)
                throw new ArgumentNullException(nameof(conditionInfoArr));

            _index = GetIndex(tempArr, conditionInfoArr);

            switch (tempArr.Count)
            {
                case 0:
                    ExecuteEmpty(tempArr, conditionInfoArr);
                    break;
                default:
                    ExecuteNotEmpty(tempArr, conditionInfoArr);
                    break;
            }
            return true;
        }

        protected abstract void ExecuteNotEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr);
        protected abstract void ExecuteEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr);

        protected static Person GetPerson((string propertyName, string neededValue) conditionInfo)
        {
            return conditionInfo.propertyName switch
            {
                nameof(Person.FirstName) => new Person() { FirstName = conditionInfo.neededValue },
                nameof(Person.LastName) => new Person() { LastName = conditionInfo.neededValue },
                _ => throw new Exception()
            };
        }
        protected static int GetIndex(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            int index = tempArr.IndexOf(tempArr.FirstOrDefault(x => conditionInfoArr.Any(y => y.neededValue == x.FirstName || y.neededValue == x.LastName)));
            return index == -1 ? 0 : index; // Если не удалось найти нужный индекс
        }
        protected static bool SetFieldByIndex(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr, int[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (conditionInfoArr[i].propertyName == nameof(Person.FirstName) && tempArr[indexes[i]].FirstName == default || tempArr[indexes[i]].FirstName == conditionInfoArr[i].neededValue)
                {
                    tempArr[indexes[i]].FirstName = conditionInfoArr[i].neededValue;
                    continue;
                }
                else if (conditionInfoArr[i].propertyName == nameof(Person.LastName) && tempArr[indexes[i]].LastName == default || tempArr[indexes[i]].LastName == conditionInfoArr[i].neededValue)
                {
                    tempArr[indexes[i]].LastName = conditionInfoArr[i].neededValue;
                    continue;
                }
                return false;
            }
            return true;
        }
    }
    class BetweenStrategy : StrategyBase
    {
        protected override void ExecuteEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
        }
        protected override void ExecuteNotEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
        }
    }
    class RightStrategy : StrategyBase
    {
        protected override void ExecuteEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            tempArr.Add(GetPerson(conditionInfoArr[1]));
            tempArr.Add(GetPerson(conditionInfoArr[0]));
        }
        protected override void ExecuteNotEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            SetFieldByIndex(tempArr, conditionInfoArr, new int[] { _index, _index + 1 });
        }
    }
    class LeftStrategy : StrategyBase
    {
        protected override void ExecuteEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            tempArr.Add(GetPerson(conditionInfoArr[0]));
            tempArr.Add(GetPerson(conditionInfoArr[1]));
        }
        protected override void ExecuteNotEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            SetFieldByIndex(tempArr, conditionInfoArr, new int[] { _index, _index - 1 });
        }
    }
    class LastNameEqualStrategy : StrategyBase
    {
        protected override void ExecuteEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            tempArr.Add(GetPerson(conditionInfoArr[0]));
        }
        protected override void ExecuteNotEmpty(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> conditionInfoArr)
        {
            for (int i = 0; i < tempArr.Count; i++)
            {
                tempArr[i].FirstName ??= conditionInfoArr[i].neededValue;
                tempArr[i].LastName ??= conditionInfoArr[i].neededValue;
            }
        }
    }
}
