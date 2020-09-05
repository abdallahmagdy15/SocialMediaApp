$(window).on('load', function () {
    //function refresh() {
    //    var conID = $(".chatMessages").attr("conID");

    //    setInterval(function () { $(".conLink." + conID).click(); }, 2000);
    //}
    $(".conLink > div.notif").each(function () {
        if ($(this).text() == "0") {
            $(this).empty();
            $(this).removeClass("activenotif");
        }
        else {
            $(this).addClass("activenotif");
        }
    });

    function scrollchat() {
        $(".chatHistory").animate({ scrollTop: $(this).height() }, 200);
    }
    function hideEmptyMsg() {
        $(".chatMessages p").each(function () {
            if ($(this).text() == "")
                $(this).css("padding", "0");
        });
    }

    $(".conLink").click(
    function () {
        var conId = $(this).attr('conId');
        var chathead = $(this).text();
        console.log($(".chatMessages").attr("conID"));
        $.ajax({
            url: '/messenger/loadChat?conID=' + conId,
            dataType: 'html',
            success: function (response) {
                $('.chatHistory').empty();
                $('.chatHistory').append(response);
                $(".conLink." + conId + " > div.notif").empty();
                $(".chatHead").text(chathead);
                scrollchat();
                hideEmptyMsg();
                $(".activeConv").removeClass("activeConv");
                $(".conLink." + conId).addClass("activeConv");
                console.log("loaded");
                $(".msg").click(function () {
                    console.log('clicked');
                    if ($(this).closest('li').find('label').css('display') == "none")
                        $(this).closest('li').find('label').css('display', 'block');
                    else
                        $(this).closest('li').find('label').css('display', 'none');
                });
            },
            error: function () {
                console.log("error");
            }
        });
    });
    $(".conLink").first().click();

    //____________________________________
    if ($("ul").attr("id") == 1)
        $("ul").addClass("starter");

    else
        $("ul").addClass("notstarter");
    //____________________________________

    //____________________________________
    $(".sendBtn").click(
        function () {
            var params = "m=" + $(".messageInput").val() + "&conID=" + $(".chatMessages").attr("conID");
            $.ajax({
                type: "post",
                url: '/messenger/sendMessage?' + params,
                success: function (response) {
                    console.log("sent!");
                }
            });
            var conID = $(".chatMessages").attr("conID");
            $(".conLink." + conID).click();
            $(".messageInput").val("");
        }
        );
    //__________________________________
    //____________________________________
});
