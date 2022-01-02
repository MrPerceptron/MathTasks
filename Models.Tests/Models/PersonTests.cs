using NUnit.Framework;

namespace Task5.Common.Models.Tests.Models
{
    public class PersonTests
    {
        [Test]
        public void Person_AreAllFieldsEqualDefaultIfFiledsWerentSet()
        {
            Person person = new();

            Assert.AreEqual(default(string), person.FirstName);
            Assert.AreEqual(default(string), person.LastName);
        }

        [Test]
        public void Person_AreAllValuesInstalledCorrectly()
        {
            Person person = new() { FirstName = "FirstName", LastName = "LastName" };

            Assert.AreEqual("FirstName", person.FirstName);
            Assert.AreEqual("LastName", person.LastName);
        }
    }
}
