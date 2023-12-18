using Default.Application.CommonFeautures.Authentication;
using Default.Application.CommonFeautures.Authentication.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Mappers
{
    public class RequestMappingProfile : AutoMapper.Profile
    {
        public RequestMappingProfile()
        {
            //TODO: Mapping dữ liệu trả về từ các layer khác (ví dụ từ repositoy với DTO data transfer object
            
            //From -> To (Map từ AuthRequest sang MakerAuth model)
            CreateMap<AuthRequest, Interfaces.Authen.dtos.MakerAuth>()
                .ForMember(des => des.username, act => act.MapFrom(src => src.UserName))
                .ForMember(des => des.password, act => act.MapFrom(dst => dst.Password))
                ;
            //CreateMap< Interfaces.Authen.dtos.MakerAuthResponse, >
            //CreateMap<CategoryCreateRequestModel, Category>().IgnoreAllNonExisting();
            //CreateMap<CategoryItemCreateRequestModel, CategoryItem>().IgnoreAllNonExisting();

            //Map for request login wso2
            CreateMap<Domain.Configs.Wso2AuthConfig, Interfaces.WSO2Authen.dtos.Wso2LoginRequestDto>()
                .ForMember(des => des.baseUrl, act => act.MapFrom(src => src.baseAuthUrl))
                  .ForMember(des => des.urlLogin, act => act.MapFrom(src => src.accessTokenEnpoint))
                  .ForMember(des => des.clientID, act => act.MapFrom(src => src.clientID))
                    .ForMember(des => des.clientSecret, act => act.MapFrom(src => src.clientSecret))
                ;
        }
    }
}
