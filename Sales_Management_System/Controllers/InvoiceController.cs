using cloudscribe.Pagination.Models;
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
        private readonly IInvoiceRepository _invoiceRepository;
        public InvoiceController(IUnitOfWork unitOfWork,IInvoiceService invoiceService,IInvoiceRepository invoiceRepository)
        {
            _unitOfWork = unitOfWork;
            _invoiceService = invoiceService;
            _invoiceRepository = invoiceRepository;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.Categories.ListAllAsync();
            var brands = await _unitOfWork.Brands.ListAllAsync();

            ViewBag.categories = categories;
            ViewBag.brands = brands;

            return View();
        }

        public async Task<IActionResult> All(DateTime? from , DateTime? to , int pageSize = 10, int pageNumber = 1)
        {
            //applying paging
            var result = await _invoiceRepository.ApplayFilterWithPagination(from, to, pageNumber, pageSize);

            if (from != null && to != null)
            {
                ViewBag.from = from.Value.Date.ToString("yyyy-MM-dd");
                ViewBag.to = to.Value.Date.ToString("yyyy-MM-dd");
            }

            return View(result);
        }

        public async Task<IActionResult> Details(int id)
        {
            var invoice = await _invoiceRepository.GetInvoiceWihtDeatils(id);

            return View(invoice);
        }

        public IActionResult Review()
        {
            return View();
        }

        public IActionResult Display()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Create(List<InvoiceItemDto> invoiceItems, CustomerDto customer)
        {
            string inputsValidationMessage = _invoiceService.ValidateInputs(invoiceItems, customer);
            if(!string.IsNullOrEmpty(inputsValidationMessage))
            {
                return Json(inputsValidationMessage);
            }

            string itemsQuantityValidationMessage = await _invoiceService.ValidateItemsQuantityAsync(invoiceItems);
            if(!string.IsNullOrEmpty(itemsQuantityValidationMessage))
            {
                return Json(itemsQuantityValidationMessage);
            }

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
