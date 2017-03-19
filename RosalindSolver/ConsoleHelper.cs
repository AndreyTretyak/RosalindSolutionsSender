using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RosalindSolver
{
    internal static class ConsoleHelper
    {
        private static readonly Func<string, bool> _defaultValidator = string.IsNullOrWhiteSpace;

        public static string RequestValue(string name)
        {
            return RequestValue(name, _defaultValidator, "Value should not be empty");
        }

        public static string RequestValue(string name, Func<string, bool> validator, string errorMessage)
        {
            while (true)
            {
                Console.WriteLine($"Please enter {name} value:");
                var value = Console.ReadLine();
                var isValid = validator(value);
                if (!isValid) Console.WriteLine(errorMessage);
                else return value;
            }
        }
    }
}
