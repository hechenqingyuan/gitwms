
var Depart = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var departName = $("#txtDepartName").val();
        var page = {};
        page["PageIndex"] = pageIndex;
        page["PageSize"] = pageSize;
        page["departName"] = departName;
        $.gitAjax({
            url: "/DepartAjax/DepartList",
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
                        Html += "<td><input type=\"checkbox\" name=\"role_item\" class=\"checkboxes\" data=\"" + item.ID + "\" value=\"" + item.DepartNum + "\"/></td>";
                        Html += "<td>" + (i + 1) + "</td>";
                        Html += "<td>" + item.DepartNum + "</td>";
                        Html += "<td>" + item.DepartName + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>";
                        
                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Depart.Add('" + item.DepartNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Depart.Delete('" + item.ID + "')\" title=\"删除\"></a>&nbsp;&nbsp;";

                        Html += "</td>";
                        Html += "</tr>";
                    });
                    $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Depart.PageClick });
                }
                $("#tabInfo tbody").html(Html);
            }
        });
    },
    Add: function (departNum) {
        departNum = departNum == undefined ? "" : departNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var departName = h.find("#txtDepartName").val();
                if (departName == undefined || departName == "") {
                    $.jBox.tip("请输入部门名", "warn");
                    return false;
                }
                var param = {};
                param["DepartName"] = departName;
                param["DepartNum"] = departNum;
                $.gitAjax({
                    url: "/DepartAjax/AddDepart", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (departNum == undefined || departNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Depart.PageClick(1,10);
                            return true;
                        } else {
                            if (departNum == undefined || departNum == "") {
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
        if (departNum == undefined || departNum == "") {
            $.jBox.open("get:/Home/AddDepart", "添加部门", 350, 120, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Home/AddDepart?departNum=" + departNum, "编辑部门", 350, 120, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (id) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["id"] = id;
                $.gitAjax({
                    url: "/DepartAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Depart.PageClick(1,10);
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
        var departName = $("#txtDepartName").val();
        var param = {};
        param["departName"] = departName;
        $.gitAjax({
            url: "/DepartAjax/ToExcel", type: "post", data: param, success: function (result) {
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
                    param["id"] = ids;
                    $.gitAjax({
                        url: "/DepartAjax/BatchDel", type: "post", data: param, success: function (result) {
                            if (result.d == "success") {
                                Depart.PageClick(1, 10);
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