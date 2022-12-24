using Core.Interfaces;
using Core.Models;
using Core.ViewModels;
using Core.Constants;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Text;

namespace Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConverter _converter;
        public InvoiceService(IUnitOfWork unitOfWork, IConverter converter)
        {
            _unitOfWork = unitOfWork;
            _converter = converter;
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

        public byte[] GeneratePDF(Invoice invoice)
        {         

            var globalSettings = new GlobalSettings()
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10 },
                DocumentTitle = "invoice Pdf",
            };

            var objectSettings = new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator(invoice),
                WebSettings = { DefaultEncoding = "utf-8" , UserStyleSheet= Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "lib","bootstrap","dist","css","bootstrap.min.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 18 },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };         

            return _converter.Convert(pdf);
        }

        private string TemplateGenerator(Invoice invoice)
        {
            var template = new StringBuilder();
            template.Append(@"
    <div class='container-fluid text-dark'>
        <div class='row'>
            <div class='col-12 d-flex justify-content-between'>
                <h6 class='text-dark'>Invoice To :</h6>               
            </div>
        </div>
        <div class=""row"">
            <div class=""col-12 d-flex justify-content-between"">
                <p>
                    <span id=""customer-name"">ahmed</span>
                    <br />
                    <span id=""customer-address"">ahmed maher</span>
                    <br />
                    <span id=""customer-phone"">01099816700</span>
                </p>
                <h6 class=""text-dark"">Invoice Date : <span>18/10/2022</span></h6>
                <h6 class=""text-dark"">INV-<span>111</span></h6>
            </div>
        </div>
        <div class=""row mt-3"">
            <div class=""table-responsive w-100"">
                <table class=""table table-bordered"">
                    <thead class=""text-dark"">
                        <tr>
                            <th>Description</th>
                            <th class=""text-center"">Quantity</th>
                            <th class=""text-center"">Unit cost</th>
                            <th class=""text-center"">Total</th>
                        </tr>
                    </thead>
                    <tbody>
                                    
                ");

            foreach (var item in invoice.InvoiceItems)
            {
                template.AppendFormat(@"
                        <tr>
                            <td>{0}</td>                       
                            <td>{1}</td>
                            <td>{2}</td>
                            <td>{3}</td>
                        </tr>
                ",item.Name,item.Quantity,item.UnitPrice,item.UnitPrice * item.Quantity);
            }

            template.Append(@"
                            </tbody>
                        </table>
                    </div>
                </div>
            ");

            template.AppendFormat(@"
    <div class=""row mt-2"">
            <div class=""col-md-6 ms-auto"">
                <div class=""table-responsive"">
                    <table class=""table text-dark"">
                        <tbody>
                            <tr>
                                <td class=""text-bold-800"">Total</td>
                                <td id=""total"" class=""text-bold-800 text-end"">{0}</td>
                            </tr>
                            <tr>
                                <td>number of items</td>
                                <td id=""number-of-items"" class=""text-danger text-end"">{1}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
            ",invoice.Amount,invoice.InvoiceItems.Count);
            
            return template.ToString();
        }
    }
}



 
