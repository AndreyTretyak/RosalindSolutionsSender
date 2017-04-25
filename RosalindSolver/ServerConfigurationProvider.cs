using System;
using System.Collections.Generic;
using RosalindSolver.Interfaces;
using System.Configuration;

namespace RosalindSolver
{
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

        internal class ConfigurationValueProvider : IDefaultValueProvider
        {
            public string Get(string name) => ConfigurationManager.AppSettings[name];
            public void Set(string name, string value) => ConfigurationManager.AppSettings[name] = value;
        }
    }
}
