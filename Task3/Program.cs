using System;

/*
    Title: Шахматные клетки

    Существовала точка зрения, что на обычной шахматной доске 204 квадрата.
    Можете ли вы подтвердить эту точку зрения?
 */

namespace Task3
{
    internal class Program
    {
        static void Main()
        {
            Console.WriteLine(SquaresCalculator.CalculateCountOfSquaresUsingLoop(8, 8));
            Console.WriteLine(SquaresCalculator.CalculateCountOfSquaresUsingRecursionAndProcessing(8, 8));
        }

        static class SquaresCalculator
        {
            public static int CalculateCountOfSquaresUsingLoop(int x, int y)
            {
                if (x < 1 || y < 1)
                    throw new ArgumentException($"{nameof(x)} или {nameof(y)} не может быть меньше 1");

                for (int result = 0; ; --x, --y)
                {
                    if (x == 0 || y == 0)
                        return result;

                    result += x * y;
                }
            }

            public static int CalculateCountOfSquaresUsingRecursionAndProcessing(int x, int y)
            {
                if (x < 1 || y < 1)
                    throw new ArgumentException($"{nameof(x)} или {nameof(y)} не может быть меньше 1");

                return CalculateCountOfSquaresUsingRecursion(x, y);
            }

            private static int CalculateCountOfSquaresUsingRecursion(int x, int y)
            {
                if (x == 0 || y == 0)
                    return 0;
                return x * y + CalculateCountOfSquaresUsingRecursion(x - 1, y - 1);
            }
        }
    }
}
