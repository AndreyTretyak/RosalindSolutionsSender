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
        public static TheoryData<SolverCheckResult> SolversTestData()
        {
            var result = new TheoryData<SolverCheckResult>();
            foreach (var data in new SolversTestGenerator().GetTestsData())
            {
                result.Add(data);
            }
            return result;
        }


        [Theory, MemberData(nameof(SolversTestData))]
        public void DynamicSolversTest(SolverCheckResult result)
        {
            throw new NotImplementedException();
        }
    }


}
