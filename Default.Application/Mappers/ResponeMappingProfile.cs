using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Mappers
{
    public class ResponeMappingProfile : AutoMapper.Profile
    {
        public ResponeMappingProfile()
        {
            CreateMap<Interfaces.Authen.dtos.MakerAuthResponse, AuthDataResponse>()
                   .ForMember(des => des.accessToken, act => act.MapFrom(src => src.JWT));
        }
    }
}
