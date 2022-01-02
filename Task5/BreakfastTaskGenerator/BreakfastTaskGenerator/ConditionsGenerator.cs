using Task5.BreakfastTaskGenerator.IterationHandlers;
using Task5.Common.Enums;
using Task5.Common.Interfaces;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator
{
    public class ConditionsGenerator : IConditionsGenerator
    {
        private PersonsTableList? _personsTableList;

        private int _сountOfUsedNames = 0;

        public IEnumerable<Condition> GenerateConditions(PersonsTableList personsTableList)
        {
            _personsTableList = CloneWithNotUsedValues(personsTableList);

            List<Condition> conditions = new();
            var middleIterationHandler = new MiddleIterationHandler(_personsTableList);

            int countOfNames = _personsTableList.Length * 2;
            int countOfNamessWithOneNotUsedName = countOfNames - 1;

            Condition addedCondition;
            do
            {
                if (_сountOfUsedNames == 0)
                    addedCondition = new FirstIterationHandler(_personsTableList).ExecuteIteration(); // Первая итерация
                else if (_сountOfUsedNames == countOfNamessWithOneNotUsedName)  // Последняя итерация
                    addedCondition = new LastIterationHandler(_personsTableList, middleIterationHandler).ExecuteIteration();
                else // Вторая, до последней
                    addedCondition = middleIterationHandler.ExecuteIteration();

                AddCondition(conditions, addedCondition);
            }
            while (_сountOfUsedNames != countOfNames);

            if (_personsTableList.IsTableListFilled == false)
                throw new Exception("Не хватает условий");

            _сountOfUsedNames = 0;

            return conditions;
        }

        private void AddCondition(IList<Condition> dest, Condition condition)
        {
            dest.Add(condition);

            ChangePersonNameState(condition.MainPerson, condition.NameType);

            if (condition.ConditionType != ConditionType.FirstNameEqual) // Потому что может быть FirstNameEqual у которого не может быть зависимых персон
            {
                int j = 0;
                foreach (var person in condition.DependentPersons)
                    ChangePersonNameState(person, condition.DependentNameTypes[j++]);
            }
        }

        // Позже сделать более читабельнее и меньше, если это возможно
        private void ChangePersonNameState(DataListItem person, PersonNameType nameType)
        {
            switch (nameType)
            {
                case PersonNameType.FirstName:
                    if (person.UsedFirstName != true)
                        _сountOfUsedNames++;
                    person.UsedFirstName = true;
                    break;
                case PersonNameType.LastName:
                    if (person.UsedLastName != true)
                        _сountOfUsedNames++;
                    person.UsedLastName = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nameType), nameType, null);
            }
        }

        private static PersonsTableList CloneWithNotUsedValues(PersonsTableList cloneablePersonsTableList)
        {
            PersonsTableList returnPersonsTableList = new(cloneablePersonsTableList.Length) { };

            foreach (var person in cloneablePersonsTableList)
            {
                DataListItem dataListItem = new()
                {
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    UsedFirstName = false,
                    UsedLastName = false
                };

                returnPersonsTableList.Add(dataListItem);
            }

            return returnPersonsTableList;
        }
    }
}