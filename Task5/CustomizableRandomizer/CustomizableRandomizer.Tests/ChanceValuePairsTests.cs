using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomizableRandomizer.Tests
{
    internal class ChanceValuePairsTests
    {
        const int KeyValuePairsArrayLength = 4;

        double[] _chances = new double[KeyValuePairsArrayLength] { 10, 20, 30, 40 };
        int[] _values = new int[KeyValuePairsArrayLength] { 0, 1, 2, 3 };

        KeyValuePair<double, int>[] _keyValuePair;
        ChanceValuePairs<int> _commonValidChanceValuePairs;

        [SetUp]
        public void Setup()
        {
            _keyValuePair = new KeyValuePair<double, int>[KeyValuePairsArrayLength];

            for (int i = 0; i < KeyValuePairsArrayLength; i++)
                _keyValuePair[i] = new(_chances[i], _values[i]);

            _commonValidChanceValuePairs = new(_keyValuePair);
        }

        [Test]
        public void Chances_ReturnsNotChangedArrayOfChancesInPassedParameter()
        {
            for (int i = 0; i < _commonValidChanceValuePairs.Count; i++)
                Assert.AreEqual(_chances[i], _commonValidChanceValuePairs.Chances[i]);
        }

        [Test]
        public void Values_ReturnsNotChangedArrayOfValuesInPassedParameter()
        {
            for (int i = 0; i < _commonValidChanceValuePairs.Count; i++)
                Assert.AreEqual(_values[i], _commonValidChanceValuePairs.Values[i]);
        }

        [Test]
        public void Count_ReturnsArrayLengthOfPassedParameter()
        {
            Assert.AreEqual(KeyValuePairsArrayLength, _commonValidChanceValuePairs.Count);
        }

        [Test]
        public void Counstructor_WithSupportedKeyValuePairParameter_ReturnsValidInstance()
        {
            Assert.IsInstanceOf<ChanceValuePairs<int>>(new ChanceValuePairs<int>(_keyValuePair));
        }

        [Test]
        public void AsignValuesAndValidate_WithNullParameter_ThrowsArgumentNullException()
        {
            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.
                Message.EqualTo("Value cannot be null. (Parameter 'chanceValuePairs')"),
                () => new ChanceValuePairs<int>(null));
        }

        [Test]
        public void AsignValuesAndValidate_WithEmptyArrayParameter_ThrowsArgumentException()
        {
            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                Message.EqualTo("chanceValuePairs не содержит элементов"),
                () => new ChanceValuePairs<int>(Array.Empty<KeyValuePair<double, int>>()));
        }

        [Test]
        public void AsignValuesAndValidate_WithNegativeOrZeroNumbersInArrayParameter_ThrowsArgumentException()
        {
            double[] chancesWithNegativeOrZeroNumbers = new double[KeyValuePairsArrayLength] { 0, -1, -2, -3 };

            var keyValuePairWithIncorrectChances = new KeyValuePair<double, int>[KeyValuePairsArrayLength];

            for (int i = 0; i < KeyValuePairsArrayLength; i++)
                keyValuePairWithIncorrectChances[i] = new(chancesWithNegativeOrZeroNumbers[i], _values[i]);

            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                Message.EqualTo("chanceValuePairs не может содержаться шанс меньше или являющимся нулём"),
                () => new ChanceValuePairs<int>(keyValuePairWithIncorrectChances));
        }

        [Test]
        public void AsignValuesAndValidate_WithChancesArrayThatSumNotEquals100_ThrowsArgumentException()
        {
            double[] chancesWithSupportedValuesAndIncorrectSum = new double[KeyValuePairsArrayLength] { 1, 2, 3, 4 };

            var keyValuePairWithIncorrectChances = new KeyValuePair<double, int>[KeyValuePairsArrayLength];

            for (int i = 0; i < KeyValuePairsArrayLength; i++)
                keyValuePairWithIncorrectChances[i] = new(chancesWithSupportedValuesAndIncorrectSum[i], _values[i]);

            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                Message.EqualTo("chanceValuePairs должен содержать сумму шансов, равняющийся числу 100"),
                () => new ChanceValuePairs<int>(keyValuePairWithIncorrectChances));
        }
    }
}
