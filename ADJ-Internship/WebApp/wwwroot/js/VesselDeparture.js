﻿$(document.body).on('keyup', '.maxLength', function () {
  var maxLength = $(this).attr('maxlength');
  var length = $(this).val().length;
  var id = $(this).attr('id') + "Error";

  if (length > maxLength - 1) {
    document.getElementById(id).innerHTML = "Cannot be longer than " + (maxLength - 1) + " characters.";
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
    changePortsResult();
  });
};

$(document).ready(function showResult() {
  $('#openmodal').trigger('click');
  changePorts();
  changePortsResult();
});

$("#origin").change(function () {
  changePorts();
});

function changePortsResult() {
  var vnPorts = document.getElementsByClassName("vnPorts");
  var hkPorts = document.getElementsByClassName("hkPorts");

  var ports = document.getElementsByClassName("ports");

  var optionsVN = [];
  var optionsHK = [];

  $.each(vnPorts, function () {
    optionsVN.push($(this).text());
  });

  $.each(hkPorts, function () {
    optionsHK.push($(this).text());
  });

  for (i = 0; i < ports.length; i++) {
    ports[i].options.length = 0;

    if (ports[i].attributes.value != null) {
      var currentValue = ports[i].attributes.value.nodeValue;
    }
    if (currentValue != null) {

      if (isExist(currentValue, optionsVN)) {
        if ((i % 2) != 0) {
          $(document.createElement('optgroup')).attr('label', "Vietnam").appendTo(ports[i]);
        }
        ports[i].options[ports[i].options.length] = new Option(currentValue, currentValue);
        for (j = 0; j < optionsVN.length; j++) {
          if ((currentValue == null) || (currentValue != optionsVN[j])) {
            ports[i].options[ports[i].options.length] = new Option(optionsVN[j], optionsVN[j]);
          }
        }
        if ((i % 2) != 0) {
          $(document.createElement('optgroup')).attr('label', "Hong Kong").appendTo(ports[i]);

          for (j = 0; j < optionsHK.length; j++) {
            ports[i].options[ports[i].options.length] = new Option(optionsHK[j], optionsHK[j]);
          }
        }
      }
      else if (isExist(currentValue, optionsHK)) {
        if ((i % 2) != 0) {
          $(document.createElement('optgroup')).attr('label', "Hong Kong").appendTo(ports[i]);
        }
        ports[i].options[ports[i].options.length] = new Option(currentValue, currentValue);
        for (j = 0; j < optionsHK.length; j++) {
          if ((currentValue == null) || (currentValue != optionsHK[j])) {
            ports[i].options[ports[i].options.length] = new Option(optionsHK[j], optionsHK[j]);
          }
        }
        if ((i % 2) != 0) {
          $(document.createElement('optgroup')).attr('label', "Vietnam").appendTo(ports[i]);

          for (j = 0; j < optionsVN.length; j++) {
            ports[i].options[ports[i].options.length] = new Option(optionsVN[j], optionsVN[j]);
          }
        }
      }
    }
  }
};

function changePorts() {
  var origin = $("#origin").val();

  var vnPorts = document.getElementsByClassName("vnPorts");
  var hkPorts = document.getElementsByClassName("hkPorts");

  var ports = document.getElementsByClassName("portsSearch");

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
  else {
    $.each(vnPorts, function () {
      options.push($(this).text());
    });
    $.each(hkPorts, function () {
      options.push($(this).text());
    });
  }

  for (i = 0; i < ports.length; i++) {
    ports[i].options.length = 0;
    ports[i].options[ports[i].options.length] = new Option("Any", "");

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