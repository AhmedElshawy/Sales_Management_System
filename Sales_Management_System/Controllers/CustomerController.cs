using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Sales_Management_System.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var customers = await _unitOfWork.Customers.ListAllAsync();
            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            Customer customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer != null)
            {
                _unitOfWork.Customers.Delete(customer);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return Content("Falied to delete this item");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Customer customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Customers.Update(customer);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Failed to update this item");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public async Task<JsonResult> GetFilteredCustomers(string search)
        {
            List<Customer> customers;
            if(search != null)
            {
                customers = await _unitOfWork.Customers.ListTopRecordsAsync(3,m => m.Name.ToLower().Contains(search));
            }
            else
            {
                customers = await _unitOfWork.Customers.ListTopRecordsAsync(3);
            }

            return Json(customers);
        }
    }
}
