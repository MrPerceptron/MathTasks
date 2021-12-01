using System;
using System.Collections.Generic;

namespace Task3
{
    internal class Program
    {
        // Суть в том, чтобы находить количество КВАДРАТОВ, как на шахмотной доске
        static void Main()
        {
            Console.WriteLine(CalculateCountOfSquaresUsingRecursion(3, 5));
            Console.WriteLine(CalculateCountOfSquaresUsingLoop(3, 5));
        }

        public static int CalculateCountOfSquaresUsingRecursion(int x, int y) => (x == 0 || y == 0) ? 0 : x * y + CalculateCountOfSquaresUsingRecursion(x - 1, y - 1);

        public static int CalculateCountOfSquaresUsingLoop(int x, int y)
        {
            if (x < 1 || y < 1)
                throw new ArgumentException($"{nameof(x)} или {nameof(y)} не может быть меньше 1");

            int result = 0;

            for (; x != 0 || x != 0; x--, y--)
                result += x * y;

            return result;
        }
    }
}
