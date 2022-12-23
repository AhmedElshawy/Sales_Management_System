using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Core.Constants;

namespace Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvoiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Invoice> GenerateInvoiceAsync(List<InvoiceItemDto> invoiceItemsDto, CustomerDto customerDto)
        {
            Customer customer;

            if (customerDto.Id == 0)
            {
                customer = new Customer()
                {
                    Name = customerDto.Name,
                    Address = customerDto.Address,
                    Phone = customerDto.Phone
                };
            }
            else
            {
                customer = await _unitOfWork.Customers.GetByIdAsync(customerDto.Id);
            }

            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
            decimal invoiecAmount = 0;

            foreach (var item in invoiceItemsDto)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);               

                product.Quantity -= item.Quantity;
                product.Sold += item.Quantity;
                invoiecAmount += product.UnitPrice * item.Quantity;

                InvoiceItem invoiceItem = new InvoiceItem()
                {
                    Name = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                };
                invoiceItems.Add(invoiceItem);
            }

            Invoice invoice = new Invoice()
            {
                Amount = invoiecAmount,
                Customer = customer,
                InvoiceItems = invoiceItems,
                Status = InvoiceStatus.Confirmed
            };

            await _unitOfWork.Invoices.AddAsync(invoice);
            await _unitOfWork.CompleteAsync();

            return invoice;
        }

        public string ValidateInputs(List<InvoiceItemDto> invoiceItemsDto, CustomerDto customerDto)
        {
            string message = "";
            if (customerDto.Name == null || invoiceItemsDto.Count == 0)
            {
                message = "Missing Data";
                return message;
            }
            return message;
        }

        public async Task<string> ValidateItemsQuantityAsync(List<InvoiceItemDto> invoiceItemsDto)
        {
            string message = "";
            foreach (var item in invoiceItemsDto)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(item.ProductId);

                if (product.Quantity < item.Quantity) 
                {
                    message = "Item quantity excceds product quantity in stock";
                    return message;
                }
            }

            return message;
        }
    }
}
