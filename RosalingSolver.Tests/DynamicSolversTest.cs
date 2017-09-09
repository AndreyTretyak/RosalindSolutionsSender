using System;
using System.Diagnostics;
using System.Linq;
using RosalindSolver.Interfaces;
using RosalingSolver.TestGenerator;
using Xunit;

namespace RosalingSolver.Tests
{
    public class DynamicSolversTests
    {
        private static TheoryData<SolverCheckResult> SolversTestData()
        {
            //return new JsonSolversTestGenerator().GetTestsData()
            //    .Select(d => new object[] { d })
            //    .ToArray();
            throw new NotImplementedException();
        }


        [Theory, MemberData(nameof(SolversTestData))]
        public void DynamicSolversTest(SolverCheckResult result)
        {
            throw new NotImplementedException();
        }
    }


}
