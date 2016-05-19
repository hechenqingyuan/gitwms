
var Role = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var roleName = $("#txtRoleName").val();
        var page = {};
        page["PageIndex"] = pageIndex;
        page["PageSize"] = pageSize;
        page["roleName"] = roleName;
        $.gitAjax({
            url: "/RoleAjax/RoleList",
            data: page,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                var data = JSON.parse(json.Data);
                if (data.List != undefined && data.List.length > 0) {
                    $(data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>";
                        if (item.RoleNum != "DR_0000") {
                            Html += "<input type=\"checkbox\" name=\"role_item\" class=\"checkboxes\" data=\"" + item.RoleNum + "\" value=\"" + item.RoleNum + "\"/>";
                        }
                        Html += "</td>";
                        Html += "<td>" + (i + 1) + "</td>";
                        Html += "<td>" + item.RoleNum + "</td>";
                        Html += "<td>" + item.RoleName + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>" + item.Remark + "</td>";
                        Html += "<td>";
                        if (item.RoleNum != "DR_0000") {
                            Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Role.Add('" + item.RoleNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Role.Delete('" + item.RoleNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";
                        }
                        Html += "<a class=\"icon-external-link\" href='/Res/Power?roleNum=" + item.RoleNum + "')\" title=\"权限分配\"></a>&nbsp;&nbsp;";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                    $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Role.PageClick });
                }
                $("#tabInfo tbody").html(Html);
            }
        });
    },
    Add: function (roleNum) {
        roleNum = roleNum == undefined ? "" : roleNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var roleName = h.find("#txtRoleName").val();
                var remark = h.find("#txtRemark").val();
                if (roleName == undefined || roleName == "") {
                    $.jBox.tip("请输入角色名称", "warn");
                    return false;
                }
                var param = {};
                param["RoleNum"] = roleNum;
                param["RoleName"] = roleName;
                param["Remark"] = remark;
                $.gitAjax({
                    url: "/RoleAjax/Add", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (roleNum == undefined || roleNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Role.PageClick(1, 10);
                            return true;
                        } else {
                            if (roleNum == undefined || roleNum == "") {
                                $.jBox.tip("添加失败", "error");
                            }
                            else {
                                $.jBox.tip("编辑失败", "error");
                            }
                        }
                    }
                });
                return true;
            } else {
                return true;
            }
        }
        if (roleNum == undefined || roleNum == "") {
            $.jBox.open("get:/Home/AddRole", "添加角色", 350, 200, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Home/AddRole?roleNum=" + roleNum, "编辑角色", 350, 200, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (roleNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["RoleNum"] = roleNum;
                $.gitAjax({
                    url: "/RoleAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Role.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);

    },
    ToExcel: function () {
        var roleName = $("#txtRoleName").val();
        var param = {};
        param["roleName"] = roleName;
        $.gitAjax({
            url: "/RoleAjax/ToExcel", type: "post", data: param, success: function (result) {
                if (result.Path != undefined && result.Path != "") {
                    var path = unescape(result.Path);
                    window.location.href = path;
                    return true;
                } else {
                    $.jBox.info(result.d, "提示");
                    return true;
                }
            }
        });
        return true;
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
                    param["roleNum"] = ids;
                    $.gitAjax({
                        url: "/RoleAjax/BatchDel", type: "post", data: param, success: function (result) {
                            if (result.d == "success") {
                                Role.PageClick(1, 10);
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
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("input[name='role_item']").attr("checked", true);
        }
        else {
            $("input[name='role_item']").attr("checked", false);
        }
    }
};