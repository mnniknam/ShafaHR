(function($) {
	
	"use strict";
	
	//Hide Loading Box (Preloader)
	function handlePreloader() {
		if($('.preloader').length){
			$('.preloader').delay(200).fadeOut(500);
		}
	}
	
	//Update Header Style and Scroll to Top
	function headerStyle() {
		if($('.main-header').length){
			var windowpos = $(window).scrollTop();
			var siteHeader = $('.main-header');
			var scrollLink = $('.scroll-to-top');
			var sticky_header = $('.main-header .sticky-header, .main-header .mobile-sticky-header');
			if (windowpos > 500) {
				siteHeader.addClass('fixed-header');
				sticky_header.addClass('animated slideInDown');
				scrollLink.fadeIn(300);
			} else{
				siteHeader.removeClass('fixed-header');
				sticky_header.removeClass('animated slideInDown');
				scrollLink.fadeOut(300);
			}
		}
	}
	headerStyle();

	// Open modal in AJAX callback
	$('#appointment-btn').on('click', function(event) {
	  event.preventDefault();
	  this.blur();
	  $.get(this.href, function(html) {
	    $(html).appendTo('body').modal({
			clickClose: false,
			fadeDuration: 300,
			fadeDelay: 0.15,
	    });
	  });
	});

	//sticky-header Hide Show
	if($('.sticky-header').length){
		var stickyMenuContent = $('.main-header .main-box .nav-outer').html();
		$('.sticky-header .main-box').append(stickyMenuContent);
		//Sidebar Cart
		$('.main-header .cart-btn, .mobile-header .cart-btn').on('click', function() {
			$('body').addClass('sidebar-cart-active');
		});

		//Menu Toggle Btn
		$('.main-header .cart-back-drop, .main-header .close-cart').on('click', function() {
			$('body').removeClass('sidebar-cart-active');
		});
	}

	// Theme Button layers
	if($('.theme-btn').length){
		$(".btn-style-one,.btn-style-two,.btn-style-three,.btn-style-four")
		.append('<span></span> <span></span> <span></span> <span></span> <span></span>');
	}

	// Title Icon Divider // SVG Code
	if($('.divider').length){
		$(".divider").append('<svg viewBox="0 0 300.08 300.08"><path d="m293.26 184.14h-82.877l-12.692-76.138c-.546-3.287-3.396-5.701-6.718-5.701-.034 0-.061 0-.089 0-3.369.027-6.199 2.523-6.677 5.845l-12.507 87.602-14.874-148.69c-.355-3.43-3.205-6.056-6.643-6.138-.048 0-.096 0-.143 0-3.39 0-6.274 2.489-6.752 5.852l-19.621 137.368h-9.405l-12.221-42.782c-.866-3.028-3.812-5.149-6.8-4.944-3.13.109-5.777 2.332-6.431 5.395l-8.941 42.332h-73.049c-3.771 0-6.82 3.049-6.82 6.82 0 3.778 3.049 6.82 6.82 6.82h78.566c3.219 0 6.002-2.251 6.67-5.408l4.406-20.856 6.09 21.313c.839 2.939 3.526 4.951 6.568 4.951h20.46c3.396 0 6.274-2.489 6.752-5.845l12.508-87.596 14.874 148.683c.355 3.437 3.205 6.056 6.643 6.138h.143c3.39 0 6.274-2.489 6.752-5.845l14.227-99.599 6.397 38.362c.546 3.287 3.396 5.702 6.725 5.702h88.66c3.771 0 6.82-3.049 6.82-6.82-.001-3.772-3.05-6.821-6.821-6.821z"/></svg>');
	}

	// Mobile Navigation
	if($('#nav-mobile').length){

		jQuery(function ($) {
		  var $navbar = $('#navbar');
		  var $mobileNav = $('#nav-mobile');
		  
		  $navbar
		    .clone()
		    .removeClass('navbar')
		    .appendTo($mobileNav);
		  
		  $mobileNav.mmenu({
		  	"counters": false,
		  	"navbars": [
              {
                 "position": "top",
                 "content": [
                    "prev",
                    "title"
                 ]
              },
              {
                 "position": "bottom",
                 "content": [
                    "<a class='fab fa-facebook-f' href='#'></a>",
                    "<a class='fab fa-twitter' href='#'></a>",
                    "<a class='fab fa-linkedin-in' href='#'></a>",
                    "<a class='fab fa-instagram' href='#'></a>",
                 ]
              }
           ],
		  	"extensions": [
	          "theme-dark"
	       ],
		    offCanvas: {
		      position: 'left',
		      zposition: 'front',
		    }
		  });
		});
	}



	// Banner Slider
	if ($('.banner-swiper').length) {
		var swiper = new Swiper(".banner-swiper", {
			slidesPerView: 1,
			loop: true,
			navigation: {
				nextEl: ".swiper-button-next",
				prevEl: ".swiper-button-prev",
			},
		});
	}
	

	//Services Carousel
	if ($('.service-swiper').length) {
		var swiper = new Swiper(".service-swiper", {
			slidesPerView: 3,
			spaceBetween: 30,
			loop: true,
			pagination: {
				el: ".swiper-pagination",
				clickable: true,
			},
			breakpoints: {
				991: {
					slidesPerView: 3,
				},
				768: {
					slidesPerView: 2,
				},
				0: {
					slidesPerView: 1,
				},
			},
		});
	}


	//Services Carousel
	if ($('.testi-three-swiper').length) {
		var swiper = new Swiper(".testi-three-swiper", {
			slidesPerView: 1,
			spaceBetween: 30,
			loop: true,
			pagination: {
				el: ".swiper-pagination",
				clickable: true,
			},
		});
	}

	//single-item Carousel
	if ($('.single-item-swiper').length) {
		var swiper = new Swiper(".single-item-swiper", {
			slidesPerView: 1,
			loop: true,
			pagination: {
				el: ".swiper-pagination",
				clickable: true,
			},
			navigation: {
				nextEl: ".swiper-button-next",
				prevEl: ".swiper-button-prev",
			},
		});
	}

	// Client Slider
	if ($('.client-swiper').length) {
		var swiper = new Swiper(".client-swiper", {
			slidesPerView: 5,
			spaceBetween: 30,
			loop: true,
			speed: 800,
			autoplay: {
				delay: 3000,
				disableOnInteraction: false,
			},
			breakpoints: {
				1200: {
					slidesPerView: 5,
				},
				991: {
					slidesPerView: 4,
				},
				768: {
					slidesPerView: 3,
				},
				430: {
					slidesPerView: 2,
				},
				0: {
					slidesPerView: 1,
				},
			},
		});
	}

	//Testimonial Carousel Two
	if ($('.testi-swiper-two').length) {
		var swiper = new Swiper(".testi-swiper-two", {
			slidesPerView: 2,
			loop: true,
			breakpoints: {
				991: {
					slidesPerView: 2,
				},
				768: {
					slidesPerView: 2,
				},
				0: {
					slidesPerView: 1,
				},
			},
		});
	}


	// Testi Gallery Swiper
	 var swiper = new Swiper(".testi-thumbs-swiper", {
		loop: true,
		spaceBetween: 10,
		freeMode: true,
		watchSlidesProgress: true,
		loopedSlides: 5,

		// Responsive breakpoints
		breakpoints: {
			0: {
				slidesPerView: 1,
			},
			601: {
				slidesPerView: 3,
			}
		}
	});
	var swiper2 = new Swiper(".testi-content-swiper", {
		loop: true,
		spaceBetween: 10,
		pagination: {
			el: ".swiper-pagination",
			clickable: true,
		},
		navigation: {
			nextEl: ".swiper-button-next",
			prevEl: ".swiper-button-prev",
		},
		thumbs: {
			swiper: swiper,
		},
		loopedSlides: 5,
	});
	
	
	//Default Masonary
	function defaultMasonry() {
		if($('.masonry-items-container').length){
	
			var winDow = $(window);
			// Needed variables
			var $container=$('.masonry-items-container');
	
			$container.isotope({
				itemSelector: '.masonry-item',
				 masonry: {
					columnWidth : '.masonry-item'
				 },
				animationOptions:{
					duration:500,
					easing:'linear'
				}
			});
	
			winDow.on('resize', function(){

				$container.isotope({ 
					itemSelector: '.masonry-item',
					animationOptions: {
						duration: 500,
						easing	: 'linear',
						queue	: false
					}
				});
			});
		}
	}
	defaultMasonry();

    //MixItup Gallery
	if($('.filter-list').length){
	 	 $('.filter-list').mixItUp();
	 }

	//Jquery Spinner / Quantity Spinner
	if($('.quantity-spinner').length){
		$("input.quantity-spinner").TouchSpin({
		  verticalbuttons: true
		});
	}

	//Header Search
	if($('.search-btn').length) {
		$('.search-btn').on('click', function() {
			$('.main-header').addClass('moblie-search-active');
		});
		$('.close-search, .search-back-drop').on('click', function() {
			$('.main-header').removeClass('moblie-search-active');
		});
	}

	//Coming Soon Coundown.
	if($('.cs-countdown').length){
	   $(function(){
		    $('[data-countdown]').each(function() {
		   var $this = $(this), finalDate = $(this).data('countdown');
		   $this.countdown(finalDate, function(event) {
		     $this.html(even.tstrftime('%D days %H:%M:%S'));
		   });
		 });
		});

	   $('.cs-countdown').countdown('').on('update.countdown', function(event) {
		  var $this = $(this).html(event.strftime('<div><span>%D</span><h6>days</h6></div> <div><span>%H</span><h6>Hours</h6></div> <div><span>%M</span><h6>Minutes</h6></div> <div><span>%S</span><h6>Seconds</h6></div>'));
		});
	}

	//Message Box
	if($('.message-box').length){
		$('.message-box .close-btn').on('click', function(e) {
			$(this).parent('.message-box').fadeOut();
		});
	}

	//Accordion Box
	if($('.accordion-box').length){
		$(".accordion-box").on('click', '.acc-btn', function() {
			
			var outerBox = $(this).parents('.accordion-box');
			var target = $(this).parents('.accordion');
			
			if($(this).hasClass('active')!==true){
				$(outerBox).find('.accordion .acc-btn').removeClass('active ');
			}
			
			if ($(this).next('.acc-content').is(':visible')){
				return false;
			}else{
				$(this).addClass('active');
				$(outerBox).children('.accordion').removeClass('active-block');
				$(outerBox).find('.accordion').children('.acc-content').slideUp(300);
				target.addClass('active-block');
				$(this).next('.acc-content').slideDown(300);	
			}
		});	
	}

	//Fact Counter + Text Count
	if($('.count-box').length){
		$('.count-box').appear(function(){
	
			var $t = $(this),
				n = $t.find(".count-text").attr("data-stop"),
				r = parseInt($t.find(".count-text").attr("data-speed"), 10);
				
			if (!$t.hasClass("counted")) {
				$t.addClass("counted");
				$({
					countNum: $t.find(".count-text").text()
				}).animate({
					countNum: n
				}, {
					duration: r,
					easing: "linear",
					step: function() {
						$t.find(".count-text").text(Math.floor(this.countNum));
					},
					complete: function() {
						$t.find(".count-text").text(this.countNum);
					}
				});
			}
			
		},{accY: 0});
	}

	//Progress Bar
	if($('.progress-line').length){
		$('.progress-line').appear(function(){
			var el = $(this);
			var percent = el.data('width');
			$(el).css('width',percent+'%');
		},{accY: 0});
	}

	//Tabs Box
	if($('.tabs-box').length){
		$('.tabs-box .tab-buttons .tab-btn').on('click', function(e) {
			e.preventDefault();
			var target = $($(this).attr('data-tab'));
			
			if ($(target).is(':visible')){
				return false;
			}else{
				target.parents('.tabs-box').find('.tab-buttons').find('.tab-btn').removeClass('active-btn');
				$(this).addClass('active-btn');
				target.parents('.tabs-box').find('.tabs-content').find('.tab').fadeOut(0);
				target.parents('.tabs-box').find('.tabs-content').find('.tab').removeClass('active-tab animated fadeIn');
				$(target).fadeIn(300);
				$(target).addClass('active-tab animated fadeIn');
			}
		});
	}

	//LightBox / Fancybox
	if($('.lightbox-image').length) {
		$('.lightbox-image').fancybox({
			openEffect  : 'fade',
			closeEffect : 'fade',
			helpers : {
				media : {}
			}
		});
	}
	
	//Contact Form Validation
	if($('#email-form').length){
		$('#submit').click(function(){
			
            var o = new Object();
            var form = '#email-form';
			
			var username = $('#email-form .username').val();
			var email = $('#email-form .email').val();
			var subject = $('#email-form .subject').val();
			
			if(username == '' || email == '' || subject == '')
			{
				$('#email-form .response').html('<div class="failed">Please fill the required fields.</div>');
				return false;
			}
            
            $.ajax({
                url:"sendemail.php",
                method:"POST",
                data: $(form).serialize(),
                beforeSend:function(){
                    $('#email-form .response').html('<div class="text-info"><img src="images/icons/preloader.gif"> Loading...</div>');
                },
                success:function(data){
                    $('form').trigger("reset");
                    $('#email-form .response').fadeIn().html(data);
                    setTimeout(function(){
                        $('#email-form .response').fadeOut("slow");
                    }, 5000);
                },
                error:function(){
                    $('#email-form .response').fadeIn().html(data);
                }
            });
        });
	}

	// Scroll to a Specific Div
	if($('.scroll-to-target').length){
		$(".scroll-to-target").on('click', function() {
			var target = $(this).attr('data-target');
		   // animate
		   $('html, body').animate({
			   scrollTop: $(target).offset().top
			 }, 1500);
	
		});
	}

	// Elements Animation
	if($('.wow').length){
		var wow = new WOW(
		  {
			boxClass:     'wow',      // animated element css class (default is wow)
			animateClass: 'animated', // animation css class (default is animated)
			offset:       0,          // distance to the element when triggering the animation (default is 0)
			mobile:       false,       // trigger animations on mobile devices (default is true)
			live:         true       // act on asynchronously loaded content (default is true)
		  }
		);
		wow.init();
	}

	//Event Countdown Timer
	if($('.time-countdown').length){  
		$('.time-countdown').each(function() {
		var $this = $(this), finalDate = $(this).data('countdown');
		$this.countdown(finalDate, function(event) {
			var $this = $(this).html(event.strftime('' + '<div class="counter-column"><span class="count">%D</span>Days</div> ' + '<div class="counter-column"><span class="count">%H</span>Hours</div>  ' + '<div class="counter-column"><span class="count">%M</span>Minutes</div>  ' + '<div class="counter-column"><span class="count">%S</span>Seconds</div>'));
		});
	 });
	}

	if($('.clock-wrapper').length){  
		(function(){
            //generate clock animations
            var now       = new Date(),
                hourDeg   = now.getHours() / 12 * 360 + now.getMinutes() / 60 * 30,
                minuteDeg = now.getMinutes() / 60 * 360 + now.getSeconds() / 60 * 6,
                secondDeg = now.getSeconds() / 60 * 360,
                stylesDeg = [
                    "@-webkit-keyframes rotate-hour{from{transform:rotate(" + hourDeg + "deg);}to{transform:rotate(" + (hourDeg + 360) + "deg);}}",
                    "@-webkit-keyframes rotate-minute{from{transform:rotate(" + minuteDeg + "deg);}to{transform:rotate(" + (minuteDeg + 360) + "deg);}}",
                    "@-webkit-keyframes rotate-second{from{transform:rotate(" + secondDeg + "deg);}to{transform:rotate(" + (secondDeg + 360) + "deg);}}",
                    "@-moz-keyframes rotate-hour{from{transform:rotate(" + hourDeg + "deg);}to{transform:rotate(" + (hourDeg + 360) + "deg);}}",
                    "@-moz-keyframes rotate-minute{from{transform:rotate(" + minuteDeg + "deg);}to{transform:rotate(" + (minuteDeg + 360) + "deg);}}",
                    "@-moz-keyframes rotate-second{from{transform:rotate(" + secondDeg + "deg);}to{transform:rotate(" + (secondDeg + 360) + "deg);}}"
                ].join("");
            document.getElementById("clock-animations").innerHTML = stylesDeg;
        })();
    }



//Image Reveal Animation
	if($('.reveal').length){
		gsap.registerPlugin(ScrollTrigger);
		let revealContainers = document.querySelectorAll(".reveal");
		revealContainers.forEach((container) => {
			let image = container.querySelector("img");
			let tl = gsap.timeline({
				scrollTrigger: {
				trigger: container,
				toggleActions: "play none none none"
				}
			});
			tl.set(container, { autoAlpha: 1 });
			tl.from(container, 1.5, {
				xPercent: -100,
				ease: Power2.out
			});
			tl.from(image, 1.5, {
				xPercent: 100,
				scale: 1.3,
				delay: -1.5,
				ease: Power2.out
			});
		});
	}
	
	// Gsap Text Animations
	window.addEventListener("DOMContentLoaded", (event) => {
		// Split text into spans
		let typeSplit = new SplitType(".text-split", {
			types: "words, chars",
			tagName: "span"
		});

		// Link timelines to scroll position
		function createScrollTrigger(triggerElement, timeline) {
			// Reset tl when scroll out of view past bottom of screen
			ScrollTrigger.create({
				trigger: triggerElement,
				start: "top bottom",
				onLeaveBack: () => {
					timeline.progress(0);
					timeline.pause();
				}
			});
			// Play tl when scrolled into view (60% from top of screen)
			ScrollTrigger.create({
				trigger: triggerElement,
				start: "top 60%",
				onEnter: () => timeline.play()
			});
		}

		$(".words-slide-up").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.from($(this).find(".word"), { opacity: 0, yPercent: 100, duration: 0.5, ease: "back.out(2)", stagger: { amount: 0.5 } });
			createScrollTrigger($(this), tl);
		});

		$(".words-rotate-in").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.set($(this).find(".word"), { transformPerspective: 1000 });
			tl.from($(this).find(".word"), { rotationX: -90, duration: 0.6, ease: "power2.out", stagger: { amount: 0.6 } });
			createScrollTrigger($(this), tl);
		});

		$(".words-slide-from-right").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.from($(this).find(".word"), { opacity: 0, x: "1em", duration: 0.6, ease: "power2.out", stagger: { amount: 0.2 } });
			createScrollTrigger($(this), tl);
		});

		$(".letters-slide-up").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.from($(this).find(".char"), { yPercent: 100, duration: 0.2, ease: "power1.out", stagger: { amount: 0.6 } });
			createScrollTrigger($(this), tl);
		});

		$(".letters-slide-down").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.from($(this).find(".char"), { yPercent: -120, duration: 0.3, ease: "power1.out", stagger: { amount: 0.7 } });
			createScrollTrigger($(this), tl);
		});

		$(".letters-fade-in").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.from($(this).find(".char"), { opacity: 0, duration: 0.2, ease: "power1.out", stagger: { amount: 0.8 } });
			createScrollTrigger($(this), tl);
		});

		$(".letters-fade-in-random").each(function (index) {
			let tl = gsap.timeline({ paused: true });
			tl.from($(this).find(".char"), { opacity: 0, duration: 0.05, ease: "power1.out", stagger: { amount: 0.4, from: "random" } });
			createScrollTrigger($(this), tl);
		});

		$(".scrub-each-word").each(function (index) {
			let tl = gsap.timeline({
				scrollTrigger: {
					trigger: $(this),
					start: "top 90%",
					end: "top center",
					scrub: true
				}
			});
			tl.from($(this).find(".word"), { opacity: 0.2, duration: 0.2, ease: "power1.out", stagger: { each: 0.4 } });
		});

		// Avoid flash of unstyled content
		gsap.set(".text-split", { opacity: 1 });
	});
	
// Text Invert
	function initTextReveal() {
		const tagetedElementContainer =
			document.querySelectorAll(".text-reveal-anim");
		if (tagetedElementContainer?.length) {
			tagetedElementContainer.forEach(e => {
				var t = new SplitType(e, {
					types: "chars",
				});
				gsap.from(t.chars, {
					scrollTrigger: {
						trigger: e,
						start: "top 75%",
						end: "top 25%",
						scrub: !0,
						duration: 0.5
					},
					opacity: 0.1,
					stagger: 5,
					ease: "back.out",
				});
			});
		}
	}
	initTextReveal();


/* ==========================================================================
   When document is Scrollig, do
   ========================================================================== */
	
	$(window).on('scroll', function() {
		headerStyle();
	});
	
/* ==========================================================================
   When document is loading, do
   ========================================================================== */
	
	$(window).on('load', function() {
		handlePreloader();
		defaultMasonry();
	});	

})(window.jQuery);