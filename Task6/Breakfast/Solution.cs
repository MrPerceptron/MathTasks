using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace Task6.Breakfast
{
    class Solution
    {
        private class ArrEqualityComparer : IEqualityComparer<List<int>>
        {
            public bool Equals(List<int> x, List<int> y) => GetHashCode(x) == GetHashCode(y);
            public int GetHashCode([DisallowNull] List<int> obj) => GetHashValue(obj);
        }

        private Dictionary<Condition.Position, INameSetterStrategy> _nameSetterStrategies = new(4);
        private Dictionary<Condition.Position, IPositionCheckerStrategy> _positionCheckerStrategies = new(4);

        private INameSetterStrategy GetOrUpdateNameSetterStrategy(Condition.Position position)
        {
            if (!_nameSetterStrategies.ContainsKey(position))
            {
                INameSetterStrategy newStrategy = position switch
                {
                    Condition.Position.Between => new BetweenNameSetterStrategy(),
                    Condition.Position.Right => new RightNameSetterStrategy(),
                    Condition.Position.Left => new LeftNameSetterStrategy(),
                    Condition.Position.LastNameEqual => new LastNameEqualNameSetterStrategy(),
                    _ => throw new Exception()
                };
                _nameSetterStrategies.Add(position, newStrategy);
            }
            return _nameSetterStrategies[position];
        }
        private IPositionCheckerStrategy GetOrUpdatePositionCheckerStrategy(Condition.Position position)
        {
            if (!_positionCheckerStrategies.ContainsKey(position))
            {
                IPositionCheckerStrategy newStrategy = position switch
                {
                    Condition.Position.Between => new BetweenPositionCheckerStrategy(),
                    Condition.Position.Right => new RightPositionCheckerStrategy(),
                    Condition.Position.Left => new LeftPositionCheckerStrategy(),
                    Condition.Position.LastNameEqual => new LastNameEqualPositionCheckerStrategy(),
                    _ => throw new Exception()
                };
                _positionCheckerStrategies.Add(position, newStrategy);
            }
            return _positionCheckerStrategies[position];
        }

        public Solution() { }

        public (CircleList<Person> answer, bool isSolved) GetSolution(Condition[] conditions)
        {
            List<List<(string propertyName, string neededValue)>> propAndValues = conditions.Select(x => x.AllConditions.Select(GetPropNameAndRequiredValue).ToList()).ToList();
            List<List<int>> requiredArr = GetAllLogicalArr(propAndValues).Where(x => x.Count == propAndValues.Count).ToList();

            var uniqueArr = requiredArr.Distinct(new ArrEqualityComparer()).ToList();
            List<int> unique = uniqueArr.Last();

            if (propAndValues.Count != unique.Count)
                throw new ArgumentException($"Количество элементов {nameof(propAndValues)} и {nameof(unique)} не может быть разным");

            CircleList<Person> answer = new(conditions.Length); // Сюда будет записываться результат
            for (int i = 0; i < GetCountOfPersons(propAndValues); i++)
                answer.Add(new Person()); // Для упрощения кода | упрощения стратегии

            var conditionInfoArr = conditions.Zip(propAndValues).ToArray(); // Для оптимизации, чтобы только 1 раз создавалось

            for (int i = 0; i < unique.Count; i++)
            {
                INameSetterStrategy strategy = GetOrUpdateNameSetterStrategy(conditions[unique[i]].ItemPosition);
                bool isCorrect = strategy.SetName(ref answer, propAndValues[unique[i]]);

                if (i == 0 && !isCorrect)
                    return (answer, false);

                if (!isCorrect) // Если не правильно, то вернуться обратно на предыдущую итерацию
                    i -= 2; // -2, потому что в начале цикла прибавится 1

                if (isCorrect && i == unique.Count - 1 && !IsAllConditionsCorrect(conditionInfoArr, answer)) // Для оптимизации, чтобы не проверялось каждый раз, а именно тогда, когда все поля заполнены
                    i -= 2; // -2, потому что в начале цикла прибавится 1
            }
            return (answer, true);
        }

        private bool IsAllConditionsCorrect((Condition conditions, List<(string propertyName, string neededValue)> propAndValues)[] conditionInfoArr, CircleList<Person> checkedArr)
        {
            for (int i = 0; i < conditionInfoArr.Length; i++)
            {
                IPositionCheckerStrategy strategy = GetOrUpdatePositionCheckerStrategy(conditionInfoArr[i].conditions.ItemPosition);
                bool isCorrect = strategy.CheckPositionByCondition(conditionInfoArr[i], checkedArr);
                if (!isCorrect)
                    return false;
            }
            return true;
        }
        private static int GetCountOfPersons(List<List<(string propertyName, string neededValue)>> propAndValues) // На выходе должно быть 5
        {
            HashSet<string> names = new();
            for (int i = 0; i < propAndValues.Count; i++)
            {
                for (int j = 0; j < propAndValues[i].Count; j++)
                    names.Add(propAndValues[i][j].neededValue);
            }
            return names.Count / 2;
        }
        private static List<List<int>> GetAllLogicalArr(List<List<(string propertyName, string neededValue)>> propAndValuesArr)
        {
            if (propAndValuesArr.Count == 0)
                throw new ArgumentException("Не может содержать 0 элементов",nameof(propAndValuesArr));

            List<List<int>> returnArr = new(propAndValuesArr.Count);
            int unassignedArr = propAndValuesArr.IndexOf(propAndValuesArr.First(x => x.Count == 1));

            for (int index = 0; index < propAndValuesArr.Count; index++)
            {
                List<int> addedArr = new(propAndValuesArr.Count) { index };
                int tempIndex = index;

                for (int i = 0; i < propAndValuesArr.Count; i++)
                {
                    if (propAndValuesArr[i].Count != 1 && !addedArr.Contains(i) && propAndValuesArr[tempIndex].Any(x => propAndValuesArr[i].Contains(x)))
                    {
                        addedArr.Add(i);
                        (tempIndex, i) = (i, -1);
                    }
                }

                addedArr.Add(unassignedArr);
                returnArr.Add(addedArr);
            }
            return returnArr;
        }
        private static int GetHashValue(List<int> arr)
        {
            if (arr.Count == 0) return 0;
            int hashCode1 = arr[0], hashCode2 = arr[^1];

            for (int counter = 1; counter < arr.Count; counter++)
            {
                hashCode1 <<= counter;
                hashCode2 <<= counter;
                hashCode1 += arr[counter];
                hashCode2 += arr[^(counter + 1)];
            }

            return ~hashCode1 * ~hashCode2;
        }
        private static (string propertyName, string neededValue) GetPropNameAndRequiredValue<TSource>(Expression<Func<TSource, bool>> condition)
        {
            var expr = (BinaryExpression)condition.Body;

            return (expr.Left, expr.Right) switch
            {
                (MemberExpression member, ConstantExpression constant) => (member.Member.Name, constant.Value.ToString()),
                (_, _) => throw new Exception()
            };
        }
    }
}
