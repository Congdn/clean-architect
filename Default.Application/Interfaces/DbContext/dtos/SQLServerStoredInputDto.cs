using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.DbContext.dtos
{
    public class SQLServerStoredInputDto
    {
        public string? conn { get; set; }
        public string? procedure { get; set; }

        public Dictionary<string, object>? paramValues { get; set; }
        public Dictionary<string, object>? paramTypes { get; set; }
    }
}
