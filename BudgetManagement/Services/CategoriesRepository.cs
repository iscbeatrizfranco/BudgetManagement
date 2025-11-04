using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Services
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly string connectionString;
        private readonly IUsersService usersService;

        public CategoriesRepository(IConfiguration configuration, IUsersService usersService)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.usersService = usersService;
        }

        public async Task Create(Category category)
        {
            var userId = usersService.GetUserID();
            var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("INSERT INTO Categories " +
                                                            "(Name, TransactionTypeId, UserId) " +
                                                            "VALUES (@Name,@TransactionTypeId, @UserId); " +
                                                            "SELECT SCOPE_IDENTITY(); ", category);
            category.Id = id;

        }

        public async Task<bool> Exists(string name, int userId)
        {
            var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>("SELECT 1 " +
                                                                    "FROM Categories " +
                                                                    "WHERE Name = @name " +
                                                                    "AND UserID = @userId ",
                                                                    new { name, userId });
            return exist == 1;
        }

        public async Task<IEnumerable<Category>> Get(int userId)
        {
            var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<Category>("SELECT Name, TransactionTypeId " +
                                                                    "FROM Categories " +
                                                                    "WHERE UserId = @userId",
                                                                    new { userId });
            return categories;

        }

        public async Task<Category> GetById(int id, int userId) 
        {
            var connection = new SqlConnection(connectionString);
            var category = await connection.QueryFirstOrDefaultAsync<Category>(
                "SELECT * FROM Categories WHERE Id = @id AND UserId=@userId",
                new { id, userId} );
            return category;
        }

        public async Task Update(Category category) 
        {
            var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UPDATE Categories SET Name = @Name, TransactionTypeId = @TransactionTypeId" +
                                            "WHERE Id=1", category);
        }
    }

}
