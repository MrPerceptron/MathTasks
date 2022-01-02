using Task5.Common.Enums;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.IterationHandlers
{
    internal class LastIterationHandler : BaseIterationHandler
    {
        private readonly MiddleIterationHandler _middleIterationHandler;

        public LastIterationHandler(PersonsTableList personsTableList, MiddleIterationHandler middleIterationHandler)
            : base(personsTableList) => _middleIterationHandler = middleIterationHandler;

        public LastIterationHandler(PersonsTableList personsTableList)
            : base(personsTableList) => _middleIterationHandler = new MiddleIterationHandler(personsTableList);

        public override Condition ExecuteIteration()
        {
            DataListItem mainPerson = _personsTableList.First(x => (x.UsedFirstName && x.UsedLastName) == false);

            PersonNameType nameType = DeterminePersonNameType(mainPerson, false);

            if (nameType == PersonNameType.LastName)
                return _middleIterationHandler.ExecuteIteration();

            ConditionType conditionType = GetRandomConditionTypeFromAvailables(_notSupportedConditionTypes);

            var dependentPersons = GetDependentPersons(mainPerson, conditionType);

            return new Condition()
            {
                MainPerson = mainPerson,
                NameType = nameType,
                ConditionType = conditionType,
                DependentPersons = dependentPersons,
                DependentNameTypes = Array.Empty<PersonNameType>()
            };
        }

        protected override IEnumerable<ConditionType> GetNotSupportedConditionTypes()
        {
            return new[]
            {
                ConditionType.Between,
                ConditionType.Left,
                ConditionType.Right
            };
        }
    }
}
