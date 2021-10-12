using System;
using System.Collections.Generic;
using System.Linq;

namespace Task5
{
    /*
    Пять дам завтракают, сидя за круглым столом. Миз Осборн сидит между Миз Льюис и Миз Мартин.
    Эллен сидит между Кэти и Миз Норрис. Миз Льюис сидит между Эллен и Эллис.
    Кэти и Дорис — сестры. Слева от Бетти сидит Миз Паркс, а справа от нее — Миз Мартин.
    Сопоставьте имена и фамилии.
    */
    internal class Program
    {
        static void Main()
        {
            Breakfast breakfast = new(new() { "Эллис", "Бетти", "Дорис", "Эллен", "Кэти" }, new() { "Миз Норрис", "Миз Льюис", "Миз Паркс", "Миз Мартин", "Миз Осборн" });
            breakfast.SetFullDifference(breakfast.SecondNames);

            var arr = Breakfast.GetFullDifference(breakfast.SecondNames);
            foreach (var item in arr)
                ShowArray(item);
            Console.WriteLine();

            ShowArray(breakfast.AddCondition((x) => Left.GetCondition(x, "Миз Мартин", "Миз Норрис")));
        }
        static bool GetArray(CircleList<string> arr, params string[] args)
        {
            if (args.Length > arr.Count)
                throw new ArgumentException($"Количество эле");



            return true;
        }

        static void ShowArray(List<string> arr)
        {
            foreach (var item in arr)
                Console.Write($"{item}\t");
            Console.WriteLine();
        }
    }
    class Breakfast
    {
        public int ArraysCount { get; }
        public CircleList<string> FirstNames { get; }
        public CircleList<string> SecondNames { get; }
        public CircleList<CircleList<string>> ResultArr { get; private set; }

        public Breakfast(CircleList<string> firstNames, CircleList<string> secondNames)
        {
            if (firstNames.Count == secondNames.Count)
                ArraysCount = firstNames.Count;
            else
                throw new Exception($"Количество элементов {nameof(firstNames)} и {nameof(secondNames)} должны быть равны");

            FirstNames = firstNames;
            SecondNames = secondNames;
        }

        public CircleList<string> AddCondition(Func<CircleList<string>, bool> condition)
        {
            for (int i = 0; i < ArraysCount; i++)
                if (condition(ResultArr[i]))
                    return ResultArr[i];

            throw new Exception("Не удалось найти соответствия");
        }

        public void SetFullDifference(CircleList<string> arr) => ResultArr = GetFullDifference(arr);
        static public CircleList<CircleList<string>> GetFullDifference(CircleList<string> arr)
        {
            if (arr.Count == 1)
                return new() { arr };

            CircleList<CircleList<string>> returnArr = GetMiddleDifference(arr);

            for (int i = 0; i < returnArr.Count; i++)
                for (int j = 0; j < returnArr[i].Count + 1; j++)
                    if (!returnArr[i].Contains(arr[j]))
                        returnArr[i].Add(arr[j]);

            return returnArr;
        }
        static private CircleList<CircleList<string>> GetMiddleDifference(CircleList<string> arr)
        {
            int countOfReturnArrays = (arr.Count % 2 == 0 ? (arr.Count - 1) * 2 : arr.Count); // Не факт что правильно, но вычисляет количество возвращаемых массивов
            CircleList<CircleList<string>> returnArr = new(countOfReturnArrays);

            for (int i = 0; i < countOfReturnArrays; i++)
            {
                for (int j = 0; j < arr.Count; j++)
                {
                    if (arr[i] != arr[j])
                    {
                        CircleList<string> newArr = new(2) { arr[i], arr[j] };

                        if (!returnArr.Select(x => x.SequenceEqual(newArr)).Contains(true))
                            returnArr.Add(newArr);
                    }
                }
            }

            return returnArr;
        }
    }
}
