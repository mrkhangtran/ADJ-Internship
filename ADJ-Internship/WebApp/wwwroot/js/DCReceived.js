$(document.body).on('keyup', '.maxLength', function () {
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

$(document.body).on('click', '#checkAll', function () {
  var checkBoxes = document.getElementsByClassName("checkBox");
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
})

$(document.body).on('click', '#achieveButton', function () {
  $("#achieveForm").submit();
});

showResult = function showResult() {
  $(document).ready(function () {
    $('#openmodal').trigger('click');
    $('#pageValue').remove();
  });
};