using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.BusinessServices.UserServices.dtos
{


    public class UserInfoResp
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }


    public class UserInfoRequest
    {
        public int userId { get; set; }
        
    }
}
