using System;
using System.Collections.Generic;

namespace CustomizableRandomizer
{
    public class CustomizableRandomizer<TValue> where TValue : notnull
    {
        public const int MaxChance = 100;

        protected readonly static Random _random = new();

        protected readonly ChanceValuePairs<TValue> _chanceValuePairs;

        public CustomizableRandomizer(ChanceValuePairs<TValue> chanceValuePairs)
        {
            if(chanceValuePairs is null)
                throw new ArgumentNullException(nameof(chanceValuePairs));

            _chanceValuePairs = chanceValuePairs;
        }

        public CustomizableRandomizer(KeyValuePair<double, TValue>[] chancesWithValues)
        {
            if (chancesWithValues is null)
                throw new ArgumentNullException(nameof(chancesWithValues));

            ChanceValuePairs<TValue> chanceValuePairs = new(chancesWithValues);

            _chanceValuePairs = chanceValuePairs;
        }

        public virtual TValue GetValue()
        {
            double maxgGeneratedNumberByChance = GetRandomNumberByChance(_chanceValuePairs.Chances[0]);
            int IndexOfMaxGeneratedNumberByChance = 0;

            for (int i = 1; i < _chanceValuePairs.Count; i++)
            {
                double generatedNumberByChance = GetRandomNumberByChance(_chanceValuePairs.Chances[i]);

                if (maxgGeneratedNumberByChance < generatedNumberByChance)
                {
                    maxgGeneratedNumberByChance = generatedNumberByChance;
                    IndexOfMaxGeneratedNumberByChance = i;
                }
            }

            return _chanceValuePairs.Values[IndexOfMaxGeneratedNumberByChance];
        }

        private static double GetRandomNumberByChance(double itemChance)
        {
            return (_random.Next(MaxChance) + _random.NextDouble()) / (MaxChance / itemChance);
        }
    }
}
