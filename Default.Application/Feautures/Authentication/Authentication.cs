using AutoMapper;
using Default.Application.Authentication.dtos;
using Default.Application.Feautures.Authentication.dtos;
using Default.Application.Interfaces.Authen;
using Default.Application.Interfaces.Common;
using Default.Application.Response;
using Microsoft.AspNetCore.Http;

namespace Default.Application.Feautures.Authentication
{
    public class Authentication : Default.Application.Response.Service, IAuthentication
    {
        private readonly IMakerAuthService _makerAuth;
        private readonly IMapper _mapper;
        public Authentication(IMakerAuthService makerAuth
            , IMapper mapper
            , ICurrentPrincipal currentPrincipal
            , IHttpContextAccessor httpContextAccessor) : base(currentPrincipal, httpContextAccessor)
        {
            _makerAuth = makerAuth;
            _mapper = mapper;   
        }
        public async Task<ServiceResponse> AuthenticateAsync(AuthRequest request)
        {
            var authReq = _mapper.Map<Interfaces.Authen.dtos.MakerAuth>(request);
            var authResp = await _makerAuth.Login(authReq);

            if(authResp == null)
            {
                return ServerError("500", "Unknow Error!");
            }

            if(String.IsNullOrWhiteSpace( authResp.JWT))
            {

                return Ok(string.Empty, "login_failure", "Invalid username or password !");
            }

            else
            {
                var authRes = _mapper.Map<AuthDataResponse>(authResp);
                return Ok(authRes, "login_success", "");
            }


           

        }

        public ServiceResponse ValidateToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
