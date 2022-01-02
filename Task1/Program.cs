using System;

namespace Task1
{
    internal class Program
    {
        /*
        Iter = 1         value = 1
        Iter = 2         value = 3
        Iter = 3         value = 7
        Iter = 4         value = 15
        Iter = 5         value = 31
        Iter = 6         value = 63
        Iter = 7         value = 127
        Iter = 8         value = 255
        Iter = 9         value = 511
        Iter = 10        value = 1023
         */
        static void FindNumber(uint x)
        {
            if (x > 31)
                throw new ArgumentException($"Данное значение нельзя вычислить, поскольку оно выходит за приделы вместимости типа {typeof(uint)}");
            uint number = 0;
            for (int i = 0; i < x; i++)
                number = number * 2 + 1;

            Console.WriteLine(number);
        }

        static void Main()
        {
            try
            {
                FindNumber(8);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
