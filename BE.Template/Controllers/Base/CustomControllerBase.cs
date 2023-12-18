using Default.Application.Interfaces.Logger;
using Microsoft.AspNetCore.Mvc;

namespace BE.Template.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {
        protected readonly ICustomLogger<T> _logger;

        public BaseController(ICustomLogger<T> logger)
        {
            _logger = logger;
        }

       // protected ICustomLogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ICustomLogger<T>>();

        protected async Task<String> GetUserByToken()
        {
            var res = String.Empty;
            const string HeaderKeyName = "Authorization";

            Object headerValue = null;
            HttpContext.Items.TryGetValue(HeaderKeyName, out headerValue);


            return res;
        }
    }
}
