using Default.Application.BusinessServices.UserServices.dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.UserServices.Queries
{
    public class GetUserByIdQuery : IRequest<UserInfoResp>
    {
        public int Id { get; set; }
    }
}
