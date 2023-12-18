using AutoMapper;
using Default.Application.Interfaces.Cache;
using Default.Application.Interfaces.Logger;
using Default.Application.Interfaces.WSO2Authen;
using Default.Application.Interfaces.WSO2Authen.dtos;
using Default.Domain.Configs;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Default.Domain.Constants.Wso2Constants;

namespace Default.Application.CommonFeautures.APIConsumingFlow.Template
{

    /// <summary>
    /// template design pattern
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class GetDataFromHttpTemplate<TRequest, TResponse>
    {

        protected ICustomLogger<GetDataFromHttpTemplate<TRequest, TResponse>> _logger;
        protected readonly IWso2Authentication _wso2Authentication;
        protected string _accessToken;
        protected readonly ISimpleCacheService _simpleCacheService;
        protected readonly Wso2AuthConfig _wso2AuthConfigs;
        protected readonly IMapper _mapper;

        protected GetDataFromHttpTemplate(ICustomLogger<GetDataFromHttpTemplate<TRequest, TResponse>> logger,
            IWso2Authentication wso2Authentication,
            ISimpleCacheService simpleCacheService
            , IOptions<Wso2AuthConfig> wso2AuthConfigs, IMapper mapper)
        {
            _logger = logger;
            _wso2Authentication = wso2Authentication;
            _accessToken = string.Empty;
            _simpleCacheService = simpleCacheService;
            _wso2AuthConfigs = wso2AuthConfigs.Value;
            _mapper = mapper;
        }



        /// <summary>
        /// Logic goi va lay dữ liệu sẽ được thực hiện tại hàm này
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract Task<TResponse> ExecuteLogic(TRequest request);


        /// <summary>
        /// Hàm wrapper, bên ngoài sẽ thực hiện gọi đến hàm này để excute
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<TResponse> Excute(TRequest request)
        {
            TResponse result;
            _accessToken = await GetAcesstoken();

            result = await ExecuteLogic(request);

            return result;
        }


        /// <summary>
        /// Logic này có thể viết thêm phần lấy access token từ refresh token, kiểm tra token còn hợp lệ không bằng introspect,
        ///  trong demo sẽ bỏ qua để đơn giản
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetAcesstoken()
        {

            var accessTokenObj = new Wso2AccessTokenDto();

            //Thuc hien map từ config sang request login sử dụng automapper
            var requestLogin = _mapper.Map<Wso2LoginRequestDto>(_wso2AuthConfigs);
            requestLogin.grant_type = Wso2AuthenType._credential;

            accessTokenObj = await _wso2Authentication.GetAccToken(requestLogin);


            if (accessTokenObj != null && !string.IsNullOrWhiteSpace(accessTokenObj?.access_token))
            {
                _accessToken = accessTokenObj.access_token;
                return accessTokenObj.access_token;
            }

            //TODO: thêm đoạn save token vào cache tại đây, và kiểm tra token ở cache tại đây

            return string.Empty;
        }
    }
}
