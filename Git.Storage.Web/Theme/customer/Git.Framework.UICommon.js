
$(document).ready(function () {
    //左侧菜单
    masterUI.ToggleMenu();

    var height = $(document).height();
    height = parseInt(height) - 80;
    $("#body").css("min-height", height);

    masterUI.Ad();

});


/********************************************左侧菜单***********************************************/
var masterUI = {
    ToggleMenu: function () {
        //用于左侧菜单展开以及显示
        $(".sidebar-toggler").click(function () {
            var MenuStatus = "open";
            if (!$("#container").hasClass("sidebar-closed")) {
                $("#container").addClass("sidebar-closed");
                MenuStatus = "close";
            } else {
                $("#container").removeClass("sidebar-closed");
                MenuStatus = "open";
            }
            var param = {};
            param["MenuStatus"] = MenuStatus;
            $.gitAjax({
                url: "/Common/SetMenuStatus", type: "post", data: param, success: function (result) {
                }
            });
        });

        //右侧菜单点击操作
        $("#sidebar ul .has-sub").click(function () {
            var index = $("#sidebar ul .has-sub").index($(this));
            $("#sidebar ul .has-sub").each(function (i, item) {
                if (index == i) {
                    $(this).children(".sub").slideDown(500, function () {
                        if (!$(item).hasClass("active")) {
                            $(item).children("a").children(".arrow").addClass("open");
                            $(item).addClass("active");
                        }
                    });
                } else {
                    $(item).children(".sub").slideUp(500, function () {
                        if ($(item).hasClass("active")) {
                            $(item).removeClass("active");
                            $(item).children("a").children(".arrow").removeClass("open");
                        }
                    });
                }
            });
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

/********************************************选择库位***********************************************/
; (function ($) {
    $.fn.LocalDialog = function (options) {
        var defaultOption = {
            data: {},
            Mult: true,
            EventName: "dblclick",
            callBack: undefined
        };
        var self = $(this);
        defaultOption = $.extend(defaultOption, options);
        var submit = function (v, h, f) {
            if (v == 1) {
                var result = undefined;
                h.find("input[type='checkbox'][name='local_item']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var data = $(item).attr("data-value");
                        if (!git.IsEmpty(data)) {
                            result = JSON.parse(unescape(data));
                        }
                    }
                });
                if (defaultOption.callBack != undefined && typeof (defaultOption.callBack) == "function") {
                    defaultOption.callBack(result, self);
                }
            }
        };
        $(this).bind(defaultOption.EventName, function () {
            $.jBox.open("get:/Storage/Location/Detail", "选择库位", 850, 500, {
                buttons: { "选择": 1, "关闭": 2 }, submit: submit, loaded: function (h) {

                }
            });
        });
    };
})(jQuery);

/********************************************选择产品信息***********************************************/
/**
* data: 传入参数
* Mult:是否允许选择多个checkbox，默认是true
* callBack: 选择之后的回调函数
**/
; (function ($) {
    $.fn.ProductDialog = function (options) {
        var defaultOption = {
            data: {},
            Mult: true,
            EventName: "dblclick",
            callBack: undefined
        };
        defaultOption = $.extend(defaultOption, options);
        var submit = function (v, h, f) {
            if (v == 1) {
                var result = undefined;
                h.find("input[type='checkbox'][name='product_item']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var data = $(item).attr("data-value");
                        if (!git.IsEmpty(data)) {
                            result = JSON.parse(unescape(data));
                        }
                    }
                });
                if (defaultOption.callBack != undefined && typeof (defaultOption.callBack) == "function") {
                    defaultOption.callBack(result);
                }
            }
        };
        $(this).bind(defaultOption.EventName, function () {
            $.jBox.open("get:/Product/Goods/Dialog", "选择产品", 850, 500, {
                buttons: { "选择": 1, "关闭": 2 }, submit: submit, loaded: function (h) {

                }
            });
        });
    };
})(jQuery);

/********************************************选择客户***********************************************/
/**
* data: 传入参数
* Mult:是否允许选择多个checkbox，默认是true
* callBack: 选择之后的回调函数
**/
; (function ($) {
    $.fn.CustomerDialog = function (options) {
        var defaultOption = {
            data: {},
            Mult: true,
            EventName: "dblclick",
            callBack: undefined
        };
        defaultOption = $.extend(defaultOption, options);
        var submit = function (v, h, f) {
            if (v == 1) {
                var result = undefined;
                h.find("input[type='checkbox'][name='product_item']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var data = $(item).attr("data-value");
                        if (!git.IsEmpty(data)) {
                            result = JSON.parse(unescape(data));
                        }
                    }
                });
                if (defaultOption.callBack != undefined && typeof (defaultOption.callBack) == "function") {
                    defaultOption.callBack(result);
                }
            }
        };
        $(this).bind(defaultOption.EventName, function () {
            $.jBox.open("get:/Client/Customer/Dialog", "选择客户", 850, 500, {
                buttons: { "选择": 1, "关闭": 2 }, submit: submit, loaded: function (h) {

                }
            });
        });
    };
})(jQuery);

