using CustomizableRandomizer;
using Task5.Common.Enums;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.IterationHandlers
{
    internal abstract class BaseIterationHandler
    {
        protected readonly static Random _rand = new();

        protected readonly static CustomizableRandomizer<ConditionType> _customizableRandomizer;

        protected readonly PersonsTableList _personsTableList;

        protected readonly IEnumerable<ConditionType> _notSupportedConditionTypes;

        private static readonly ConditionType[] _supportedConditionTypes = new[]
        {
            ConditionType.Between,
            ConditionType.FirstNameEqual,
            ConditionType.Left,
            ConditionType.Right
        };

        static BaseIterationHandler()
        {
            var chancesWithConditionTypes = new KeyValuePair<double, ConditionType>[]
            {
                new(30, ConditionType.Between),
                new(30, ConditionType.FirstNameEqual),
                new(20, ConditionType.Left),
                new(20, ConditionType.Right),
            };
            ChanceValuePairs<ConditionType> chanceValuePairs = new(chancesWithConditionTypes);

            _customizableRandomizer = new CustomizableRandomizer<ConditionType>(chanceValuePairs);
        }

        public BaseIterationHandler(PersonsTableList personTableList)
        {
            if (personTableList is null)
                throw new ArgumentNullException(nameof(personTableList));

            if (personTableList.Length < 2)
                throw new ArgumentException($"{nameof(personTableList)} не может содержать меньше 2х элементов");

            _personsTableList = personTableList;
            _notSupportedConditionTypes = GetNotSupportedConditionTypes();
        }

        public abstract Condition ExecuteIteration();

        protected virtual IEnumerable<ConditionType> GetNotSupportedConditionTypes()
        {
            // Для того, чтобы если например вызвать второй раз этот метод
            if (_notSupportedConditionTypes != null)
                return _notSupportedConditionTypes;

            if (_personsTableList.Length == 2)
                return new[] { ConditionType.FirstNameEqual, ConditionType.Between };

            return new[] { ConditionType.FirstNameEqual };
        }

        /// <summary>
        /// Находит первую персону у которой не использовано имя и возвращает персону которая находится на одну итерацию раньше от неё,
        /// но если ничего не удалось найти, возвращает рандомную персону из <paramref name="_personsTableList"></paramref>
        /// </summary>
        /// <returns><typeparamref name="DataListItem"></typeparamref></returns>
        protected virtual DataListItem GetMainPerson()
        {
            if (_personsTableList.Length == 0)
                throw new IndexOutOfRangeException($"Не удалось найти персону, т.к. {nameof(_personsTableList)} ничего не содержит");

            for (int i = 0; i < _personsTableList.Length; i++)
            {
                if (_personsTableList[i].UsedFirstName || _personsTableList[i].UsedLastName) // Позже подумать как это сократить
                {
                    DataListItem person = _personsTableList[i - 1];
                    if ((person.UsedFirstName && person.UsedLastName) == false)
                        return _personsTableList[i - 1];
                }
            }

            return _personsTableList[_rand.Next(_personsTableList.Length)];
        }

        protected virtual DataListItem[] GetDependentPersons(DataListItem mainPerson, ConditionType conditionType)
        {
            int indexOfMainPerson = Array.IndexOf(_personsTableList.ToArray(), mainPerson);

            if (indexOfMainPerson == -1)
                throw new ArgumentException("Не удалось найти mainPerson в _personsTableList");

            return conditionType switch
            {
                ConditionType.Between => new[] { _personsTableList[indexOfMainPerson - 1], _personsTableList[indexOfMainPerson + 1] },
                ConditionType.Left => new[] { _personsTableList[indexOfMainPerson + 1] },
                ConditionType.Right => new[] { _personsTableList[indexOfMainPerson - 1] },
                ConditionType.FirstNameEqual => Array.Empty<DataListItem>(),
                _ => throw new Exception()
            };
        }

        protected static ConditionType GetRandomConditionTypeFromAvailables(IEnumerable<ConditionType> except)
        {
            ConditionType[] availableConditionTypes = _supportedConditionTypes.Where(x => !except.Contains(x)).ToArray();

            if (availableConditionTypes.Length == 0)
                throw new Exception("Нет доступных условий");

            ConditionType randomConditionType = availableConditionTypes[_rand.Next(availableConditionTypes.Length)];

            return randomConditionType;
        }

        /// <summary>
        /// Если было использовано 1 имя, то вернёт то что было не использовано, в ином случае, вернёт на рандом
        /// </summary>
        /// <param name="person">Персона хранящая информацию об использовании</param>
        /// <param name="isUsedName">Обозначение при каком условии возвращать тип</param>
        /// <returns>PersonNameType</returns>
        protected static PersonNameType DeterminePersonNameType(DataListItem person, bool isUsedName)
        {
            if (isUsedName)
            {
                if (person.UsedFirstName && person.UsedLastName)
                    return _rand.Next(2) == 0 ? PersonNameType.FirstName : PersonNameType.LastName;
                else if (person.UsedFirstName || person.UsedLastName)
                    return person.UsedFirstName ? PersonNameType.FirstName : PersonNameType.LastName;
            }
            else
            {
                if ((person.UsedFirstName || person.UsedLastName) == false)
                    return _rand.Next(2) == 0 ? PersonNameType.FirstName : PersonNameType.LastName;
                else if ((person.UsedFirstName || person.UsedLastName) == true)
                    return person.UsedFirstName ? PersonNameType.LastName : PersonNameType.FirstName;
            }
            throw new ArgumentException("Не удалось определить тип");
        }
    }
}
