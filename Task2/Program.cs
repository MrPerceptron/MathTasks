using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Task2
{
    public class Program
    {
        const int NUMBERLENGTH = 9;

        static void Main()
        {
            var sw = new Stopwatch();

            sw.Start(); // -<<<<<

            var arr = Palindrom.GetPalendromArr(NUMBERLENGTH);
            Palindrom.ShowArray(arr);

            sw.Stop(); // -<<<<<

            Console.WriteLine($"ElapsedMilliseconds: {sw.ElapsedMilliseconds}\tElapsed: {sw.Elapsed}"); // 19 - 43 ||
            Console.WriteLine($"Является ли корректное количество палиндромов: {arr.Count == Palindrom.GetCountOfReceivedPalindromes(NUMBERLENGTH)}");
        }
    }

        
    public class Palindrom
    {
        private static Dictionary<int, Dictionary<int, int>> _degreeDictionary = new()
        { // 1: Длина числа; 2:
            { 3, new Dictionary<int, int>() { { 1, 1 } } },
            { 4, new Dictionary<int, int>() { { 2, 1 } } },
            { 5, new Dictionary<int, int>() { { 1, 2 }, { 3, 1 } } },
            { 6, new Dictionary<int, int>() { { 2, 2 }, { 4, 1 } } },
            { 7, new Dictionary<int, int>() { { 1, 3 }, { 3, 2 }, { 5, 1 } } },
            { 8, new Dictionary<int, int>() { { 2, 3 }, { 4, 2 }, { 6, 1 } } },
            { 9, new Dictionary<int, int>() { { 1, 4 }, { 3, 3 }, { 5, 2 }, { 7, 1 } } }
        };

        public static void ShowArray(IEnumerable<int> arr)
        {
            foreach (var item in arr)
                Console.WriteLine(item);
        }

        public static bool IsDivideBy(int denominator, int numerator)
        {
            return denominator / numerator * numerator == denominator;
        }

        public static bool IsPalindromNumber(int palindromNumber)
        {
            string palindromTextNumber = palindromNumber.ToString();
            for (int i = 0; i < palindromTextNumber.Length / 2; i++)
                if (palindromTextNumber[i] != palindromTextNumber[^(i + 1)])
                    return false;
            return true;
        }

        public static int GetMinimalPalindrom(int lenOfNumber)
        {
            int value = lenOfNumber == 0 || lenOfNumber > 10 ? throw new ArgumentException($"Value cannot be {lenOfNumber}") : 1;

            for (uint i = 1; i < lenOfNumber; i++)
                value *= 10;

            return value == 1 ? 0 : value + 1;
        }

        public static int GetCountOfReceivedPalindromes(int lenOfNumber)
        {
            if (lenOfNumber == 1)
                return 10;
            int levelOfReceivedPalindromes = lenOfNumber % 2 == 0 ? lenOfNumber / 2 : (lenOfNumber + 1) / 2;
            return (int)Math.Pow(10, levelOfReceivedPalindromes - 1) * 9;
        }

        public static List<int> GetPalendromArr(int lenOfNumber)
        {
            int minimalPalindrom = GetMinimalPalindrom(lenOfNumber);
            int currentPalindrom = minimalPalindrom;
            int countOfReceivedPalindromes = GetCountOfReceivedPalindromes(lenOfNumber);

            int[][] prePalindromArrays = new int[(lenOfNumber - 1) / 2][];

            List<int> palindromArr = new(countOfReceivedPalindromes);
            FillDependentPalindromes(prePalindromArrays, lenOfNumber);

            for (int i = 0; i < countOfReceivedPalindromes; i++)
            {
                if (lenOfNumber < 3)
                {
                    palindromArr.Add(minimalPalindrom);
                    minimalPalindrom += lenOfNumber == 1 ? 1 : 11;
                }
                else
                {
                    for (int j = 0; j < prePalindromArrays.Length; j++)
                    {
                        if (lenOfNumber % 2 == 0 && lenOfNumber > 2)
                            palindromArr.Add(currentPalindrom);

                        for (int k = 0; k < prePalindromArrays[j].Length; k++)
                        {
                            var numberLength = prePalindromArrays[j][k].ToString().Length;
                            palindromArr.Add(currentPalindrom + prePalindromArrays[j][k] * (int)Math.Pow(10, _degreeDictionary[lenOfNumber][numberLength]));
                            i++;
                        }
                    }
                    currentPalindrom += minimalPalindrom;
                }
            }

            return palindromArr;
        }

        private static void FillDependentPalindromes(int[][] fillingArray, int lenOfNumber)
        {
            for (int i = 0; i < fillingArray.Length; i++)
            {
                var arr = GetPalendromArr(lenOfNumber % 2 == 0 ? i * 2 + 2 : i * 2 + 1);
                fillingArray[i] = new int[arr.Count];

                for (int j = 0; j < arr.Count; j++)
                    fillingArray[i][j] = arr[j];
            }
        }
    }
    
    /*
               1! При переходе на новвую длину цифр нужно прибавлять <2>
               2! При итерации на следующую единицу, нужно прибавлять <1>, в ином случае, при итерации на следующий десяток, сотню, тысячу и т.к прибавлять <11>
               3! При нахождении единичного нового палиндрома нужно прибавлять <1>, в ином случае, <11>
               
                1: 8 + 1 = 9 + 2 = <11>                                             можно получить <9> палиндромов
                2: 88 + 11 = 99 + 2 = <101>                                         можно получить <9> палиндромов
                3: 898 + 11 = 909 + <10> = 919 ... 999 + 2 = <1001>                 можно получить <9*10> палиндромов
                4: 8998 + 11 = 9009 ... 9889 + <110> = 9999 + 2 = <10001>           можно получить <9*10> палиндромов
                5: 89998 + 11 = 90009 ... 98889 + <1110> = 99999 + 2 = <100001>     можно получить <9*10*10> палиндромов
                6:                                                                  можно получить <9*10*10> палиндромов
                7:                                                                  можно получить <9*10*10*10> палиндромов
                8: ...
      */
}
