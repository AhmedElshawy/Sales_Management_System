﻿using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Sales_Management_System.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvoiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

            return View();
        }
    }
}