$(document.body).on('click', '#checkAll', function () {
  var checkBoxes = document.getElementsByClassName("checkBoxes");
  var current = $("#checkAll")[0].checked;
  for (i = 0; i < checkBoxes.length; i++) {
    checkBoxes[i].checked = current;
  };
});

$(document.body).on('click', '.paging', function () {
  var value = $(this).attr("value");
  var name = $(this).attr("name");
  $('<input />').attr('type', 'hidden')
    .attr('name', name)
    .attr('value', value)
    .attr('id', "pageValue")
    .appendTo('#bookingForm');

  $("#bookingForm").submit();
});

$(document.body).on('click', '.searchButton', function () {
  if ($("#filterForm").valid()) {
    //get data from form
    var page = 1;
    var filters = [];
    var items = document.getElementsByClassName("filters");
    $.each(items, function () {
      filters.push($(this).val());
    });

    var origin = filters[0];
    var vendor = filters[1];
    var originPort = filters[2];
    var warehouse = filters[3];
    var mode = filters[4];
    var status = filters[5];
    var poNumber = filters[6];
    var itemNumber = filters[7];

    $.ajax({
      type: "POST",
      data: {
        page: page,
        origin: origin,
        originPort: originPort,
        mode: mode,
        warehouse: warehouse,
        status: status,
        vendor: vendor,
        poNumber: poNumber,
        itemNumber: itemNumber
      },
      url: "/ShipmentBooking/Filter",
      success: function (objOperations) {
        showResult();
        $("#resultPartial").html(objOperations);
        changePorts();
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
};

showResult = function showResult() {
  $(document).ready(function () {
    changePorts();
    $('#openmodal').trigger('click');
    $('#pageValue').remove();
  });
};

$(document).ready(function () {
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