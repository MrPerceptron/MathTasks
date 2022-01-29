using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Benchmark
{
    [BenchmarkCategory]
    [MemoryDiagnoser]
    [RankColumn]
    public class Benchmark
    {
        [Benchmark]
        public void Test1() 
        {
            DoesItPalindrom(567898765);
        }

        [Benchmark]
        public void Test2()
        {
            DoesItPalindromUsingConvertation(567898765);
        }

        public static bool DoesItPalindrom(int number)
        {
            Stack<int> stack = new();

            int copiedNumber = number;

            while (number > 0)
            {
                stack.Push(number % 10);
                number /= 10;
            }

            foreach (var item in stack)
            {
                if (copiedNumber % 10 != item)
                    return false;
                copiedNumber /= 10;
            }

            return true;
        }

        public static bool DoesItPalindromUsingConvertation(int number)
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
    }
}
