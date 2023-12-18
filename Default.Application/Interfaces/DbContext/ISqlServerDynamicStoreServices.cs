using Default.Application.Interfaces.DbContext.dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Default.Application.Interfaces.DbContext
{
    public interface ISqlServerDynamicStoreServices
    {
        public SQLServerStoredResultDto GetByStoreProcedure(SQLServerStoredInputDto sQLServerStoredInput);

      //  public ResultModel GetDataBySqlQuery(SQLServerStoredInput sQLServerStoredInput);
    }
}
