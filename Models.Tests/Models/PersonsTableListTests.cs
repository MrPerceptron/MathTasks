using System.Collections;
using NUnit.Framework;

namespace Task5.Common.Models.Tests.Models
{
    public class PersonsTableListTests
    {
        private static PersonsTableList GetPersonTableList(bool usedFirstName, bool usedLastName, int countOfElements)
        {
            PersonsTableList personsTableList = new();

            for (int i = 0; i < countOfElements; i++)
                personsTableList.Add(GetDataListItem(usedFirstName, usedLastName, i));

            return personsTableList;
        }

        private static DataListItem GetDataListItem(bool usedFirstName, bool usedLastName, int nameNumber)
        {
            return new DataListItem()
            {
                FirstName = $"Имя_{nameNumber}",
                LastName = $"Фамилия_{nameNumber}",
                UsedFirstName = usedFirstName,
                UsedLastName = usedLastName
            };
        }

        [Test]
        public void Add_ExecutesCorrectly()
        {
            PersonsTableList personTableList = new();

            Assert.DoesNotThrow(() => personTableList.Add(new DataListItem()));
        }

        [Test]
        public void Length_IsPropertyReturnsCorrectValue()
        {
            var personTableList = GetPersonTableList(true, true, 1);

            Assert.AreEqual(1, personTableList.Length);
        }

        [Test]
        public void IsTableListFilled_IsPropertyReturnsTrue()
        {
            var personTableList = GetPersonTableList(true, true, 1);

            Assert.AreEqual(true, personTableList.IsTableListFilled);
        }

        [Test]
        public void IsTableListFilled_IsPropertyReturnsFalse()
        {
            var personTableList = GetPersonTableList(false, false, 1);

            Assert.AreEqual(false, personTableList.IsTableListFilled);
        }

        [Test]
        public void LastNameEqualAvailable_IsPropertyReturnsFalse()
        {
            var personTableList = GetPersonTableList(false, false, 1);

            Assert.AreEqual(false, personTableList.LastNameEqualAvailable);
        }

        [Test]
        public void LastNameEqualAvailable_IsPropertyReturnsTrue()
        {
            var personTableList = GetPersonTableList(false, true, 1);

            Assert.AreEqual(true, personTableList.LastNameEqualAvailable);
        }

        [Test]
        public void Index_GetterWorksCorrectly()
        {
            PersonsTableList personTableList = new();
            var dataListItem = GetDataListItem(false, true, 1);

            personTableList.Add(dataListItem);

            for (int i = -5; i <= 5; i++)
                Assert.AreSame(dataListItem, personTableList[i]);
        }

        [Test]
        public void Index_SetterWorksCorrectly()
        {
            var personTableList = GetPersonTableList(false, true, 3);
            var dataListItem = GetDataListItem(false, true, 4);

            personTableList.Add(dataListItem);

            for (int i = -5; i <= 5; i++)
            {
                personTableList[i] = dataListItem;
                Assert.AreSame(dataListItem, personTableList[i]);
            }
        }

        [Test]
        public void GetEnumerator_Generic()
        {
            var personTableList = GetPersonTableList(false, false, 1);

            IEnumerator enumerator = personTableList.GetEnumerator();

            Assert.NotNull(enumerator);
        }

        [Test]
        public void GetEnumerator_NonGeneric()
        {
            var personTableList = GetPersonTableList(false, false, 1);

            IEnumerator enumerator = ((IEnumerable)personTableList).GetEnumerator();

            Assert.NotNull(enumerator);
        }

        [Test]
        public void Clone_AreReferencesNotEquals()
        {
            PersonsTableList personTableList = GetPersonTableList(false, false, 1);
            PersonsTableList copiedPersonTableList = (PersonsTableList)personTableList.Clone();

            Assert.AreEqual(copiedPersonTableList[0].FirstName, personTableList[0].FirstName);
            Assert.AreEqual(copiedPersonTableList[0].LastName, personTableList[0].LastName);
            Assert.AreEqual(copiedPersonTableList[0].UsedFirstName, personTableList[0].UsedFirstName);
            Assert.AreEqual(copiedPersonTableList[0].UsedLastName, personTableList[0].UsedLastName);
        }
    }
}
