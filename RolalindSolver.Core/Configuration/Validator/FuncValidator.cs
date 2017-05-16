using System;
using RosalindSolver.Interfaces;

namespace RolalindSolver.Core.Configuration.Validator
{
    public class FuncValidator : IValueValidator
    {
        private readonly Func<string, bool> _validationFunction;
        private readonly string _errorMessage;

        public FuncValidator(Func<string, bool> validationFunction, string errorMessage)
        {
            _validationFunction = validationFunction;
            _errorMessage = errorMessage;
        }

        public ValidationResult Validate(string value)
        {
            var result = _validationFunction(value);
            return new ValidationResult(result, result ? value : _errorMessage);
        }
    }
}