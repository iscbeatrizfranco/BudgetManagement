using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace BudgetManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountsTypesRepository accountsTypesRepository;
        private readonly IUsersService usersService;
        private readonly IAccountsRepository accountsRepository;

        public AccountsController(IAccountsTypesRepository accountsTypesRepository, 
            IUsersService usersService, IAccountsRepository accountsRepository)
        {
            this.accountsTypesRepository = accountsTypesRepository;
            this.usersService = usersService;
            this.accountsRepository = accountsRepository;
        }

        public async Task<IActionResult> Index() 
        {
            var userId = usersService.GetUserID();
            var accountsWithAccountType =  await accountsRepository.Search(userId);

            var model = accountsWithAccountType.GroupBy(x => x.AccountType)
                                               .Select(group => new AccountIndexViewModel
                                               {
                                                   AccountType = group.Key,
                                                   Accounts = group.AsEnumerable()

                                               }).ToList();
            return View(model);

        }

        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            var userId = usersService.GetUserID();           
            var model = new AccountCreateViewModel();
            model.AccountsTypes = await GetAccountsTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateViewModel account) 
        {
            var userId = usersService.GetUserID();
            var existsAccountTypeId = await accountsTypesRepository.GetById(account.AccountTypeId, userId);
            if (existsAccountTypeId is null) 
            {
                return RedirectToAction("NoFound","Home");
            }
            if (!ModelState.IsValid)
            {
                account.AccountsTypes = await GetAccountsTypes(userId);
                return View(account);
            }
            await accountsRepository.Create(account);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = usersService.GetUserID();
            var account = await accountsRepository.GetById(id, userId);
            if (account is null) 
            {
                return RedirectToAction("NoFound", "Home");
            }

            var model = new AccountCreateViewModel() { 
                Id = account.Id,
                Name = account.Name,
                AccountTypeId = account.AccountTypeId,
                Balance = account.Balance,
                Description = account.Description
            };
            model.AccountsTypes = await GetAccountsTypes(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountCreateViewModel accountEdit) 
        {
            var userId = usersService.GetUserID();

            var account = accountsRepository.GetById(accountEdit.Id, userId);
            if (account is null) 
            {
                return RedirectToAction("NoFound", "Home");
            }
            
            var accountTypes = await accountsTypesRepository.GetById(accountEdit.AccountTypeId, userId);
            if (accountTypes is null) 
            {
                return RedirectToAction("NoFound","Home");
            }

            await accountsRepository.Update(accountEdit);
            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAccountsTypes(int userId) 
        {
            var accountsTypes = await accountsTypesRepository.GetAll(userId);
            return accountsTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
    }
}
