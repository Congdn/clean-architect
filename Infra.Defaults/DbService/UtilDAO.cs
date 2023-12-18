using Dapper;

using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Infra.Defaults.DbService
{
    public static class UtilDAO
    {


        public static long GetNextSequence(String conn, String sequenceName)
        {
            SqlConnection sqlConn = null;
            try
            {
                using (sqlConn = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand();
                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Closed)
                    {
                        sqlConn.Open();
                    }
                    cmd.Connection = sqlConn;

                    cmd.CommandText = " select NEXT VALUE FOR " + sequenceName + "";
                    var lastId = cmd.ExecuteScalar();
                    return (long)lastId;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sqlConn != null && sqlConn.State == ConnectionState.Open)
                {
                    sqlConn.Close();
                }
            }



        }
        public static object UpdateOrDeleteQueryID(SqlConnection sqlConn, String sql, Dictionary<string, object> param)
        {
            object change = null;

            try
            {
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Closed)
                    {
                        sqlConn.Open();
                    }
                    cmd.Connection = sqlConn;

                    //DynamicParameters parameters = new DynamicParameters();
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            cmd.Parameters.AddWithValue(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value == null ? DBNull.Value : item.Value);

                        }
                    }


                    change = Convert.ToInt64(cmd.ExecuteScalar());

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return change;
        }

        public static bool UpdateOrDeleteQuery(SqlConnection sqlConn, SqlTransaction transaction, String sql, Dictionary<string, object> param)
        {
            bool result = true;

            try
            {


                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;
                    cmd.Transaction = transaction;
                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Closed)
                    {
                        sqlConn.Open();
                    }
                    cmd.Connection = sqlConn;

                    //DynamicParameters parameters = new DynamicParameters();
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            cmd.Parameters.AddWithValue(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value == null ? DBNull.Value : item.Value);

                        }
                    }


                    int change = cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        public static void CloseConn(SqlConnection sqlConn)
        {
            if (sqlConn != null && sqlConn.State == ConnectionState.Open)
            {
                sqlConn.Close();
            }
        }

        public static IEnumerable<T> GetByStoreProcedureIE<T>(SqlConnection sqlConn, String procedure, Dictionary<string, object> param)
        {
            IEnumerable<T> result = null;

            List<String> paramString = new List<String>();
            try
            {
                if (sqlConn.State == ConnectionState.Closed)
                {
                    sqlConn.Open();
                }

                SqlCommand cmd = new SqlCommand(procedure, sqlConn);

                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which will be passed to the stored procedure
                if (param != null)
                {
                    foreach (var item in param)
                    {
                        cmd.Parameters.AddWithValue(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value == null ? DBNull.Value : item.Value);

                    }
                }

                cmd.ExecuteReader();

            }
            catch (Exception ex)
            {

                result = null;
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


        public static bool UpdateOrDeleteQuery(string conn, String sql, Dictionary<string, object> param)
        {
            bool result = true;
            SqlConnection sqlConn = null;
            try
            {


                using (sqlConn = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Closed)
                    {
                        sqlConn.Open();
                    }
                    cmd.Connection = sqlConn;

                    //DynamicParameters parameters = new DynamicParameters();
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            cmd.Parameters.AddWithValue(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value == null ? DBNull.Value : item.Value);

                        }
                    }


                    int change = cmd.ExecuteNonQuery();
                    if (change == 0)
                    {
                        result = false;
                    }

                }
            }
            catch (Exception ex)
            {
                result = false;
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

        public static DataTable SelectObject(SqlConnection sqlConn, String sql, Dictionary<string, object> param)
        {
            DataTable dataTable = new DataTable();

            try
            {


                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = sql;

                    if (sqlConn != null && sqlConn.State == System.Data.ConnectionState.Closed)
                    {
                        sqlConn.Open();
                    }
                    cmd.Connection = sqlConn;
                    cmd.Prepare();

                    //DynamicParameters parameters = new DynamicParameters();
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            cmd.Parameters.AddWithValue(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value == null ? DBNull.Value : item.Value);

                        }
                    }


                    SqlDataAdapter dataAdapt = new SqlDataAdapter();
                    dataAdapt.SelectCommand = cmd;

                    dataAdapt.Fill(dataTable);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataTable;
        }

        public static List<T> ToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }



        public static List<T> GetBySQLDapper<T>(string conn, String sql, Dictionary<string, object> param)
        {
            List<T> result = new List<T>();
            SqlConnection sqlConn = null;
            try
            {
                using (sqlConn = new SqlConnection(conn))
                {
                    DynamicParameters parameters = new DynamicParameters();
                    if (param != null)
                    {
                        foreach (var item in param)
                        {
                            parameters.Add(item.Key.StartsWith("@") ? item.Key : "@" + item.Key, item.Value);
                        }
                    }
                    List<String> sqlP = new List<string>();

                    result = (List<T>)SqlMapper.Query<T>((SqlConnection)sqlConn, sql, parameters);


                }
            }
            catch (Exception ex)
            {

                result = null;
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
    }




}
