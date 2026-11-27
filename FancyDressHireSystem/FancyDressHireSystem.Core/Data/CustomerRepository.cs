using System.Data;
using Microsoft.Data.SqlClient;
using FancyDressHireSystem.Core.Models;

namespace FancyDressHireSystem.Core.Data
{
    ///<summary>
    ///Reads and saves customers in the Customers table
    ///</summary>
    public class CustomerRepository
    {
        //SQL Server error number for a duplicate primary key
        //(ref:Microsoft Learn,"Database engine events and errors")
        private const int DuplicateKeyError = 2627;
        private readonly DatabaseConnection _database;
        ///<summary>
        ///Create a CustomerRepository that uses the shared database connection</summary>
        public CustomerRepository(DatabaseConnection database)
        {
            _database = database;
        }

        ///<summary>
        ///Gets every customer from the database
        ///</summary>
        ///<returns>An array of all customers, stays empty if there are no customers</returns>
        public async Task<Customer[]> GetAllAsync()
        {
            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                //need to count the number of rows to know how big the array must be
                using SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Customers;", connection);
                int count = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
                Customer[] customers = new Customer[count];

                //read each row into the array
                string sql = "SELECT CustomerId, FullName, Email, Phone" + "FROM Customers ORDER BY CustomerId;";
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                int index = 0;
                while(index < count && await reader.ReadAsync())
                {
                    customers[index] = ReadCustomer(reader);
                    index++;

                }
                return customers;

            }

            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");
            }

        }

        ///<summary>
        ///Finds one customer by their ID
        ///</summary>
        ///<param name="customerId">The customer ID</param>
        ///<returns> The matching customer or null is id not found</returns>
        public async Task<Customer?> GetByIdAsync(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
                return null;

            string sql = "SELECT CustomerId, FullName, Email, Phone" + "FROM Customers WHERE CustomerId = @CustomerId;";
            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@CustomerId", SqlDbType.NVarChar, 10).Value = customerId.Trim();

                using SqlDataReader reader = await command.ExecuteReaderAsync();
                if (!await reader.ReadAsync())
                    return null;
                return ReadCustomer(reader);

            }

            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");

            }

        }

        ///<summary>
        ///Adds a new customer to the database
        ///</summary>
        ///<param name="customer">The customer to add</param>
        public async Task InsertAsync(Customer customer)
        {
            string sql = "INSERT INTO Customers (CustomerId, FullName,Email,Phone)" +
                "VALUES(@CustomerId,@FullName,@Email,@Phone);";

            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@CustomerId", SqlDbType.NVarChar, 10).Value = customer.CustomerId;
                command.Parameters.Add("@FullName", SqlDbType.NVarChar, 100).Value = customer.FullName;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 15).Value = customer.Phone;
                await command.ExecuteNonQueryAsync();

            }

            catch (SqlException ex) when (ex.Number == DuplicateKeyError)
            {
                throw new ArgumentException($"A customer with ID {customer.CustomerId} already exists.");

            }

            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");

            }

        }

        ///<summary>
        ///Turns the current database into a Customer object.
        ///</summary>
        private static Customer ReadCustomer(SqlDataReader reader)
        {
            return new Customer(
                reader.GetString(reader.GetOrdinal("CustomerId")),
                reader.GetString(reader.GetOrdinal("FullName")),
                reader.GetString(reader.GetOrdinal("Email")),
                reader.GetString(reader.GetOrdinal("Phone")));
                
        }
    }
}

