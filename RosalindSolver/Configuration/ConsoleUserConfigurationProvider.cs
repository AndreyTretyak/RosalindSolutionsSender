using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using RosalindSolver.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConsoleUserConfigurationProvider : IConfigurationProvider<UserConfiguration>
    {
        private readonly UserConfigurationValueProvider userConfigurationProvider;

        public ConsoleUserConfigurationProvider(ConfigurationValueProvider provider)
        {
            var fileName = provider.Get(ConfigurationConstants.UserConfigFileKey) ?? ConfigurationConstants.DefaultUserConfigFileName;
            userConfigurationProvider = new UserConfigurationValueProvider(fileName);
        }

        public UserConfiguration GetConfiguration()
        {
            var name = ConsoleHelper.RequestValueAndSaving(ConfigurationConstants.UserNameKey, userConfigurationProvider);
            var password = ConsoleHelper.RequestValueAndSaving(ConfigurationConstants.PasswordKey, userConfigurationProvider);
            return new UserConfiguration(name, password);
        }

        public void ClearConfiguration()
        {
            userConfigurationProvider.Clear();
        }

        internal class UserConfigurationValueProvider : IValueProvider
        {
            private readonly RijndaelManaged cryptoManaged;
            private readonly string configFileName;
            private readonly byte[] key;
            private Dictionary<string, string> values;
            
            public UserConfigurationValueProvider(string name)
            {
                configFileName = name;
                cryptoManaged = new RijndaelManaged();
                key = Encoding.Unicode.GetBytes("testKey9");
            }

            private void ReadValuesFromFile()
            {
                values = new Dictionary<string, string>();
                if (!File.Exists(configFileName)) return;
                using (var fstream = new FileStream(configFileName, FileMode.Open))                
                //using (var cstream = new CryptoStream(fstream, cryptoManaged.CreateDecryptor(key, key), CryptoStreamMode.Read))
                using (var stream = new StreamReader(fstream))
                {
                    while (!stream.EndOfStream)
                    {
                        values.Add(stream.ReadLine(), stream.ReadLine());
                    }
                }
            }

            private void SaveValuesToFile()
            {
                using (var fstream = new FileStream(configFileName, FileMode.OpenOrCreate))
                //using (var cstream = new CryptoStream(fstream, cryptoManaged.CreateEncryptor(key, key), CryptoStreamMode.Write))
                using (var stream = new StreamWriter(fstream))
                {
                    foreach (var pair in values)
                    {
                        stream.WriteLine(pair.Key);
                        stream.WriteLine(pair.Value);
                    }
                }
            }

            public void Clear()
            {
                File.Delete(configFileName);
            }

            public string Get(string name)
            {
                if (values == null) ReadValuesFromFile();
                return values.TryGetValue(name, out string value) ? value : null;
            }

            public void Set(string name, string value)
            {
                values[name] = value;
                SaveValuesToFile();
            }
        }
    }
}
