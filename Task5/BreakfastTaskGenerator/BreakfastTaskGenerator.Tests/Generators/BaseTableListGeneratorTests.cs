using NUnit.Framework;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator.Tests.Generators
{
    public class BaseTableListGeneratorTests
    {
        private BaseTableListGenerator _tableListGenerator = new();
        private readonly PersonsTableList _list;

        public BaseTableListGeneratorTests()
        {
            _list = _tableListGenerator.Generate(10);
        }

        [Test]
        public void Generate_IsReturnValueNotNull()
        {
            Assert.IsNotNull(_list);
        }

        [Test]
        public void Generate_AreAllFirstNamesNotEqualDefault()
        {
            for (int i = 0; i < _list.Length; i++)
            {
                Assert.AreNotEqual(default, _list[i].FirstName);
            }
        }

        [Test]
        public void Generate_AreAllLastNamesNotEqualDefault()
        {
            for (int i = 0; i < _list.Length; i++)
            {
                Assert.AreNotEqual(default, _list[i].LastName);
            }
        }

        [Test]
        public void Generate_AreReturnNamesCorrect()
        {
            for (int i = 0; i < _list.Length; i++)
            {
                Assert.AreEqual($"Имя_{i}", _list[i].FirstName);
                Assert.AreEqual($"Фамилия_{i}", _list[i].LastName);
            }
        }
    }
}