using System;
using System.Collections.Generic;
using RosalindSolver.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConsoleServerConfigurationProvider : IConfigurationProvider<ServerConfiguration>
    {
        private readonly IValueProvider valueProvider;

        public ConsoleServerConfigurationProvider(ConfigurationValueProvider configurationValueProvider)
        {
            valueProvider = configurationValueProvider;
        }

        public ServerConfiguration GetConfiguration()
        {
            var host = ConsoleHelper.RequestValueAndSaving(ConfigurationConstants.RosalidHostKey, valueProvider);
            return new ServerConfiguration(host);
        }

        public void ClearConfiguration()
        {
            valueProvider.Set(ConfigurationConstants.RosalidHostKey, null);
        }
    }
}
