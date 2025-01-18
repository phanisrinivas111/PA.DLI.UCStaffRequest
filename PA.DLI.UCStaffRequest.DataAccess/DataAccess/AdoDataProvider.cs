using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PA.DLI.UCStaffRequest.DataAccess.DataAccess
{
    public class AdoDataProvider
    {
        private readonly string _connectionString;
        public AdoDataProvider()
        {
            var connectionStringSettings = ConfigurationManager.ConnectionStrings["LocalConnection"];
            if (connectionStringSettings == null)
            {
                throw new Exception("Connection String is not defined");
            }
            _connectionString = connectionStringSettings.ConnectionString;
        }

        public SqlConnection GetDbConnection()
        {
            return new SqlConnection(_connectionString);
        }


        public DataTable ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);

                    }
                }
                var dataTable = new DataTable();
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public int ExecuteNonQuery(string query, Dictionary<string, object> parameters, SqlParameter outputParameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }
                if(outputParameter != null)
                { 
                     command.Parameters.Add(outputParameter);
                }
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
