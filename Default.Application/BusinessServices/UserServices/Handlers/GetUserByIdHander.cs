using Default.Application.BusinessServices.UserServices.dtos;
using Default.Application.BusinessServices.UserServices.Queries;
using Default.Application.CommonFeautures.APIConsumingFlow.Template;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.UserServices.Handlers
{
    public class GetUserByIdHander : IRequestHandler<GetUserByIdQuery, UserInfoResp>
    {
        private readonly GetDataFromHttpTemplate<UserInfoRequest, UserInfoResp> _getUserById;

        public GetUserByIdHander(GetDataFromHttpTemplate<UserInfoRequest, UserInfoResp> getUserById)
        {
            _getUserById = getUserById;
        }

        public async Task<UserInfoResp> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var req = new UserInfoRequest();
            return await _getUserById.Excute(req);
        }
    }
   

}
