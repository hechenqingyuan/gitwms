var User = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var search = $("#txtSearch").val();
        var userCode = $("#txtUserCode").val();
        var userName = $("#txtUserName").val();
        var roleNum = $("#ddlRole").val();
        var departNum = $("#ddlDepart").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["userCode"] = userCode;
        param["userName"] = userName;
        param["roleNum"] = roleNum;
        param["departNum"] = departNum;
        param["search"] = search;
        $.gitAjax({
            url: "/UserAjax/GetAdminList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>";
                        if (item.UserCode != "DA_0000") {
                            Html += "<input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\"  data=\"" + item.UserCode + "\" value=\"" + item.UserCode + "\"/>";
                        }
                        Html += "</td>";
                        Html += "<td>" + item.UserName + "</td>";
                        Html += "<td>" + item.UserCode + "</td>";
                        Html += "<td>" + item.RealName + "</td>";
                        Html += "<td>" + item.Email + "</td>";
                        Html += "<td>" + item.Mobile + "</td>";
                        Html += "<td>" + item.LoginCount + "</td>";
                        Html += "<td>" + item.DepartName + "</td>";
                        Html += "<td>";
                        Html += item.RoleName == null ? "" : item.RoleName;
                        Html += "</td>";
                        Html += "<td>";
                        if (item.UserCode != "DA_0000") {
                            Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"User.Add('" + item.UserCode + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"User.Delete('" + item.UserCode + "')\" title=\"删除\"></a>";
                        }
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: User.PageClick });
            }
        });
    },
    ToExcel: function () {
        var userCode = $("#txtUserCode").val();
        var userName = $("#txtUserName").val();
        var roleNum = $("#ddlRole").val();
        var departNum = $("#ddlDepart").val();
        var param = {};
        param["userCode"] = userCode;
        param["userName"] = userName;
        param["roleNum"] = roleNum;
        param["departNum"] = departNum;
        $.gitAjax({
            url: "/UserAjax/ToExcel", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                if (result.Path != undefined && result.Path != "") {
                    var path = unescape(result.Path);
                    window.location.href = path;
                    return true;
                } else {
                    $.jBox.info(result.d, "提示");
                    return true;
                }
                return true;
            }
        });
        return true;
    },
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("input[name='user_item']").attr("checked", true);
        }
        else {
            $("input[name='user_item']").attr("checked", false);
        }
    },
    Add: function (userCode) {
        userCode = userCode == undefined ? "" : userCode;
        var submit = function (v, h, f) {
            if (v == true) {
                var userCode = h.find("#txtUserCode").val();
                var userName = h.find("#txtUserName").val();
                var passWord = h.find("#txtPassword").val();
                var confirm = h.find("#txtConfirm").val();
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
                param["UserCode"] = userCode;
                param["UserName"] = userName;
                param["PassWord"] = passWord;
                param["RealName"] = realName;
                param["Email"] = email;
                param["Phone"] = phone;
                param["Mobile"] = mobile;
                param["RoleNum"] = roleNum;
                param["DepartNum"] = departNum;
                $.gitAjax({
                    url: "/UserAjax/AddUser", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (userCode == undefined || userCode == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            User.PageClick(1);
                            return true;
                        } else {
                            if (userCode == undefined || userCode == "") {
                                $.jBox.tip("添加失败" + result.d, "error");
                            }
                            else {
                                $.jBox.tip("编辑失败" + result.d, "error");
                            }
                        }
                    }
                });
                return true;
            } else {
                return true;
            }
        }
        if (userCode == undefined || userCode == "") {
            $.jBox.open("get:/Home/AddUser", "添加用户", 500, 300, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Home/AddUser?userCode=" + userCode, "编辑用户", 500, 300, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (userCode) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["userCode"] = userCode;
                $.gitAjax({
                    url: "/UserAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            User.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["userCode"] = ids;
                    $.gitAjax({
                        url: "/UserAjax/BatchDel", type: "post", data: param, success: function (result) {
                            if (result.d == "success") {
                                User.PageClick(1);
                            } else {
                                $.jBox.tip("删除失败", "error");
                            }
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    },
    SearchEvent: function () {
        $("#btnHSearch").click(function () {
            var flag = $("#divHSearch").css("display");
            if (flag == "none") {
                $("#txtSearch").val("");
                $("#divHSearch").slideDown("slow");
            } else {
                $("#divHSearch").slideUp("slow");
            }
        });
    }
};

