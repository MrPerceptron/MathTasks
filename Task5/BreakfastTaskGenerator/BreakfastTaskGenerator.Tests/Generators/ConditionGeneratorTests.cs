using System.Collections.Generic;
using Task5.Common.Enums;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.Tests.Generators
{
    public class ConditionGeneratorTests
    {
        private readonly ConditionsGenerator _conditionGenerator = new();

        private readonly BaseTableListGenerator _tableListGenerator = new();
        private readonly Dictionary<ConditionType, int> _dict = new()
        {
            { ConditionType.Between, 2 },
            { ConditionType.FirstNameEqual, 0 },
            { ConditionType.Left, 1 },
            { ConditionType.Right, 1 }
        };

        private IEnumerable<Condition> _generatedConditionList;

        public ConditionGeneratorTests() => Reset();

        public void Reset()
        {
            var list = _tableListGenerator.Generate(10);
            _generatedConditionList = _conditionGenerator.GenerateConditions(list);
        }
    }
}