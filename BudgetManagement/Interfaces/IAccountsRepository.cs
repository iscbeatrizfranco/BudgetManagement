
using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAccountsRepository
    {
        Task Create(Account account);
        Task Delete(int id);
        Task<Account> GetById(int Id, int UserId);
        Task<IEnumerable<Account>> Search(int UserId);
        Task Update(AccountCreateViewModel account);
    }
}
