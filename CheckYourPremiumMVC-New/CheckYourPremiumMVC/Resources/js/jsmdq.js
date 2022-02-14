// media query event handler
if (matchMedia) {
    const mq = window.matchMedia("(max-width:667px)");
    mq.addListener(WidthChange);
    WidthChange(mq);
}

// media query change
function WidthChange(mq) {
    if (mq.matches) {
        $('.mhasjc').css('top', '65px');
    } else {
        $('.mhasjc').css('top', '65px');
    }

}

