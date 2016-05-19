
$(document).ready(function () {
    $("#input-password").keydown(function (e) {
        if (e.which == 13) {
            User.Login();
        }
    });

    $("#imgCode").click(function () {
        $("#imgCode").attr("src", "/Common/Val");
    });

    User.Ad();
});

var User = {
    Login: function () {
        var userName = $("#input-username").val();
        var passWord = $("#input-password").val();
        var code = $("#txtCode").val();
        if (git.IsEmpty(userName)) {
            $.jBox.tip("请输入密码", "warn");
            return false;
        }
        if (git.IsEmpty(passWord)) {
            $.jBox.tip("请输入密码", "warn");
            return false;
        }
        //if (git.IsEmpty(code)) {
        //    $.jBox.tip("请输入4位验证码", "warn");
        //    return false;
        //}
        var param = {};
        param["userName"] = userName;
        param["passWord"] = passWord;
        param["code"] = code;
        $.ajax({
            url: "/UserAjax/Login?t=" + Math.random(),
            data: param,
            type: "post",
            success: function (msg) {
                if (msg == "1000") {
                    var url = $("#hdUrl").val();
                    if (url == undefined || url == "") {
                        window.location.href = "/Home/Welcome";
                    } else {
                        window.location.href = url;
                    }
                } else if (msg == "1002") {
                    $.jBox.tip("验证码已经过期,请重新输入", "warn");
                    $("#imgCode").attr("imgCode", "/Common/Val");
                    return false;
                }
                else if (msg == "1003") {
                    $.jBox.tip("验证码错误,请重新输入", "warn");
                    $("#imgCode").attr("src", "/Common/Val");
                    return false;
                }
                else {
                    $.jBox.tip("登录失败,用户名或密码错误！", "error");
                    $("#imgCode").attr("src", "/Common/Val");
                }
            }
        });
    },
    Ad: function () {
        //$.jBox.messager("<img alt='' src='/Theme/img/PDA.jpg'/>", "&nbsp;&nbsp;系统动态:新增Android版本手持机支持 ", 10000, {
        //    width: 350, showType: 'fade', buttons: { '关闭': true }, submit: function (v, h, f) {
        //        //window.open("http://www.jooshow.com/");
        //        return true;
        //    }
        //});
    }
};