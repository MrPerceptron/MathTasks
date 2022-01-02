using System.Collections.Generic;
using Task5.Common.Models;

namespace Task5.Common.Interfaces
{
    public interface IConditionsGenerator
    {
        public IEnumerable<Condition> GenerateConditions(PersonsTableList personsTableList);
    }
}