using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Domain.Configs
{
    public class Wso2AuthConfig
    {
        public string clientID { get; set; }
        public string clientSecret { get; set; }

        public string baseActionUrl { get; set; }
        public string baseAuthUrl { get; set; } 
        public string introspectEndpoint { get; set; }
        public string accessTokenEnpoint { get; set; }

    }
}
