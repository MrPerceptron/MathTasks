using System;
using System.Collections;
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
namespace Task6
{
    internal class Program
    {
        static void Main() // Найти логическую связь между условиями
        {
            var conditions = new Condition[6]
            {
                new Condition(x => x.LastName == "Миз Осборн",
                    x => x.LastName == "Миз Льюис",
                    x => x.LastName == "Миз Мартин")
                    { ItemPosition = Condition.Position.Between },
                new Condition(x => x.FirstName == "Эллен",
                    x => x.FirstName == "Кэти",
                    x => x.LastName == "Миз Норрис")
                    { ItemPosition = Condition.Position.Between },
                new Condition (x => x.LastName == "Миз Льюис",
                    x => x.FirstName == "Эллен",
                    x => x.FirstName == "Эллис")
                    { ItemPosition = Condition.Position.Between },
                new Condition(x => x.FirstName == "Миз Паркс",
                    x => x.LastName == "Бетти")
                    { ItemPosition = Condition.Position.Left },
                new Condition (x => x.LastName == "Миз Мартин",
                    x => x.LastName == "Бетти")
                    { ItemPosition = Condition.Position.Right },
                new Condition (x => x.FirstName == "Дорис")
                    { ItemPosition = Condition.Position.LastNameEqual }
            };
            Array.Sort(conditions);

            //var cond = conditions[0];

            var conditionInfoArr = conditions.Select(x => x.AllConditions.Select(GetPropNameAndRequiredValue).ToList()).ToList();
            new MyClass(conditionInfoArr);
            #region
            //List<(string propertyName, string neededValue)[]> a = new(); // Массив логически связанных условий
            //for (int i = 0; i < conditionInfoArr.Length; i++)
            //{
            //    for (int j = 0; j < conditionInfoArr.Length; j++)
            //    {
            //        if (i != j && conditionInfoArr[i].Intersect(conditionInfoArr[j]).Any())
            //        {
            //            a.Add(conditionInfoArr[i]); // ТУТ ЧТО-ТО СДЕЛАТЬ С СОВПАДАЮЩИМИ ПО ЛОГИКЕ УСЛОВИЯМИ
            //        }
            //    }
            //}

            //OperateMainLogic(cond, persons, conditionInfoArr[0]);
            #endregion
        }
        public class MyClass
        {
            List<List<(string propertyName, string neededValue)>> _conditionInfo;
            CircleList<Person> _result;
            public MyClass(List<List<(string propertyName, string neededValue)>> conditionInfo)
            {
                _result = new(conditionInfo.Count) {};
                AddToResult(conditionInfo[0]);

                
                conditionInfo.RemoveAt(0);
                _conditionInfo = conditionInfo;
                ShowArray();
            }
            void F(){}
            void AddToResult(List<(string propertyName, string neededValue)> conditionInfo) 
            {
                for (int i = 0; i < conditionInfo.Count; i++)
                {
                    if (conditionInfo[i].propertyName == "FirstName")
                        _result.Add(new() { FirstName = conditionInfo[i].neededValue });
                    else if (conditionInfo[i].propertyName == "LastName")
                        _result.Add(new() { LastName = conditionInfo[i].neededValue });
                }
            }
            void FirstAction()
            {
                
            }

            public void ShowArray()
            {
                foreach (var item in _result)
                {
                    Console.WriteLine($"FirstName = {item.FirstName}\tLastName = {item.LastName}");
                }
            }
        }

        public static void OperateMainLogic(Condition condition, CircleList<Person> persons, (string propertyName, string neededValue)[] conditionInfo)
        {
            if (condition.ItemPosition == Condition.Position.Between)
            {
                if (persons.Count == 0)
                {
                    var pss = conditionInfo.Select(x =>
                    {
                        Person newPerson = null;
                        switch (x.propertyName)
                        {
                            case nameof(Person.FirstName):
                                newPerson = new Person { FirstName = x.neededValue };
                                break;
                            case nameof(Person.LastName):
                                newPerson = new Person { LastName = x.neededValue };
                                break;
                        }

                        return newPerson;
                    }).ToArray();

                    persons.Add(pss[1]);
                    persons.Add(pss[0]);
                    persons.Add(pss[2]);
                }
            }
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
    }
}
