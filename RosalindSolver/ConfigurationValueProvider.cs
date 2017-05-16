using System.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConfigurationValueProvider : IConfigurationValueProvider
    {
        public string Get(string name) => ConfigurationManager.AppSettings[name];
        public void Set(string name, string value) => ConfigurationManager.AppSettings[name] = value;
    }
}