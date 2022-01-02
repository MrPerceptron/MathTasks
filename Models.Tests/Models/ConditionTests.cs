using System.Collections.Generic;
using NUnit.Framework;
using Task5.Common.Enums;
using Task5.Common.Interfaces;

namespace Task5.Common.Models.Tests.Models
{
    public class ConditionTests
    {
        readonly DataListItem _mainPerson = new() { FirstName = "Имя_1", LastName = "Фамилия_1" };
        readonly DataListItem[] _dependentPersons = new[] { new DataListItem() { FirstName = "Имя_2", LastName = "Фамилия_2" } };
        readonly PersonNameType[] _dependentNameTypes = new[] { PersonNameType.FirstName, PersonNameType.LastName };
        readonly ConditionType _conditionType = ConditionType.Left;
        readonly PersonNameType _nameType = PersonNameType.FirstName;
        readonly Condition _condition;

        public ConditionTests()
        {
            _condition = new()
            {
                ConditionType = _conditionType,
                DependentNameTypes = _dependentNameTypes,
                DependentPersons = _dependentPersons,
                MainPerson = _mainPerson,
                NameType = _nameType
            };
        }

        [Test]
        public void Condition_AreAllFieldsEqualDefaultIfFiledsWerentSet()
        {
            Condition condition = new();

            Assert.AreEqual(default(ConditionType), condition.ConditionType);
            Assert.AreEqual(default(PersonNameType[]), condition.DependentNameTypes);
            Assert.AreEqual(default(IEnumerable<DataListItem>), condition.DependentPersons);
            Assert.AreEqual(default(DataListItem), condition.MainPerson);
            Assert.AreEqual(default(PersonNameType), condition.NameType);
        }

        [Test]
        public void Condition_AreAllValuesInstalledCorrectly()
        {
            Assert.AreEqual(_conditionType, _condition.ConditionType);
            Assert.AreEqual(_dependentNameTypes, _condition.DependentNameTypes);
            Assert.AreEqual(_dependentPersons, _condition.DependentPersons);
            Assert.AreEqual(_mainPerson, _condition.MainPerson);
            Assert.AreEqual(_nameType, _condition.NameType);
        }

        [Test]
        public void ConstructorAndEquals_ConditionCloningAndChecking() 
        {
            Condition condition = _condition.CloneInstance();
            Assert.IsTrue(condition.Equals(_condition));
        }

        [Test]
        public void ToString_IsReturnValueCorrectIfNotUseFirstNameEqual()
        {
            Assert.AreEqual("Имя_1 Left Имя_2", _condition.ToString());
        }

        [Test]
        public void ToString_IsReturnValueCorrectIfUseFirstNameEqual()
        {
            _condition.ConditionType = ConditionType.FirstNameEqual;
            Assert.AreEqual("FirstNameEqual Имя_1", _condition.ToString());
        }
    }
}
