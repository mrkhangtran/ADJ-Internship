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

    var method = $(this).attr("value");
    var currentPage = parseInt($("#currentPage").text());

    $.ajax({
      type: "POST",
      data: {
        method: method,
        newItem: newItem,
        orderDetails: orderDetails,
        currentPage: currentPage
      },
      url: "/PurchaseOrder/AddItem",
      success: function (objOperations) {
        $("#orderDetailPartial").html(objOperations);
        rebindValidators();
      }
    });
  }
});

$(document).ready(function showResult() {
  $(document).ready(function () {
    $('#openmodal').trigger('click');
  });
});

$("#resultModal").on({
  'hide.uk.modal': function () {
    window.location.href = "/Order/Display";
  }
});

function rebindValidators() {
  var $form = $("#orderForm");
  $form.unbind();
  $form.data("validator", null);
  $.validator.unobtrusive.parse($form);
  $form.validate($form.data("unobtrusiveValidation").options);
};