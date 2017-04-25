using System;
using System.Threading.Tasks;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class SolutionSender : ISolutionSender
    {
        private readonly IServerAdapter _server;
        private readonly ISolverProvider _solverProvider;

        public SolutionSender(IServerAdapter server, ISolverProvider solverProvider)
        {
            _server = server;
            _solverProvider = solverProvider;
        }

        public async Task Send(string key)
        {
            var solver = _solverProvider.Get(key);
            var result = await _server.SendSolutionAsync(solver);
            await CreateUnitTest(key, result);
        }

        private Task CreateUnitTest(string key, SolverCheckResult result)
        {
            throw new NotImplementedException();
        } 
    }
}
