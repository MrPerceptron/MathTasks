using System;
using System.Collections.Generic;

namespace Task6
{
    public static class ListExtension
    {
        public static void FullAssign<T>(this IList<T> firstArr, IList<T> secondArr)
        {
            if (firstArr.Count != secondArr.Count)
                throw new ArgumentException("Массивы не могут быть разной длины");

            firstArr = secondArr;

            for (int i = 0; i < firstArr.Count; i++)
                firstArr[i] = secondArr[i];
        }
    }
}
