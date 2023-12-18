
using Default.Application.Interfaces.WSO2Authen.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.WSO2Authen
{
    public interface IWso2Authentication
    {
        public Task<Wso2AccessTokenDto> GetAccToken(Wso2LoginRequestDto requestLogin);

        public Task<Wso2AccessTokenDto> RefreshToken(Wso2AccessTokenDto requestRefresh);


        public Task<bool> IntrospectToken(String accessToken);
    }
}
