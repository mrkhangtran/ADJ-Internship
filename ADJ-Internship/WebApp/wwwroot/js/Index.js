$(window).resize(function matchHeight() {
  windowHeight = $(window).height();
  var style = setStyle();
  document.getElementById('background').setAttribute("style", style + " height:" + windowHeight + "px");
});

$(document).ready(function matchHeight() {
  windowHeight = $(window).height();
  var style = setStyle();
  document.getElementById('background').setAttribute("style", style + " height:" + windowHeight + "px");
});

function setStyle() {
  var style = "background-color: #d9d9d9;";

  return style;
}