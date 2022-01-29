using System;
using System.Collections.Generic;
using System.Linq;
using Task5.BreakfastTaskGenerator;
using Task5.Common.Models;

namespace BreakfastTaskSolver
{
    internal class Program
    {
        const int CountOfGeneratedPersons = 4;

        static readonly BaseTableListGenerator _tableListGenerator = new();
        static readonly ConditionsGenerator _conditionsGenerator = new();

        static void Main()
        {
            PersonsTableList _personsTableList = _tableListGenerator.Generate(CountOfGeneratedPersons);
            List<Condition> _generatedConditions = _conditionsGenerator.GenerateConditions(_personsTableList).ToList();

            ShowConditions(_generatedConditions);
        }

        static void ShowConditions(IEnumerable<Condition> conditions)
        {
            foreach (var item in conditions)
                Console.WriteLine(item.ToString());
        }
    }
}
