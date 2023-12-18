using Default.Application.Interfaces.Authen;
using Default.Application.Interfaces.Authen.dtos;
using Default.Domain.Configs;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.MakerConsumer.AuthServices
{
    public class MakerAuthService : IMakerAuthService
    {
        private readonly AspnetMakerConfig _mkconfig;

        public MakerAuthService(IOptions<AspnetMakerConfig> mkconfig)
        {
            _mkconfig = mkconfig.Value;
        }

        public async Task<MakerAuthResponse> Login(MakerAuth authReq)
        {
            var resp = new MakerAuthResponse(); 
            //var urlLogin = _mkconfig.endpoint + ;
            var client = new RestClient(_mkconfig.endpoint);
         
            var request = new RestRequest("/api/login" , Method.Post);
            request.AddHeader("Accept", "application/json");
           // request.AddHeader("Cookie", ".AspNetCore.Cookies=CfDJ8B_WjvncreNJvPV6y5W-9ukeDXMzFOeHH-iEucr-b75CvSX8rECK9EqDH-Kc33uLO6NGH-JK7J4PbZzZQ6OJ19s-pHqQy7wfBzfJxz9Q8mAg--WDbxrJMFEo81KgboThnUuVVR4xFQ7jPr9ncbLoA7mMzI5l0EMnYex-yzKwgNbFM8ZCyeSnfkZp6SETyxVvWSGa1kyLTjIAyeAtyRbn24AWtcKzSnsNExfukxSCdEYoy1uF--xEiGq4Gx8Ouiwqr4I_XGUb6R4dJhT77eblRuRYnzcSNFd_YjfKsSZRjkcyu2fdv_RZ7J04pZ-BnoyXsB6PBzp0q1XtWROtkmyZ0TuJgpJ18m_eNSku6ij4jA72ZV9KXDybFiPgKHstHQwdzsYNJD5UMCUi1H37GTlhiYE");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("username", authReq.username);
            request.AddParameter("password", authReq.password);
            var response =  await client.ExecuteAsync(request);
            //Console.WriteLine(response.Content);
            var temp_JWT_obj = response.Content;
            if(temp_JWT_obj != null && temp_JWT_obj.IndexOf("JWT") > -1)
            {
                resp = JsonConvert.DeserializeObject<MakerAuthResponse>(temp_JWT_obj);
            }

            return resp;
        }
    }
}
