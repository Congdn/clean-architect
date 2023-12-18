using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Domain.Constants
{
    public enum LogType
    {
        Insert,
        Update,
        Delete,
        Get,
        Send,
        Wait,
        View,

        Trace,
        Error,
        Warning,
        Info,
    }
}