/********************************************选择供应商***********************************************/
/**
* data: 传入参数
* Mult:是否允许选择多个checkbox，默认是true
* callBack: 选择之后的回调函数
**/
; (function ($) {
    $.fn.SupplierDialog = function (options) {
        var defaultOption = {
            data: {},
            Mult: true,
            EventName: "dblclick",
            callBack: undefined
        };
        defaultOption = $.extend(defaultOption, options);
        var submit = function (v, h, f) {
            if (v == 1) {
                var result = undefined;
                h.find("input[type='checkbox'][name='product_item']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var data = $(item).attr("data-value");
                        if (!git.IsEmpty(data)) {
                            result = JSON.parse(unescape(data));
                        }
                    }
                });
                if (defaultOption.callBack != undefined && typeof (defaultOption.callBack) == "function") {
                    defaultOption.callBack(result);
                }
            }
        };
        $(this).bind(defaultOption.EventName, function () {
            $.jBox.open("get:/Client/Supplier/Dialog", "选择客户", 850, 500, {
                buttons: { "选择": 1, "关闭": 2 }, submit: submit, loaded: function (h) {

                }
            });
        });
    };
})(jQuery);


/********************************************选择员工***********************************************/
/**
* data: 传入参数
* Mult:是否允许选择多个checkbox，默认是true
* callBack: 选择之后的回调函数
**/
; (function ($) {
    $.fn.UserDialog = function (options) {
        var defaultOption = {
            data: {},
            Mult: true,
            EventName: "dblclick",
            callBack: undefined
        };
        defaultOption = $.extend(defaultOption, options);
        var submit = function (v, h, f) {
            if (v == 1) {
                var result = undefined;
                h.find("input[type='checkbox'][name='user_item']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var data = $(item).attr("data-value");
                        if (!git.IsEmpty(data)) {
                            result = JSON.parse(unescape(data));
                        }
                    }
                });
                if (defaultOption.callBack != undefined && typeof (defaultOption.callBack) == "function") {
                    defaultOption.callBack(result);
                }
            }
        };
        $(this).bind(defaultOption.EventName, function () {
            $.jBox.open("get:/Home/Dialog", "选择用户", 850, 500, {
                buttons: { "选择": 1, "关闭": 2 }, submit: submit, loaded: function (h) {

                }
            });
        });
    };
})(jQuery);

/****************************************************账户设置************************************************/
var AccountSetting = {
    Add: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var userCode = h.find("#txtUserCode").val();
                var userName = h.find("#txtUserName").val();
                var realName = h.find("#txtRealName").val();
                var email = h.find("#txtEmail").val();
                var phone = h.find("#txtPhone").val();
                var mobile = h.find("#txtMobile").val();
                var roleNum = h.find("#ddlRole").val();
                var departNum = h.find("#ddlDepart").val();
                if (userName == undefined || userName == "") {
                    $.jBox.tip("请输入用户名", "warn");
                    return false;
                }
                var param = {};
                param["UserCode"] = userCode;
                param["UserName"] = userName;
                param["RealName"] = realName;
                param["Email"] = email;
                param["Phone"] = phone;
                param["Mobile"] = mobile;
                param["RoleNum"] = roleNum;
                param["DepartNum"] = departNum;
                $.gitAjax({
                    url: "/UserAjax/AddUser", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            $.jBox.tip("编辑成功", "success");
                            User.PageClick(1, 10);
                        } else {
                            $.jBox.tip("编辑失败", "error");
                        }
                    }
                });
                return true;
            } else {
                return true;
            }
        }
        $.jBox.open("get:/Home/AccountSetting", "编辑用户", 500, 270, { buttons: { "确定": true, "关闭": false }, submit: submit });
    },
    Edit: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var currentPassword = h.find("#currentPassword").val();
                var passWord = h.find("#txtPassword").val();
                var confirm = h.find("#txtConfirm").val();

                if (currentPassword == undefined || currentPassword == "") {
                    $.jBox.tip("请输入当前密码", "warn");
                    return false;
                }
                if (passWord == undefined || passWord == "") {
                    $.jBox.tip("请输入密码", "warn");
                    return false;
                }
                if (confirm == undefined || confirm == "") {
                    $.jBox.tip("请输入确认密码", "warn");
                    return false;
                }
                if (passWord != confirm) {
                    $.jBox.tip("密码和确认密码不一致", "warn");
                    return false;
                }
                var param = {};
                param["currentPassword"] = currentPassword;
                param["PassWord"] = passWord;

                $.gitAjax({
                    url: "/UserAjax/ChangePwd", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            $.jBox.tip("修改成功", "success");

                        } else {
                            $.jBox.tip("修改失败，当前密码不正确！", "error");

                        }
                    }
                });
                return true;
            } else {
                return true;
            }
        }
        $.jBox.open("get:/Home/ChangePwd", "修改密码", 350, 200, { buttons: { "确定": true, "关闭": false }, submit: submit });
    }
};