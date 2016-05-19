
var UserMenu = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var resName = $("#txtResName").val();
        var parentNum = $("#ddlParentNum").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["resName"] = resName;
        param["parentNum"] = parentNum;
        $.gitAjax({
            url: "/ResAjax/GetMenuList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined) {
                    var data = JSON.parse(json.Data);
                    if (data.List != undefined && data.List.length > 0) {
                        $(data.List).each(function (i, item) {
                            Html += "<tr class=\"odd gradeX\">";
                            Html += "<td>" + item.ResNum + "</td>";
                            Html += "<td>" + item.ResName + "</td>";
                            Html += "<td>" + item.ParentName + "</td>";
                            Html += "<td>" + item.ResouceType + "</td>";
                            Html += "<td>" + item.CssName + "</td>";
                            Html += "<td>" + item.Sort + "</td>";
                            Html += "<td>" + item.Url + "</td>";
                            Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            Html += "<td>";
                            Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"UserMenu.Add('" + item.ResNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"UserMenu.Delete('" + item.ResNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";
                            Html += "</td>";
                            Html += "</tr>";
                        });
                    }
                    $("#tabInfo tbody").html(Html);
                    $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: UserMenu.PageClick });
                }
            }
        });
    },
    Delete: function (ResNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["ResNum"] = ResNum;
                $.gitAjax({
                    url: "/ResAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            UserMenu.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);

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
    Add: function (resNum) {
        resNum = resNum == undefined ? "" : resNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var resName = h.find("#txtResName").val();
                var parentNum = h.find("#ddlParentNum").val();
                var sort = h.find("#txtSort").val();
                var url = h.find("#txtUrl").val();
                var ResType = h.find("#ddlResType").val();
                var CssName = h.find("#txtCssName").val();
                if (resName == undefined || resName == "") {
                    $.jBox.tip("请输入菜单名称！", "warn");
                    return false;
                }

                var param = {};
                param["ResNum"] = resNum;
                param["ResName"] = resName;
                param["ParentNum"] = parentNum;
                param["Sort"] = sort;
                param["Url"] = url;
                param["ResType"] = ResType;
                param["CssName"] = CssName;
                $.gitAjax({
                    url: "/ResAjax/AddMenu", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (resNum == undefined || resNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            UserMenu.PageClick(1);
                            return true;
                        } else {
                            if (resNum == undefined || resNum == "") {
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
        if (resNum == undefined || resNum == "") {
            $.jBox.open("get:/Res/AddMenu", "添加菜单", 400, 330, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Res/AddMenu?resNum=" + resNum, "编辑菜单", 400, 330, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    }
}