using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDiscipline.Infrastructure.Provider
{
    internal interface IDisciplineConnectionProvider
    {
        string GetConnectionString();
    }
}
