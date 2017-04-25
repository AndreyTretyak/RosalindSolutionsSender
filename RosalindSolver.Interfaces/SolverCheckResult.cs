namespace RosalindSolver.Interfaces
{
    public struct SolverCheckResult
    {
        public bool IsCorrect { get; }
        public string Dataset { get; }
        public string Answer { get; }

        public SolverCheckResult(bool isCorrect, string dataset, string answer)
        {
            IsCorrect = isCorrect;
            Dataset = dataset;
            Answer = answer;
        }
    }
}
