$(document).ready(function () {
	$('#header #logo img').ifixpng();
	$('#slider_pager img').ifixpng();
	$('#header #main_menu ul ul').ifixpng();
	$('#sidebar .dec').ifixpng();
	
	$('#slider_items').blur(function () {
		$('#slider_pager img').ifixpng();
	}).focus(function () {
		$('#slider_pager img').ifixpng();
	});
})