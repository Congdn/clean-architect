using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Domain.Configs
{
    public class RedisCacheSettings :  CacheConfig
    {
        public bool Enabled { get; set; }

        public string? ConnectionString { get; set; }
        public string? AbortOnConnectFail { get; set; }
    }
}
