//$(document.body).on('click', '.bookButton', function () {
//  if ($("#bookingForm").valid()) {
//    //get data from form
//    var booking = [];
//    var items = document.getElementsByClassName("booking");
//    $.each(items, function () {
//      filters.push($(this).val());
//    });

//    $.ajax({
//      type: "POST",
//      data: {
//        booking: booking
//      },
//      url: "/ShipmentBooking/Booking",
//      success: function (objOperations) {
//        alert("Your shipment is successfully made");
//        $("#resultPartial").html(objOperations);
//        rebindValidators();
//      }
//    });
//  }
//});

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
        $("#resultPartial").html(objOperations);
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