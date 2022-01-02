using Task5.Common.Enums;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.IterationHandlers
{
    internal class FirstIterationHandler : BaseIterationHandler
    {
        private static readonly Dictionary<ConditionType, int> _quantificationByConditionType = new()
        {
            { ConditionType.Between, 2 },
            { ConditionType.FirstNameEqual, 0 },
            { ConditionType.Left, 1 },
            { ConditionType.Right, 1 },
        };

        public FirstIterationHandler(PersonsTableList personTableList) : base(personTableList) { }

        public override Condition ExecuteIteration()
        {
            ConditionType conditionType = GetRandomConditionTypeFromAvailables(_notSupportedConditionTypes);

            DataListItem mainPerson = GetMainPerson();

            PersonNameType nameType = GetRandomNameType();

            return new Condition()
            {
                MainPerson = mainPerson,
                NameType = nameType,
                ConditionType = conditionType,
                DependentNameTypes = GetRandomNameTypesByConditionType(conditionType).ToArray(),
                DependentPersons = GetDependentPersons(mainPerson, conditionType)
            };
        }

        private static IEnumerable<PersonNameType> GetRandomNameTypesByConditionType(ConditionType conditionType)
        {
            var countOfDependentPersons = _quantificationByConditionType[conditionType];

            for (int i = 0; i < countOfDependentPersons; i++)
                yield return GetRandomNameType();
        }
        private static PersonNameType GetRandomNameType() => (_rand.Next(2) == 0) ? PersonNameType.FirstName : PersonNameType.LastName;
    }
}
