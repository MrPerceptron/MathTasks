using System;
using System.Collections.Generic;

namespace CustomizableRandomizer
{
    internal class Program
    {
        static Random _random = new();

        static void Main()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine((_random.Next(0, 100) + _random.NextDouble()) / (100 / 20));
                Console.WriteLine((_random.Next(0, 100) + _random.NextDouble()) / (100 / 30));
                Console.WriteLine((_random.Next(0, 100) + _random.NextDouble()) / (100 / 40));
                Console.WriteLine((_random.Next(0, 100) + _random.NextDouble()) / (100 / 10));
                Console.WriteLine("======================");
            }
        }
    }
}
