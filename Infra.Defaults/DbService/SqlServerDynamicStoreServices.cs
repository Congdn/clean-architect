using Default.Application.Interfaces.DbContext;
using Default.Application.Interfaces.DbContext.dtos;
using Default.Application.Interfaces.Logger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.DbService
{
    public class SqlServerDynamicStoreServices : ISqlServerDynamicStoreServices
    {
        
        private readonly ICustomLogger<SqlServerDynamicStoreServices> _logger;

        public SqlServerDynamicStoreServices(ICustomLogger<SqlServerDynamicStoreServices> logger)
        {
            _logger = logger;
        }
        private SqlDbType ConvertiTipo(string giveType)
        {

            SqlDbType res = SqlDbType.NVarChar;
            giveType = giveType.ToLower();

            switch (giveType)
            {
                case "nvarchar":
                    res = SqlDbType.NVarChar;
                    break;
                case "varchar":
                    res = SqlDbType.VarChar;
                    break;
                case "char":
                    res = SqlDbType.Char;
                    break;
                case "ntext":
                    res = SqlDbType.NText;
                    break;
                case "text":
                    res = SqlDbType.Text;
                    break;
                case "int":
                    res = SqlDbType.Int;
                    break;
                case "int32":
                    res = SqlDbType.Int;
                    break;
                case "int64":
                    res = SqlDbType.BigInt;
                    break;
                case "int16":
                    res = SqlDbType.SmallInt;
                    break;
                case "boolean":
                    res = SqlDbType.Bit;
                    break;
                case "datetime":
                    res = SqlDbType.DateTime;
                    break;
                case "datetime2":
                    res = SqlDbType.DateTime2;
                    break;
                case "date":
                    res = SqlDbType.Date;
                    break;
                case "decimal":
                    res = SqlDbType.Decimal;
                    break;
                case "float":
                    res = SqlDbType.Float;
                    break;
                case "uuid":
                    res = SqlDbType.UniqueIdentifier;
                    break;
                case "uniqueidentifier":
                    res = SqlDbType.UniqueIdentifier;
                    break;
                default:
                    res = SqlDbType.NVarChar;
                    break;

            }


            return res;

        }


        /// <summary>
        /// convert fieldName to true param Detect
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name=""></param>
        /// <returns></returns>
        private String ConvertFieldNameFromDb(string fieldName, Dictionary<string, object> paramTypesDetected)
        {
            var res = fieldName;
            if (String.IsNullOrWhiteSpace(fieldName) || paramTypesDetected == null || paramTypesDetected.Count == 0)
                return String.Empty;

            foreach (var item in paramTypesDetected)
            {
                // if (item == null) continue;
                if (item.Key.ToLower().Equals(fieldName.ToLower()))
                {
                    res = item.Key;
                    return res;
                }
            }


            return res;

        }

        private List<SqlParameter> convertInputToParam(SQLServerStoredInputDto input)
        {
            var result = new List<SqlParameter>();
            if (input != null && input.paramValues != null && input.paramTypes != null
                && input.paramTypes.Count > 0 && input.paramTypes.Count == input.paramValues.Count
                )
            {

                foreach (var item in input.paramValues)
                {
                    // Get fields (name, type, value)
                    var fieldName = item.Key;

                    fieldName = ConvertFieldNameFromDb(fieldName, input.paramTypes);

                    var fieldValue = item.Value;
                    var fieldType = input.paramTypes[fieldName].ToString();

                    SqlParameter param = new SqlParameter(fieldName, SqlDbType.NVarChar, 256);
                    result.Add(param);
                    param.SqlDbType = ConvertiTipo(fieldType);
                    switch (fieldType)
                    {
                        case "uuid":

                            param.Value = Guid.Parse(fieldValue.ToString());
                            break;
                        case "uniqueidentifier":

                            param.Value = Guid.Parse(fieldValue.ToString());
                            break;
                        case "datetime":

                            param.Value = DateTime.Parse(fieldValue.ToString());
                            break;
                        case "nvarchar":
                            param.Value = fieldValue.ToString();
                            param.Size = fieldValue.ToString().Length;
                            break;

                        default:
                            param.Value = fieldValue;
                            break;
                    }
                }
            }
            else
            {
                var message = "[CUS_ERROR] " + "Param truyền vào không khớp với param trên Stored";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return result;
        }

        private Dictionary<string, object> DetectParamInputStore(SQLServerStoredInputDto sQLServerStoredInput, out bool isDetectOK)
        {
            isDetectOK = true;
            var result = new Dictionary<string, object>();
            var sql = "select PARAMETER_MODE,PARAMETER_NAME,DATA_TYPE from information_schema.parameters where specific_name=  @specific_name";
            var queryParams = new Dictionary<string, object>();
            queryParams.Add("specific_name", sQLServerStoredInput.procedure);

            //  var inputs = UtilDAO.GetBySQLDapper<StoreInputMetadata>(sQLServerStoredInput.conn, sql, queryParams, out isDetectOK);
            var inputs = UtilDAO.GetBySQLDapper<StoreInputMetadata>(sQLServerStoredInput.conn, sql, queryParams);
            if (inputs != null && inputs.Count > 0)
            {
                foreach (var item in inputs)
                {
                    if (item == null || String.IsNullOrWhiteSpace(item.PARAMETER_NAME))
                    {
                        continue;
                    }
                    var paramName = item.PARAMETER_NAME;
                    if (item.PARAMETER_NAME.StartsWith("@"))
                    {
                        paramName = item.PARAMETER_NAME.Substring(1);
                    }
                    result.Add(paramName, item.DATA_TYPE.ToLower());

                }
            }
            else if (inputs == null)
            {
                isDetectOK = false;
            }

            return result;
        }

        public SQLServerStoredResultDto GetByStoreProcedure(SQLServerStoredInputDto sQLServerStoredInput)
        {
            bool isDetectOk = true;
            SqlConnection sqlConn = null;
            var result = new SQLServerStoredResultDto();
            try
            {
                using (sqlConn = new SqlConnection(sQLServerStoredInput.conn))
                {
                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Closed) sqlConn.Open();
                    var cmd = new SqlCommand(sQLServerStoredInput.procedure, sqlConn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Detect Param
                    sQLServerStoredInput.paramTypes = DetectParamInputStore(sQLServerStoredInput, out isDetectOk);

                    List<SqlParameter> inputParams = convertInputToParam(sQLServerStoredInput);

                    #region ca2

                    foreach (var item in inputParams)
                    {

                        cmd.Parameters.Add(item);
                    }

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    #endregion


                    if (ds.Tables != null && ds.Tables.Count > 0)
                    {
                        result.result = JsonConvert.SerializeObject(ds.Tables[0]);
                    }
                    else
                    {
                        result.result = "";
                    }


                    result.msgCode = "1";
                    result.msg = "Thành công";
                }
            }
            catch (Exception e)
            {
                var message = "[CUS_ERROR] " + "User Db không có quyền detect param store procedure";
                result.msgCode = "-1";
                result.msg = "Thất bại";
                result.result = JsonConvert.SerializeObject(e);
                if (isDetectOk == false)
                {
                    result.msg = message;
                }


            }
            finally
            {
                if (sqlConn != null && sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }

            return result;
        }




        public SQLServerStoredResultDto GetDataBySqlQuery(SQLServerStoredInputDto sQLServerStoredInput)
        {

            bool isDetectOk = true;
            SqlConnection sqlConn = null;
            var result = new SQLServerStoredResultDto();
            try
            {
            }
            catch (Exception e)
            {
                var message = "[CUS_ERROR] " + "User Db không có quyền detect param store procedure";
                result.msgCode = "-1";
                result.msg = "Thất bại";
                result.result = JsonConvert.SerializeObject(e);
                if (isDetectOk == false)
                {
                    result.msg = message;
                }
                _logger.LogError(message);

            }
            finally
            {
                if (sqlConn != null && sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }

            return result;
        }

        //public SQLServerStoredResultDto GetByStoreProcedure(SQLServerStoredInputDto sQLServerStoredInput)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
