using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface ICategoriesRepository
    {
        Task Create(Category category);
        Task<bool> Exists(string nombre, int userID);
        Task<IEnumerable<Category>> Get(int userId);
    }
}
