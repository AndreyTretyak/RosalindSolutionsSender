using System.Collections;
using System.Text;
using RolalindSolver.Core;
using RolalindSolver.Core.IoC;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new DependencyContainerBuilder();
            builder.Register<ConsoleIntputProvider, IUserInputProvider>();
            var container = builder.Build();
            var consoleManager = container.Resolve<SendingManager>();
            consoleManager.StartExecutionLoopAsync().GetAwaiter().GetResult();
        }
    }
}
