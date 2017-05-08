using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RosalindSolver.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConsoleSelectedProblemProvider : ISelectedProblemProvider, IUnsolvedProblemProvider
    {
        private readonly ConfigurationValueProvider _configurationProvider;
        private readonly SolvedConfigurationProvider _solvedConfigurationProvider;
        private readonly ISolverProvider _solverProvider;

        public ConsoleSelectedProblemProvider(ISolverProvider solverProvider, ConfigurationValueProvider provider, SolvedConfigurationProvider solvedConfigurationProvider)
        {
            _configurationProvider = provider;
            _solvedConfigurationProvider = solvedConfigurationProvider;
            _solverProvider = solverProvider;
        }

        public IEnumerable<string> AvailableSolvers()
        {
            return _solverProvider.AvailableSolvers();
        }

        public string GetCurrentProblemKey()
        {
            var solvers = _solverProvider.AvailableSolvers().ToList();
            var problemKey = _configurationProvider.Get(ConfigurationConstants.ProblemKeyKey);
            if (solvers.Contains(problemKey)) return problemKey;
            var key = ConsoleHelper.SelectOption(solvers);
            ConsoleHelper.RequestValueSaving(ConfigurationConstants.ProblemKeyKey, key, _configurationProvider);
            return key;
        }

        public void ClearSelected()
        {
            _configurationProvider.Set(ConfigurationConstants.ProblemKeyKey, null);
        }

        public void MarkAsSolved(string key)
        {
            _solvedConfigurationProvider.MarkAsSolved(key);
        }

        public IEnumerable<string> GetUnsolvedProblems()
        {
            var solved = _solvedConfigurationProvider.GetSolvedProblems();
            return _solverProvider.AvailableSolvers().Where(s => !solved.Contains(s));
        }

        public void ClearSolvedMarks()
        {
            _solvedConfigurationProvider.ClearSolvedMarks();
        }
    }
}