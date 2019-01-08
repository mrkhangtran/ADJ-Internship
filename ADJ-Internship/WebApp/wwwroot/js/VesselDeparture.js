$(document.body).on('click', '.checkAll', function () {
  var id = $(this)[0].id.match(/\d+/)[0]; //get number from string
  var checkBoxes = document.getElementsByClassName("checkBox_" + id);
  var current = $(this)[0].checked;
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
    .appendTo('#searchForm');

  $("#searchForm").submit();
});

$(document.body).on('click', '#achieveButton', function () {
  $("#achieveForm").submit();
});

showResult = function showResult() {
  $(document).ready(function () {
    $('#openmodal').trigger('click');
    $('#pageValue').remove();
    changePorts();
  });
};

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