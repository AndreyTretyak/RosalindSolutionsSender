using System;
using System.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConfigurationValueProvider : IConfigurationValueProvider
    {
        public string Get(string name) => throw new NotImplementedException(); //ConfigurationManager.AppSettings[name];
        public void Set(string name, string value) => throw new NotImplementedException(); //ConfigurationManager.AppSettings[name] = value;
    }
}