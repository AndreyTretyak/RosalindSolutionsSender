using System;
using System.Collections.Generic;

namespace RosalindSolver.Interfaces
{
    public class UserConfiguration
    {
        public string Username { get; }
        public string Password { get; }
        public UserConfiguration(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }

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

    public interface IValueValidator
    {
        ValidationResult Validate(string value);
    }

    public interface IValueProvider
    {
        string Get(string name);
        void Set(string name, string value);
    }

    public interface IConfigurationValueProvider : IValueProvider
    {
    }

    public interface IUserInputProvider
    {
        void RequestValueSaving(string name, string value, IValueProvider valueProvider);
        string RequestValueAndSaving(string name, IValueProvider valueProvider);
        string RequestValueAndSaving(string name, IValueValidator validator, IValueProvider valueProvider);
        string SelectOption(IList<string> items);
        T SelectOption<T>(IList<T> items, Func<T, string> getName);
    }
}
