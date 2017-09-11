
//点击图片获取验证码
$(document).on("click","#imgcode",function() {
    var date=new Date();
    $(this).attr("src","/Account/ValidCode/t="+date.getMilliseconds());
});
//登录事件
$(document).on("click","#userLogin",function() {
    var fromdata=$("#fromlogin").serialize();
    $.ajax({
        type: "post",
        dataType: "json",
        data: fromdata,
        async: false,
        url: "/Account/Login",
        success: function(data) {
            if(data.status===6) {
                window.location.href=data.msg;
                return;
            } else {
                alertError(data.msg);
                if(data.status===8||data.status===11) {
                    var date=new Date();
                    $("#imgcode").attr("src","/Account/ValidCode/t="+date.getMilliseconds());
                }
            }
        }
    });
});
//按下回车键
$(document).on("keydown",function(e) {
    if(e.which===13) {
        $("#userLogin").trigger("click");
    }
});

function alertError(msg) {
    var $err=$("#error");
    $err.find("li").first().text(msg).fadeIn("slow").fadeOut("slow");;
}