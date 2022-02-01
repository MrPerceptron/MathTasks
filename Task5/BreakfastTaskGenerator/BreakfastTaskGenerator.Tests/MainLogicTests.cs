using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Task5.Common.Enums;
using Task5.Common.Interfaces;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.Tests
{
    public class MainLogicTests
    {
        const int CountOfGeneratedPersons = 5;
        readonly BaseTableListGenerator _tableListGenerator = new();
        readonly ConditionsGenerator _conditionsGenerator = new();
        PersonsTableList _personsTableList;
        IEnumerable<Condition> _allConditions;

        List<Condition> _conditionsWithMyGenerator;

        [SetUp]
        public void Setup()
        {
            _personsTableList = _tableListGenerator.Generate(CountOfGeneratedPersons);

            foreach (var item in _personsTableList)
            {
                item.UsedFirstName = true;
                item.UsedLastName = true;
            }

            _allConditions = GenerateAllConditions();

            _conditionsWithMyGenerator = _conditionsGenerator.GenerateConditions(_personsTableList).ToList();
        }

        [Test]
        public void TestMainLogic()
        {
            for (int j = 0; j < 100; j++) // 100 раз выполняется, для лучшей точности
            {
                for (int i = 0; i < _conditionsWithMyGenerator.Count; i++) // Проверка всех условий в проверяемом генераторе
                {
                    bool containsCondition = _allConditions.Any(x => x.Equals(_conditionsWithMyGenerator[i]));

                    Assert.IsTrue(containsCondition);
                }
                // Перегенерирование условий при помощи проверяемого генератора
                _conditionsWithMyGenerator = _conditionsGenerator.GenerateConditions(_personsTableList).ToList();
            }
        }

        private IEnumerable<Condition> GenerateAllConditions()
        {
            List<Condition> allConditionsWithoutDependentNameTypes = new(GetAllConditionsWithoutDependentNameTypes());

            Dictionary<ConditionType, int> dict = new()
            {
                { ConditionType.Between, 4 },
                { ConditionType.FirstNameEqual, 1 },
                { ConditionType.Left, 2 },
                { ConditionType.Right, 2 }
            };

            // Итерация по всем условиям
            for (int i = 0; i < allConditionsWithoutDependentNameTypes.Count; i++)
            {
                int countOfAddedConditions = dict[allConditionsWithoutDependentNameTypes[i].ConditionType];

                for (int j = 0; j < countOfAddedConditions; j++)
                {
                    Condition clonedCondition = allConditionsWithoutDependentNameTypes[i].CloneInstance();

                    if (clonedCondition.ConditionType == ConditionType.FirstNameEqual)
                        clonedCondition.DependentNameTypes = Array.Empty<PersonNameType>();
                    else if (clonedCondition.ConditionType == ConditionType.Left
                        || clonedCondition.ConditionType == ConditionType.Right)
                    {
                        clonedCondition.DependentNameTypes = j switch
                        {
                            0 => new PersonNameType[] { PersonNameType.FirstName },
                            1 => new PersonNameType[] { PersonNameType.LastName }
                        };
                    }
                    else if (clonedCondition.ConditionType == ConditionType.Between)
                    {
                        clonedCondition.DependentNameTypes = j switch
                        {
                            0 => new PersonNameType[] { PersonNameType.FirstName, PersonNameType.LastName },
                            1 => new PersonNameType[] { PersonNameType.LastName, PersonNameType.FirstName },
                            2 => new PersonNameType[] { PersonNameType.FirstName, PersonNameType.FirstName },
                            3 => new PersonNameType[] { PersonNameType.LastName, PersonNameType.LastName },
                        };
                    }

                    yield return clonedCondition;
                }
            }
        }

        private IEnumerable<Condition> GetAllConditionsWithoutDependentNameTypes()
        {
            for (int i = 0; i < _personsTableList.Length; i++)
            {
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.FirstName,
                    ConditionType = ConditionType.FirstNameEqual,
                    DependentPersons = Array.Empty<DataListItem>(),
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.FirstName,
                    ConditionType = ConditionType.Left,
                    DependentPersons = new[] { _personsTableList[i + 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.LastName,
                    ConditionType = ConditionType.Left,
                    DependentPersons = new[] { _personsTableList[i + 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.FirstName,
                    ConditionType = ConditionType.Right,
                    DependentPersons = new[] { _personsTableList[i - 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.LastName,
                    ConditionType = ConditionType.Right,
                    DependentPersons = new[] { _personsTableList[i - 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.FirstName,
                    ConditionType = ConditionType.Between,
                    DependentPersons = new[] { _personsTableList[i - 1], _personsTableList[i + 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.LastName,
                    ConditionType = ConditionType.Between,
                    DependentPersons = new[] { _personsTableList[i - 1], _personsTableList[i + 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.FirstName,
                    ConditionType = ConditionType.Between,
                    DependentPersons = new[] { _personsTableList[i + 1], _personsTableList[i - 1] }
                };
                yield return new Condition
                {
                    MainPerson = _personsTableList[i],
                    NameType = PersonNameType.LastName,
                    ConditionType = ConditionType.Between,
                    DependentPersons = new[] { _personsTableList[i + 1], _personsTableList[i - 1] }
                };
            }
        }
    }
}
