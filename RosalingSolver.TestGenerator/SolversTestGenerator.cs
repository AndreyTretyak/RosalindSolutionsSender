using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RosalindSolver.Interfaces;

namespace RosalingSolver.TestGenerator
{
    public class SolversTestGenerator : ISolversTestGenerator
    {
        private const string DataFileName = "SolversTestInfo.json";
        private const string CorrectResut = "Correct";
        private const string WrongResult = "WrongResult{0}";
        private IConfigurationRoot Configuration { get; }

        public SolversTestGenerator()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(DataFileName)
                .Build();
        }

        public void CreateTestAsync(SolverCheckResult result)
        {
            var section = Configuration.GetSection(result.Key).GetSection(result.Dataset);
            if (section[CorrectResut] != null) return;
            if (result.IsCorrect)
            {
                foreach (var pair in section.AsEnumerable())
                {
                    section[pair.Key] = null;
                }

                section[CorrectResut] = result.Answer;
            }
            else
            {
                var count = 0;
                foreach (var pair in section.AsEnumerable())
                {
                    if (pair.Value == result.Answer) return;
                    count++;
                }

                section[string.Format(WrongResult, count + 1)] = result.Answer;
            }
        }

        public IEnumerable<SolverCheckResult> GetTestsData()
        {
            foreach (var keySection in Configuration.GetChildren())
            {
                var key = keySection.Key;
                foreach (var datasetSection in keySection.GetChildren())
                {
                    var dataset = datasetSection.Key;
                    foreach (var result in datasetSection.AsEnumerable())
                    {
                        yield return new SolverCheckResult(key, result.Key == CorrectResut, dataset, result.Value);
                    }

                }
            }
        }
    }
}