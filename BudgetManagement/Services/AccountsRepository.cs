using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class AccountsRepository:IAccountsRepository
    {
        private readonly string connectionString;
        private readonly IUsersService usersService;

        public AccountsRepository(IConfiguration configuration,IUsersService usersService)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.usersService = usersService;
        }

        public async Task Create(Account account) 
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("INSERT INTO Accounts " +
                                                    "(Name, AccountTypeId, Balance, Description) " +
                                                    "VALUES (@Name, @AccountTypeId, @Balance, @Description) " +
                                                    "SELECT SCOPE_IDENTITY()",
                                                    account);
            account.Id = id;
        }

        //public async Task<bool> ExistsAccount(string Name, int UserId)
        //{
        //    var connection = new SqlConnection(connectionString);
        //    await connection.QueryFirstOrDefaultAsync<int>("SELECT 1 " +
        //        "FROM Accounts " +
        //        "WHERE Name = @Name " +
        //        "AND ");
        //    return 
        //}
    }
}
