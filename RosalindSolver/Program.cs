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

            builder.Register(c => new ServerConfigurationProvider())
                   .As<IConfigurationProvider<ServerConfiguration>>();

            builder.Register(c => new UserConfigurationProvider())
                   .As<IConfigurationProvider<UserConfiguration>>();

            builder.Register(c => new DefaultServerAdapter(
                    c.Resolve<IConfigurationProvider<ServerConfiguration>>().GetConfiguration(), 
                    c.Resolve<IConfigurationProvider<UserConfiguration>>().GetConfiguration()
                   )).AsSelf();

            var container = builder.Build();
            var t = container.Resolve<DefaultServerAdapter>();

            var server = new DefaultServerAdapter(
                new ServerConfigurationProvider().GetConfiguration(),
                new UserConfigurationProvider().GetConfiguration());

            //server.SendSolutionAsync("fibo").GetAwaiter().GetResult();
        }
    }

    internal class ProblemSelector
    {
        
    }
}
