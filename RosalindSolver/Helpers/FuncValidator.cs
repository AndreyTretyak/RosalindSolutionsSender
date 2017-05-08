using System;

namespace RosalindSolver
{
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
}