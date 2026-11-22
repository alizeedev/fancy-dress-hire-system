using Microsoft.Data.SqlClient;

namespace FancyDressHireSystem.Core.Data

{
    /// <summary>
    /// Opens connections to the sql server database
    /// </summary>
    public class DatabaseConnection
    {
        private readonly string _connectionString;
        /// <summary>
        /// stores the connection string from appsettings.json.
        /// </summary>
        /// <param name="connectionString">The connection string for the database.</param>
        public DatabaseConnection(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string is missing in appsettings.json");
            _connectionString = connectionString;
        }

        /// <summary>
        /// Opens a new connection to the database
        /// in order to always close the connection, it is used inside a block.
        /// </summary>
        /// <returns> An open SqlConnection</returns>
        public SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException)
            {
                throw new InvalidOperationException("Could not connect to the database. Please check that Sql Server is running");
            }
            return connection;
        }
    }
}

