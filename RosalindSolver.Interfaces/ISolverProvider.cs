namespace RosalindSolver.Interfaces
{
    public interface ISolverProvider
    {
        ISolver Get(string key);
    }
}
