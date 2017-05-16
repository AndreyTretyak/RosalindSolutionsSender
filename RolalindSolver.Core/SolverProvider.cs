using System.Collections.Generic;
using System.Linq;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class SolverProvider : ISolverProvider
    {
        private readonly IReadOnlyDictionary<string, ISolver> _solvers;

        public SolverProvider(IEnumerable<ISolver> solvers)
        {
            _solvers = solvers.ToDictionary(s => s.Key);
        }

        public ISolver Get(string key)
        {
            _solvers.TryGetValue(key, out var solver);
            return solver;
        }

        public IEnumerable<string> AvailableSolvers() => _solvers.Keys;
    }
}