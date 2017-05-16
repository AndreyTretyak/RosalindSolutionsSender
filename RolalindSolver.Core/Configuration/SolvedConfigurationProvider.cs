using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RosalindSolver.Configuration;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    internal class SolvedConfigurationProvider
    {
        private readonly string _fileName;
        private HashSet<string> _set;

        public SolvedConfigurationProvider(IConfigurationValueProvider configurationProvider)
        {
            _fileName = configurationProvider.Get(ConfigurationConstants.SolvedConfigFileKey) ?? ConfigurationConstants.DefaultSolvedConfigFileName;
        }

        private void InitialRead()
        {
            var lines = File.ReadAllLines(_fileName);
            _set = new HashSet<string>(lines.Distinct());
            if (lines.Length == _set.Count) return;
            File.WriteAllLines(_fileName, _set);
        }

        public void MarkAsSolved(string key)
        {
            var solved = GetSolvedProblems();
            if (solved.Contains(key)) return;
            solved.Add(key);
            File.AppendAllText(_fileName, key + Environment.NewLine);
        }

        private bool IsFileExist() => File.Exists(_fileName);

        public ISet<string> GetSolvedProblems()
        {
            if (_set == null) InitialRead();
            return _set;
        }

        public void ClearSolvedMarks()
        {
            if (!IsFileExist()) return;
            File.Delete(_fileName);
        }
    }
}