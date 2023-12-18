using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.Logger
{
    public interface ICustomLogger<T>
    {
        public void LogInfo(object input);

        public void LogDebug(object input);

        public void LogError(object input, Exception e);

        public void LogError(object input);

    }

}
