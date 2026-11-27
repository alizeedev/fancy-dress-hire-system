using System.Data;
using Microsoft.Data.SqlClient;
using FancyDressHireSystem.Core.Models;
using System.Security.Cryptography.X509Certificates;

namespace FancyDressHireSystem.Core.Data
{
    ///<summary>
    ///Reads and saves costumes in the Costumes table.
    ///CostumeService loads every costume from here in the hash table at startup.
    ///</summary>
    public class CostumeRepository
    {
        //sql server error numbers that we are turning into clear messages
        /// sql server error numbers (ref:Microsoft Learn, "Database engine events and errors")
        private const int DuplicateKeyError = 2627;
        private const int ForeignKeyError = 547;

        private readonly DatabaseConnection _database;

        ///<summary>
        ///Creates a costume repository that uses the shared database connection
        ///</summary>
        public CostumeRepository(DatabaseConnection database)
        {
            _database = database;
        }

        ///<summary>
        ///Gets every costume from the database
        ///</summary>
        ///<returns> An array of all costumes and stays empty if there are no costumes</returns>
        
        public async Task<Costume[]> GetAllAsync()
        {
            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                // Counts the rows to know how big the array must be
                using SqlCommand countCommand = new SqlCommand("SELECT COUNT(*) FROM Costumes;", connection);
                int count = (int)(await countCommand.ExecuteScalarAsync() ?? 0);
                Costume[] costumes = new Costume[count];

                //read each row into the array
                string sql = "SELECT CostumeId, CostumeName, Category, Era, Size," +
                    "DailyHirePrice, TotalQuantity,AvailableQuantity " + "FROM Costumes ORDER BY CostumeId;";
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();

                int index = 0;
                while (index <count && await reader.ReadAsync())
                {
                    costumes[index] = ReadCostume(reader);
                    index++;
                }

                return costumes;

            }
            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");
            }
        }

        ///<summary>
        ///Adds a new costume to the database</summary>
        ///<param name="costume">The costume to add</param>
        public async Task InsertAsync(Costume costume)
        {
            string sql= "INSERT INTO Costumes (CostumeId,CostumeName, Category, Era, Size," +
                "DailyHirePrice, TotalQuantity, AvailableQuantity)" + 
                "VALUES (@CostumeId, @CostumeName, @Category, @Era, @Size," +
                "@DailyHirePrice, @TotalQuantity, @AvailableQuantity);";

            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                AddCostumeParameters(command, costume);
                await command.ExecuteNonQueryAsync();
            }

            catch(SqlException ex) when (ex.Number == DuplicateKeyError)
            {
                throw new InvalidOperationException($"Costume with id {costume.CostumeId} already exists");
            }
            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");
            }
        }

        ///<summary>
        ///Saves changes to an existing costume
        ///</summary>
        ///<param name="costume">The costume with it's new details</param>
        ///<returns> True if the costume was updated and false if the ID was not found</returns>
        public async Task<bool> UpdateAsync(Costume costume)
        {
            string sql = "UPDATE Costumes SET CostumeName=@CostumeName, Category=@Category, Era=@Era," +
                "Size=@Size, DailyHirePrice=@DailyHirePrice, TotalQuantity=@TotalQuantity," +
                "AvailableQuantity=@AvailableQuantity WHERE CostumeId=@CostumeId;";
            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                AddCostumeParameters(command, costume);
                int rowsChanged = await command.ExecuteNonQueryAsync();
                return rowsChanged > 0;
            }
            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");
            }
            
        }

        ///<summary>
        ///Deletes a csotume. Cant delete if costume is being rented
        ///</summary>
        ///<param name="costumeId">The ID of the costume to delete</param>
        ///<return>True is deleted, false if Id wasnt found</return>
        public async Task<bool> DeleteAsync(string costumeId)
        {
            string sql = "DELETE FROM COSTUMES WHERE CostumeId=@CostumeId;";
            using SqlConnection connection = await _database.GetConnectionAsync();
            try
            {
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.Add("@CostumeId", SqlDbType.NVarChar, 10).Value = costumeId;
                int rowsChanged = await command.ExecuteNonQueryAsync();
                return rowsChanged > 0;
            }
            catch (SqlException ex) when (ex.Number == ForeignKeyError)
            {
                throw new InvalidOperationException($"Costume {costumeId} has rentals and cannot be deleted");
            }

            catch (SqlException)
            {
                throw new InvalidOperationException("Database is not available");
            }
        }

            ///<summary>
            ///Changes how many units of a costume are available</summary>
            ///<param name="costumeId">The ID of the costume</param>
            ///<param name="availableQuantity">The new number of available units</param>
            ///<returns>True if updated,false if the ID was not found</returns>
            public async Task<bool> SetAvailabilityAsync(string costumeId, int availableQuantity)
            {
                string sql = "UPDATE Costumes SET AvailableQuantity=@AvailableQuantity WHERE CostumeId=@CostumeId;";
                using SqlConnection connection = await _database.GetConnectionAsync();
                try
                {
                    using SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.Add("@CostumeId", SqlDbType.NVarChar, 10).Value = costumeId;
                    command.Parameters.Add("@AvailableQuantity", SqlDbType.Int).Value = availableQuantity;
                    int rowsChanged = await command.ExecuteNonQueryAsync();
                    return rowsChanged > 0;
                }
                catch (SqlException)
                {
                    throw new InvalidOperationException("Database is not available");
                }
            }

        ///<summary>
        ///Turns the current database row into a costume object</summary>
        private static Costume ReadCostume(SqlDataReader reader)
        {
            return new Costume(
                reader.GetString(reader.GetOrdinal("CostumeId")),
                reader.GetString(reader.GetOrdinal("CostumeName")),
                reader.GetString(reader.GetOrdinal("Category")),
                reader.GetString(reader.GetOrdinal("Era")),
                reader.GetString(reader.GetOrdinal("Size")),
                reader.GetDecimal(reader.GetOrdinal("DailyHirePrice")),
                reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                reader.GetInt32(reader.GetOrdinal("AvailableQuantity"))
            );

        }

        ///<summary>
        ///Adds every costume field to a command as a paarmeter
        ///</summary>
        private static void AddCostumeParameters(SqlCommand command, Costume costume)
        {
            command.Parameters.Add("@CostumeId", SqlDbType.NVarChar, 10).Value = costume.CostumeId;
            command.Parameters.Add("@CostumeName", SqlDbType.NVarChar, 120).Value = costume.CostumeName;
            command.Parameters.Add("@Category", SqlDbType.NVarChar, 30).Value = costume.Category;
            command.Parameters.Add("@Era", SqlDbType.NVarChar, 40).Value = costume.Era;
            command.Parameters.Add("@Size", SqlDbType.NVarChar, 10).Value = costume.Size;

            SqlParameter price = command.Parameters.Add("@DailyHirePrice", SqlDbType.Decimal);
            price.Precision = 8;
            price.Scale = 2;
            price.Value = costume.DailyHirePrice;
            command.Parameters.Add("@TotalQuantity", SqlDbType.Int).Value = costume.TotalQuantity;
            command.Parameters.Add("@AvailableQuantity", SqlDbType.Int).Value = costume.AvailableQuantity;
        }
    }
}





