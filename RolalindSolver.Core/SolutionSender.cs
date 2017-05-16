using System;
using System.Threading.Tasks;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class SolutionSender : ISolutionSender
    {
        private readonly IServerAdapter _server;
        private readonly ISolverProvider _solverProvider;
        private readonly ISolversTestGenerator _testGenerator;

        public SolutionSender(IServerAdapter server, ISolverProvider solverProvider, ISolversTestGenerator testGenerator)
        {
            _server = server;
            _solverProvider = solverProvider;
            _testGenerator = testGenerator;
        }

        public async Task<SolverCheckResult> SendAsync(string key)
        {
            var solver = _solverProvider.Get(key);
            var result = await _server.SendSolutionAsync(solver);
            await CreateUnitTest(result);
            return result;
        }

        private Task CreateUnitTest(SolverCheckResult result)
        {
            return _testGenerator.CreateTestAsync(result);
        } 
    }
}
