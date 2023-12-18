using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.WSO2Authen.dtos
{
    public class Wso2LoginRequestDto
    {
        public string clientID { get; set; }
        public string clientSecret { get; set; }
        public string userName { get; set; }
        public string password { get; set; }    
        public string grant_type { get; set; }

        public string baseUrl { get; set; }
        public string urlLogin { get; set; }
    }
}
