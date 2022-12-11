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
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    },

    drawTable: function (arr) {
        tableBody.innerHTML = "";

        let result = arr.map((element, index) => {
            return `<tr>
                        <td><input class="form-check-input" type="checkbox"></td>
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
    getProductStatus: function (availableQuantity) {
        if (availableQuantity > 0) {
            return `<span class="text-success">In stock</span>`;
        } else {
            return `<span class="text-danger">Out of stock</span>`;
        }
    }
}

let tableBody = document.getElementById("table-body");
let searchBox = document.getElementById("search-box");
let categorySelectList = document.getElementById("category-select-list");
let brandSelectList = document.getElementById("brand-select-list");


invoiceIndexPage.getAllProducts();

searchBox.addEventListener("keyup", (event) => {
    invoiceIndexPage.getAllProducts();
});

categorySelectList.addEventListener("change", () => {
    invoiceIndexPage.getAllProducts();
});

brandSelectList.addEventListener("change", () => {
    invoiceIndexPage.getAllProducts();
});
