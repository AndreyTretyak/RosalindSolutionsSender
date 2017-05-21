using System;
using System.Linq;
using Autofac;
using RolalindSolver.Core.Configuration;
using RosalindSolver.Interfaces;
using RosalindSolver.ServerAdapter;
using RosalingSolver.TestGenerator;

namespace RolalindSolver.Core.IoC
{
    public class DependencyContainerBuilder
    {
        private readonly ContainerBuilder _builder;

        public DependencyContainerBuilder()
        {
            _builder = new ContainerBuilder();
        }

        public void Register<TRegisterType, TRegisterAs>() where TRegisterType : TRegisterAs
        {
            _builder.RegisterType<TRegisterType>().As<TRegisterAs>();
        }

        public DependencyContainer Build()
        {
            _builder.RegisterType<ServerConfigurationProvider>()
                    .As<IConfigurationProvider<ServerConfiguration>>();

            _builder.RegisterType<UserConfigurationProvider>()
                    .As<IConfigurationProvider<UserConfiguration>>();

            _builder.Register(c => c.Resolve<IConfigurationProvider<ServerConfiguration>>().GetConfiguration())
                    .As<ServerConfiguration>();

            _builder.Register(c => c.Resolve<IConfigurationProvider<UserConfiguration>>().GetConfiguration())
                    .As<UserConfiguration>();

            _builder.RegisterType<DefaultServerAdapter>()
                    .As<IServerAdapter>();

            _builder.RegisterType<SolutionSender>()
                    .As<ISolutionSender>();

            _builder.RegisterType<SolverProvider>()
                    .As<ISolverProvider>();

            _builder.RegisterType<SelectedProblemProvider>()
                    .As<ISelectedProblemProvider>()
                    .As<IUnsolvedProblemProvider>();

            _builder.RegisterType<JsonSolversTestGenerator>()
                    .As<ISolversTestGenerator>();

            //builder.RegisterType<FiboSolver>().As<ISolver>().Keyed<ISolver>("fibo");
            //builder.RegisterType<TestSolver>().As<ISolver>().Keyed<ISolver>("test");

            //var solvers = Assembly.Load("RosalindSolver.Solvers").GetTypes()

            var solverType = typeof(ISolver);
            var solvers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(r => !r.IsInterface && !r.IsAbstract && r.IsAssignableFrom(solverType));

            foreach (var solver in solvers)
            {
                _builder.RegisterType(solver).As<ISolver>();
            }

            return new DependencyContainer(_builder.Build());
        }
    }
}
