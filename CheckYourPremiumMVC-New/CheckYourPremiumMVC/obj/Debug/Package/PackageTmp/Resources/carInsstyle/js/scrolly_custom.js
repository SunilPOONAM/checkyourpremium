$(document).ready(function() {
    $("a").on('click', function(event) {
        if (this.hash !== "") {
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
            }, 800, function() {
                window.location.hash = hash;
            });
        }
    });



    $("html").niceScroll({
        cursorcolor: "#FFF",
        cursorborder: "0",
    });

    var isVisible = false;
    $('.scroll-to-top').click(function() {
        $(window).scrollTop(0);
    });
    $(window).scroll(function() {
        var shouldBeVisible = $(window).scrollTop() > 200;
        if (shouldBeVisible && !isVisible) {
            isVisible = true;
            $('.scroll-to-top').show();
        } else if (isVisible && !shouldBeVisible) {
            isVisible = false;
            $('.scroll-to-top').hide();
        }
    });


});