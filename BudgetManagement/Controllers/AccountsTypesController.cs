using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Controllers
{
    public class AccountsTypesController : Controller
    {
        private readonly IAccountsTypesRepository accountsTypesRepository;
        private readonly IUsersService usersService;

        public AccountsTypesController(IAccountsTypesRepository accountsTypesRepository,
            IUsersService usersService)
        {
            this.accountsTypesRepository = accountsTypesRepository;
            this.usersService = usersService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = usersService.GetUserID();
            var accountsTypes = await accountsTypesRepository.GetAll(userId);
            return View(accountsTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            if(!ModelState.IsValid)
            {
                return View(accountType);
            }
            accountType.UserId = usersService.GetUserID();

            var existsAccountType = await accountsTypesRepository.Exists(accountType.Name, accountType.UserId);
            if (existsAccountType)
            {
                ModelState.AddModelError(nameof(accountType.Name), $"El nombre {accountType.Name} ya existe.");
                return View(accountType);
            }

            await accountsTypesRepository.Create(accountType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id) 
        {
            var userId = usersService.GetUserID();
            var accountType = await accountsTypesRepository.GetById(id, userId);

            if (accountType is null) 
            {
               return RedirectToAction("No encontrado", "Home");
            }

            return View(accountType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountType accountType) 
        {
            var userId = usersService.GetUserID();
            var accountTypeExists = await accountsTypesRepository.GetById(accountType.Id, userId);

            if (accountTypeExists is null) 
            {
                RedirectToAction("NoFound","Home");
            }

            await accountsTypesRepository.Update(accountType);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id) 
        {
            var userId = usersService.GetUserID();
            var accountType = await accountsTypesRepository.GetById(id, userId);

            if (accountType is null) 
            {
                return RedirectToAction("NoFound","Home");
            }

            return View(accountType);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(AccountType accountType) 
        {
            var userId = usersService.GetUserID();
            var accountTypeExists = await accountsTypesRepository.GetById(accountType.Id, userId);

            if (accountTypeExists is null)
            {
                RedirectToAction("NoFound", "Home");
            }

            await accountsTypesRepository.Delete(accountType);
            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> VerifyExistsAccountType(string name)
        {
            var userId = usersService.GetUserID();
            var existsAccountType = await accountsTypesRepository.Exists(name, userId);
            if (existsAccountType)
            {
                return Json($"El nombre {name} ya existe.");
            }
            return Json(true);
        }

        [HttpPost]
        public async Task<IActionResult> Sortable([FromBody] int[] ids) 
        {
            var userId = usersService.GetUserID();
            var accountsTypes = await accountsTypesRepository.GetAll(userId);
            var idsAccountsTypes = accountsTypes.Select(x => x.Id);
            var accountsTypesIdsNotAssignedToUser = ids.Except(idsAccountsTypes);

            if (accountsTypesIdsNotAssignedToUser.Count() > 0)
                return Forbid();

            var accountsTypesSortable = ids.Select((value,index) => 
                                        new AccountType() { Id = value,  DisplayOrder = index + 1} ).AsEnumerable();

            await accountsTypesRepository.Sortable(accountsTypesSortable);
            return Ok();
        }
    }
}
