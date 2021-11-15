using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
/*
Пять дам завтракают, сидя за круглым столом. Миз Осборн сидит между Миз Льюис и Миз Мартин.
Эллен сидит между Кэти и Миз Норрис. Миз Льюис сидит между Эллен и Эллис.
Кэти и Дорис — сестры. Слева от Бетти сидит Миз Паркс, а справа от нее — Миз Мартин.
Сопоставьте имена и фамилии.
*/

// TODO: Зарефакторить conditionInfoArr
// TODO: Заменить IList на Readonly в классах стратегии
namespace Task6
{
    public static class Program
    {
        #region
        public class ArrEqualityComparer : IEqualityComparer<List<int>>
        {
            public bool Equals(List<int> x, List<int> y)
            {
                return GetHashCode(x) == GetHashCode(y);
            }

            public int GetHashCode([DisallowNull] List<int> obj)
            {
                return Program.GetHashValue(obj);
            }
        }

        static void Main()
        {
            Condition[] conditions = new Condition[4] {
                new Condition(x => x.FirstName == "Ваня",
                    x => x.FirstName == "Сергей")
                    { ItemPosition = Condition.Position.Left },
                new Condition (x => x.FirstName == "Ваня",
                    x => x.FirstName == "Сергея")
                    { ItemPosition = Condition.Position.Right },
                new Condition (x => x.LastName == "Иванов",
                    x => x.FirstName == "Сергея")
                    { ItemPosition = Condition.Position.Left},
                new Condition (x => x.LastName == "Петров",
                    x => x.LastName == "Сидоров")
                    { ItemPosition = Condition.Position.Left },
            };

            List<List<(string propertyName, string neededValue)>> conditionInfoArr = conditions.Select(x => x.AllConditions.Select(GetPropNameAndRequiredValue).ToList()).ToList();
            List<List<int>> requiredArr = GetAllLogicalArr(conditionInfoArr).Where(x => x.Count == conditionInfoArr.Count).ToList();

            var unique = requiredArr.Distinct(new ArrEqualityComparer()).ToList();
            var result = new Solution().GetResult(conditions, conditionInfoArr, unique.First());
        }
        #endregion
        #region ГОТОВЫЙ КОД
        public static List<List<int>> GetAllLogicalArr(List<List<(string propertyName, string neededValue)>> conditionArr)
        {
            List<List<int>> returnArr = new(conditionArr.Count);

            for (int index = 0; index < conditionArr.Count; index++)
            {
                List<int> addedArr = new(conditionArr.Count) { index };
                int tempIndex = index;

                for (int i = 0; i < conditionArr.Count; i++)
                {
                    if (conditionArr[i].Count != 1 && !addedArr.Contains(i) && conditionArr[tempIndex].Any(x => conditionArr[i].Contains(x)))
                    {
                        addedArr.Add(i);
                        (tempIndex, i) = (i, -1);
                    }
                }

                for (int i = 0; i < conditionArr.Count; i++)
                {
                    if (!addedArr.Contains(i))
                        addedArr.Add(i);
                }
                returnArr.Add(addedArr);
            }
            return returnArr;
        }
        public static int GetHashValue(List<int> arr)
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
        public static (string propertyName, string neededValue) GetPropNameAndRequiredValue<TSource>(Expression<Func<TSource, bool>> condition)
        {
            var expr = (BinaryExpression)condition.Body;

            return (expr.Left, expr.Right) switch
            {
                (MemberExpression member, ConstantExpression constant) =>
                    (member.Member.Name, constant.Value.ToString()),
                (_, _) => throw new Exception()
            };
        }
        #endregion
    }
    class Solution
    {
        private Dictionary<Condition.Position, IStrategy> _strategies = new(4);
        private IStrategy GetOrUpdateStrategy(Condition.Position position)
        {
            if (!_strategies.ContainsKey(position))
            {
                IStrategy newStrategy = position switch
                {
                    Condition.Position.Between => new BetweenStrategy(),
                    Condition.Position.Right => new RightStrategy(),
                    Condition.Position.Left => new LeftStrategy(),
                    Condition.Position.LastNameEqual => new LastNameEqualStrategy(),
                    _ => throw new Exception()
                };
                _strategies.Add(position, newStrategy);
            }
            return _strategies[position];
        }

        public Solution() { }

        public CircleList<Person> GetResult(Condition[] conditions, List<List<(string propertyName, string neededValue)>> conditionInfoArr, List<int> unique)
        {
            if (conditionInfoArr.Count != unique.Count)
                throw new ArgumentException($"Количество элементов {nameof(conditionInfoArr)} и {nameof(unique)} не может быть разным");

            CircleList<Person> returnArr = new(conditions.Length);

            foreach (var index in unique)
            {
                IStrategy strategy = GetOrUpdateStrategy(conditions[index].ItemPosition);
                bool isCorrect = strategy.SetName(returnArr, conditionInfoArr[index]);
            }
            return returnArr;
        }
    }
}
