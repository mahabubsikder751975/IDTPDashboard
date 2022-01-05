using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SuperAdminPortal.Models
{
    public class SqlClientHelper
    {
        //public static string IDTPConnectionString;

        public static List<T> GetData<T>(string commandText, System.Data.CommandType commandType, string connString)
        {
            try
            {
                List<T> list = new List<T>();
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = new SqlCommand(commandText, conn);
                    command.CommandType = commandType;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            T t = Activator.CreateInstance<T>();
                            for (int inc = 0; inc < reader.FieldCount; inc++)
                            {
                                PropertyInfo prop = typeof(T).GetProperty(reader.GetName(inc));
                                if (prop != null)
                                {
                                    var valueFromDb = reader.GetValue(inc);
                                    if (valueFromDb != DBNull.Value && valueFromDb != null)
                                        prop.SetValue(t, valueFromDb, null);
                                }
                            }
                            list.Add(t);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return new List<T>();
            }
        }

        public static string GetSingleRecord(string commandText, System.Data.CommandType commandType, string connString, Dictionary<string, string> paramsDict = null)
        {
            string result = string.Empty;
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = connString;
                    conn.Open();
                    SqlCommand command = new SqlCommand(commandText, conn);
                    command.CommandType = commandType;
                    if (paramsDict != null)
                    {
                        foreach(var item in paramsDict)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                }
                return result;
            }
            catch (SqlException ex)
            {
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public static bool ExecuteDBCommand(string cmdText, string connString)
        {
            string connectionString = connString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ExecuteProcedure<T>(string cmdText, T obj, string connString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        foreach (var pro in typeof(T).GetProperties())
                        {
                            if (pro.Name != "RoleName" && pro.Name != "ConfirmPassword")
                                command.Parameters.AddWithValue(pro.Name, pro.GetValue(obj).ToString());
                        }
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return true;
            }
            catch (SqlException ex2)
            {
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
