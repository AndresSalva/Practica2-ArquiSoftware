using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDiscipline.Infrastructure.Provider
{
    public interface IDisciplineLoggerProvider
    {
        Task LogInfo(string message);
        Task LogError(string message, Exception ex = null);
        Task LogWarning(string message);
    }
}
