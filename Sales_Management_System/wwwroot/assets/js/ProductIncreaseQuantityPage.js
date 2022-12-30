let productIncreaseQuantityPage = {
    tableBody: document.getElementById("table-body"),
    searchBox: document.getElementById("search-box"),
    increaseQantityBtn: document.getElementById("increase-quantity-btn"),

    showProducts: function () {
        $.ajax({
            type: "POST",
            data: {
                search: productIncreaseQuantityPage.searchBox.value,             
            },
            url: "/Product/GetAllProducts",
            dataType: "json",
            success: function (response) {
                productIncreaseQuantityPage.drawTable(response);
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    },


    drawTable: function (arr) {
        this.tableBody.innerHTML = "";

        let result = arr.map((element, index) => {
            return `<tr id = ${element.id}>
                        <td><input onchange="productIncreaseQuantityPage.modifyObjectToSubmit(this)" class="form-check-input product-check-box" type="checkbox"></td>
                        <td>${element.id}</td>
                        <td>${element.name}</td>
                        <td>${element.quantity}</td>
                        <td style="width:15%;"><input class="form-control" value="1" type="number" /></td> 
                    </tr>`;
        });

        for (var i = 0; i < result.length; i++) {
            this.tableBody.innerHTML += result[i];
        }
    },

    objectToSubmit: { productId: null, productQuntity:null },

    modifyObjectToSubmit: function (selctedItem) {
        let productId = selctedItem.parentElement.parentElement.id;
        let productQuntity = selctedItem.parentElement.parentElement.children[4].children[0].value;

        this.objectToSubmit.productId = productId;
        this.objectToSubmit.productQuntity = productQuntity;
    },

    submit: function () {
        if (this.objectToSubmit.productId == null) {
            window.alert("you have to chose any product");
            return;
        }
        $.ajax({
            type: "POST",
            data: {
                productId : this.objectToSubmit.productId,
                quantity : this.objectToSubmit.productQuntity
            },
            url: "/Product/IncreaseQuantity",
            dataType: "json",
            success: function (response) {
                window.alert(response);
            },
            failure: function (err) {
                window.alert("حدث خطأ غير متوقع");
            }
        });
    }
}


productIncreaseQuantityPage.searchBox.addEventListener("keyup", (event) => {
    productIncreaseQuantityPage.showProducts();
});

window.addEventListener("load", () => {
    productIncreaseQuantityPage.showProducts();
});

productIncreaseQuantityPage.increaseQantityBtn.addEventListener("click", () => {
    productIncreaseQuantityPage.submit();
});
