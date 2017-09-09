using System.Collections.Generic;
using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface ISolversTestGenerator
    {
        void CreateTestAsync(SolverCheckResult result);

        IEnumerable<SolverCheckResult> GetTestsData();
    }
}