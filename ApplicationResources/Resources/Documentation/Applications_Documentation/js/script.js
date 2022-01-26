$(document).ready(function () {
	/* ********************************************************************* */
	// Text replacement
	Cufon.replace('#logo .title');	
	Cufon.replace('#header_sub h2');
	Cufon.replace('#content h1');
	Cufon.replace('#content h2');
	Cufon.replace('#slider_desc li');
	
	/* ********************************************************************* */
	// Contact form	
	$('#contact_form').validate();	
	// AJAX form
	$('form#contact_form').submit(function() {
		if ($('label.error:visible').length !== 0) {
			return false;
		}
		
		var s = $(this).serialize();
		
		if (($(this).attr('action') === '') || ($(this).attr('action')==='#')) 
			action = '?'; 
		else 
			action = $(this).attr('action');

		$.ajax({
			type: $(this).attr('method'),
			data: s,
			url: action,
			success: function(result) {
				if (result == '1') {
					alert('E-mail sent');
					$('form#contact_form')[0].reset();
				}
				else { 
					alert('E-mail can not be sent!');
				}
				
				return false;
			}
		});	

		return false;
	});
	/* ********************************************************************* */		
	// Slider Nivo
	$('#slider_nivo_items').nivoSlider({
		effect: 									'random',
		slices: 									15,
		animSpeed: 								500,
		pauseTime: 								4000,
		startSlide: 							0, 
		directionNav: 						true,
		directionNavHide: 				false, 
		controlNav: 							true, 
		controlNavThumbs: 				false,
		controlNavThumbsSearch: 	'.jpg', 
		controlNavThumbsReplace: 	'_thumb.jpg', 
		keyboardNav: 							true, 
		pauseOnHover: 						false,
		manualAdvance: 						false, 
		beforeChange: 						function(){
		},
		afterChange: 							function(){			
		},
		slideshowEnd: 						function(){}		
	});
		
	/* ********************************************************************* */		
	// Slider
	$('#slider_items').roundabout({
	  btnNext: '#next a',
    btnPrev: '#prev a',
		minOpacity: '1',
		childSelector: '.item',
		reflect: true
	});

	// Slider speed
	setInterval(function () {
		$('#slider_items').roundabout_animateToNextChild();
	}, 4000);
		
	// Slider descriptions & pager
	var amount 		= $('#slider_items .item').size();
	var imageIndex 	= 0;
	
	// Adds bullets over the slider
	for (i = 0; i < amount; i++)
		$('#slider_pager').append('<img src="images/slider-pager.png" alt="' + (i + 1) + '">');
	// Show first bullet	
	$('#slider_pager img:first').attr('src', 'images/slider-pager-active.png');
	
	$('#slider_pager img').click(function () {
		$('#slider_items').roundabout_animateToChild($(this).index());
	});
	
	// Show first description
	$('#slider_desc ul li:first').show();
	// Focus
	$('#slider_items').focus(function () {
		imageIndex = $(this).find('.roundabout-in-focus').index();
		$('#slider_desc ul li').each(function(index) {
			if (index == imageIndex) {
				$(this).show();
			}
		});
		
		$('#slider_pager img').each(function(index) {
			if (index == imageIndex) {
				$(this).attr('src', 'images/slider-pager-active.png');
			}
		});
		return false;
	// Blur	
	}).blur(function () {
		$('#slider_pager img').attr('src', 'images/slider-pager.png')
		$('#slider_desc ul li').hide();
	});
	
	/* ********************************************************************* */	
	// Dropdown navigation
	$('#main_menu ul ul').hover(
		function () {
			$(this).parent().find('a:first').addClass('parent');
		}, 
		function () {
			$(this).parent().find('a:first').removeClass('parent');
	});
	
	$("#main_menu li").hover(
		function() {
			$(this).find('ul:first').css({'visibility': 'visible', 'display': 'none'}).slideDown();
		},
		function() { 
			$(this).find('ul:first').css({visibility: "hidden"});
		}
	);
	
	/* ********************************************************************* */	
	// Image titles
	$('#content .image img').each(function (index) {
		$(this).parent().parent().append('<div class="desc">' + $(this).attr('alt') + '</div>');
	});
	// Image fancybox
	$("#content a[href$='gif']").fancybox();
	$("#content a[href$='jpg']").fancybox();
	$("#content a[href$='png']").fancybox();

	/* ********************************************************************* */	
	// Sidebar actions
	
	// Hide & show sidebar
	$('#remove').toggle(function () {
			$('#sidebar_box').hide(0, function () {
				$('#main').addClass('full');
			});
			
			$(this).html('Show');
			return false;
		}, 
		function () {
			$('#sidebar_box').show(0);
			$(this).html('Hide');
			$('#main').removeClass('full');
			return false;
	});
	
	// Change sidebar color
	$('#variant').toggle(function () {
			$(this).html('Black variant');
			$('#sidebar').addClass('light');
			Cufon.replace('#sidebar h2');			
			return false;
		}, 
		function () {
			$(this).html('Light variant');
			$('#sidebar').removeClass('light');
			Cufon.replace('#sidebar h2');
			return false;
	});	
	// Change sidebar side
	$('#move').click(function () {
		if ($('#sidebar').hasClass('left')) {
			$('#sidebar').removeClass('left');
			$('#sidebar').addClass('right');
			$('#sidebar_nav').css({'right': '50px', 'left': 'auto'});
				
			$('#main').removeClass('right');
			$('#main').addClass('left');
						
			$('#move').html('Move left');
			return false;
		}
		else {
			$('#sidebar').removeClass('right');
			$('#sidebar').addClass('left');
			$('#sidebar_nav').css({'left': '50px', 'right': 'auto'});
			
			$('#main').removeClass('left');
			$('#main').addClass('right');
						
			$('#move').html('Move right');	
			return false;		
		}
	});	

	/* ********************************************************************* */	
	// Corners

	$('#contact').corner('5px');
	$('#contact .confirm').corner('3px');
});
