using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface ISelectedProblemProvider
    {
        IEnumerable<string> AvailableSolvers();
        string GetCurrentProblemKey();
        void ClearSelected();
    }

    public interface IUnsolvedProblemProvider
    {
        void MarkAsSolved(string key);
        IEnumerable<string> GetUnsolvedProblems();
        void ClearSolvedMarks();
    }
}
