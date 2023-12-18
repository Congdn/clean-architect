using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.Cors.dtos
{
    public class GemCorsOptions
    {
        public IEnumerable<string> AllowedOrigins { get; set; }
    }
}
