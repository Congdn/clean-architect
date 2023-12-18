
using Default.Application.CommonFeautures.Authentication;
using Default.Application.CommonFeautures.Authentication.dtos;
using Default.Application.Response;
using Default.Domain.Auth;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BE.Template.Controllers
{
    [ApiController]
    [Route("api/auth")]
    //[SwaggerTag("Authentication")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthentication _authService;
        public AuthController(IAuthentication authService) => _authService = authService;

        [HttpPost("login")]
        public async Task<ServiceResponse> Login(LoginModel model) => await _authService.AuthenticateAsync(new AuthRequest
        {
            Password = model.Password,
            UserName = model.UserName,
            ExtraProps = model.ExtraProps,
            RemoteIpAddress = GetIpAddress()
        });

        private string GetIpAddress()
        {
            return Request.Headers.ContainsKey("X-Forwarded-For")
                ? Request.Headers["X-Forwarded-For"].ToString()
                : HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
