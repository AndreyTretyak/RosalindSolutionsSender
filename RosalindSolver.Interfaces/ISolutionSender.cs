using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface ISolutionSender
    {
        Task Send(string key);
    }
}
