using Task5.Common.Interfaces;
using Task5.Common.Models;

namespace Task5.BreakfastTaskGenerator
{
    public class BaseTableListGenerator : ITableListGenerator
    {
        public PersonsTableList Generate(int count)
        {
            PersonsTableList list = new();

            for (int i = 0; i < count; i++)
            {
                list.Add(new DataListItem
                {
                    FirstName = $"Имя_{i}",
                    LastName = $"Фамилия_{i}"
                });
            }
            return list;
        }
    }
}