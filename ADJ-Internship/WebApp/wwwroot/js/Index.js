$(window).resize(function matchHeight() {
  windowHeight = $(window).height();
  var style = setStyle();
  document.getElementById('background').setAttribute("style", style + " min-height:" + windowHeight + "px");
});

$(document).ready(function matchHeight() {
  windowHeight = $(window).height();
  var style = setStyle();
  document.getElementById('background').setAttribute("style", style + " min-height:" + windowHeight + "px");
});

function setStyle() {
  var style = "background-color: white;";

  return style;
}