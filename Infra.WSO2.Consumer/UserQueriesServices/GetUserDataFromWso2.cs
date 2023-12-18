using AutoMapper;
using Default.Application.BusinessServices.UserServices.dtos;
using Default.Application.CommonFeautures.APIConsumingFlow.Template;
using Default.Application.Interfaces.Cache;
using Default.Application.Interfaces.Logger;
using Default.Application.Interfaces.WSO2Authen;
using Default.Domain.Configs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.WSO2.Consumer.UserQueriesServices
{
    public class GetUserDataFromWso2 : GetDataFromHttpTemplate<UserInfoRequest, UserInfoResp>
    {
        public GetUserDataFromWso2(ICustomLogger<GetDataFromHttpTemplate<UserInfoRequest, UserInfoResp>> logger,
            IWso2Authentication wso2Authentication, 
            ISimpleCacheService simpleCacheService, 
            IOptions<Wso2AuthConfig> wso2AuthConfigs, 
            IMapper mapper) : base(logger, wso2Authentication, simpleCacheService, wso2AuthConfigs, mapper)
        {
        }

     

        /// <summary>
        /// Hàm viết tạm vui lòng sau xử lý logic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected async override  Task<UserInfoResp> ExecuteLogic(UserInfoRequest request)
        {
            var client = new RestClient(_wso2AuthConfigs.baseActionUrl); // );

            var requestHttp = new RestRequest("/apires/v1?q=" + request.userId , Method.Get);
            requestHttp.AddHeader("Authorization", "Bearer " + _accessToken);
            var response = await client.ExecuteAsync(requestHttp);
            Console.WriteLine(response.Content);
            var result  = JsonConvert.DeserializeObject<UserInfoResp>(response?.Content);

            return result;
        }
    }
}
