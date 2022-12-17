using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Sales_Management_System.Models;
using System.Diagnostics;

namespace Sales_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var totalSale =  _unitOfWork.Invoices.SumColumn(c=>c.Amount);
            var todaySale = _unitOfWork.Invoices.SumColumn(c=>c.Date == DateTime.Now.Date, c=>c.Amount);
            var monthlySale = _unitOfWork.Invoices.SumColumn(d=>d.Date.Month == DateTime.Now.Month, c=>c.Amount);
            var productsValue = _unitOfWork.Products.SumColumn(c=>c.UnitPrice * c.Quantity).ToString("00.00");
            var recentSale = await _unitOfWork.Invoices.ListTopRecordsAsync(5,o=>o.Customer);

            ViewBag.TotalSale = totalSale;
            ViewBag.TodaySale = todaySale;
            ViewBag.MonthlySale = monthlySale;
            ViewBag.ProductsValue = productsValue;
            ViewBag.RecentSale = recentSale;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}