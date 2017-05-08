using System.Collections;
using System.Text;
using Autofac;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var container = DependencyRegistration.Build();
            var consoleManager = container.Resolve<ConsoleManager>();
            consoleManager.StartExecutionLoopAsync().GetAwaiter().GetResult();
        }
    }
}
