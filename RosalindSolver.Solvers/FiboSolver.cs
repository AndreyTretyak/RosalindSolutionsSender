using System;
using System.Threading.Tasks;
using RosalindSolver.Interfaces;

namespace RosalindSolver.Solvers
{
    public class FiboSolver : ISolver
    {
        public string Key => "fibo";

        public Task<string> GetSourceCodeAsync()
        {
            return Task.FromResult(string.Empty);
        }

        public Task<string> SolveAsync(string dataset)
        {
            throw new NotImplementedException();
        }
    }
}
