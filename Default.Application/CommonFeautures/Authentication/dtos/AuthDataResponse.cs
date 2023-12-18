using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.CommonFeautures.Authentication.dtos
{
    public class AuthDataResponse
    {
        public string? accessToken { get; set; } 
        public string? refreshToken { get; set; }
    }
}
