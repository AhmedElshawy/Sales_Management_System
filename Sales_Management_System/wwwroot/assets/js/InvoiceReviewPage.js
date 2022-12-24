let invoiceReviewPage = {
    invoiceData: JSON.parse(localStorage.getItem("invoiceData")),

    fillCustomerData: function () {
        customerNameField.innerHTML = this.invoiceData.customer.name;
        customerAddressField.innerHTML = this.invoiceData.customer.address;
        customerPhoneField.innerHTML = this.invoiceData.customer.phone;
    },

    drawItemsTable: function (arr) {
        itemsTableBody.innerHTML = "";

        let result = arr.map((element) => {
            return `<tr class="text-center" id = ${element.id}>                       
                        <td class="text-start">${element.name}</td>
                        <td>${element.quantity}</td>
                        <td>${element.unitPrice} EGP</td>                      
                        <td>${(element.unitPrice * element.quantity).toFixed(2)} EGP</td>
                    </tr>`;
        });

        for (var i = 0; i < result.length; i++) {
            itemsTableBody.innerHTML += result[i];
        }
    },

    fillInvoiceGeneralData: function () {
        numberOfItemsFiled.innerHTML = this.invoiceData.invoiceItems.length;
        totalField.innerHTML = this.calculateInvoiceTotal(this.invoiceData.invoiceItems) + " EGP";
    },

    calculateInvoiceTotal: function (invoiceItems) {
        let total = 0;
        for (var i = 0; i < invoiceItems.length; i++) {
            total += invoiceItems[i].unitPrice * invoiceItems[i].quantity;
        }
        return total.toFixed(2);
    },

    submitInvoice: function () {
        $.ajax({
            type: "POST",
            data: {
                invoiceItems: this.invoiceData.invoiceItems,
                customer: this.invoiceData.customer
            },
            url: "/Invoice/Create",
            dataType: "json",
            success: function (response) {
                invoiceReviewPage.saveCreatedInvoiceDataAtLocalStorage(response);
                printBtn.removeAttribute("disabled");
                if (typeof response === "object") {
                    window.alert("Invoice added successfuly");
                    confirmBtn.setAttribute("disabled", "");
                }
                else {
                    window.alert(response);
                }            
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    },

    saveCreatedInvoiceDataAtLocalStorage: function (data) {
        let invoiceData = data;
        localStorage.setItem("createdInvoiceData", JSON.stringify(invoiceData));
    }
}


window.addEventListener("load", () => {
    invoiceReviewPage.fillCustomerData();
    invoiceReviewPage.drawItemsTable(invoiceReviewPage.invoiceData.invoiceItems);
    invoiceReviewPage.fillInvoiceGeneralData();
});

// getting customer data fields
let customerNameField = document.getElementById("customer-name");
let customerAddressField = document.getElementById("customer-address");
let customerPhoneField = document.getElementById("customer-phone");


// getting items data fields
let itemsTableBody = document.getElementById("items-table-body");


// getting invoice general data fields
let numberOfItemsFiled = document.getElementById("number-of-items");
let totalField = document.getElementById("total");

//getting confirm button
let confirmBtn = document.getElementById("btn-confirm");

//getting print button
let printBtn = document.getElementById("print-btn");



confirmBtn.addEventListener("click", () => {
    invoiceReviewPage.submitInvoice();
});

printBtn.addEventListener("click", () => {
    window.open("/Invoice/Display", '_blank').focus();
});
