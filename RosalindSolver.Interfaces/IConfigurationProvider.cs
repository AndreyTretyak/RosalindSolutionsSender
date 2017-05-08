using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RosalindSolver.Interfaces
{
    public interface IConfigurationProvider<out T>
    {
        T GetConfiguration();

        void ClearConfiguration();
    }
}
