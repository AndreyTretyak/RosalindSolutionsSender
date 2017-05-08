using System;
using NUnit.Framework;

namespace RosalingSolver.Tests
{
    [TestFixture]
    public class DynamicSolversTests
    {
        private static object[] SolversTestData()
        {
            throw new NotImplementedException();
            return new object[]
            {
                new object[] {"", "", ""},
                new object[] {"", "", ""},
                new object[] {"", "", ""}
            };
        }


        [Test]
        [TestCaseSource(nameof(SolversTestData))]
        public void DynamicSolversTest(string key, string input, string result)
        {
            throw new NotImplementedException();
        }
    }
}
