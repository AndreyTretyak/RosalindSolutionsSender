using RosalindSolver.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosalingSolver.TestGenerator
{
    public class SolversTestGenerator : ISolversTestGenerator
    {
        public Task CreateTestAsync(SolverCheckResult result)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SolverCheckResult> GetTestsData()
        {
            throw new NotImplementedException();
        }
    }
}
