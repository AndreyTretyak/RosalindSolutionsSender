using System;
using System.Linq;
using NUnit.Framework;
using RosalindSolver.Interfaces;
using RosalingSolver.TestGenerator;

namespace RosalingSolver.Tests
{
    [TestFixture]
    public class DynamicSolversTests
    {
        private static object[] SolversTestData()
        {
            return new SolversTestGenerator().GetTestsData()
                .Select(d => new object[] { d })
                .ToArray();
        }


        [Test]
        [TestCaseSource(nameof(SolversTestData))]
        public void DynamicSolversTest(SolverCheckResult result)
        {
            throw new NotImplementedException();
        }
    }
}
