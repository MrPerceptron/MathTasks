using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CustomizableRandomizer.Tests
{
    internal class CustomizableRandomizerTests
    {
        const int KeyValuePairsArrayLength = 4;

        double[] _chances = new double[KeyValuePairsArrayLength] { 10, 20, 30, 40 };
        int[] _values = new int[KeyValuePairsArrayLength] { 0, 1, 2, 3 };

        ChanceValuePairs<int> _commonValidChanceValuePairs;

        [SetUp]
        public void Setup()
        {
            var keyValuePair = new KeyValuePair<double, int>[KeyValuePairsArrayLength];

            for (int i = 0; i < KeyValuePairsArrayLength; i++)
                keyValuePair[i] = new(_chances[i], _values[i]);

            _commonValidChanceValuePairs = new(keyValuePair);
        }

        [Test]
        public void Counstructor_WithSupportedChanceValuePairsParameter_ReturnsValidInstance()
        {
            Assert.IsInstanceOf<CustomizableRandomizer<int>>(new CustomizableRandomizer<int>(_commonValidChanceValuePairs));
        }

        [Test]
        public void Counstructor_WithSupportedKeyValuePairParameter_ReturnsValidInstance()
        {
            var chancesWithValues = new KeyValuePair<double, int>[]
            {
                new(50, 1),
                new(50, 2)
            };
            Assert.IsInstanceOf<CustomizableRandomizer<int>>(new CustomizableRandomizer<int>(chancesWithValues));
        }

        [Test]
        public void Counstructor_WithNullChanceValuePairsParameter_ThrowsArgumentNullException()
        {
            ChanceValuePairs<int> chanceValuePairs = null;

            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.
                Message.EqualTo("Value cannot be null. (Parameter 'chanceValuePairs')"),
                () => new CustomizableRandomizer<int>(chanceValuePairs));
        }

        [Test]
        public void Counstructor_WithNullKeyValuePair_ThrowsArgumentNullException()
        {
            KeyValuePair<double, int>[] chanceValuePairs = null;

            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.
                Message.EqualTo("Value cannot be null. (Parameter 'chancesWithValues')"),
                () => new CustomizableRandomizer<int>(chanceValuePairs));
        }

        [Test]
        public void GetItem_ReturnsRandomItemByChance()
        {
            var customizableRandomizer = new CustomizableRandomizer<int>(_commonValidChanceValuePairs);

            int randomlyObtainedItem = customizableRandomizer.GetValue();

            bool IsRandomlyObtainedItemCorrect = _values.Contains(randomlyObtainedItem);

            Assert.IsTrue(IsRandomlyObtainedItemCorrect);
        }

        /// <summary>
        /// В данном тесте есть шанс, на то что он окажется ложным, поскольку он является рандомным
        /// </summary>
        [Test]
        public void GetItem_ReturnsCorrectNumberOfItemsAtGivenChances()
        {
            var customizableRandomizer = new CustomizableRandomizer<int>(_commonValidChanceValuePairs);

            const int ArrayLength = 100;

            Dictionary<int, int> countOfReceivedItemsTable = new(KeyValuePairsArrayLength);

            for (int i = 0; i < KeyValuePairsArrayLength; i++)
                countOfReceivedItemsTable.Add(i, _values[i]);

            for (int i = 0; i < ArrayLength; i++)
            {
                int receivedRandomItem = customizableRandomizer.GetValue();
                countOfReceivedItemsTable[receivedRandomItem]++;
            }

            int prevCountOfReceivedItem = countOfReceivedItemsTable[0];
            for (int i = 1; i < countOfReceivedItemsTable.Count; i++)
            {
                bool isCountOfReceivedItemsCorrect = countOfReceivedItemsTable[i] > prevCountOfReceivedItem;
                prevCountOfReceivedItem = countOfReceivedItemsTable[i];
                Assert.IsTrue(isCountOfReceivedItemsCorrect);
            }
        }
    }
}
