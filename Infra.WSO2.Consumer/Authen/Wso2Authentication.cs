using Default.Application.Interfaces.Logger;
using Default.Application.Interfaces.WSO2Authen;
using Default.Application.Interfaces.WSO2Authen.dtos;
using Default.Domain.Constants;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Default.Domain.Constants.Wso2Constants;

namespace Infra.WSO2.Consumer.Authen
{
    public class Wso2Authentication : IWso2Authentication
    {
        private readonly ICustomLogger<Wso2Authentication> _logger;

        public Wso2Authentication(ICustomLogger<Wso2Authentication> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Hàm viết tạm, phải chuyển vào trong 1 project utilities dùng chung
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private String basicAuthenHeader(String username , string password)
        {
            var textBytes = Encoding.UTF8.GetBytes(username + ":" + password);
            // after: 72 101 108 108 111 32 119 111 114 108 100 33 
            var base64String = "Basic " + Convert.ToBase64String(textBytes);
            return base64String;
        }

        public async Task<Wso2AccessTokenDto> GetAccToken(Wso2LoginRequestDto requestLogin)
        {
            var resp = new Wso2AccessTokenDto();
            var msg = string.Empty;

            if (requestLogin == null)
            {
                msg = "Vui lòng truyền thông tin đăng nhập";
                _logger.LogError(msg);
                throw new Exception(msg);
            }
               

            if(String.IsNullOrWhiteSpace(requestLogin.grant_type))
            {
                msg =("Vui lòng loại xác thực grant_type");
                _logger.LogError(msg);
                throw new Exception(msg);
            }


            var client = new RestClient(requestLogin.baseUrl);
           
            var request = new RestRequest(requestLogin.urlLogin, Method.Post);

            request.AddHeader(AuthorizationHeader._Defaul, basicAuthenHeader(requestLogin.clientID, requestLogin.clientSecret));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        
          
         


            switch (requestLogin.grant_type)
            {
               case Wso2AuthenType._credential:
                    request.AddParameter("grant_type", Wso2AuthenType._credential);
                    break;
               case Wso2AuthenType._password:
                    request.AddParameter("grant_type", Wso2AuthenType._password);
                    request.AddParameter("username", requestLogin.userName);
                    request.AddParameter("password", requestLogin.password);
                    break;
                default:
                   msg = ("Không hỗ trợ loại xác thực grant_type này");
                    _logger.LogError(msg);
                    throw new Exception(msg);
                  
            }

            var response = await client.ExecuteAsync(request);
            resp = Newtonsoft.Json.JsonConvert.DeserializeObject<Wso2AccessTokenDto>  (response.Content);

            return resp;
        }

        public Task<Wso2AccessTokenDto> RefreshToken(Wso2AccessTokenDto requestRefresh)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IntrospectToken(string accessToken)
        {
            throw new NotImplementedException();
        }
    }
}
