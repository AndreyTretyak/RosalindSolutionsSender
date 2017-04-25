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
}
