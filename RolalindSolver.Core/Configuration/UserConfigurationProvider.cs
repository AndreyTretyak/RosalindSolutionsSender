using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using RosalindSolver.Interfaces;

namespace RolalindSolver.Core.Configuration
{
    internal class UserConfigurationProvider : IConfigurationProvider<UserConfiguration>
    {
        private readonly IUserInputProvider _inputProvider;
        private readonly UserConfigurationValueProvider _userConfigurationProvider;

        public UserConfigurationProvider(IUserInputProvider inputProvider, IConfigurationValueProvider provider)
        {
            _inputProvider = inputProvider;
            var fileName = provider.Get(ConfigurationConstants.UserConfigFileKey) ?? ConfigurationConstants.DefaultUserConfigFileName;
            _userConfigurationProvider = new UserConfigurationValueProvider(fileName);
        }

        public UserConfiguration GetConfiguration()
        {
            var name = _inputProvider.RequestValueAndSaving(ConfigurationConstants.UserNameKey, _userConfigurationProvider);
            var password = _inputProvider.RequestValueAndSaving(ConfigurationConstants.PasswordKey, _userConfigurationProvider);
            return new UserConfiguration(name, password);
        }

        public void ClearConfiguration()
        {
            _userConfigurationProvider.Clear();
        }

        internal class UserConfigurationValueProvider : IValueProvider
        {
            public IConfigurationRoot Configuration { get; }

            
            public UserConfigurationValueProvider(string name)
            {
                Configuration = new ConfigurationBuilder().AddUserSecrets(name).Build();
            }

            public void Clear()
            {
                foreach (var value in Configuration.AsEnumerable())
                {
                    Configuration[value.Key] = string.Empty;
                }
            }

            public string Get(string name) => Configuration[name];
            public void Set(string name, string value) => Configuration[name] = value;
        }
    }
}
