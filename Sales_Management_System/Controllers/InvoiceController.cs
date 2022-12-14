using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Sales_Management_System.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceService _invoiceService;
        public InvoiceController(IUnitOfWork unitOfWork,IInvoiceService invoiceService)
        {
            _unitOfWork= unitOfWork;
            _invoiceService= invoiceService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

            return View();
        }

        public IActionResult Review()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(List<InvoiceItemDto> invoiceItems, CustomerDto customer)
        {
            Invoice result = await _invoiceService.GenerateInvoiceAsync(invoiceItems, customer);


            List<InvoiceItemDto> dtos = new List<InvoiceItemDto>();

            foreach (var item in result.InvoiceItems)
            {
                InvoiceItemDto inv = new InvoiceItemDto()
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                dtos.Add(inv);
            }

            CustomerDto customerDto = new CustomerDto()
            {
                Id= result.Customer.Id,
                Name= result.Customer.Name,
                Address= result.Customer.Address,
                Phone= result.Customer.Phone,
            };

            InvoiceDto invoiceDto = new InvoiceDto()
            {
                Id = result.Id,
                Amount = result.Amount,
                Customer = customerDto,               
                Date = result.Date,
                Status = result.Status,
                InvoiceItems = dtos
            };

            return Json(invoiceDto);
        }
    }
}
