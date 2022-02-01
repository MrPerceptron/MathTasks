using System;
using System.Collections.Generic;

namespace CustomizableRandomizer
{
    public class ChanceValuePairs<TValue>
    {
        public double[] Chances { get; private set; }
        public TValue[] Values { get; private set; }
        public int Count { get; private set; }

        public const int MaxChance = 100;

        public ChanceValuePairs(KeyValuePair<double, TValue>[] chanceValuePairs)
        {
            AsignValuesAndValidate(chanceValuePairs);
        }

        protected void AsignValuesAndValidate(KeyValuePair<double, TValue>[] chanceValuePairs)
        {
            if (chanceValuePairs is null)
                throw new ArgumentNullException(nameof(chanceValuePairs));

            Count = chanceValuePairs.Length;

            if (Count is 0) 
                throw new ArgumentException($"{nameof(chanceValuePairs)} не содержит элементов");

            Chances = new double[Count];
            Values = new TValue[Count];

            double maxChance = 0;
            for (int i = 0; i < Count; i++)
            {
                Chances[i] = chanceValuePairs[i].Key;

                if (Chances[i] <= 0)
                    throw new ArgumentException
                        ($"{nameof(chanceValuePairs)} не может содержаться шанс меньше или являющимся нулём");

                Values[i] = chanceValuePairs[i].Value;
                maxChance += chanceValuePairs[i].Key;
            }
            if (maxChance != MaxChance)
                throw new ArgumentException($"{nameof(chanceValuePairs)} должен содержать сумму шансов, равняющийся числу 100");
        }
    }
}
