using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RosalindSolver.SolutionSender;

namespace RosalindSolver
{
    public interface IConfigurationProvider<out T>
    {
        T GetConfiguration();
    }

    internal class ServerConfigurationProvider : IConfigurationProvider<ServerConfiguration>
    {
        private const string Key = "RosalindHost";
        private readonly IDefaultValueProvider defaultValueProvider;

        public ServerConfigurationProvider()
        {
            defaultValueProvider = new ConfigurationValueProvider();
        }

        public ServerConfiguration GetConfiguration()
        {
            var host = ConsoleHelper.RequestValueAndSaving(Key, defaultValueProvider);
            return new ServerConfiguration(host);
        }
    }

    internal class ConfigurationValueProvider : IDefaultValueProvider
    {
        public string Get(string name) => ConfigurationManager.AppSettings[name];
        public void Set(string name, string value) => ConfigurationManager.AppSettings[name] = value;
    }
}
