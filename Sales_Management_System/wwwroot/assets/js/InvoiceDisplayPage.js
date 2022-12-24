let invoiceDisplayPage = {
    createdInvoiceData: JSON.parse(localStorage.getItem("createdInvoiceData")),

    addScript: function (url) {
            var script = document.createElement('script');
            script.type = 'application/javascript';
            script.src = url;
            document.head.appendChild(script);              
    },

    // getting customer data fields
    customerNameField : document.getElementById("customer-name"),
    customerAddressField : document.getElementById("customer-address"),
    customerPhoneField : document.getElementById("customer-phone"),

    // getting items data fields
    itemsTableBody : document.getElementById("items-table-body"),

    // getting invoice general data fields
    numberOfItemsFiled : document.getElementById("number-of-items"),
    totalField: document.getElementById("total"),
    invoiceId: document.getElementById("invoice-id"),
    invoiceDate: document.getElementById("invoice-date"),

    printBtn: document.getElementById("btn-print"),

    fillCustomerData: function () {
        this.customerNameField.innerHTML = this.createdInvoiceData.customer.name;
        this.customerAddressField.innerHTML = this.createdInvoiceData.customer.address;
        this.customerPhoneField.innerHTML = this.createdInvoiceData.customer.phone;
        this.invoiceId.innerHTML = this.createdInvoiceData.id;
        this.invoiceDate.innerHTML = this.formatDate(this.createdInvoiceData.date);
    },

    formatDate: function (stringDate) {
        let date = new Date(stringDate);

        let day = date.getDate();
        let month = date.getMonth() + 1;
        let year = date.getFullYear();

        return `${day}/${month}/${year}`;
    },

    drawItemsTable: function (arr) {
        this.itemsTableBody.innerHTML = "";

        let result = arr.map((element) => {
            return `<tr class="text-center" id = ${element.id}>                       
                        <td class="text-start">${element.name}</td>
                        <td>${element.quantity}</td>
                        <td>${element.unitPrice} EGP</td>                      
                        <td>${(element.unitPrice * element.quantity).toFixed(2)} EGP</td>
                    </tr>`;
        });

        for (var i = 0; i < result.length; i++) {
            this.itemsTableBody.innerHTML += result[i];
        }
    },

    fillInvoiceGeneralData: function () {
        this.numberOfItemsFiled.innerHTML = this.createdInvoiceData.invoiceItems.length;
        this.totalField.innerHTML = this.calculateInvoiceTotal(this.createdInvoiceData.invoiceItems) + " EGP";
    },

    calculateInvoiceTotal: function (invoiceItems) {
        let total = 0;
        for (var i = 0; i < invoiceItems.length; i++) {
            total += invoiceItems[i].unitPrice * invoiceItems[i].quantity;
        }
        return total.toFixed(2);
    },

    downloadInvoice: function (invoiceId) {
        var element = document.getElementById('element-to-print');
        var opt = {
            margin: .3,
            filename: `Invoice-${invoiceId}.pdf`,
            image: { type: 'jpeg', quality: 0.98 },
            html2canvas: { scale: 2 },
            jsPDF: { unit: 'mm', format: 'a4', orientation: 'portrait' }
        };

        html2pdf(element, opt);
    }
}


window.addEventListener("load", () => {
    invoiceDisplayPage.fillCustomerData();
    invoiceDisplayPage.drawItemsTable(invoiceDisplayPage.createdInvoiceData.invoiceItems);
    invoiceDisplayPage.fillInvoiceGeneralData();
    invoiceDisplayPage.downloadInvoice(invoiceDisplayPage.createdInvoiceData.id);
});