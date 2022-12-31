using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
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
        public async Task<IActionResult> Index(ProductParams productParams)
        {        
            var products = await _unitOfWork.ProductRepository.ApplayFillter(productParams,null);

            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

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

        public IActionResult IncreaseQuantity()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int productId , int quantity)
        {
            
            var product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product != null)
            {
                product.Quantity += quantity;
               
                AdditionOrder additionOrder = new AdditionOrder()
                {
                    AddedQunatity= quantity,
                    ProdutcId=productId
                };

                await _unitOfWork.AdditionOrders.AddAsync(additionOrder); 
                //save to DB
                await _unitOfWork.CompleteAsync();
                return Json($"successly increased product ({product.Name}) with {quantity} items");
            }

            return Json("Failed to increase product quntity");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;
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

            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

            return View(product);
        }

        [HttpPost]
        public async Task<JsonResult> GetAllProducts(ProductParams productParams)
        {           
            var products = await _unitOfWork.ProductRepository.ApplayFillter(productParams,3);
            return Json(products);
        }

        [HttpPost]
        public async Task<JsonResult> GetNearlyOutOfStockProducts()
        {
            var products = await _unitOfWork.Products.ListAllAsync(3,m=>m.Quantity < 3);
            return Json(products);
        }

        [HttpPost]
        public async Task<JsonResult> GetNumberOfNotifications()
        {
            int numberOfNotifications = await _unitOfWork.Products.CountEntityAsync(m => m.Quantity < 3);
            return Json(numberOfNotifications);
        }
        
        public async Task<IActionResult> NotificationCenter()
        {
            var products = await _unitOfWork.Products.ListAllAsync(m => m.Quantity < 3);
            return View(products);
        }
        
    }
}
