using System.Threading.Tasks;

namespace RosalindSolver.Interfaces
{
    public interface ISolver
    {
        string Key { get; }
        Task<string> SolveAsync(string dataset);
        Task<string> GetSourceCodeAsync();
    }
}
