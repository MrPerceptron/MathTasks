using System.Collections.Generic;

namespace CustomizableRandomizer
{
    public class SortedChanceValuePairs<TValue> : ChanceValuePairs<TValue>
    {
        public SortedChanceValuePairs(KeyValuePair<float, TValue>[] chanceValuePairs) : base(chanceValuePairs)
        {
            SortChancesWithValues();
        }

        protected void SortChancesWithValues()
        {
            if (Count == 1)
                return;

            for (int prev = 0, next = 1; next < Count; prev++, next++)
            {
                if (Chances[prev] > Chances[next])
                {
                    (Chances[prev], Chances[next]) = (Chances[next], Chances[prev]);
                    (Values[prev], Values[next]) = (Values[next], Values[prev]);
                }
            }
        }
    }
}
