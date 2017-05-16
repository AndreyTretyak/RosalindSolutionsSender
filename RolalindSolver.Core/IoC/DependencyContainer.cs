using Autofac;

namespace RolalindSolver.Core.IoC
{
    public class DependencyContainer
    {
        private readonly IContainer _container;

        public DependencyContainer(IContainer container)
        {
            _container = container;
        }

        public T Resolve<T>() => _container.Resolve<T>();
    }
}