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
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    },

    drawTable: function (arr) {
        tableBody.innerHTML = "";

        let result = arr.map((element, index) => {
            return `<tr id = ${element.id}>
                        <td><input class="form-check-input product-check-box" ${this.isInCart(element.id)} type="checkbox"></td>
                        <td>${element.name}</td>
                        <td>${element.unitPrice}</td>
                        <td style="width:15%;"><input class="form-control" value="1" type="number" /></td>
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
            return ele.id == id;
        });

        if (result == undefined) {
            return "unchecked";
        } else {
            return "checked = true";
        }
    },

    drawCustomersTable: function (arr) {
        customersTableBody.innerHTML = "";

        let result = arr.map((element, index) => {
            return `<tr>
                        <td><input class="form-check-input" type="checkbox"></td>
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
                        id: product.id,
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
                        return ele.id == id;
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
