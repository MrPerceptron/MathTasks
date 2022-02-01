using System.Collections.Generic;
using System.Linq;
using Task5.BreakfastTaskGenerator;
using Task5.Common.Models;

/*
    Title: Завтрак дам

    Пять дам завтракают, сидя за круглым столом. Миз Осборн сидит между Миз Льюис и Миз Мартин.
    Эллен сидит между Кэти и Миз Норрис. Миз Льюис сидит между Эллен и Эллис. Кэти и Дорис — сестры. 
    Слева от Бетти сидит Миз Паркс, а справа от нее — Миз Мартин. Сопоставьте имена и фамилии.
 */

namespace Task5
{
    internal class Program
    {
        const int CountOfGeneratedPersons = 2;

        static readonly BaseTableListGenerator _tableListGenerator = new();
        static readonly ConditionsGenerator _conditionsGenerator = new();

        static void Main()
        {
            PersonsTableList _personsTableList = _tableListGenerator.Generate(CountOfGeneratedPersons);
            List<Condition> _generatedConditions = _conditionsGenerator.GenerateConditions(_personsTableList).ToList();
        }
    }
}
