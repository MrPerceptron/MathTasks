using System;

/*
    Title: Палиндромы

    Палиндромом называется число, например 12321, если оно читается одинаково — как слева направо, так и справа налево.
    Один мой друг уверяет, что все четырехзначные палиндромы делятся на 11. Так ли это?
 */

namespace Task2
{
    public class Program
    {
        public static bool IsPalindrom(int number)
        {
            string strNumber = number.ToString();

            int halfAstrNumberLength = strNumber.Length / 2 + 1;

            for (int i = 0; i < halfAstrNumberLength; i++)
            {
                if (strNumber[i] != strNumber[^(i + 1)])
                    return false;
            }

            return true;
        }

        public static bool CheckAreAllFourDigitPalindromesEqualEleven()
        {
            for (int i = 1000; i < 10000; i++)
            {
                if (IsPalindrom(i) && i / 11 == 0)
                    return false;
            }
            return true;
        }

        static void Main()
        {
            Console.WriteLine(CheckAreAllFourDigitPalindromesEqualEleven());
        }
    }
}
