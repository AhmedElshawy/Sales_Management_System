let invoiceIndexPage = {
    getAllProducts: function () {
        $.ajax({
            type: "POST",
            data: {
                search: searchBox.value,
                BrandId: brandSelectList.value,
                CategoryId: categorySelectList.value
            },
            url: "/Product/GetAllProducts",
            dataType: "json",
            success: function (response) {
                invoiceIndexPage.drawTable(response);
                invoiceIndexPage.fillCart(document.querySelectorAll(".product-check-box"), response);
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    },

    getAllCustomers: function () {
        $.ajax({
            type: "POST",
            data: {
                search: customersTableSearchBox.value 
            },
            url: "/Customer/GetFilteredCustomers",
            dataType: "json",
            success: function (response) {
                invoiceIndexPage.drawCustomersTable(response);
                invoiceIndexPage.choseFormExistingCustomers(document.querySelectorAll(".customer-check-box"), response);
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    },

    customer: {} 
    ,

    choseFormExistingCustomers: function (arr, response) {

        for (var i = 0; i < arr.length; i++) {
            arr[i].addEventListener("change", (ev) => {
                if (ev.target.checked) {
                    let id = ev.target.parentElement.parentElement.id;
                    let customer = response.find((ele) => {
                        return ele.id == id;
                    });
                    this.customer = customer;
                }
                else {
                    this.customer = {};
                }
            });
        }
    },

    addNewCustomer: function () {
        let name = document.getElementById("name-input").value;
        let address = document.getElementById("address-input").value;
        let phone = document.getElementById("phone-input").value;

        if (this.validateNewCustomerInputs(name, address, phone)) {
            let customer = {
                name: name,
                address: address,
                phone: phone
            }
        this.customer = customer;       
        }
        else
        {
            window.alert("Please enter customer name at least");
        }       
    },

    validateNewCustomerInputs: function (name, address, phone) {
        if (name == "") {
            return false;
        }
        return true;
    },

    isExistingCustomerSectionExpanded: function () {
        let ariaExpanded = existingCustomersSection.getAttribute("aria-expanded");
        if (ariaExpanded == "false") return false;
        else return true;
    },

    drawTable: function (arr) {
        tableBody.innerHTML = "";

        let result = arr.map((element, index) => {
            return `<tr id = ${element.id}>
                        <td><input class="form-check-input product-check-box" ${this.isOutOfStock(element.quantity)} ${this.isInCart(element.id)} type="checkbox"></td>
                        <td>${element.name}</td>
                        <td>${element.unitPrice}</td>
                        <td style="width:15%;"><input onchange="invoiceIndexPage.validateQuantity(this)" class="form-control" value="1" type="number" /></td>
                        <td>${element.quantity}</td>
                        <td>${this.getProductStatus(element.quantity)}</td>
                    </tr>`;
        });

        for (var i = 0; i < result.length; i++) {
            tableBody.innerHTML += result[i];
        }
    },

    isInCart: function (id) {
        let result = this.cart.find((ele) => {
            return ele.productId == id;
        });

        if (result == undefined) {
            return "unchecked";
        } else {
            return "checked = true";
        }
    },

    isOutOfStock: function (quantity) {
        if (quantity == 0) {
            return "disabled";
        }
        return "";
    },

    drawCustomersTable: function (arr) {
        customersTableBody.innerHTML = "";

        let result = arr.map((element, index) => {
            return `<tr id = ${element.id}>
                        <td><input class="form-check-input customer-check-box" type="checkbox"></td>
                        <td>${element.name}</td>
                        <td>${element.address}</td>
                        <td>${element.phone}</td>
                    </tr>`;
        });

        for (var i = 0; i < result.length; i++) {
            customersTableBody.innerHTML += result[i];
        }
    },
    getProductStatus: function (availableQuantity) {
        if (availableQuantity > 0) {
            return `<span class="text-success">In stock</span>`;
        } else {
            return `<span class="text-danger">Out of stock</span>`;
        }
    },

    cart: [],

    fillCart: function (arr, result) {
        for (var i = 0; i < arr.length; i++) {
            arr[i].addEventListener("change", (ev) => {
                if (ev.target.checked) {
                    let id = ev.target.parentElement.parentElement.id;
                    let product = result.find((ele) => {
                        return ele.id == id;
                    });

                    let invoiceItem = {
                        productId: product.id,
                        name: product.name,
                        quantity: ev.target.parentElement.parentElement.children[3].children[0].value,
                        unitPrice: product.unitPrice
                    }
                    this.cart.push(invoiceItem);
                    this.drawCart(this.cart);
                }
                else {
                    let id = ev.target.parentElement.parentElement.id;
                    let productToRemoveIndex = this.cart.findIndex((ele) => {
                        return ele.productId == id;
                    });
                    this.cart.splice(productToRemoveIndex, 1);
                    this.drawCart(this.cart);
                }
            });
        }
    },

    drawCart: function (cart) {
        productsCart.innerHTML = "";

        let result = cart.map((element, index) => {
            return `<a href="#" class="dropdown-item">
                     <div class="d-flex align-items-center">
                             <div class="ms-2">
                                  <h6 class="fw-normal mb-0">${element.name}</h6>
                                   <small>Quantity: ${element.quantity}</small>
                             </div>
                     </div>
                 </a>
                 <hr class="dropdown-divider">`;
        });

        for (var i = 0; i < result.length; i++) {
            productsCart.innerHTML += result[i];
        }
    },
    saveInvoiceDataIntoLocalStorage: function () {
        let invoiceData = { invoiceItems: this.cart, customer: this.customer }
        localStorage.setItem("invoiceData", JSON.stringify(invoiceData));
    },

    isInvoiceDataEmpty: function () {
        if (Object.keys(invoiceIndexPage.customer).length === 0 || invoiceIndexPage.cart.length === 0) {
            return true;
        }
        return false;
    },

    validateQuantity: function (element) {
        let availableQuantity = Number(element.parentElement.parentElement.children[4].innerHTML);
        if (element.value > availableQuantity) {
            window.alert("You cannot chose quantity more than the available")
            element.value = availableQuantity;
        } else if (element.value <= 0) {
            window.alert("You cannot chose quantity equals or less than zero");
            element.value = 1;
        } 
    }

}

// products table selections
let tableBody = document.getElementById("table-body");
let searchBox = document.getElementById("search-box");
let categorySelectList = document.getElementById("category-select-list");
let brandSelectList = document.getElementById("brand-select-list");

// customers table selections
let customersTableBody = document.getElementById("table-body-customers");
let customersTableSearchBox = document.getElementById("search-box-customers");

// product cart selections
let productsCart = document.getElementById("products-cart");

// getting review button
let reviewBtn = document.getElementById("btn-review");


// on page load -initialization-
invoiceIndexPage.getAllProducts();
invoiceIndexPage.getAllCustomers();


// products table events
searchBox.addEventListener("keyup", (event) => {
    invoiceIndexPage.getAllProducts();
});

categorySelectList.addEventListener("change", () => {
    invoiceIndexPage.getAllProducts();
});

brandSelectList.addEventListener("change", () => {
    invoiceIndexPage.getAllProducts();
});



// customer table events
customersTableSearchBox.addEventListener("keyup", (event) => {
    invoiceIndexPage.getAllCustomers();
});


reviewBtn.addEventListener("click", () => {
    if (!invoiceIndexPage.isExistingCustomerSectionExpanded()) {
        invoiceIndexPage.addNewCustomer();
    }

    if (invoiceIndexPage.isInvoiceDataEmpty()) {
        window.alert("Please chose at least one product and customer");
    }
    else
    {
        invoiceIndexPage.saveInvoiceDataIntoLocalStorage();
        window.location.href = "/Invoice/Review";
    }
});



let existingCustomersSection = document.getElementById("testbtn");

existingCustomersSection.addEventListener("click", () => {
    let isExpanded = existingCustomersSection.getAttribute("aria-expanded");
    console.log(isExpanded);
});

