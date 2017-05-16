namespace RosalindSolver.Interfaces
{
    public struct SolverCheckResult
    {
        public string Key { get; }
        public bool IsCorrect { get; }
        public string Dataset { get; }
        public string Answer { get; }

        public SolverCheckResult(string key, bool isCorrect, string dataset, string answer)
        {
            Key = key;
            IsCorrect = isCorrect;
            Dataset = dataset;
            Answer = answer;
        }
    }
}
