using Default.Application.BusinessServices.UserServices.dtos;
using Default.Application.BusinessServices.UserServices.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Template.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalUserSampleController : ControllerBase
    {
        private readonly IMediator mediator;

        public ExternalUserSampleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("studentId")]
        public async Task<UserInfoResp> GetUserById(int UserID)
        {
            var studentDetails = await mediator.Send(new GetUserByIdQuery() { Id = UserID });

            return studentDetails;
        }

    }
}
