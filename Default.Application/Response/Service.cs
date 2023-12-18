using Default.Application.Interfaces.Common;
using Default.Domain.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Response
{
    public abstract class Service
    {
        protected PrincipalModel _principal ;
        private readonly ICurrentPrincipal _currentPrincipal;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public Service(ICurrentPrincipal currentPrincipal, IHttpContextAccessor httpContextAccessor)
        {
            _currentPrincipal = currentPrincipal;
            _principal = _currentPrincipal.Principal;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual ServiceResponse Accepted(object data ) => ServiceResponse.Succeed(StatusCodes.Status202Accepted, data);

        public virtual ServiceResponse BadRequest(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status400BadRequest, code, message);

        public virtual ServiceResponse Created(object data) => ServiceResponse.Succeed(StatusCodes.Status201Created, data);

        public virtual ServiceResponse Forbidden(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status403Forbidden, code, message);

        public virtual ServiceResponse NotFound(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status404NotFound, code, message);

        public virtual ServiceResponse Ok(object data , string code = "", string message = "") => ServiceResponse.Succeed(StatusCodes.Status200OK, data, code, message);

        public virtual ServiceResponse Unauthorized(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status401Unauthorized, code, message);

        public virtual ServiceResponse ServerError(string code = "", string message = "") => ServiceResponse.Fail(StatusCodes.Status500InternalServerError, code, message);
    }
}
