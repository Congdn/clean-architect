using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.DbContext.dtos
{
    public class SQLServerStoredResultDto
    {
        public string? msgCode { get; set; }
        public string? msg { get; set; }
        public string? result { get; set; }
    }
}
