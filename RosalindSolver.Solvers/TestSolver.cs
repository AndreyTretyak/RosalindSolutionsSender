using System;
using System.Threading.Tasks;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    public class TestSolver : ISolver
    {
        public string Key => "test";

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
