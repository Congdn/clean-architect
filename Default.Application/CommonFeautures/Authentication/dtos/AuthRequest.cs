using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.CommonFeautures.Authentication.dtos
{
    public class AuthRequest
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? RemoteIpAddress { get; set; }
        public object? ExtraProps { get; set; }
    }
}
