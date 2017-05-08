using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface ISolutionSender
    {
        Task<SolverCheckResult> SendAsync(string key);
    }
}
