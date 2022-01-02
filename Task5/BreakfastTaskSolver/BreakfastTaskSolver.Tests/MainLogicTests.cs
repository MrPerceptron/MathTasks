using NUnit.Framework;
using System.Collections.Generic;
using Task5.BreakfastTaskGenerator;
using Task5.Common.Interfaces;
using Task5.Common.Models;

namespace Task5.BreakfastTaskSolver.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            ITableListGenerator tableListGenerator = new BaseTableListGenerator();
            IConditionsGenerator conditionsGenerator = new ConditionsGenerator();

            PersonsTableList list = tableListGenerator.Generate(3);

            IEnumerable<Condition> conditions = conditionsGenerator.GenerateConditions(list);
            // Найти решение исходя из conditions
        }

        [Test]
        public void Test()
        {
            Assert.IsTrue(true);
        }
    }
}