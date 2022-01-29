using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task5.Common.Models;

namespace BreakfastTaskSolver
{
    public class Solver
    {
        private readonly IEnumerable<Condition> _conditions;

        public Solver(IEnumerable<Condition> conditions)
        {
            _conditions = conditions;
        }
    }
}
