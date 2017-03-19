using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using RosalindSolver.SolutionSender;

namespace RosalindSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => new ServerConfiguration("")).AsSelf();
            builder.Register(c => new UserConfiguration("", "")).AsSelf();
            var container = builder.Build();
            var t = container.Resolve<DefaultServerAdapter>();

            var server = new DefaultServerAdapter(
                new ServerConfigurationProvider().GetConfiguration(),
                new UserConfigurationProvider().GetConfiguration());

            server.SolveAsync("fibo").GetAwaiter().GetResult();
        }
    }
}
