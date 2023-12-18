using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Domain.Auth
{
    public class PrincipalModel
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Permission { get; set; }
    }
}
