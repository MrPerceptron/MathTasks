using NUnit.Framework;
using BreakfastTaskGenerator.Generators;
using Task6.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using Task6.Breakfast;
using System.Linq;

namespace Tesk6.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            ITableListGenerator tableListGenerator = new BaseTableListGenerator();
            IConditionsGenerator conditionsGenerator = new ConditionsGenerator();

            BreakfastTaskGenerator.Models.PersonsTableList list = tableListGenerator.Generate(3);

            IEnumerable<BreakfastTaskGenerator.Models.Condition> conditions =
                conditionsGenerator.GenerateConditions(list);

            Condition[] baseConditions = conditions.Select(x => x.Adapt()).ToArray();

            var solution = new Solver().GetSolution(baseConditions);
        }

        [Test]
        public void Test()
        {
            Assert.IsTrue(true);
        }
    }

    static class MyClass
    {
        public static Condition Adapt(this BreakfastTaskGenerator.Models.Condition condition)
        {
            var expressions = new Expression<Func<Person, bool>>[condition.DependentPersons.Count() + 1];

            expressions[0] = ExpressionHelper.GetExpression(condition.MainPerson, condition.NameType);

            for (int i = 0; i < condition.DependentPersons.Count(); i++)
            {
                expressions[i + 1] = ExpressionHelper.GetExpression
                    (condition.DependentPersons.ElementAt(i), condition.DependentNameTypes[i]);
            }

            return new Condition(expressions)
            {
                ItemPosition = condition.ConditionType switch
                {
                    BreakfastTaskGenerator.Enums.ConditionType.Between => Condition.Position.Between,
                    BreakfastTaskGenerator.Enums.ConditionType.FirstNameEqual => Condition.Position.LastNameEqual,
                    BreakfastTaskGenerator.Enums.ConditionType.Left => Condition.Position.Left,
                    BreakfastTaskGenerator.Enums.ConditionType.Right => Condition.Position.Right,
                    _ => throw new Exception()
                }
            };;
        }
    }
}