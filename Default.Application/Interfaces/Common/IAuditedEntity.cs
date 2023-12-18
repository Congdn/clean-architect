using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace Default.Application.Interfaces.Common
{
    public class AuditedEntity
    {
       public DateTime CreatedAt { get; set; }
        public String CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public String UpdatedBy { get; set; }
    }
}
