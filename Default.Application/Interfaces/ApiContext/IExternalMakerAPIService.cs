using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.ApiContext
{
    public  interface IExternalMakerAPIService 
    {
        public Task<String> getAccessToken(String userName, String password);

        public Task<String> getAccessToken(String authenCode);


        public dynamic GetDeail(String id);

        public IList<Object> GetList(String table, String query);

        public String Update(String id, String paramJson);

        public String Create(String paramJson);

        public bool Delete(String id);  
 

        
       
    }
}
