using NUnit.Framework;

namespace Task5.Common.Models.Tests.Models
{
    public class DataListItemTests
    {
        [Test]
        public void DataListItem_AreAllFieldsEqualDefaultIfFiledsWerentSet()
        {
            DataListItem person = new();

            Assert.AreEqual(default(string), person.FirstName);
            Assert.AreEqual(default(string), person.LastName);

            Assert.AreEqual(default(bool), person.UsedFirstName);
            Assert.AreEqual(default(bool), person.UsedLastName);
        }

        [Test]
        public void DataListItem_AreAllFieldsWereSetCorrectly()
        {
            DataListItem person = new()
            {
                FirstName = "FirstName",
                LastName = "LastName",
                UsedFirstName = true,
                UsedLastName = true
            };

            Assert.AreEqual("FirstName", person.FirstName);
            Assert.AreEqual("LastName", person.LastName);
            Assert.AreEqual(true, person.UsedFirstName);
            Assert.AreEqual(true, person.UsedLastName);
        }
    }
}
