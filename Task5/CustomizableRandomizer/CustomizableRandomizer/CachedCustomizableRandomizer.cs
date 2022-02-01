using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomizableRandomizer
{
    public class CachedCustomizableRandomizer<TValue> where TValue : notnull
    {
        protected readonly Dictionary<TValue[], CustomizableRandomizer<TValue>> _cashedCustomizableRandomizer = new();

        protected readonly Dictionary<TValue, double> _valueChancePair = new();

        protected readonly TValue[] _allSupportedValues;

        public CachedCustomizableRandomizer(KeyValuePair<double, TValue>[] chancesAndValues)
        {
            if (chancesAndValues is null)
                throw new ArgumentNullException(nameof(chancesAndValues));

            ChanceValuePairs<TValue> chanceValuePairs = new(chancesAndValues);

            _allSupportedValues = chanceValuePairs.Values;

            CustomizableRandomizer<TValue> defaultRandomizableCustomizer = new(chanceValuePairs);

            _cashedCustomizableRandomizer.Add(chanceValuePairs.Values, defaultRandomizableCustomizer);

            for (int i = 0; i < chanceValuePairs.Count; i++)
                _valueChancePair.Add(chanceValuePairs.Values[i], chanceValuePairs.Chances[i]);
        }

        public TValue GetValue(params TValue[] missingValues)
        {
            if (missingValues.Length > _allSupportedValues.Length)
                throw new ArgumentException
                    ($"{nameof(missingValues)} не может содержать значений больше чем в {nameof(_allSupportedValues)}");

            TValue[] availableValues = _allSupportedValues.Except(missingValues).ToArray();

            CustomizableRandomizer<TValue> customizableRandomizer = GetOrUpdateCashedCustomizableRandomizer(availableValues);

            return customizableRandomizer.GetValue();
        }

        private CustomizableRandomizer<TValue> GetOrUpdateCashedCustomizableRandomizer(TValue[] values)
        {
            if (_cashedCustomizableRandomizer.ContainsKey(values) == false)
            {
                KeyValuePair<double, TValue>[] chancesWithValues = GetChancesWithValuesWithConfiguredChances(values);

                CustomizableRandomizer<TValue> notUsedCustomizableRandomizer = new(chancesWithValues);

                _cashedCustomizableRandomizer.Add(values, notUsedCustomizableRandomizer);
            }
            return _cashedCustomizableRandomizer[values];
        }

        private KeyValuePair<double, TValue>[] GetChancesWithValuesWithConfiguredChances(TValue[] values)
        {
            double sumOfChancesByValue = values.Sum(value => _valueChancePair[value]);

            double notUsedChances = CustomizableRandomizer<object>.MaxChance - sumOfChancesByValue;

            double coefficientPerValue = notUsedChances / values.Length;

            var chancesWithValuesWithConfiguredChances = new KeyValuePair<double, TValue>[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                TValue currentValue = values[i];

                double chance = _valueChancePair[currentValue] + coefficientPerValue;

                chancesWithValuesWithConfiguredChances[i] = new(chance, currentValue);
            }

            return chancesWithValuesWithConfiguredChances;
        }
    }
}
