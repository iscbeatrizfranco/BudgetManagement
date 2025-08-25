using BudgetManagement.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BudgetManagement.Controllers
{
    public class AccountsTypesController : Controller
    {
        public AccountsTypesController()
        {
           
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AccountType accountType)
        {
            if(!ModelState.IsValid)
            {
                return View(accountType);
            }
            return View();
        }
    }
}
