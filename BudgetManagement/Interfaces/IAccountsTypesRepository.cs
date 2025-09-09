using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAccountsTypesRepository
    {
        Task Create(AccountType accountType);
        Task Delete(AccountType accountType);
        Task<bool> Exists(string nombre, int userID);
        Task<IEnumerable<AccountType>> GetAll(int userId);
        Task<AccountType> GetById(int id, int userId);
        Task Sortable(IEnumerable<AccountType> accountsTypesSortable);
        Task Update(AccountType accountType);
    }
}
