using System;
using System.Collections.Generic;

namespace CustomizableRandomizer
{
    public class ChanceValuePairs<TValue>
    {
        public float[] Chances { get; private set; }
        public TValue[] Values { get; private set; }
        public int Count { get; private set; }

        public ChanceValuePairs(KeyValuePair<float, TValue>[] chanceValuePairs)
        {
            AsignValuesAndValidate(chanceValuePairs);
        }

        protected void AsignValuesAndValidate(KeyValuePair<float, TValue>[] chanceValuePairs)
        {
            if (chanceValuePairs is null)
                throw new ArgumentNullException(nameof(chanceValuePairs));

            Count = chanceValuePairs.Length;

            if (Count is 0)
                throw new ArgumentException($"{nameof(chanceValuePairs)} не содержит элементов");

            Chances = new float[Count];
            Values = new TValue[Count];

            for (int i = 0; i < Count; i++) // Проверить что будет, если 2 раза конвертнуть значения в сигмоиду
            {
                float compressedChance = CalculateSigmoid(chanceValuePairs[i].Key);

                Chances[i] = compressedChance;
                Values[i] = chanceValuePairs[i].Value;
            }
        }

        protected void AsignValuesAndValidate(float[] chances, TValue[] values)
        {
            if (chances is null)
                throw new ArgumentNullException(nameof(chances));

            if (values is null)
                throw new ArgumentNullException(nameof(values));

            if (chances.Length != values.Length)
                throw new ArgumentException("Параметры не могут содержать разное количество элементов");

            var chanceValuePairs = new KeyValuePair<float, TValue>[chances.Length];

            for (int i = 0; i < chances.Length; i++)
                chanceValuePairs[i] = new KeyValuePair<float, TValue>(chances[i], values[i]);

            AsignValuesAndValidate(chanceValuePairs);
        }

        private static float CalculateSigmoid(float number)
        {
            return (float)(1 / (1 + Math.Pow(Math.E, -number)));
        }
    }
}
