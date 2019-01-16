$("#to").keyup(function () { $("#from").val($("#to").val()); });
$("#from").keyup(function () { $("#to").val($("#from").val()); });

function getTotalUnit() {
  document.getElementById("TotalUnit").innerHTML = (document.getElementById("Quantity").value * document.getElementById("UnitPrice").value).toFixed(2);
};
function getTotalRetail() {
  document.getElementById("TotalRetail").innerHTML = (document.getElementById("Quantity").value * document.getElementById("RetailPrice").value).toFixed(2);
};

$(document.body).on('click', '.itemButton', function () {
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

  if (method != "Cancel") {
    if (parseFloat(newItem[7]) < 0) {
      document.getElementById("quantityError").innerHTML = "Must be a positive value.";
      return false;
    }

    if (parseFloat(newItem[7]) == 0) {
      document.getElementById("quantityError").innerHTML = "Must not be zero.";
      return false;
    }

    if (parseFloat(newItem[7]) > 9999999999) {
      document.getElementById("quantityError").innerHTML = "Must be less than 10 digits.";
      return false;
    }

    if (String(newItem[7]).includes('.')) {
      document.getElementById("quantityError").innerHTML = "Must be an integer.";
      return false;
    }
  }

  if (($("#orderForm").valid()) || (method == "Cancel")) {
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
  else if (origin == "Hong Kong") {
    $.each(hkPorts, function () {
      options.push($(this).text());
    });
  }

  for (i = 0; i < ports.length; i++) {
    ports[i].options.length = 0;
    var optgroup = ports[i].getElementsByTagName('optgroup');
    while (optgroup.length > 0) {
      ports[i].removeChild(optgroup[0]);
    }
    if (i > 0) {
      $(document.createElement('optgroup')).attr('label', origin).appendTo(ports[i]);
    }

    if (ports[i].attributes.value != null) {
      var currentValue = ports[i].attributes.value.nodeValue;
    }
    if ((currentValue != null) && (isExist(currentValue, options))) {
      ports[i].options[ports[i].options.length] = new Option(currentValue, currentValue);
    }
    for (j = 0; j < options.length; j++) {
      if ((currentValue == null) || (currentValue != options[j])) {
        ports[i].options[ports[i].options.length] = new Option(options[j], options[j]);
      }
    }
    if (i == 1) {
      if (origin == "Vietnam") {
        $(document.createElement('optgroup')).attr('label', "Hong Kong").appendTo(ports[i]);

        $.each(hkPorts, function () {
          ports[i].options[ports[i].options.length] = new Option($(this).text(), $(this).text());
        });
      }
      else if (origin == "Hong Kong") {
        $(document.createElement('optgroup')).attr('label', "Vietnam").appendTo(ports[i]);

        $.each(vnPorts, function () {
          ports[i].options[ports[i].options.length] = new Option($(this).text(), $(this).text());
        });
      }
    }
  }
};

function isExist(value, arr) {
  var exist = false;
  for (k = 0; k < arr.length; k++) {
    if (arr[k] == value) {
      exist = true;
      break;
    }
  }

  return exist;
};

$(document.body).on('keyup', '.maxLength', function () {
  var maxLength = $(this).attr('maxlength');
  var length = $(this).val().length;
  var id = $(this).attr('id') + "Error";
  
  if (length > maxLength-1) {
    document.getElementById(id).innerHTML = "Cannot be longer than " + (maxLength-1) + " characters.";
    var currentValue = "" + document.getElementById($(this).attr('id')).value;
    currentValue = currentValue.substr(0, currentValue.length - 1);
    document.getElementById($(this).attr('id')).value = currentValue;
  }
  else {
    document.getElementById(id).innerHTML = "";
  };
});

$(document.body).on('focusout', '.maxLength', function () {
  var id = $(this).attr('id') + "Error";
  document.getElementById(id).innerHTML = "";
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