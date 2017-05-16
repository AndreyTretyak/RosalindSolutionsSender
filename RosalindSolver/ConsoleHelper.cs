using System;
using System.Collections.Generic;
using System.Linq;
using RosalindSolver.App_LocalResourses;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class ConsoleIntputProvider : IUserInputProvider
    {
        private static readonly IValueValidator DefaultValidator;

        static ConsoleIntputProvider()
        {
            DefaultValidator = new FuncValidator(v => !string.IsNullOrWhiteSpace(v), Resources.ValueShoulNotBeEmpty);
        }

        public void RequestValueSaving(string name, string value, IValueProvider valueProvider)
        {
            if (valueProvider == null) return;
            while (true)
            {
                Console.WriteLine(Resources.RememberValueRequest);
                var answer = char.ToUpper(Console.ReadKey().KeyChar);
                switch (answer)
                {
                    case 'Y':
                        valueProvider.Set(name, value);
                        return;
                    case 'N':
                        return;
                }
            }
        }

        public string RequestValueAndSaving(string name, IValueProvider valueProvider)
        {
            return RequestValueAndSaving(name, DefaultValidator, valueProvider);
        }

        public string RequestValueAndSaving(string name, IValueValidator validator, IValueProvider valueProvider)
        {
            var result = validator.Validate(valueProvider?.Get(name));
            while (!result.IsValid)
            {
                Console.WriteLine(Resources.PleaseEnterValue, name);
                result = validator.Validate(Console.ReadLine());
                if (!result.IsValid)
                {
                    Console.WriteLine(result.Value);
                    continue;
                }

                RequestValueSaving(name, result.Value, valueProvider);
            }

            return result.Value;
        }

        public string SelectOption(IList<string> items)
        {
            return SelectOption(items, s => s);
        }

        public T SelectOption<T>(IList<T> items, Func<T, string> getName)
        {
            var optionsWithNumbers = items.Select((e, i) => $"{i + 1}. {getName(e)}");
            foreach (var option in optionsWithNumbers)
            {
                Console.WriteLine(option);
            }

            while (true)
            {
                Console.Write(Resources.PleaseEnterValidIndex);
                if(!int.TryParse(Console.ReadLine(), out var index)) continue;
                if (index < 1 || index > items.Count) continue;
                return items[index - 1];
            }
        }
    }
}
