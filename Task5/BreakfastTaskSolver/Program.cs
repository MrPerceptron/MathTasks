using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using BreakfastTaskGenerator.Models;
/*
Condition[] conditions = new Condition[6] {
new Condition(x => x.LastName == "Миз Осборн",
x => x.LastName == "Миз Мартин",
x => x.LastName == "Миз Льюис")
{ ItemPosition = Condition.Position.Between },
new Condition(x => x.FirstName == "Эллен",
x => x.LastName =="Миз Норрис",
x => x.FirstName == "Кэти")
{ ItemPosition = Condition.Position.Between },
new Condition (x => x.LastName == "Миз Льюис",
x => x.FirstName == "Эллен",
x => x.FirstName == "Эллис")
{ ItemPosition = Condition.Position.Between },
new Condition(x => x.LastName == "Миз Паркс",
x => x.FirstName == "Бетти")
{ ItemPosition = Condition.Position.Left },
new Condition (x => x.FirstName == "Бетти",
x =>x.LastName == "Миз Мартин")
{ ItemPosition = Condition.Position.Left },
new Condition (x => x.FirstName == "Дорис")
{ ItemPosition = Condition.Position.LastNameEqual }
};
*/
/*
Пять дам завтракают, сидя за круглым столом. Миз Осборн сидит между Миз Льюис и Миз Мартин.
Эллен сидит между Кэти и Миз Норрис. Миз Льюис сидит между Эллен и Эллис.
Кэти и Дорис — сестры. Слева от Бетти сидит Миз Паркс, а справа от нее — Миз Мартин.
Сопоставьте имена и фамилии.
*/

namespace Task6
{
    public static class Program
    {
        static void Main()
        {
            ConditionConverter[] conditions = new ConditionConverter[6] {
                new ConditionConverter(x => x.LastName == "Миз Осборн",
                x => x.LastName == "Миз Мартин",
                x => x.LastName == "Миз Льюис")
                { ItemPosition = ConditionConverter.Position.Between },
                new ConditionConverter(x => x.FirstName == "Эллен",
                x => x.LastName =="Миз Норрис",
                x => x.FirstName == "Кэти")
                { ItemPosition = ConditionConverter.Position.Between },
                new ConditionConverter (x => x.LastName == "Миз Льюис",
                x => x.FirstName == "Эллен",
                x => x.FirstName == "Эллис")
                { ItemPosition = ConditionConverter.Position.Between },
                new ConditionConverter(x => x.LastName == "Миз Паркс",
                x => x.FirstName == "Бетти")
                { ItemPosition = ConditionConverter.Position.Left },
                new ConditionConverter (x => x.FirstName == "Бетти",
                x =>x.LastName == "Миз Мартин")
                { ItemPosition = ConditionConverter.Position.Left },
                new ConditionConverter (x => x.FirstName == "Дорис")
                { ItemPosition = ConditionConverter.Position.LastNameEqual }
            };
        }

        private class ArrEqualityComparer : IEqualityComparer<List<int>>
        {
            public bool Equals(List<int> x, List<int> y) => GetHashCode(x) == GetHashCode(y);
            public int GetHashCode([DisallowNull] List<int> obj) => GetHashValue(obj);
        }

        public static void ShowAnswer(PersonsTableList persons)
        {
            foreach (var person in persons)
                Console.WriteLine($"{person.FirstName}\t{person.LastName}");
        }

        private static List<List<int>> GetAllLogicalArr(List<List<(string propertyName, string neededValue)>> propAndValuesArr)
        {
            if (propAndValuesArr.Count == 0)
                throw new ArgumentException("Не может содержать 0 элементов", nameof(propAndValuesArr));

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

        private static (string propertyName, string neededValue)
            GetPropNameAndRequiredValue<TSource>(Expression<Func<TSource, bool>> condition)
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
