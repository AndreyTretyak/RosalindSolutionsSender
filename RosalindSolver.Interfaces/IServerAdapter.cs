using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface IServerAdapter
    {
        Task<SolverCheckResult> SendSolutionAsync(ISolver solver);
    }
}
