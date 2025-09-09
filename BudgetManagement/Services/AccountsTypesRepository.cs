using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class AccountsTypesRepository:IAccountsTypesRepository
    {
        private readonly string connectionString;

        public AccountsTypesRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(AccountType accountType)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("AccountsTypes_Insert",
                                                            new
                                                            {
                                                                Name = accountType.Name,
                                                                UserId = accountType.UserId
                                                            },
                                                            commandType:System.Data.CommandType.StoredProcedure
                                                            );
            accountType.Id = id;
        }

        public async Task<bool> Exists(string nombre, int userID) 
        {
            using var connection = new SqlConnection(connectionString);
            var exists = await connection.QueryFirstOrDefaultAsync<int>(
                                                                    "SELECT 1 " +
                                                                    "FROM AccountsTypes " +
                                                                    "WHERE Name = @Name " +
                                                                    "AND UserId = @UserId;",
                                                                    new { Name = nombre, UserId = userID });

            return exists == 1;
        }

        public async Task<IEnumerable<AccountType>> GetAll(int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return  await connection.QueryAsync<AccountType>(
                                                            "SELECT Id, Name, DisplayOrder " +
                                                            "FROM AccountsTypes " +
                                                            "WHERE UserId = @UserId " +
                                                            "ORDER BY DisplayOrder;",
                                                            new { userId });
        }

        public async Task Update(AccountType accountType) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE AccountsTypes SET Name = @Name WHERE Id = @Id",accountType);
        }

        public async Task Delete(AccountType accountType) 
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE FROM AccountsTypes WHERE Id = @Id", accountType);
        }

        public async Task<AccountType> GetById(int id, int userId) 
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<AccountType>(
                                                             "SELECT Id, Name, DisplayOrder " +
                                                             "FROM AccountsTypes " +
                                                             "WHERE Id = @Id AND UserId = @UserId", 
                                                             new { id, userId});

        }

        public async Task Sortable(IEnumerable<AccountType> accountsTypesSortable) 
        {
            var query = "UPDATE AccountsTypes SET DisplayOrder = @DisplayOrder WHERE Id = @Id;";
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(query, accountsTypesSortable);
        }
    }
}
