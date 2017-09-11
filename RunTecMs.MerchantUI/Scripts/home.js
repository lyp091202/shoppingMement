$(function() {
    menu.getSelectMenu();
});

//修改密码窗口
function WinUpdatePwd() {
    $("#w-updatepwd").window({
        iconCls: "icon-edit",
        closed: true,
        modal: true,
        width: 350,
        height : 300,
        collapsible: false,
        minimizable: false,
        maximizable: false,
        onClose: function() {
            $(this).find("#newpwd").textbox("clear");
            $(this).find("#confirmpwd").textbox("clear");
        }
    });

}

//退出登录事件
$(document).on("click", "#logout", function() {
    $.messager.confirm("提示", "您确定要退出系统吗?", function(r) {
        if(r) {
            location.href = "/Account/LogOut";
            return true;
        }
        return false;
    });
});

//修改密码事件
$(document).on("click", "#editpass", function () {
    $('#pwdFrame').attr('src', '/Home/UpdateEmployeePwd/');
    $('#updatePwd').dialog({
         modal : true,
         title : "修改密码",
         resizable : true,
         height: 300,
         width: 370,
    });
    //$("#w-updatepwd").window("open");
});

//修改密码确定事件
$(document).on("click", "#w-updateok", function() {
    var isValid = $("#form-updatepwd").form("validate");
    var newpwd = $("#newpwd").val();
    if(isValid) {
        $.ajax({
            type: "post",
            dataType: "json",
            data: { "newpwd": newpwd },
            url: "/Account/UpdatePwd/",
            async: false,
            success: function(data) {
                if(data.status === 4) {
                    parent.$('#updatePwd').dialog('close');
                    parent.$.messager.show({ title: "提示", msg: "密码修改成功,请重新登录" });
                    setTimeout(function() {
                        parent.location.href = data.msg;
                    }, 3000);
                }
                if(data.status === 7) {
                    $.messager.show({ title: "提示", msg: "登录超时,请重新登录" });
                    parent.location.href = data.msg;
                }
            },
            error: function(xmlHttpRequest, textStatus, errorThrown) {

            }
        });
    }
});

$(document).on("click","#accountInfo", function () {
     var $dialog = $("#accountInfoDialog");
        $dialog.dialog({
            title: "个人信息",
            width: 800,
            height: 350,
            cache: false, //禁用后就不会出现第二次点击添加弹出框自动验证表单错误 或用下main的 refreash方法也可
            modal: true, //是否显示遮罩层
            loadingMessage: '内容加载中...',
            onLoad: function () { },
            buttons: [
                {
                    text: '取消',
                    handler: function () {
                        $dialog.dialog('close');
                    }
                }
            ]
        });
        //打开弹出框
        $dialog.dialog('open');
        $dialog.panel("move",{ top: $(document).scrollTop()+($(window).height()-350)*0.5 });
});