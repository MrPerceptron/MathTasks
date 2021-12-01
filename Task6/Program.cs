using System;
using Task6.Breakfast;
using Task6.Breakfast_Test;
/*
            Condition[] conditions = new Condition[6] {
                new Condition(x => x.LastName == "Миз Осборн",
                    x => x.LastName == "Миз Мартин",
                    x => x.LastName == "Миз Льюис")
                    { ItemPosition = Condition.Position.Between },
                new Condition(x => x.FirstName == "Эллен",
                    x => x.LastName =="Миз Норрис",
                    x => x.FirstName == "Кэти")
                    { ItemPosition = Condition.Position.Between },
                new Condition (x => x.LastName == "Миз Льюис",
                    x => x.FirstName == "Эллен",
                    x => x.FirstName == "Эллис")
                    { ItemPosition = Condition.Position.Between },
                new Condition(x => x.LastName == "Миз Паркс",
                    x => x.FirstName == "Бетти")
                    { ItemPosition = Condition.Position.Left },
                new Condition (x => x.FirstName == "Бетти",
                    x =>x.LastName == "Миз Мартин")
                    { ItemPosition = Condition.Position.Left },
                new Condition (x => x.FirstName == "Дорис")
                    { ItemPosition = Condition.Position.LastNameEqual }
            };
            */

/*
Пять дам завтракают, сидя за круглым столом. Миз Осборн сидит между Миз Льюис и Миз Мартин.
Эллен сидит между Кэти и Миз Норрис. Миз Льюис сидит между Эллен и Эллис.
Кэти и Дорис — сестры. Слева от Бетти сидит Миз Паркс, а справа от нее — Миз Мартин.
Сопоставьте имена и фамилии.
*/

// TODO: Зарефакторить conditionInfoArr
// TODO: Заменить IList на Readonly в классах стратегии
namespace Task6
{
    public static class Program
    {
        static void Main()
        {
            //var conditions = BreakfastConditionsGenerator.GetConditions();
            //(CircleList<Person> answer, bool isSolved) = new Solution().GetSolution(conditions);

            Condition[] conditions = new Condition[4] {
                new Condition(x=>x.LastName == "Миз Льюис", x=>x.LastName == "Миз Осборн", x=>x.FirstName == "Бэти") { ItemPosition = Condition.Position.Between },
                new Condition(x=>x.FirstName == "Бэти", x=>x.LastName == "Миз Осборн", x=>x.FirstName == "Кэти") { ItemPosition = Condition.Position.Between },
                new Condition(x=>x.FirstName == "Кэти", x=>x.LastName == "Миз Мартин") { ItemPosition = Condition.Position.Left },
                new Condition(x=>x.FirstName == "Дорис") { ItemPosition = Condition.Position.LastNameEqual },
            };

            Condition[] conditions2 = new BreakfastConditionsGenerator().GenerateConditions();
            (CircleList<Person> answer, bool isSolved) = new Solution().GetSolution(conditions2);
            ShowAnswer(answer);
        }
        public static void ShowAnswer(CircleList<Person> persons)
        {
            foreach (var person in persons)
                Console.WriteLine($"{person.FirstName}\t{person.LastName}");
        }
    }
}
