using System;
using System.Data;
using System.Data.SqlClient;
using SpargoTech_Test.Configuration;

namespace SpargoTech_Test.Data
{
    public class SQLWorker
    {
        private string ConnectionString = ConfigWorker.GetConnectionString();
        private static SqlConnection Connection = new SqlConnection();    

        /// <summary>
        /// Открыть соединение с БД
        /// </summary>
        /// <param name="connectionString" - строка подключения к БД></param>
        /// <returns></returns>
        public bool OpenConnection()
        {
            Connection.ConnectionString = ConnectionString;
            try
            {
                if (Connection.State != ConnectionState.Open) Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Закрыть соединение с БД
        /// </summary>
        /// <returns></returns>
        public bool CloseConnection()
        {
            try
            {
                if (Connection == null)
                    return false;
                if (Connection.State == ConnectionState.Open) Connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;               
            }
        }

        /// <summary>
        /// Выполнить запрос Select
        /// </summary>
        /// <param name="sqlExpression" - выражение SELECT - запроса></param>
        /// <returns></returns>
        public DataTable SelectRequest (string sqlExpression)
        {
                try
                {
                OpenConnection();
                    using (SqlCommand command = new SqlCommand(sqlExpression, Connection))
                    {
                        using (SqlDataAdapter DataAdapter = new SqlDataAdapter(command))
                        {
                            var ds = new DataSet();
                            DataAdapter.Fill(ds);
                            DataTable dt = ds.Tables[0];                           
                            return dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nОшбка запроса к БД...\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    return new DataTable();
                }
                finally
                {
                    CloseConnection();
                }
        }

        /// <summary>
        /// Выполнить запрос INSERT, DELETE, UPDATE
        /// </summary>
        /// <param name="sqlExpression" - выражение DELETE, INSERT, UPDATE - запроса></param>
        /// <returns></returns>
        public int ExecuteNonQuery (string sqlExpression)
        {
            try
            {
                OpenConnection();
                using (SqlCommand command = new SqlCommand(sqlExpression, Connection))
                {
                    var result = command.ExecuteNonQuery();
                    return result;
                }
            }
            catch (Exception ex)
            {               
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nОшбка запроса к БД...\n");            
                Console.ForegroundColor = ConsoleColor.White;
                return 0;
            }
            finally
            {
                CloseConnection();
            }

        }


    }
}



