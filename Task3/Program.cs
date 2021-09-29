using System;

namespace Task3
{
    internal class Program
    {
        // Суть в том, чтобы находить количество КВАДРАТОВ, как на шахмотной доске
        static void Main()
        {
            Console.WriteLine(CalculateCountOfSquares(8, 8));
        }

        public static int CalculateCountOfSquares(int x, int y) => (x == 0 || y == 0) ? 0 : x * y + CalculateCountOfSquares(x - 1, y - 1);
    }
}