using System.Data;
using Microsoft.Data.SqlClient;
using FancyDressHireSystem.Core.Models;

namespace FancyDressHireSystem.Core.Data
{
    ///<summary>
    ///Reads users from the Users table for login.
    ///</summary>
    public class UserRepository
    {
        private readonly DatabaseConnection _database;

        ///<summary>
        ///Creates a user repository that users the shared database connection.
        ///</summary>

        public UserRepository(DatabaseConnection database)
        {
            _database = database;
        }

        ///<summary>
        ///find a user by login name
        ///</summary>
        ///<returns> The matching user or null if user not found.</returns>
        public async Task<User?> GetByUsernameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return null;

            string sql = "SELECT UserId, UserName, PasswordHash,PasswordSalt,UserRole" +
                                    " FROM Users WHERE UserName = @UserName";
            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@UserName", SqlDbType.NVarChar, 20).Value = userName.Trim();

                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                    return null;

                return new User(
                    reader.GetString(reader.GetOrdinal("UserId")),
                    reader.GetString(reader.GetOrdinal("UserName")),
                    reader.GetString(reader.GetOrdinal("PasswordHash")),
                    reader.GetString(reader.GetOrdinal("PasswordSalt")),
                    reader.GetString(reader.GetOrdinal("UserRole")));
            }
            
           catch (SqlException)
            {
                throw new InvalidOperationException("Database not available");
            }
        }
    }
}