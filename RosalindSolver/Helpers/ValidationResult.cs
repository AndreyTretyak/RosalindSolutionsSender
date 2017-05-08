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
}