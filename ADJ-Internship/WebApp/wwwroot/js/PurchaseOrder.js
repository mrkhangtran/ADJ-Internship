

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
  $('#openmodal').trigger('click');
  changePorts();
});

$("#origin").change(function () {
  changePorts();
});

function changePorts() {
  var origin = $("#origin").val();

  var vnPorts = document.getElementsByClassName("vnPorts");
  var hkPorts = document.getElementsByClassName("hkPorts");

  var ports = document.getElementsByClassName("ports");

  var options = [];

  if (origin == "Vietnam") {
    $.each(vnPorts, function () {
      options.push($(this).text());
    });
  }
  else if (origin == "HongKong") {
    $.each(hkPorts, function () {
      options.push($(this).text());
    });
  }

  for (i = 0; i < ports.length; i++) {
    ports[i].options.length = 0;
    for (j = 0; j < options.length; j++) {
      ports[i].options[ports[i].options.length] = new Option(options[j], options[j]);
    }
  }

};

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