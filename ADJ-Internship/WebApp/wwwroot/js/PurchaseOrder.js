$("#to").keyup(function () { $("#from").val($("#to").val()); });
$("#from").keyup(function () { $("#to").val($("#from").val()); });

function getTotalUnit() {
  document.getElementById("TotalUnit").innerHTML = document.getElementById("Quantity").value * document.getElementById("UnitPrice").value;
};
function getTotalRetail() {
  document.getElementById("TotalRetail").innerHTML = document.getElementById("Quantity").value * document.getElementById("RetailPrice").value;
};

$(document.body).on('click', '.itemButton', function () {
  if ($("#orderForm").valid()) {
    var newItem = [];
    var items = document.getElementsByClassName("newItem");
    $.each(items, function () {
      newItem.push($(this).val());
    });

    var orderDetails = [];
    var items = document.getElementsByClassName("itemProperty");
    $.each(items, function () {
      orderDetails.push($(this).val());
    });

    $.ajax({
      type: "POST",
      data: {
        method: $(this).val(),
        newItem: newItem,
        orderDetails: orderDetails
      },
      url: "/PurchaseOrder/AddItem",
      success: function (objOperations) {
        $("#orderDetailPartial").html(objOperations);
        rebindValidators();
      }
    });
  }
});

function rebindValidators() {
  var $form = $("#orderForm");
  $form.unbind();
  $form.data("validator", null);
  $.validator.unobtrusive.parse($form);
  $form.validate($form.data("unobtrusiveValidation").options);
}

// $("#applyButton").click(function () {
//  var addModel = @Html.Raw(Json.Serialize(Model));
//  var orderDetails = [];
//  $.each($("input[id='itemProperty']"), function () {
//    orderDetails.push($(this).val());
//  });

//  $.ajax({
//    type: "POST",
//    data: {
//      orderDetails: orderDetails,
//      progressCheckDTOs: addModel
//    },
//    url: "@Url.Action("Create", "PurchaseOrder")",
//    success: function () {
//      alert("Success");
//    }
//  });

//});

function bothFormSubmit() {
  document.getElementById("detailForm").submit();
  document.getElementById("orderForm").submit();
};

function detailFormSubmit() {
  document.getElementById("detailForm").submit();
};

function orderFormSubmit() {
  document.getElementById("orderForm").submit();
};