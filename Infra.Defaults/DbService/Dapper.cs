using Dapper;
using Default.Application.Interfaces.DbContext;
using Default.Domain.Configs;

using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Defaults.DbService
{
    public class Dapper : IDapper
    {
        private readonly IOptions<DefaultDbConfig> _dbconfig;

        private readonly string _connectionString;
            
   

        public Dapper(IOptions<DefaultDbConfig> dbconfig)
        {
         
            _dbconfig = dbconfig;
            if (!String.IsNullOrWhiteSpace(_dbconfig.Value?.connectionString))
            {
                _connectionString = _dbconfig.Value?.connectionString;
            }
            
        }
        public void Dispose()
        {
            // no impliment
        }

        public async Task<int> Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            int result;
            IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                var tran = db.BeginTransaction();
                try
                {
                    result = await db.ExecuteAsync(sp, parms, commandType: commandType, transaction: tran);
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
            }
            catch
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_connectionString);
            var result = await db.QueryAsync<T>(sp, parms, commandType: commandType);
            return result.ToList();
        }

        public List<T> GetAll2<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_connectionString);
            var result = db.Query<T>(sp, parms, commandType: commandType);
            return result.ToList();
        }

        public DbConnection GetDbconnection()
        {
            return new SqlConnection(_connectionString);
        }

        public async Task<T> GetDetail<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            IDbConnection db = new SqlConnection(_connectionString);
            var result =
               await Task.Run(() =>
                {
                    return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
                }); 
            return result;
        }

        public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;

                }
            }
            catch
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
                throw;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }
        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            result = Insert<T>(sp, parms, commandType);
            return result;
        }

        public async Task<DynamicParameters> AddParam<T>(T model, string storeName, Guid? userId = null)
        {
            DynamicParameters dbparam = new DynamicParameters();
            var lstParam = await GetParamFromProc(storeName);

            foreach (string param in lstParam)
            {
                if (param == "TotalCount")
                {
                    dbparam.Add("TotalCount", 0, DbType.Int32, ParameterDirection.Output);
                }
                else if (param == "CreatedBy" || param == "UpdatedBy" || param == "CurrentUserId")
                {
                    dbparam.Add(param, userId, DbType.Guid);
                }
                else
                {
                    var prop = model.GetType().GetProperty(param);
                    if (prop != null)
                    {
                        var typeDB = dbTypeMap.ContainsKey(prop.PropertyType.FullName) ? dbTypeMap[prop.PropertyType.FullName] : DbType.String;
                        if (prop.PropertyType.FullName == typeof(List<Guid>).FullName)
                        {
                            List<Guid> listValue = prop.GetValue(model, null) != null ? (List<Guid>)prop.GetValue(model, null) : new List<Guid>();
                            dbparam.Add(param, listValue.Count > 0 ? string.Join(";", listValue) : "", typeDB);
                        }
                        else
                        {
                            dbparam.Add(param, prop.GetValue(model, null), typeDB);
                        }
                    }
                }
            }
            return dbparam;
        }

        private async Task<List<string>> GetParamFromProc(string storeName)
        {
            DynamicParameters dbparam = new DynamicParameters();
            dbparam.Add("StoreName", storeName, DbType.String);
            return await GetAll<string>("Proc_GetListParam", dbparam);
        }
        private Dictionary<string, DbType> dbTypeMap = new Dictionary<string, DbType>()
        {
            { typeof(bool).FullName, DbType.Boolean},
            { typeof(DateTime).FullName, DbType.DateTime},
            { typeof(DateTime?).FullName, DbType.DateTime},
            { typeof(decimal).FullName, DbType.Decimal},
            { typeof(double).FullName, DbType.Double},
            { typeof(Guid).FullName, DbType.Guid},
            { typeof(Guid?).FullName, DbType.Guid},
            { typeof(short).FullName, DbType.Int16},
            { typeof(int).FullName, DbType.Int32},
            { typeof(long).FullName, DbType.Int64},
            { typeof(string).FullName, DbType.String},
            { typeof(List<Guid>).FullName, DbType.String},
        };
    }
}
