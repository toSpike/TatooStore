// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
'use strict'
function AddSupplyClick(id, goodName) {
    modalSupplyLable.textContent = "Добавить поставку товара: " + goodName;
    goodId.value = id;
};

function AddToBasket (id) {
    $.ajax({
        type: "POST",
        url: "/Catalog?handler=AddGoodToBasket",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: { "id": id },
        success: function (response) {
            basketNav.innerHTML = '<i class="fa-solid fa-basket-shopping"></i> ' + response.totalCost.toFixed(2).replace(".", ",") + ' &#8381';
            basketNav.hidden = false;
            basketNavEmpty.hidden = true;
        },
        failure: function (response) {
            alert("failure");
        },
        error: function (response) {
            alert("Error");
        }
    });
};

function AddCountItemBasket(id) {
    let row = document.getElementById(id);
    let value = parseInt(row.children[2].firstElementChild.value);
    if (Number.isInteger(value)) {
        $.ajax({
            type: "POST",
            url: "/Basket?handler=AddCountItemBasket",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { "id": id, "value": value },
            success: function (response) {
                //row = document.getElementById(response.id);
                row.children[3].innerHTML = response.cost.toFixed(2).replace(".", ",") + ' &#8381';
                basketNav.innerHTML = '<i class="fa-solid fa-basket-shopping"></i> ' + response.totalCost.toFixed(2).replace(".", ",") + ' &#8381';
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert("Error");
            }
        });
    } 
};

function PlaceOrder() {
        $.ajax({
            type: "POST",
            url: "/Basket?handler=PlaceOrder",
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: {
                "address": document.getElementById("address").value,
            },
            success: function (response) {
                if (typeof response === "string") {
                    document.getElementsByClassName("modal-body")[0].innerHTML = response;
                    var myModal = new bootstrap.Modal(document.getElementById('modalBasket'), {
                        keyboard: false
                    })
                    myModal.show();
                } else {
                    window.location.href = response.redirect;
                }
            },
            failure: function (response) {
                alert("failure");
            },
            error: function (response) {
                alert("Error");
            }
        });
};