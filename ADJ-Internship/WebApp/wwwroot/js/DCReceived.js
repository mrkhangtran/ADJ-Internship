$(document.body).on('click', '.checkAll', function () {
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