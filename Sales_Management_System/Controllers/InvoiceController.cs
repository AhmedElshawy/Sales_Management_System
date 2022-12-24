﻿using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Sales_Management_System.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInvoiceService _invoiceService;
        private readonly IConverter _converter;
        public InvoiceController(IUnitOfWork unitOfWork,IInvoiceService invoiceService,IConverter converter)
        {
            _unitOfWork= unitOfWork;
            _invoiceService= invoiceService;
            _converter= converter;
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
        
        //public async Task<IActionResult> DownloadPDF()
        //{
        //    InvoiceItem invItem = new InvoiceItem() { Id=1,Name="test",Quantity=5,UnitPrice = 100};
        //    List<InvoiceItem> invoiceItems= new List<InvoiceItem>() { invItem };

        //    Invoice inv = new Invoice() { Amount = 500 , Date = DateTime.Now , Id = 1000, InvoiceItems = invoiceItems};


        //    var file = _invoiceService.GeneratePDF(inv);

        //    return File(file,"application/pdf");
        //}
    }
}
