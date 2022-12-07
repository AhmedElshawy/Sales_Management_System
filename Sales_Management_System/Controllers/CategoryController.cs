using Microsoft.AspNetCore.Mvc;

namespace Sales_Management_System.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
