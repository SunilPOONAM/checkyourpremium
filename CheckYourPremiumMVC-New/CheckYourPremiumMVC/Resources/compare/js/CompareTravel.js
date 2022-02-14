/// <reference path="jquery-1.12.3.js" />

(function ($) {
    var list = [];

    /* function to be executed when product is selected for comparision*/

    $(document).on('click', '.addToCompare', function () {
        $(".comparePanle").show();
        $(this).toggleClass("rotateBtn");
        $(this).parents(".selectProduct").toggleClass("selected");
        var PremiumDesc = $(this).parents('.selectProduct').attr('data-id');
        //  var Premiumtitle = $(this).parents('.selectProduct').attr('data-title');
        var inArray = $.inArray(PremiumDesc, list);
        if (inArray < 0) {
            if (list.length > 2) {
                $("#WarningModal").show();
                $("#warningModalClose").click(function () {
                    $("#WarningModal").hide();
                });
                $(this).toggleClass("rotateBtn");
                $(this).parents(".selectProduct").toggleClass("selected");
                return;
            }

            if (list.length < 3) {
                list.push(PremiumDesc);

                var displayTitle = $(this).parents('.selectProduct').attr('data-title');

                var image = $(this).siblings(".productImg").attr('src');

                $(".comparePan").append('<div id="' + PremiumDesc + '" class="relPos titleMargin w3-margin-bottom"><div class="w3-white titleMargin"><a class="selectedItemCloseBtn w3-closebtn cursor">&times</a><img src="' + image + '" alt="image"/><p id="' + PremiumDesc + '" class="titleMargin1">' + PremiumDesc + '</p></div></div>');
            }
        } else {
            list.splice($.inArray(PremiumDesc, list), 1);
            var prod = PremiumDesc.replace(" ", "");
            $('#' + prod).remove();
            hideComparePanel();

        }
        if (list.length > 1) {

            $(".cmprBtn").addClass("active");
            $(".cmprBtn").removeAttr('disabled');
        } else {
            $(".cmprBtn").removeClass("active");
            $(".cmprBtn").attr('disabled', '');
        }

    });
    /*function to be executed when compare button is clicked*/
    $(document).on('click', '.cmprBtn', function () {
        if ($(".cmprBtn").hasClass("active")) {
            /* this is to print the  features list statically*/
            $(".contentPop").append('<div class="col-md-3 dlwefjlerj">' + '<ul class="product">' + '<li class="compHeaderd"><p class="w3-display-middleg">Features</p></li>' + '<li>Total Premium</li> ' + '<li>Benefit</li>' + '<li>Co_Pay1</li>' + '<li>Room_Rent</li>' + '<li>OPD</li>' + '<li>Day_Care_Treatment</li>' + '<li>Medical_Checkup</li>' + '<li>Pre_Existing_Disease Covered After</li>' + '<li>Domicilliary_Expenses</li>' + '<li>Organ_Donar_Expenses</li>' + '<li>Hospital_Cash_Daily_Limit</li>' + '<li>Maternity_Benefit</li>' + '<li>New_Born_Baby</li>' + '<li>Pre_Hospitalization1</li>' + '<li>Post_Hospitalization1</li>' + '<li>Ambulance_Charges</li>' + '<li>Health_Check_Up</li>' + '<li>Restoration_Benefit1</li>' + '<li>Free_Look_Period1</li></ul>' + '</div>');

            // $(".contentPop").append('<div class="w3-col s3 m3 l3 compareItemParent relPos">' + '</div>');



            for (var i = 0; i < list.length; i++) {
                /* this is to add the items to popup which are selected for comparision */
                product = $('.selectProduct[data-id="' + list[i] + '"]');
                var image = $('[data-id=' + list[i] + ']').find(".productImg").attr('src');

                var title = $('[data-id=' + list[i] + ']').attr('data-id');
                //   Compare      
                var com1 = $('[data-id=' + list[i] + ']').find(".compare1").val();
                var com2 = $('[data-id=' + list[i] + ']').find(".compare2").val();
                var com3 = $('[data-id=' + list[i] + ']').find(".compare3").val();
                var com4 = $('[data-id=' + list[i] + ']').find(".compare4").val();
                var com5 = $('[data-id=' + list[i] + ']').find(".compare5").val();
                var com6 = $('[data-id=' + list[i] + ']').find(".compare6").val();
                var com7 = $('[data-id=' + list[i] + ']').find(".compare7").val();
                var com8 = $('[data-id=' + list[i] + ']').find(".compare8").val();
                var com9 = $('[data-id=' + list[i] + ']').find(".compare9").val();
                var com10 = $('[data-id=' + list[i] + ']').find(".compare10").val();
                var com11 = $('[data-id=' + list[i] + ']').find(".compare11").val();
                var com12 = $('[data-id=' + list[i] + ']').find(".compare12").val();
                var com13 = $('[data-id=' + list[i] + ']').find(".compare13").val();
                var com14 = $('[data-id=' + list[i] + ']').find(".compare14").val();
                var com15 = $('[data-id=' + list[i] + ']').find(".compare15").val();
                var com16 = $('[data-id=' + list[i] + ']').find(".compare16").val();
                var com17 = $('[data-id=' + list[i] + ']').find(".compare17").val();
                var com18 = $('[data-id=' + list[i] + ']').find(".compare18").val();
                var button = $('[data-id=' + list[i] + ']').find(".AddD").text();
                var premiumid = $('[data-id=' + list[i] + ']').find(".PremiumChart").val();
                var premiumtotal = $('[data-id=' + list[i] + ']').find(".Premiumtotal").val();
                /*appending to div*/
                $(".contentPop").append('<div class="col-md-3 compareItemParent prodetls">' + '<ul class="product">' + '<li class="compHeader"><img src="' + image + '" class="compareThumb"></li>' + '<li>' + premiumtotal + '</li>' + '<li>' + com1 + '</li>' + '<li>' + com2 + '<li>' + com3 + '</li>' + '<li>' + com4 + '</li>' + '<li>' + com5 + '</li>' + '<li>' + com6 + '</li>' + '<li>' + com7 + '</li>' + '<li>' + com8 + '</li>' + '<li>' + com9 + '</li>' + '<li>' + com10 + '</li>' + '<li>' + com11 + '</li>' + '<li>' + com12 + '</li>' + '<li>' + com13 + '</li>' + '<li>' + com14 + '</li>' + '<li>' + com15 + '</li>' + '<li>' + com16 + '</li>' + '<li>' + com17 + '</li>' + '<li>' + com18 + '</li><a class="AddD" href="#" onclick="NavigateDetail(' + premiumid + ')">' + button + '</a></li></ul>' + '</div>');
                // $(".contentPop").append('<div class="col-md-3 compareItemParent prodetls">' + '<ul class="product">' + '<li class="compHeader"><img src="' + image + '" class="compareThumb"></li>' + '<li>' + premiumtotal + '</li>' + '<li>Jhgsdhf sf sdvs vdfb</li>' + '<li>Hkshd kjghjr ergj lkjhkt</li>' + '<li>Phfwjf fej gelkgr grkjg</li>' + '<li>Oeet wejf fkejhf eg</li>' + '<li>Khwkjf fjkf fefhj</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li>' + '<li>Trgh djwhd dwf fkjf</li><a class="AddD" href="#" onclick="NavigateDetail(' + premiumid + ')">' + button + '</a></li></ul>' + '</div>');


            }
        }
        $(".modPos").show();
    });

    /* function to close the comparision popup */
    $(document).on('click', '.closeBtn', function () {
        $(".contentPop").empty();
        $(".comparePan").empty();
        $(".comparePanle").hide();
        $(".modPos").hide();
        $(".selectProduct").removeClass("selected");
        $(".cmprBtn").attr('disabled', '');
        list.length = 0;
        $(".rotateBtn").toggleClass("rotateBtn");
    });

    /*function to remove item from preview panel*/
    $(document).on('click', '.selectedItemCloseBtn', function () {

        var test = $(this).siblings("p").attr('id');
        $('[data-id=' + test + ']').find(".addToCompare").click();
        hideComparePanel();
    });

    function hideComparePanel() {
        if (!list.length) {
            $(".comparePan").empty();
            $(".comparePanle").hide();
        }
    }
})(jQuery);