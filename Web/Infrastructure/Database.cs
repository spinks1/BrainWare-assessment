
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Web.Infrastructure
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;


    public interface IDatabase
    {
        IDbConnection GetConnection();
        IDbConnection GetConnection(string config);
        IDataReader ExecuteReader(string myQuery);
    }

    public class Database : IDatabase
    {
        private readonly SqlConnection _connection;

        public Database()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BrainWAre"].ConnectionString);

            _connection.Open();

        }

        /// <summary>
        /// This will get the default value from the web.config file.
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnection()
        {
            return GetConnection(ConfigurationManager.ConnectionStrings["BrainWAre"].ConnectionString);
        }

        /// <summary>
        /// This will allow you to override the connection string. 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public IDataReader ExecuteReader(string query)
        {
            var sqlQuery = new SqlCommand(query, _connection);

            return sqlQuery.ExecuteReader();

        }

    }
}

