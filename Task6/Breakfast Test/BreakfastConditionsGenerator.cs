using System;
using System.Collections.Generic;
using Bogus.DataSets;
using System.Linq;
using Task6.Breakfast;
using System.Linq.Expressions;

namespace Task6.Breakfast_Test
{
    public class BreakfastConditionsGenerator
    {
        protected enum NameSelector
        {
            FirstName = 0,
            LastName = 1,
            Nothing = 2,
            Everything = 3
        }

        protected readonly static Dictionary<Condition.Position, int[]> _indexes = new(4)
        {
            { Condition.Position.Left, new int[] { 0, 1 } }, // CORRECT
            { Condition.Position.Between, new int[] { 0, -1, 1 } }, // CORRECT
            { Condition.Position.Right, new int[] { 1, 0 } }, // CORRECT
            { Condition.Position.LastNameEqual, new int[] { 0 } },
        };
        protected readonly static Dictionary<Condition.Position, int> _numbersByPosition = new(4)
        {
            { Condition.Position.Left, 2 },
            { Condition.Position.Between, 3 },
            { Condition.Position.Right, 2 },
            { Condition.Position.LastNameEqual, 1 }
        };
        protected readonly List<string> _usedNames = new();
        private readonly CircleList<Person> _persons;
        private static Condition.Position _position = default;

        public BreakfastConditionsGenerator(int personsCount = default)
        {
            if (personsCount == default)
                personsCount = new Random().Next(7) + 3;

            if (personsCount < 3)
                throw new ArgumentException("Не может быть меньше чем 3", nameof(personsCount));

            _persons = new CircleList<Person>()
            {
                new Person { FirstName = "Кэти", LastName = "Миз Льюис" },
                new Person { FirstName = "Дорис", LastName = "Миз Осборн" },
                new Person { FirstName = "Бэти", LastName = "Миз Мартин" },
                new Person { FirstName = "Эллен", LastName = "Миз Норрис" }
            };
            //_persons = GetRandomPersons(personsCount).ToCircleList();
        }

        /// <summary>
        /// Генерирует массив условий, из рандомно сгенерированного массива людей
        /// </summary>
        /// <returns>Массив условий</returns>
        public Condition[] GenerateConditions()
        {
            List<Condition> conditions = new();

            string lastUsedName = default;

            var pos = new Condition.Position[4] { Condition.Position.Between, Condition.Position.Between, Condition.Position.Between, Condition.Position.LastNameEqual }; // ПОЗЖЕ УДАЛИТЬ

            int countOfNames = (_persons.Count * 2); // ПОФИКИСТЬ ЭТО
            for (int i = 0; _usedNames.Count < countOfNames; i++) // ПОФИКИСТЬ ЭТО // ЕСЛИ ПЕРВАЯ ИТЕРАЦИЯ, ТО МОЖНО СДЕЛАТЬ LastNameEqual, но придётся не много переделать код. При этом, если вернуть LastNameEqual в самом начале, то оно позже отсортируется
            {
                int addedIndex = GetLogicalIndex(_persons, lastUsedName);

                _position = pos[i];
                //_position = (_usedNames.Count == countOfNames - 1) ? GetRandomPosition() : GetRandomPosition(Condition.Position.LastNameEqual); // ПОФИКИСТЬ ЭТО
                
                var propAndNames = GetPropAndNames(_persons, _numbersByPosition[_position], addedIndex);

                conditions.Add(GetCondition(propAndNames));

                lastUsedName = propAndNames[^1].name;
            }
            return conditions.ToArray();
        }

