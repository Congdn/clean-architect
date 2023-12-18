using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.Common
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
