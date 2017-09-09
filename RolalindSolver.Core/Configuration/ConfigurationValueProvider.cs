using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConfigurationValueProvider : IConfigurationValueProvider
    {
        private const string ConfigurationFileName = "appconfig.json";
        private IConfigurationRoot Configuration { get; }

        public ConfigurationValueProvider()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(ConfigurationFileName);
            Configuration = builder.Build();
        }

        public string Get(string name) => Configuration[name];
        public void Set(string name, string value) => Configuration[name] = value;
    }
}