using System.Collections.Generic;

namespace RosalindSolver.Interfaces
{
    public interface ISolverProvider
    {
        ISolver Get(string key);
        IEnumerable<string> AvailableSolvers();
    }
}
