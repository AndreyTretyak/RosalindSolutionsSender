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
            //return new JsonSolversTestGenerator().GetTestsData()
            //    .Select(d => new object[] { d })
            //    .ToArray();
            throw new NotImplementedException();
        }


        [Test]
        [TestCaseSource(nameof(SolversTestData))]
        public void DynamicSolversTest(SolverCheckResult result)
        {
            throw new NotImplementedException();
        }
    }
}
