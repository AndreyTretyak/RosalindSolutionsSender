using System.Collections.Generic;
using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface ISolversTestGenerator
    {
        Task CreateTestAsync(SolverCheckResult result);

        IEnumerable<SolverCheckResult> GetTestsData();
    }
}