$(document.body).on('click', '.checkBoxes', function () {
  GetEarliestShipDate();
  GetLatestDeliveryDate();
});

$(document.body).on('click', '#makeBooking', function () {
  var value = $(this).attr("value");
  var name = $(this).attr("name");
  $('<input />').attr('type', 'hidden')
    .attr('name', name)
    .attr('value', value)
    .attr('id', "pageValue")
    .appendTo('#bookingForm');

  var ETDcheck = AfterShipDate();
  var ETAcheck = NotAfterDeliveryDate();

  if ((ETDcheck) && (ETAcheck)) {
    $("#bookingForm").submit();
  }
  else {
    if (!ETDcheck) {
      document.getElementById("ETDError").innerHTML = "Please set ETD after the date below.";
    }
    if (!ETAcheck) {
      document.getElementById("ETAError").innerHTML = "Please set ETA no later than the date below.";
    }
  }
});

function AfterShipDate() {
  var shipDate = GetEarliestShipDate();
  var ETD = document.getElementById("ETD").value.replace(/-/g, "");

  if (shipDate < ETD) {
    return true;
  }
  else {
    return false;
  }
};

function NotAfterDeliveryDate() {
  var deliveryDate = GetLatestDeliveryDate();
  var ETA = document.getElementById("ETA").value.replace(/-/g, "");

  if (ETA <= deliveryDate) {
    return true;
  }
  else {
    return false;
  }
};

function GetLatestDeliveryDate() {
  var checkBoxes = document.getElementsByClassName("checkBoxes");
  var maxDate = new Date(9999, 1, 1);
  maxDate = DatetoString(maxDate);
  var latest = maxDate;

  var shipDate = GetEarliestShipDate();
  var shipDateMonth = (shipDate[4] + shipDate[5]) - 1;
  shipDate = new Date(shipDate[0] + shipDate[1] + shipDate[2] + shipDate[3], shipDateMonth, shipDate[6] + shipDate[7]);
  shipDate.setDate(shipDate.getDate() + 2);
  shipDate = DatetoString(shipDate);

  for (i = 0; i < checkBoxes.length; i++) {
    if (checkBoxes[i].checked == true) {
      var deliveryDate = document.getElementById("DeliveryDate_" + i).attributes.string.nodeValue;
      if (deliveryDate >= shipDate) {
        if (deliveryDate < latest) {
          latest = deliveryDate;
        }
      }
    }
  }

  if (latest == maxDate) {
    latest = new Date();
    latest.setDate(latest.getDate() + 2);
    latest = DatetoString(latest);
  }

  document.getElementById("latestDeliveryDate").innerHTML = "Latest date can be set is " + latest[4] + latest[5] + "/" + latest[6] + latest[7] + "/" + latest[0] + latest[1] + latest[2] + latest[3];

  return latest;
};

function GetEarliestShipDate() {
  var checkBoxes = document.getElementsByClassName("checkBoxes");
  var maxDate = new Date(9999, 1, 1);
  maxDate = DatetoString(maxDate);
  var earliest = maxDate;
  for (i = 0; i < checkBoxes.length; i++) {
    if (checkBoxes[i].checked == true) {
      var shipDate = document.getElementById("ShipDate_" + i).attributes.string.nodeValue;
      var today = new Date();
      today = DatetoString(today);
      if (shipDate >= today) {
        if (shipDate < earliest) {
          earliest = shipDate;
        }
      }
    }
  }

  if (earliest == maxDate) {
    earliest = new Date();
    earliest = DatetoString(earliest);
  }

  document.getElementById("earliestShipDate").innerHTML = "Earliest date can be set is AFTER " + earliest[4] + earliest[5] + "/" + earliest[6] + earliest[7] + "/" + earliest[0] + earliest[1] + earliest[2] + earliest[3];

  return earliest;
};

function DatetoString(date) {
  return "" + date.getFullYear() + ('0' + (date.getMonth() + 1)).slice(-2) + ('0' + date.getDate()).slice(-2);
};

$(document.body).on('click', '#checkAll', function () {
  var checkBoxes = document.getElementsByClassName("checkBoxes");
  var current = $("#checkAll")[0].checked;
  for (i = 0; i < checkBoxes.length; i++) {
    checkBoxes[i].checked = current;
  };

  GetEarliestShipDate();
  GetLatestDeliveryDate();
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
  GetEarliestShipDate();
  GetLatestDeliveryDate();
};

$(document).ready(function () {
  $('#openmodal').trigger('click');
  changePorts();
  GetEarliestShipDate();
  GetLatestDeliveryDate();
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