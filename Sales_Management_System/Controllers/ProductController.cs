﻿using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sales_Management_System.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _unitOfWork.Products.ListAllAsync();          
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product != null)
            {
                _unitOfWork.Products.Delete(product);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return Content("Falied to delete this item");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Products.Update(product);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Failed to update this item");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
    }
}