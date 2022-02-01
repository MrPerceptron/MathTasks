using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace CustomizableRandomizer.Tests
{
    internal class CachedCustomizableRandomizerTests
    {
        static KeyValuePair<double, int>[] _chancesAndValues = new KeyValuePair<double, int>[]
        {
            new(10, 0),
            new(20, 1),
            new(30, 2),
            new(40, 3)
        };

        static CachedCustomizableRandomizer<int> _commonCachedCustomizableRandomizer = new(_chancesAndValues);

        [Test]
        public void Constructor_WithSupportedKeyValuePairParameter_ReturnsValidInstance()
        {
            Assert.IsInstanceOf<CachedCustomizableRandomizer<int>>(_commonCachedCustomizableRandomizer);
        }

        [Test]
        public void Counstructor_WithNullKeyValuePairParameter_ThrowsArgumentNullException()
        {
            KeyValuePair<double, int>[] chancesAndValues = null;

            Assert.Throws(Is.TypeOf<ArgumentNullException>().And.
                Message.EqualTo("Value cannot be null. (Parameter 'chancesAndValues')"),
                () => new CachedCustomizableRandomizer<int>(chancesAndValues));
        }

        [Test]
        public void GetValue_WithExceededElementsCountArray_ThrowsArgumentException()
        {
            int[] arrayWithExceededElementsCount = new int[] { 1, 2, 3, 4, 5 };

            Assert.Throws(Is.TypeOf<ArgumentException>().And.
                Message.EqualTo("missingValues не может содержать значений больше чем в _allSupportedValues"),
                () => _commonCachedCustomizableRandomizer.GetValue(arrayWithExceededElementsCount));
        }

        [Test]
        public void GetValue_WithMissingValues_ReturnsRandomValueThatNotEqualsAnyMissingValue()
        {
            int expectedNotMissedValue = 3;
            int receivedExpectedNotMissedValue = _commonCachedCustomizableRandomizer.GetValue(0, 1, 2);

            Assert.AreEqual(expectedNotMissedValue, receivedExpectedNotMissedValue);
        }
    }
}
