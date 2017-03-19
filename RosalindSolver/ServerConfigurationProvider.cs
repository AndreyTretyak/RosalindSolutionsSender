using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using RosalindSolver.SolutionSender;

namespace RosalindSolver
{
    public interface IConfigurationProvider<out T>
    {
        T GetConfiguration();
    }

    internal class ServerConfigurationProvider : IConfigurationProvider<ServerConfiguration>
    {
        public ServerConfiguration GetConfiguration()
        {
            var host = ConfigurationManager.AppSettings["RosalindHost"];
            if (string.IsNullOrWhiteSpace(host))
                ConsoleHelper.RequestValue("Rosalind Host");
            return new ServerConfiguration(host);
        }
    }
}
