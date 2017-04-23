using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RosalindSolver
{
    public struct ValidationResult
    {
        public bool IsValid { get; }
        public string Value { get; }
        
        public ValidationResult(bool isValid, string value)
        {
            IsValid = isValid;
            Value = value;
        }
    }

    public interface IDefaultValueProvider
    {
        string Get(string name);
        void Set(string name, string value);
    }

    public interface IValueValidator
    {
        ValidationResult Validate(string value);
    }

    public class FuncValidator : IValueValidator
    {
        private readonly Func<string, bool> ValidationFunction;
        private readonly string ErrorMessage;

        public FuncValidator(Func<string, bool> validationFunction, string errorMessage)
        {
            ValidationFunction = validationFunction;
            ErrorMessage = errorMessage;
        }

        public ValidationResult Validate(string value)
        {
            var result = ValidationFunction(value);
            return new ValidationResult(result, result ? value : ErrorMessage);
        }
    }


    internal static class ConsoleHelper
    {
        private static readonly IValueValidator _defaultValidator;

        static ConsoleHelper()
        {
            _defaultValidator = new FuncValidator(v => !string.IsNullOrWhiteSpace(v), "Value should not be empty");
        }

        public static bool RequestValueSaving(string name)
        {
            while (true)
            {
                Console.WriteLine($"Remember this value in config file? [Y] Yes / [N] No");
                var value = char.ToUpper(Console.ReadKey().KeyChar);
                switch (value)
                {
                    case 'Y':
                        return true;
                    case 'N':
                        return false;
                }
            }
        }

        public static string RequestValueAndSaving(string name, IDefaultValueProvider defaultValueProvider)
        {
            return RequestValueAndSaving(name, _defaultValidator, defaultValueProvider);
        }

        public static string RequestValueAndSaving(string name, IValueValidator validator, IDefaultValueProvider defaultValueProvider)
        {
            var result = validator.Validate(defaultValueProvider?.Get(name));
            while (!result.IsValid)
            {
                Console.WriteLine($"Please enter '{name}' value:");
                result = validator.Validate(Console.ReadLine());
                if (!result.IsValid)
                {
                    Console.WriteLine(result.Value);
                    continue;
                } 

                if (RequestValueSaving(name))
                {
                    defaultValueProvider.Set(name, result.Value);
                }
            }

            return result.Value;
        }
    }
}
