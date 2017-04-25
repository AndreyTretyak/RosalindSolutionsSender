using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RosalindSolver.Interfaces;
using RosalindSolver.ServerAdapter;

namespace RosalindSolver
{
    internal static class DependencyRegistration
    {
        public static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ServerConfigurationProvider>()
                   .As<IConfigurationProvider<ServerConfiguration>>();

            builder.RegisterType<UserConfigurationProvider>()
                   .As<IConfigurationProvider<UserConfiguration>>();

            builder.Register(c => c.Resolve<IConfigurationProvider<ServerConfiguration>>().GetConfiguration())
                   .AsSelf();

            builder.Register(c => c.Resolve<IConfigurationProvider<UserConfiguration>>().GetConfiguration())
                   .AsSelf();

            builder.RegisterType<DefaultServerAdapter>()
                   .As<IServerAdapter>();

            builder.RegisterType<SolverProvider>()
                   .As<ISolverProvider>();

            builder.RegisterType<SolutionSender>()
                   .As<ISolutionSender>();

            return builder.Build();
        }
    }
}