        /// <summary>
        /// Создаёт массив имён с их оприделением выбирая правильно имя или фамилию
        /// </summary>
        /// <param name="persons">Массив людей</param>
        /// <param name="countOfPropAndNames">Количество имён</param>
        /// <param name="addedIndex">Индекс для выбора правильного имени</param>
        /// <returns>Массив имён и их оприделение: Имена или Фамилии</returns>
        protected (string name, string propName)[] GetPropAndNames(CircleList<Person> persons, int countOfPropAndNames, int addedIndex)
        {
            if (countOfPropAndNames < 1)
                throw new ArgumentException("Не может быть меньше 1", nameof(countOfPropAndNames));

            var propAndNames = new (string name, string propName)[countOfPropAndNames];

            if (countOfPropAndNames == 1)
            {
                Person personWithNotUsedName = persons.First(x => !_usedNames.Contains(x.FirstName) || !_usedNames.Contains(x.LastName));

                string lastName = personWithNotUsedName.FirstName ?? personWithNotUsedName.LastName;
                string propertyName = DeterminePropertyNameByName(personWithNotUsedName, lastName);

                propAndNames[0] = (lastName, propertyName);
                return propAndNames;
            }

            string propName = DeterminePropertyNameByName(persons[addedIndex], _usedNames.LastOrDefault()); // Название свойства

            if (propName != default)
                propAndNames[0] = GetPropAndName(persons[_indexes[_position][0] + addedIndex], propName == "FirstName");

            for (int i = propName == default ? 0 : 1; i < countOfPropAndNames; i++)
            {
                Person currentPerson = persons[_indexes[_position][i] + addedIndex];

                NameSelector isFirstName = DetermineNameSelector(currentPerson, _usedNames); // ТУТ ВСЁ РЕШИТЬ

                //propAndNames[i] = GetPropAndName(currentPerson, isFirstName);
                _usedNames.Add(propAndNames[i].name);
            }

            return propAndNames;
        }

        /// <summary>
        /// Определяет использованность имён
        /// </summary>
        /// <param name="person">Проверяемая персона/param>
        /// <param name="arrWithUsedNames">Массив с использованными именами</param>
        /// <returns>Оприделение NameSelector</returns>
        /// <exception cref="ArgumentException">Если <param name="person"> является null</para></exception>
        /// <exception cref="ArgumentException">Если <param name="arrWithUsedNames"> является null</exception>
        protected static NameSelector DetermineNameSelector(Person person, IList<string> arrWithUsedNames)
        {
            if (person == null)
                throw new ArgumentException("Не может быть null", nameof(person));

            if (arrWithUsedNames == null)
                throw new ArgumentException("Не может быть null", nameof(arrWithUsedNames));

            bool containsFirstName = arrWithUsedNames.Contains(person.FirstName);
            bool containsLastName = arrWithUsedNames.Contains(person.LastName);

            return (containsFirstName, containsLastName) switch
            {
                (false, true) => NameSelector.FirstName,
                (true, false) => NameSelector.LastName,
                (true, true) => NameSelector.Nothing,
                (false, false) => NameSelector.Everything
            };
        }

        /// <summary>
        /// Определяет к какому свойству принадлежит имя
        /// </summary>
        /// <param name="person">Персона для определения свойста</param>
        /// <param name="name">Имя персоны</param>
        /// <returns>Имя свойства</returns>
        protected static string DeterminePropertyNameByName(Person person, string name)
        {
            if (person.FirstName == name)
                return nameof(person.FirstName);
            else if (person.LastName == name)
                return nameof(person.LastName);
            else
                return default;
        }

        /// <summary>
        /// Определяет имя по названию свойства
        /// </summary>
        /// <param name="person">Персона для определения имени</param>
        /// <param name="propertyName">Названия свойства персоны</param>
        /// <returns>Имя</returns>
        protected static string DetermineNameByPropertyName(Person person, string propertyName)
        {
            if (propertyName == nameof(person.FirstName))
                return person.FirstName;
            else if (propertyName == nameof(person.LastName))
                return person.LastName;
            else
                return default;
        }

        /// <summary>
        /// Метод для получения имени и его названия свойства
        /// </summary>
        /// <param name="isFirstName">Определение имени</param>
        /// <param name="person">Персона для получения имени</param>
        /// <returns>Кортеж из имени и его названием свойства</returns>
        protected static (string name, string propName) GetPropAndName(Person person, bool isFirstName)
        {
            return isFirstName ? (person.FirstName, "FirstName") : (person.LastName, "LastName");
        }

        /// <summary>
        /// Находит индекс персоны, который использовался в propAndNames
        /// </summary>
        /// <param name="persons">Массив персон</param>
        /// <param name="lastPropAndNames">Использованные имена</param>
        /// <returns>Индекс</returns>
        protected static int GetLogicalIndex(CircleList<Person> persons, string lastUsedName)
        {
            if (persons.Count == 0 || lastUsedName == default)
                return 0;

            return persons.IndexOf(persons.First(x => x.FirstName == lastUsedName || x.LastName == lastUsedName));
        }

