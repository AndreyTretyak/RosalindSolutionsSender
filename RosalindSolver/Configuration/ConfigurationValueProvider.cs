using System.Configuration;

namespace RosalindSolver
{
    internal class ConfigurationValueProvider : IValueProvider
    {
        public string Get(string name) => ConfigurationManager.AppSettings[name];
        public void Set(string name, string value) => ConfigurationManager.AppSettings[name] = value;
    }
}