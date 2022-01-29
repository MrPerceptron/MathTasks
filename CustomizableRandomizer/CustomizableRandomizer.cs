using System;

namespace CustomizableRandomizer
{
    public class CustomizableRandomizer<TItem>
    {
        const int MaxChance = 100;

        protected static Random _random = new();

        protected readonly ChanceValuePairs<TItem> _chanceValuePairs;

        public CustomizableRandomizer(SortedChanceValuePairs<TItem> chanceValuePairs)
        {
            _chanceValuePairs = chanceValuePairs;
        }

        public TItem GetItem()
        {
            return default;
        }

        private static double GetRandomValueByChance(float itemChance) 
        {
            return (_random.Next(MaxChance) + _random.NextDouble()) / (MaxChance / itemChance);
        }
    }
}
