using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using RosalindSolver.Interfaces;

namespace RosalingSolver.TestGenerator
{
    public class BinarySolversTestGenerator : ISolversTestGenerator
    {
        private readonly string _fileName;
        private readonly BinaryFormatter _serializer;

        public BinarySolversTestGenerator(IConfigurationValueProvider provider)
        {
            _serializer = new BinaryFormatter();
            _fileName = provider.Get(ConfigurationConstants.GeneratedTestCasesFileKey)
                        ?? ConfigurationConstants.GeneratedTestCasesFileName;
        }

        public Task CreateTestAsync(SolverCheckResult result)
        {
            using (var fileStream = new FileStream(_fileName, FileMode.Append))
            {
                _serializer.Serialize(fileStream, result);
            }

            return Task.CompletedTask;
        }

        public IEnumerable<SolverCheckResult> GetTestsData()
        {
            using (var fileStream = new FileStream(_fileName, FileMode.Open))
            {
                while (fileStream.Position != fileStream.Length)
                {
                    yield return (SolverCheckResult)_serializer.Deserialize(fileStream);
                }
            }
        }
    }
}