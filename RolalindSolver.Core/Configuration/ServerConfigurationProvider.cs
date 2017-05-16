using System;
using System.Collections.Generic;
using RosalindSolver.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ServerConfigurationProvider : IConfigurationProvider<ServerConfiguration>
    {
        private readonly IUserInputProvider _inputProvider;
        private readonly IConfigurationValueProvider _valueProvider;

        public ServerConfigurationProvider(IUserInputProvider inputProvider, IConfigurationValueProvider configurationValueProvider)
        {
            _inputProvider = inputProvider;
            _valueProvider = configurationValueProvider;
        }

        public ServerConfiguration GetConfiguration()
        {
            var host = _inputProvider.RequestValueAndSaving(ConfigurationConstants.RosalidHostKey, _valueProvider);
            return new ServerConfiguration(host);
        }

        public void ClearConfiguration()
        {
            _valueProvider.Set(ConfigurationConstants.RosalidHostKey, null);
        }
    }
}