        #region
        /// <summary>
        /// Конструирует условие, исходя из параметров
        /// </summary>
        /// <param name="propAndNames">Массив имён с обозначением чем является имя: Фамилия или имя</param>
        /// <returns>Условие</returns>
        private static Condition GetCondition((string name, string propName)[] propAndNames)
        {
            var expressions = new Expression<Func<Person, bool>>[propAndNames.Length];

            for (int i = 0; i < expressions.Length; i++)
                expressions[i] = GetExpression(propAndNames[i].name, propAndNames[i].propName);

            return new Condition(expressions) { ItemPosition = _position };
        }

        /// <summary>
        /// Создаёт выражение выбирая правильное свойство
        /// </summary>
        /// <param name="name">Имя или фамилия</param>
        /// <param name="propName">Обозначение чем является имя: Фамилия или имя</param>
        /// <returns>Выражение</returns>
        /// <exception cref="NotSupportedException">Если <paramref name="propName"/> содержит что-то, помимо FirstName и LastName</exception>
        private static Expression<Func<Person, bool>> GetExpression(string name, string propName)
        {
            Expression<Func<Person, bool>> expression = propName switch
            {
                "FirstName" => x => x.FirstName == name,
                "LastName" => x => x.LastName == name,
                _ => throw new NotSupportedException("Человек может содержать только \"Имя\" и \"Фамилию\"")
            };

            Expression modifiedExpression = new Visitor().Visit(expression);

            return modifiedExpression as Expression<Func<Person, bool>>;
        }

        /// <summary>
        /// Выберает рандомное условие из Condition.Position
        /// </summary>
        /// <param name="except">Позиции которые будут пропущены</param>
        /// <returns>Позицию</returns>
        /// <exception cref="ArgumentException">Если больше 3х позиций в массиве</exception>
        /// <exception cref="Exception">Если позиция отсувствует</exception>
        private static Condition.Position GetRandomPosition(params Condition.Position[] except)
        {
            int coutOfElements = Enum.GetNames<Condition.Position>().Length;
            if (except.Length >= coutOfElements)
                throw new ArgumentException($"Параметр {nameof(except)} не может содержать больше {coutOfElements} исключений, так как существует всего 4 позиции");

            Condition.Position position;
            do
            {
                int numberPosition = new Random().Next(4) + 1;

                position = numberPosition switch
                {
                    1 => Condition.Position.Left,
                    2 => Condition.Position.Between,
                    3 => Condition.Position.Right,
                    4 => Condition.Position.LastNameEqual,
                    _ => throw new NotSupportedException()
                };

            } while (except.Contains(position));

            return position;
        }

        /// <summary>
        /// Генерирует массив людей женского пола
        /// </summary>
        /// <param name="personsCount">Количество людей</param>
        /// <returns>Перечисление людей</returns>
        private static IEnumerable<Person> GetRandomPersons(int personsCount)
        {
            var randomNames = new Name(locale: "ru");

            int countOfFirstAndLastNames = personsCount * 2;

            HashSet<string> firstAndLastNames = new(countOfFirstAndLastNames);

            do firstAndLastNames.Add(randomNames.FirstName(Name.Gender.Female)); while (firstAndLastNames.Count != countOfFirstAndLastNames / 2);
            do firstAndLastNames.Add(randomNames.LastName(Name.Gender.Female)); while (firstAndLastNames.Count != countOfFirstAndLastNames);

            var names = firstAndLastNames.ToList();

            for ((int first, int last) = (0, names.Count - 1); first < personsCount; first++, last--)
                yield return new Person { FirstName = names[first], LastName = names[last] };
        }
        #endregion
    }

    public class BreakfastTaskChecker
    {
        /// <summary>
        /// Генерирует задачу и проверяет найдётся ли ответ
        /// </summary>
        /// <returns>Корретно ли отработала задача</returns>
        public bool IsWorksCorrectly()
        {
            var conditions = new BreakfastConditionsGenerator().GenerateConditions();
            return new Solution().GetSolution(conditions).isSolved;
        }
    }
}
