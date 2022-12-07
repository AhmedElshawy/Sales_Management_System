using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sales_Management_System.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.ListAllAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(ModelState.IsValid)
            {
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));   
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _unitOfWork.Categories.GetByIdAsync(id);
            if(category != null)
            {
                _unitOfWork.Categories.Delete(category);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return Content("Falied to delete this item");
        }

        public async Task<IActionResult> Edit(int id)
        {          
            Category category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id ,Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Categories.Update(category);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Failed to update this item");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
    }
}
