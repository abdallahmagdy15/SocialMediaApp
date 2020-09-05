
//var _url , successM, errorM;
//var data;
//var userid = document.getElementById("userDataDiv").getAttribute("userid");

//function createPost(_postContent) {
//    _url = 'Home/createPost';
//    data = {
//        "postContent": _postContent,
//        "userid" : userid
//    }
//    successM = "post submitted!";
//    errorM = "error in submittiong post!";
//    callAjax();
//}

//function callAjax() {
//    $.ajax({
//        url: _url,
//        data: data,
//        success: function (response) {
//            console.log(successM);
//        },
//        error: function () {
//            console.log(errorM);
//        }
//    });
//};
$(".commentsWrapper").hide();
function toggleComments(postid) {
    $("#" + postid + " .commentsWrapper").toggle("normal");
}

function toggleEdit(b , p , s) {
    var content = $(b).parents(p).siblings(s);
    content.find("form").css("display", "block");
    content.find("p").css("display", "none");
}
$(".editPostBtn").each(function () {
    $(this).on("click", function () {
        toggleEdit(this, ".modifyPost", ".postContent");
    });
});
    
$(".editCommentBtn").each(function () {
    $(this).on("click", function(){
        toggleEdit(this, ".modifyPost", ".commentContent");
    });
});
$(".editReplyBtn").each(function () {
    $(this).on("click", function () {
        toggleEdit(this, ".modifyPost", ".replyContent");
    });
});

$(".editReplyBtn").each(function () {
    $(this).on("click", function () {
        toggleEdit(this, ".editReply", ".replyContent");
    });
});
$("#uploadPostMedia").change(function () {
    var file =$("#uploadPostMedia").val();
    $("#chosedFile").text(file.substring( file.lastIndexOf("\\")+1));
});
$(".createPost textarea").focus(function () {
    $(".createPostFocusBG").toggleClass("active");
});

$(".createPost textarea").focusout(function () {
    $(".createPostFocusBG").toggleClass("active");
});
$(".postOptions").click(function () {
    $(this).next(".modifyPost").toggleClass("active");
});
$(".replyComment a").click(function(){
    $(this).parent(".replyComment").next(".repliesWrapper").find(".createReply").css("display", "block");
});
$("#uploadBG").on("change",function(){
    $(".submitUploadBG").css("display", "inline-block");
});
$("#uploadDP").on("change", function () {
    $(".submitUploadDP").click();
});
if ($(".profileStatus span").html() == "none") {
    $(".profileStatus").css("display", "none");
}