using Task5.Common.Models;

namespace Task5.Common.Interfaces
{
    public interface ITableListGenerator
    {
        public PersonsTableList Generate(int count);
    }
}