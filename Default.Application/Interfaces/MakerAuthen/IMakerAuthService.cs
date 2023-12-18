using Default.Application.Interfaces.Authen.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.Authen
{
    public interface IMakerAuthService
    {
        public  Task<MakerAuthResponse> Login(MakerAuth authReq);
    }
}
