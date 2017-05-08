using System;
using Autofac;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using RosalindSolver.Interfaces;
using RosalindSolver.ServerAdapter;

namespace RosalindSolver
{
    internal static class DependencyRegistration
    {
        public static IContainer Build()
        {

            var builder = new ContainerBuilder();

            builder.RegisterType<ConfigurationValueProvider>()
                   .AsSelf();

            builder.RegisterType<ConsoleServerConfigurationProvider>()
                   .As<IConfigurationProvider<ServerConfiguration>>();

            builder.RegisterType<ConsoleUserConfigurationProvider>()
                   .As<IConfigurationProvider<UserConfiguration>>();

            builder.Register(c => c.Resolve<IConfigurationProvider<ServerConfiguration>>().GetConfiguration())
                   .As<ServerConfiguration>();

            builder.Register(c => c.Resolve<IConfigurationProvider<UserConfiguration>>().GetConfiguration())
                   .As<UserConfiguration>();

            builder.RegisterType<DefaultServerAdapter>()
                   .As<IServerAdapter>();

            builder.RegisterType<SolutionSender>()
                   .As<ISolutionSender>();

            builder.RegisterType<SolverProvider>()
                .As<ISolverProvider>();

            builder.RegisterType<ConsoleSelectedProblemProvider>()
                   .As<ISelectedProblemProvider>()
                   .As<IUnsolvedProblemProvider>();

            //builder.RegisterType<FiboSolver>().As<ISolver>().Keyed<ISolver>("fibo");
            //builder.RegisterType<TestSolver>().As<ISolver>().Keyed<ISolver>("test");

            //var solvers = Assembly.Load("RosalindSolver.Solvers").GetTypes()

            var solverType = typeof(ISolver);
            var solvers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(r => !r.IsInterface && !r.IsAbstract && r.IsAssignableFrom(solverType));

            foreach (var solver in solvers)
            {
                builder.RegisterType(solver).As<ISolver>();
            }

            return builder.Build();
        }
    }
}
