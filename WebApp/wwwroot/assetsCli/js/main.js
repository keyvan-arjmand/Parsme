$(document).ready(function (e) {

    $('#SearchEl').on('input', function () {
        // Your function code here
        $.ajax({
            type: "Get",
            data: {search: $(this).val()},
            url: "/Search/SearchProd/",
            beforeSend: function () {
            },
            error: function (xhr) {
                Notiflix.Notify.failure('خطای داخلی');
            },
            complete: function () {
            },
            timeout: 40000,
            success: function (data) {
                let html = ``;
                if (data.length > 0) {
                    data.map(x => {
                        html += `  <li>
                                            <a href="/Home/ProductPage/${x.id}">
                                                <img src="https://newdev.parsme.com/Images/ProductImage/${x.imageUri}" width="30px">
                                                ${x.persianTitle}
                                                <a class="btn btn-light btn-continue-search" href="/Home/ProductPage/${x.id}">
                                                    <i class="fa fa-angle-left"></i>
                                                </a>
                                            </a>
                            </li>`;
                    })
                } else {
                    html += `  <li>
                                            <a href="">
                                              نتیجه ای یافت نشد
                                            </a>
                            </li>`;
                }


                document.getElementById("SearchResult").innerHTML = html;
            },
        });
    });


    //    hover-menu-overlay--------------------------
    $('li.nav-overlay').hover(function () {
        $('.mega-menu').removeClass('active');
        $('.nav-categories-overlay').addClass('active');
    }, function () {
        $('.nav-categories-overlay').removeClass('active');
    });

    //    resposive-megamenu-mobile------------------
    $('.dropdown-toggle').on('click', function (e) {
        e.stopPropagation();
        e.preventDefault();

        var self = $(this);
        if (self.is('.disabled, :disabled')) {
            return false;
        }
        self.parent().toggleClass("open");
    });

    $(document).on('click', function (e) {
        if ($('.dropdown').hasClass('open')) {
            $('.dropdown').removeClass('open');
        }
    });

    $('.nav-btn.nav-slider').on('click', function () {
        $('.overlay').show();
        $('nav').toggleClass("open");
    });

    $('.overlay').on('click', function () {
        if ($('nav').hasClass('open')) {
            $('nav').removeClass('open');
        }
        $(this).hide();
    });


    $('li.active').addClass('open').children('ul').show();
    $("li.has-sub > a").on('click', function () {
        $(this).removeAttr('href');
        var e = $(this).parent('li');
        if (e.hasClass('open')) {
            e.removeClass('open');
            e.find('li').removeClass('opne');
            e.find('ul').slideUp(200);
        } else {
            e.addClass('open');
            e.children('ul').slideDown(200);
            e.siblings('li').children('ul').slideUp(200);
            e.siblings('li').removeClass('open');
            e.siblings('li').find('li').removeClass('open');
            e.siblings('li').find('ul').slideUp(200);
        }
    });
    //    resposive-megamenu-mobile------------------

    // searchResult--------------------------------------
    $('.header-search .header-search-box .form-search .header-search-input').on('click', function () {
        $(this).parents('.header-search').addClass('show-result').find('.search-result').fadeIn();
        $(".overlay-search-box").css({"opacity": "1", "visibility": "visible"});
    })
    $(document).click(function (e) {
        if ($(e.target).is('.header-search *')) return;
        $('.search-result').hide();
        $(".overlay-search-box").css({"opacity": "0", "visibility": "hidden"});
    });
    // searchResult--------------------------------------

    // slider-product------------------------
    $(".offer-carousel").owlCarousel({
        rtl: true,
        margin: 10,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        dots: false,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1.1,
                slideBy: 1
            },
            576: {
                items: 2.2,
                slideBy: 1
            },
            768: {
                items: 3.3,
                slideBy: 2
            },
            992: {
                items: 3.3,
                slideBy: 2
            },
            1400: {
                items: 4.4,
                slideBy: 3
            }
        }
    });

    $(".product-carousel").owlCarousel({
        rtl: true,
        margin: 10,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        dots: false,
        responsiveClass: true,
        fluidSpeed: true,
        autoplay: true,
        autoplayTimeout: 6000,
        autoplayHoverPause: true,
        smartSpeed: 800,
        touchDrag: true,
        mouseDrag: true,
        pullDrag: true,
        freeDrag: false,
        loop: true,
        responsive: {
            0: {
                items: 1.4,
            },
            576: {
                items: 2.2,
            },
            768: {
                items: 2.7,
            },
            992: {
                items: 3.4,
            },
            1400: {
                items: 4.2,
            }
        }
    });


    // brand---------------------------------------
    $(".product-carousel-brand").owlCarousel({
        items: 4,
        rtl: true,
        margin: 10,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        dots: false,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
            },
            576: {
                items: 1,
            },
            768: {
                items: 3,
            },
            992: {
                items: 5,
                slideBy: 2
            },
            1400: {
                items: 5,
                slideBy: 3
            }
        }
    });
    // brand---------------------------------------

    // Symbol--------------------------------------
    $(".product-carousel-symbol").owlCarousel({
        rtl: true,
        items: 2,
        loop: true,
        margin: 10,
        dots: false,
        autoplay: true,
        autoplayTimeout: 3000,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        autoplayHoverPause: true,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                slideBy: 1,
                autoplay: true,
            },
            576: {
                items: 1,
                slideBy: 1,
                autoplay: true,
            },
            768: {
                items: 1,
                slideBy: 1,
                autoplay: true,
            },
            992: {
                items: 1,
                slideBy: 1,
                autoplay: true,
            },
            1400: {
                items: 1,
                slideBy: 1,
                autoplay: true,
            }
        }
    });
    // Symbol--------------------------------------

    $("#suggestion-slider").owlCarousel({
        rtl: true,
        items: 1,
        autoplay: true,
        autoplayTimeout: 5000,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        loop: true,
        dots: false,
        onInitialized: startProgressBar,
        onTranslate: resetProgressBar,
        onTranslated: startProgressBar
    });

    function startProgressBar() {
        $(".slide-progress").css({
            width: "100%",
            transition: "width 5000ms"
        });
    }

    function resetProgressBar() {
        $(".slide-progress").css({
            width: 0,
            transition: "width 0s"
        });
    }

    // product-more
    $(".product-carousel-more").owlCarousel({
        rtl: true,
        autoplay: true,
        autoplayTimeout: 5000,
        margin: 10,
        nav: true,
        navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
        dots: true,
        loop: true,
        responsiveClass: true,
        responsive: {
            0: {
                items: 1,
                slideBy: 1
            },
            576: {
                items: 1,
                slideBy: 1
            },
            768: {
                items: 1,
                slideBy: 2
            },
            992: {
                items: 1,
                slideBy: 2
            },
            1400: {
                items: 1,
                slideBy: 3
            }
        }
    });

    // advantages-----------------------------
    var inputs = $('#advantage-input, #disadvantage-input');
    var inputChangeCallback = function () {
        var self = $(this);
        if (self.val().trim().length > 0) {
            self.siblings('.js-icon-form-add').show();
        } else {
            self.siblings('.js-icon-form-add').hide();
        }
    };
    inputs.each(function () {
        inputChangeCallback.bind(this)();
        $(this).on('change keyup', inputChangeCallback.bind(this));
    });
    $("#advantages").delegate(".js-icon-form-add", 'click', function (e) {

        var parent = $('.js-advantages-list');
        if (parent.find(".js-advantage-item").length >= 5) {
            return;
        }

        var advantageInput = $('#advantage-input');

        if (advantageInput.val().trim().length > 0) {
            parent.append(
                '<div class="ui-dynamic-label ui-dynamic-label--positive js-advantage-item">\n' +
                advantageInput.val() +
                '<button type="button" class="ui-dynamic-label-remove js-icon-form-remove"></button>\n' +
                '<input type="hidden" name="comment[advantages][]" value="' + advantageInput
                    .val() + '">\n' +
                '</div>');

            advantageInput.val('').change();
            advantageInput.focus();
        }

    }).delegate(".js-icon-form-remove", 'click', function (e) {
        $(this).parent('.js-advantage-item').remove();
    });

    $("#disadvantages").delegate(".js-icon-form-add", 'click', function (e) {

        var parent = $('.js-disadvantages-list');
        if (parent.find(".js-disadvantage-item").length >= 5) {
            return;
        }

        var disadvantageInput = $('#disadvantage-input');

        if (disadvantageInput.val().trim().length > 0) {
            parent.append(
                '<div class="ui-dynamic-label ui-dynamic-label--negative js-disadvantage-item">\n' +
                disadvantageInput.val() +
                '<button type="button" class="ui-dynamic-label-remove js-icon-form-remove"></button>\n' +
                '<input type="hidden" name="comment[disadvantages][]" value="' +
                disadvantageInput.val() + '">\n' +
                '</div>');

            disadvantageInput.val('').change();
            disadvantageInput.focus();
        }

    }).delegate(".js-icon-form-remove", 'click', function (e) {
        $(this).parent('.js-disadvantage-item').remove();
    });
    // advantages-----------------------------

    // sidebar-sticky-------------------------
    if ($('.sticky-sidebar').length) {
        $('.sticky-sidebar').theiaStickySidebar();
    }

    !function (l) {
        var t = {
            init: function () {
                t.countDown()
            },
            countDown: function () {
                l(".countdown").each(function () {
                    var t = l(this),
                        a = l(this).data("date-time"),
                        e = l(this).data("labels");

                    // مقدار نهایی زمان به میلی‌ثانیه
                    var targetTime = new Date(a).getTime();
                    setInterval(function () {
                        var now = new Date().getTime();
                        var distance = targetTime - now;

                        if (distance > 0) {
                            var totalHours = Math.floor(distance / (1000 * 60 * 60));
                            var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                            var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                            // به فرمت HH:MM:SS نمایش داده می‌شود
                            var timeFormatted =
                                String(totalHours).padStart(2, '0') + ":" +
                                String(minutes).padStart(2, '0') + ":" +
                                String(seconds).padStart(2, '0');

                            t.html(timeFormatted); // نمایش زمان به شکل HH:MM:SS
                        }
                        // else {
                        //     t.html('<div class="expired-message">زمان تمام شد!</div>');
                        // }
                    }, 1000);
                });
            },
        };
        l(function () {
            t.init()
        });
    }(jQuery);


