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
            Person[] it1 = new Person[2] {
                new Person().AddLastName("Миз Паркс"),
                new Person().AddFirstName("Бетти")
            };
            Person[] it2 = new Person[2] {
                new Person().AddFirstName("Бетти"),
                new Person().AddLastName("Миз Мартин")
            };

            foreach (Person item in UnitePersons(it1, it2))
                Console.WriteLine($"FirstName = {item.FirstName}\tLastName = {item.LastName}");
        }

        /// <summary>
        /// Объединяет 2 массива
        /// </summary>
        /// <param name="item1">Объединяемый массив</param>
        /// <param name="item2">Объединяемый массив</param>
        /// <returns>Объеденённый массив</returns>
        static Person[] UnitePersons(Person[] item1, Person[] item2)
        {
            if ((item1.Length, item2.Length) != (2, 2))
                throw new ArgumentException($"Длина массива {nameof(item1)} или {nameof(item2)} не является двум");

            if (item1.SequenceEqual(item2))
                throw new ArgumentException($"Все значения {nameof(item1)} не могут совпадать с {nameof(item2)}");

            const int CountOfReturnArr = 3;

            Person[] returnValue = new Person[CountOfReturnArr];

            for (int i = 0; i < CountOfReturnArr; i++)
            {
                if (item1[i] == item2[i])
                    returnValue[i] = new Person().AddFirstName(item1[i].FirstName.value);
                else // ДОПИСАТЬ ТУТ НОРМАЛЬНО МЕТОД ОБЪЕДИНЕНИЯ И ПРИДУМАТЬ КАК РЕШИТЬ ПРОВЕРКУ
                    returnValue[i] = new Person().AddFirstName(item1[i].FirstName.value == null ? item2[i].FirstName.value : item1[i].FirstName.value);
            }
            return returnValue;
        }
    }
}
