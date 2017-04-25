using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = DependencyRegistration.Build();
            var server = container.Resolve<IServerAdapter>();
           
            //server.SendSolutionAsync("fibo").GetAwaiter().GetResult();
        }
    }

    internal class SolverProvider : ISolverProvider
    {
        public ISolver Get(string key)
        {
            return null;
        }
    }
}
