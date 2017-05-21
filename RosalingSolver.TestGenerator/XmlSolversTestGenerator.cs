using RosalindSolver.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RosalingSolver.TestGenerator
{
    public class XmlSolversTestGenerator : ISolversTestGenerator
    {
        private readonly string _fileName;
        private readonly XmlSerializer _serializer;

        public XmlSolversTestGenerator(IConfigurationValueProvider provider)
        {
            _serializer = new XmlSerializer(typeof(SolverCheckResult));
            _fileName = provider.Get(ConfigurationConstants.GeneratedTestCasesFileKey)
                        ?? ConfigurationConstants.GeneratedTestCasesFileName;
        }

        public Task CreateTestAsync(SolverCheckResult result)
        {
            using (var stream = new FileStream(_fileName, FileMode.Append))
            {
                _serializer.Serialize(stream, result);
            }
            
            return Task.CompletedTask;
        }

        public IEnumerable<SolverCheckResult> GetTestsData()
        {
            using (var stream = new FileStream(_fileName, FileMode.Open))
            {
                while (stream.Position != stream.Length)
                {
                    yield return (SolverCheckResult)_serializer.Deserialize(stream);
                }
            }
        }
    }
}
