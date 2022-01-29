using Task5.Common.Enums;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.IterationHandlers
{
    internal class MiddleIterationHandler : BaseIterationHandler
    {
        public MiddleIterationHandler(PersonsTableList personsTableList) : base(personsTableList) { }

        public override Condition ExecuteIteration()
        {
            DataListItem mainPerson = GetMainPerson();

            List<ConditionType> notSupportedConditionTypes = new(_notSupportedConditionTypes);
            do
            {
                if (notSupportedConditionTypes.Count == 3)
                    notSupportedConditionTypes = new(_notSupportedConditionTypes);

                ConditionType conditionType = GetRandomConditionTypeFromAvailables(notSupportedConditionTypes);
                DataListItem[] dependentPersons = GetDependentPersons(mainPerson, conditionType);

                // Если хоть у одной персоны имя было использованно, то:
                if (dependentPersons.Any(x => x.UsedFirstName || x.UsedLastName))
                {
                    return new Condition
                    {
                        ConditionType = conditionType,
                        DependentNameTypes = GetDependentNameTypes(dependentPersons).ToArray(),
                        DependentPersons = dependentPersons,
                        MainPerson = mainPerson,
                        NameType = DeterminePersonNameType(mainPerson, false)
                    };
                }
                notSupportedConditionTypes.Add(conditionType);
            } while (true);
        }

        /// <summary>
        /// Выбирает рандомную персону с использованным именем и возвращает этот тип.
        /// Из остальных персон, возвращает тип, которые не был использован
        /// </summary>
        /// <param name="dependentPersons">Персоны для определения типа</param>
        /// <returns>Все типы которые не были использованы, за исключением одного</returns>
        private static IEnumerable<PersonNameType> GetDependentNameTypes(DataListItem[] dependentPersons)
        {
            IEnumerable<DataListItem> personsWithUsedName = dependentPersons.Where(x => x.UsedFirstName || x.UsedLastName);

            DataListItem randomSelectedPersonWithUsedName = personsWithUsedName.ElementAt(_rand.Next(personsWithUsedName.Count()));

            yield return DeterminePersonNameType(randomSelectedPersonWithUsedName, true);

            for (int i = 0; i < dependentPersons.Length; i++)
            {
                if (dependentPersons[i] != randomSelectedPersonWithUsedName)
                    yield return DeterminePersonNameType(dependentPersons[i], false);
            }
        }
    }
}
