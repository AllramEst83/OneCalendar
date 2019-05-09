
var Accordion = {
    BidnAccordion: function () {
        $('.accordion .item .heading').click(function () {

            var a = $(this).closest('.item');
            var b = $(a).hasClass('open');
            var c = $(a).closest('.accordion').find('.open');

            if (b != true) {
                $(c).find('.content').slideUp(200);
                $(c).removeClass('open');
            }

            $(a).toggleClass('open');
            $(a).find('.content').slideToggle(200);

        });
    }
};