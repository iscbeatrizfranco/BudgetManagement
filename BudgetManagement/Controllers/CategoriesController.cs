using BudgetManagement.Interfaces;
using BudgetManagement.Models;
using BudgetManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BudgetManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUsersService usersService;
        private readonly ICategoriesRepository categoriesRepository;

        public CategoriesController(IUsersService usersService, ICategoriesRepository categoriesRepository)
        {
            this.usersService = usersService;
            this.categoriesRepository = categoriesRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = usersService.GetUserID();
            var categories = await categoriesRepository.Get(userId);
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category) 
        {
            if (!ModelState.IsValid) 
            {
                return View(category);
            }
            var userId = usersService.GetUserID();
            category.UserId = userId;
            var existsCategory = await categoriesRepository.Exists(category.Name, category.UserId);
            if (existsCategory)
            {
                ModelState.AddModelError(nameof(category.Name), $"La categoría { category.Name} ya existe");
                return View(category);

            }
            await categoriesRepository.Create(category);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = usersService.GetUserID();
            var category = await categoriesRepository.GetById(id,userId);
            if (category is null) 
            {
                return RedirectToAction("NoFound","Home");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category categoryEdit)
        {
            if (!ModelState.IsValid) 
            {
                return View(categoryEdit); 
            }

            var userId = usersService.GetUserID();
            var category = await categoriesRepository.GetById(categoryEdit.Id, userId);
            if (category is null)
            {
                return RedirectToAction("NoFound", "Home");
            }
            categoryEdit.UserId = userId;
            await categoriesRepository.Update(categoryEdit);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) 
        {
            var userId = usersService.GetUserID();
            var category = await categoriesRepository.GetById(id, userId);
            if (category is null) 
            {
                return RedirectToAction("NoFound","Home");
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Category categoryDelete) 
        {
            var userId = usersService.GetUserID();
            var category = await categoriesRepository.GetById(categoryDelete.Id, userId);
            if (category is null) 
            {
                return RedirectToAction("NoFound", "Home");
            }
            await categoriesRepository.Delete(categoryDelete.Id);
            return RedirectToAction("Index");
        }
    }
}
