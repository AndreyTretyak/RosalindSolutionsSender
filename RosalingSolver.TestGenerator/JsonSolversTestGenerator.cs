using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RosalindSolver.Interfaces;

namespace RosalingSolver.TestGenerator
{
    public class JsonSolversTestGenerator : ISolversTestGenerator
    {
        private readonly string _fileName = "TestCases.json";
        private readonly JsonSerializer _serializer;

        public JsonSolversTestGenerator()
        {
            _serializer = JsonSerializer.Create();
        }

        public Task CreateTestAsync(SolverCheckResult result)
        {
            using (var stream = new JsonTextWriter(File.AppendText(_fileName)))
            {
                _serializer.Serialize(stream, result);
            }

            return Task.CompletedTask;
        }

        public IEnumerable<SolverCheckResult> GetTestsData()
        {
            using (var stream = new JsonTextReader(File.OpenText(_fileName)))
            {
                while (stream.Read())
                {
                    yield return _serializer.Deserialize<SolverCheckResult>(stream);
                }
            }
        }
    }
}