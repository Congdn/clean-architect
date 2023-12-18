using Default.Application.Authentication.dtos;
using Default.Application.Response;
using Default.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Feautures.Authentication
{
    public interface IAuthentication
    {
        Task<ServiceResponse> AuthenticateAsync(AuthRequest request);

        //Task<ServiceResponse> RefreshTokenAsync(RefreshTokenRequest request);

        ServiceResponse ValidateToken(string token);
    }
}
