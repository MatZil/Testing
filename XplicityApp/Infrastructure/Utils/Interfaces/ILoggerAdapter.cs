using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XplicityApp.Infrastructure.Utils.Interfaces
{
    public interface ILoggerAdapter<T>
    {
        void LogInformation(string message);
    }
}
