
using BudgetManagement.Models;

namespace BudgetManagement.Interfaces
{
    public interface IAccountsRepository
    {
        Task Create(Account account);
        Task<IEnumerable<Account>> Search(int UserId);
    }
}
