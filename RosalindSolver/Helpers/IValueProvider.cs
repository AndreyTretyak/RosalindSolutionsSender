namespace RosalindSolver
{
    public interface IValueProvider
    {
        string Get(string name);
        void Set(string name, string value);
    }
}