using System;

/*
    Title: Коза на привязи

    Коза пасётся на привязи длиной 6 метров, прикрепленной к наружному углу сарая размером 4 на 5 метров.
    На какой площади коза может съесть траву?
 */

namespace Task4
{
    internal class Program
    {
        static void Main()
        {
            var mainCircle = new Circle(6 / 2);
            var circleWithR1 = new Circle(1);
            var circleWithR2 = new Circle(2);

            double thirdPartPerThreeMainCircleArea = (mainCircle.Area / 4) * 3;
            double thirdPartPerOneCircleWithR1Area = (circleWithR1.Area / 4) * 1;
            double thirdPartPerOnecircleWithR2Area = (circleWithR2.Area / 4) * 1;

            double answer =
                thirdPartPerThreeMainCircleArea +
                thirdPartPerOneCircleWithR1Area +
                thirdPartPerOnecircleWithR2Area;

            Console.WriteLine(answer);
        }
    }

    class Circle
    {
        public double Radius { get; private set; }
        public double Area { get; private set; }

        public Circle(double radius)
        {
            if (radius < 0)
                throw new ArgumentException($"Параметр {nameof(radius)} не может быть меньше нуля");

            Radius = radius;
            Area = Math.PI * Math.Pow(radius, 2);
        }
    }
}
