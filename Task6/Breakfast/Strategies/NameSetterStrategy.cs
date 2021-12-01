using System;
using System.Collections.Generic;
using System.Linq;

namespace Task6
{
    interface INameSetterStrategy
    {
        bool SetName(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues);
    }
    internal abstract class NameSetterStrategyBase : INameSetterStrategy
    {
        protected int _index = 0;
        public bool SetName(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            if (tempArr is null)
                throw new ArgumentNullException(nameof(tempArr));

            if (propAndValues is null)
                throw new ArgumentNullException(nameof(propAndValues));

            if (propAndValues.Count != 1 && tempArr.Any(x => x.FirstName != default || x.LastName != default))
                _index = GetIndex(ref tempArr, propAndValues);

            return SetNameByPosition(ref tempArr, propAndValues);
        }
        protected abstract bool SetNameByPosition(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues);
        protected static bool SetFieldByIndex(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues, int[] indexes) // На этапе Эллен, Кэти, Миз Норрис МОЖНО ПОСТАВИТЬ МЕТОДОМ ИСКЛЮЧЕНИЯ !!!
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                if (propAndValues[i].propertyName == nameof(Person.FirstName) && tempArr[indexes[i]].FirstName == default || tempArr[indexes[i]].FirstName == propAndValues[i].neededValue)
                {
                    tempArr[indexes[i]].FirstName = propAndValues[i].neededValue;
                    continue;
                }
                else if (propAndValues[i].propertyName == nameof(Person.LastName) && tempArr[indexes[i]].LastName == default || tempArr[indexes[i]].LastName == propAndValues[i].neededValue)
                {
                    tempArr[indexes[i]].LastName = propAndValues[i].neededValue;
                    continue;
                }
                return false;
            }
            return true;
        }
        protected virtual int GetIndex(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            return tempArr.IndexOf(tempArr.First(x => propAndValues.Any(y => y.neededValue == x.FirstName || y.neededValue == x.LastName)));
        }
    }
    class BetweenNameSetterStrategy : NameSetterStrategyBase
    {
        private static readonly int[][] _middlePositionIndexes = new int[2][] { new int[] { -1, 1 }, new int[] { 1, -1 } };
        private static readonly int[][] _sideIndexes = new int[2][] { new int[] { 1, 2 }, new int[] { -1, -2 } };

        internal class State
        {
            private CircleList<Person> _defaultState;
            public CircleList<Person> DefaultState { get => _defaultState.GenericClone(); }

            public int Index { get; set; }

            public State(CircleList<Person> persons) => _defaultState = persons.GenericClone();
        }

        private Dictionary<IList<(string propertyName, string neededValue)>, State> _states = new();

        protected override bool SetNameByPosition(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            if (!_states.ContainsKey(propAndValues)) // Если данное событие происходит в первый раз, то 
                _states.Add(propAndValues, new State(tempArr)); // создаётся новое СОСТОЯНИЕ

            State currentState = _states[propAndValues]; // Выбирается нужное СОСТОЯНИЕ

            int conditionInfoArrIndex = GetConditionInfoArrIndex(tempArr, propAndValues); // Индекс который был найден по логической связи..

            bool isCorrect = false;
            for (; !isCorrect && currentState.Index < 2; currentState.Index++)
            {
                isCorrect = conditionInfoArrIndex switch // Выбирает нужное действие и устанавливает корректное ли значение
                {
                    0 => SetFieldByIndex(tempArr, propAndValues, new int[] { _index, _middlePositionIndexes[currentState.Index][0] + _index, _middlePositionIndexes[currentState.Index][1] + _index }),
                    1 => SetFieldByIndex(tempArr, propAndValues, new int[] { _sideIndexes[currentState.Index][0] + _index, _index, _sideIndexes[currentState.Index][1] + _index }),
                    2 => SetFieldByIndex(tempArr, propAndValues, new int[] { _sideIndexes[currentState.Index][0] + _index, _sideIndexes[currentState.Index][1] + _index, _index }),
                    _ => throw new Exception($"{nameof(conditionInfoArrIndex)} не может быть меньше 0 или больше 2")
                };
                if (!isCorrect) // Если не правильно, то
                    tempArr = currentState.DefaultState; // устанавливает стандартное состояние tempArr
            }
            if (!isCorrect)
                _states.Remove(propAndValues);

            return isCorrect;
        }
        protected override int GetIndex(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            if (_states.ContainsKey(propAndValues)) // Для того, чтобы установить верное значение/массив, после чего установился правильный индекс
                tempArr = _states[propAndValues].DefaultState;

            return base.GetIndex(ref tempArr, propAndValues);
        }
        private int GetConditionInfoArrIndex(CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            return propAndValues.IndexOf(propAndValues.First(x => x.neededValue == tempArr[_index].FirstName || x.neededValue == tempArr[_index].LastName));
        }
    }
    class RightNameSetterStrategy : NameSetterStrategyBase
    {
        protected override bool SetNameByPosition(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            return SetFieldByIndex(tempArr, propAndValues, new int[] { _index + 1, _index });
        }
    }
    class LeftNameSetterStrategy : NameSetterStrategyBase
    {
        protected override bool SetNameByPosition(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            return SetFieldByIndex(tempArr, propAndValues, new int[] { _index, _index + 1 }); ;
        }
    }
    class LastNameEqualNameSetterStrategy : NameSetterStrategyBase
    {
        protected override bool SetNameByPosition(ref CircleList<Person> tempArr, IList<(string propertyName, string neededValue)> propAndValues)
        {
            int countOfEmptyFields = tempArr.Where(x => x.FirstName == null || x.LastName == null).Count();
            if (countOfEmptyFields > 1)
                return false;

            for (int i = 0; i < tempArr.Count; i++)
            {
                tempArr[i].FirstName ??= propAndValues[0].neededValue;
                tempArr[i].LastName ??= propAndValues[0].neededValue;
            }
            return true;
        }
    }
}