// برای نمونه شمارش معکوس با یک سال جدید
    const cd = new Date().getFullYear() + 1;
    $('#countdown').countdown({
        year: cd
    });


    // checkout-coupon-------------------------------
    $(".showcoupon").on("click", function () {
        $(".checkout-coupon").slideToggle(200);
    });
    // checkout-coupon-------------------------------

    // add-product-wishes----------------------------
    $(".add-product-wishes").on("click", function () {
        $(this).toggleClass("active");
    });
    // add-product-wishes----------------------------
    // SweetAlert -----------------------------------
    // cart-item-close
    $('.mini-cart-item-close').on('click', function () {
        Swal.fire({
            text: "آیا مطمئن هستید حذف شود؟",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'بله',
            cancelButtonText: 'خیر'
        }).then((result) => {
            if (result.isConfirmed) {
                Swal.fire({
                    title: 'حذف شد!',
                    confirmButtonText: 'باشه',
                    icon: 'success'
                })
            }
        })
    });


    // add-to-cart
    // $('.btn-add-to-cart').on('click', function (event) {
    //     event.preventDefault();
    //     const Toast = Swal.mixin({
    //         toast: true,
    //         position: 'top-end',
    //         showConfirmButton: false,
    //         timer: 2000,
    //         didOpen: (toast) => {
    //             toast.addEventListener('mouseenter', Swal.stopTimer)
    //             toast.addEventListener('mouseleave', Swal.resumeTimer)
    //         }
    //     })
    //
    //     Toast.fire({
    //         icon: 'success',
    //         title: 'به سبد خرید خود اضافه شد'
    //     })
    // });
    // compare
    $('.btn-compare').on('click', function (event) {
        event.preventDefault();
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'success',
            title: 'محصول برای مقایسه اضافه شد'
        })
    });
    // wishes 
    $('.add-product-wishes').on('click', function (event) {
        event.preventDefault();
        const Toast = Swal.mixin({
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 2000,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })

        Toast.fire({
            icon: 'success',
            title: 'به لیست علاقه مندی خود اضافه شد'
        })
    });
    // SweetAlert -----------------------------------
    // nice-select-----------------------------------
    if ($('.custom-select-ui').length) {
        $('.custom-select-ui select').niceSelect();
    }
    // nice-select-----------------------------------
    //    price-range--------------------------------
    var nonLinearStepSlider = document.getElementById('slider-non-linear-step');

    if ($('#slider-non-linear-step').length) {
        noUiSlider.create(nonLinearStepSlider, {
            start: [0, 5000000],
            connect: true,
            direction: 'rtl',
            format: wNumb({
                decimals: 0,
                thousand: ','
            }),
            range: {
                'min': [0],
                '10%': [3000, 3000],
                '50%': [80000, 4000],
                'max': [100000000]
            }
        });
        var nonLinearStepSliderValueElement = document.getElementById('slider-non-linear-step-value');

        nonLinearStepSlider.noUiSlider.on('update', function (values) {
            nonLinearStepSliderValueElement.innerHTML = values.join(' - ');
        });
    }
    //    price-range--------------------------

    //    quantity-selector--------------------
    jQuery('<div class="quantity-nav"><div class="quantity-button quantity-up">+</div><div class="quantity-button quantity-down">-</div></div>').insertAfter('.quantity input');

    jQuery('.quantity').each(function () {
        var spinner = jQuery(this),
            input = spinner.find('input[type="number"]'),
            btnUp = spinner.find('.quantity-up'),
            btnDown = spinner.find('.quantity-down'),
            min = parseFloat(input.attr('min')) || 0,
            max = parseFloat(input.attr('max')) || Infinity;

        // جلوگیری از رفتار پیش‌فرض مرورگر در ورودی عددی
        input.on('wheel keydown', function (e) {
            e.preventDefault();
        });

        // رویداد کلیک دکمه افزایش
        btnUp.off('click').on('click', function (e) {
            e.preventDefault(); // جلوگیری از رفتار پیش‌فرض
            var oldValue = parseFloat(input.val()) || 0; // مقدار فعلی
            var newVal = (oldValue >= max) ? oldValue : oldValue + 1; // مقدار جدید
            input.val(newVal).trigger("change"); // تنظیم مقدار جدید و فعال کردن رویداد
        });

        // رویداد کلیک دکمه کاهش
        btnDown.off('click').on('click', function (e) {
            e.preventDefault(); // جلوگیری از رفتار پیش‌فرض
            var oldValue = parseFloat(input.val()) || 0; // مقدار فعلی
            var newVal = (oldValue <= min) ? oldValue : oldValue - 1; // مقدار جدید
            input.val(newVal).trigger("change"); // تنظیم مقدار جدید و فعال کردن رویداد
        });
    });


    //    quantity-selector-------------------

    // Page Loader----------------------------
    // var preloader = $('.P-loader');
    // $(window).on("load", function () {
    //     var preloaderFadeOutTime = 500;
    //     function hidePreloader() {
    //         preloader.fadeOut(preloaderFadeOutTime);
    //     }
    //     hidePreloader();
    // });
    $(".P-loader").fadeOut(2000, "swing");
    // Page Loader----------------------------

    // scroll_progress-------------------------
    // var progressPath = document.querySelector('.progress-wrap path');
    // var pathLength = progressPath.getTotalLength();
    // progressPath.style.transition = progressPath.style.WebkitTransition = 'none';
    // progressPath.style.strokeDasharray = pathLength + ' ' + pathLength;
    // progressPath.style.strokeDashoffset = pathLength;
    // progressPath.getBoundingClientRect();
    // progressPath.style.transition = progressPath.style.WebkitTransition = 'stroke-dashoffset 10ms linear';
    // var updateProgress = function () {
    //     var scroll = $(window).scrollTop();
    //     var height = $(document).height() - $(window).height();
    //     var progress = pathLength - (scroll * pathLength / height);
    //     progressPath.style.strokeDashoffset = progress;
    // }
    // updateProgress();
    // $(window).scroll(updateProgress);
    // var offset = 50;
    // var duration = 1500;
    // jQuery(window).on('scroll', function () {
    //     if (jQuery(this).scrollTop() > offset) {
    //         jQuery('.progress-wrap').addClass('active-progress');
    //     } else {
    //         jQuery('.progress-wrap').removeClass('active-progress');
    //     }
    // });
    // jQuery('.progress-wrap').on('click', function (event) {
    //     event.preventDefault();
    //     jQuery('html, body').animate({ scrollTop: 0 }, duration);
    //     return false;
    // });


    if ($("#countdown-verify-end2").length) {
        var $countdownOptionEnd = $("#countdown-verify-end2");
        $countdownOptionEnd.countdown({
            date: (new Date()).getTime() + 180 * 1000, // 1 minute later
            text: '<span style="color: #FE0002">مانده تا دریافت مجدد کد</span>',
            end: function () {
                $countdownOptionEnd.html(" ");
            }
        });
    }
    //    verify-phone-number------------------------
    if ($("#countdown-verify-end").length) {
        var $countdownOptionEnd = $("#countdown-verify-end");

        $countdownOptionEnd.countdown({
            date: (new Date()).getTime() + 180 * 1000, // 1 minute later
            text: ' <span class="day">%s</span><span class="hour">%s</span><span>: %s</span><span>%s</span> ',
            end: function () {
                $countdownOptionEnd.html("<a onclick='ResendCode()' class='link-border-verify'>ارسال مجدد</a>");
            }
        });
    }
    $(".line-number-account").keyup(function (e) {
        // بررسی اینکه آیا کاربر یک عدد وارد کرده است
        if (e.key >= '0' && e.key <= '9') {
            $(this).val(e.key);  // وارد کردن عدد در فیلد
            $(this).next().focus();  // رفتن به فیلد بعدی
        } else if (e.key === "Backspace" || e.key === "Delete") {
            $(this).val('');  // پاک کردن مقدار فعلی
            $(this).prev().focus();  // رفتن به فیلد قبلی
        }
    });
    $(".line-number-account").on("keydown", function (e) {
        // اگر کلید زده شده غیر از اعداد 0-9 باشد، جلوگیری می‌کند
        if (e.key < '0' || e.key > '9') {
            e.preventDefault();
        }
    });
    //    verify-phone-number-----------------------

    // tab-------------------------------------
    $(".mask-handler").onclick(function (e) {
        e.preventDefault();
        var sumaryBox = $(this).parents('.content-expert-summary');
        sumaryBox.find('.mask-text').toggleClass('active');
        sumaryBox.find('.shadow-box').fadeToggle(0);
        $(this).find('.show-more').fadeToggle(0);
        $(this).find('.show-less').fadeToggle(0);
    });

    $(".content-expert-button").click(function (e) {
        e.preventDefault();
        var sumaryBox = $(this).parents('.content-expert-article');
        sumaryBox.find('.content-expert-article').toggleClass('active');
        sumaryBox.find('.content-expert-text').slideToggle();
        $(this).find('.show-more').fadeToggle(0);
        $(this).find('.show-less').fadeToggle(0);
    });
    // tab-------------------------------------


    // product-img-----------------------------
    // $("#gallery-slider").owlCarousel({
    //     rtl: true,
    //     margin: 10,
    //     nav: true,
    //     navText: ['<i class="fa fa-angle-right"></i>', '<i class="fa fa-angle-left"></i>'],
    //     dots: false,
    //     responsiveClass: true,
    //     responsive: {
    //         0: {
    //             items: 4,
    //             slideBy: 1
    //         }
    //     }
    // });

    $('.back-to-top').click(function (e) {
        e.preventDefault();
        $('html, body').animate({scrollTop: 0}, 800, 'easeInExpo');
    });

    // if ($("#img-product-zoom").length) {
    //     $("#img-product-zoom").ezPlus({
    //         zoomType: "inner",
    //         containLensZoom: true,
    //         gallery: 'gallery_01f',
    //         cursor: "crosshair",
    //         galleryActiveClass: "active",
    //         responsive: true,
    //         imageCrossfade: true,
    //         zoomWindowFadeIn: 500,
    //         zoomWindowFadeOut: 500
    //     });
    // }

    //zoomgallerymodal---------------------------
    $(function () {
        $(".zoom-box img").jqZoom({
            selectorWidth: 30,
            selectorHeight: 30,
            viewerWidth: 400,
            viewerHeight: 300
        });
    });

    var $customEvents = $('#custom-events');
    $customEvents.lightGallery();

    var colours = ['#21171A', '#81575E', '#9C5043', '#8F655D'];
    $customEvents.on('onBeforeSlide.lg', function (event, prevIndex, index) {
        $('.lg-outer').css('background-color', colours[index])
    });
    // product-img-----------------------------

    window.onscroll = function () {
        stickyNavbar();
    };
    var navbar = document.querySelector('.header-main-nav');
    var sticky = navbar.offsetTop;

    function stickyNavbar() {

        if (window.pageYOffset > sticky) {
            navbar.classList.add("sticky-nav");
        } else {
            navbar.classList.remove("sticky-nav");
        }
    }
});