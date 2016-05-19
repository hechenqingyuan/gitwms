
var Measure = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var name = $("#txtMeasure").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["name"] = name;
        $.gitAjax({
            url: "/Storage/MeasureAjax/GetMeasure",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = JSON.parse(result.List);
                var Html = "";
                if (json != undefined && json.length > 0) {
                    $(json).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>" + (i + 1) + "</td>";
                        Html += "<td>" + item.MeasureNum + "</td>";
                        Html += "<td>" + item.MeasureName + "</td>";
                        Html += "<td>";
                        
                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Measure.Create('" + item.MeasureNum + "','" + item.MeasureName + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Measure.Delete('" + item.MeasureNum + "')\" title=\"删除\"></a>";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: Measure.PageClick });
            }
        });
    },
    Create: function (Num, Name) {
        var submit = function (v, h, f) {
            if (v == true) {
                var Num = h.find("#txtNum").val();
                var Name = h.find("#txtName").val();
                if (git.IsEmpty(Name)) {
                    $.jBox.tip("请输入计量单位名称", "warn");
                    return false;
                }
                var param = {};
                param["Num"] = Num;
                param["Name"] = Name;
                $.gitAjax({
                    url: "/Storage/MeasureAjax/Create",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        if (git.IsEmpty(Num)) {
                            if (result.Key == "1000") {
                                Measure.PageClick(1,10);
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("添加失败", "error");
                            }
                        } else {
                            if (result.Key == "1000") {
                                Measure.PageClick(1, 10);
                                $.jBox.tip("编辑成功", "success");
                            } else {
                                $.jBox.tip("编辑失败", "error");
                            }
                        }
                    }
                });
            }
        }
        if (Num == undefined || Num == "") {
            $.jBox.open("get:/Storage/Measure/AddMeasure", "新增计量单位", 300, 180, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Storage/Measure/AddMeasure", "编辑计量单位", 300, 180, {
                buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                    $(item).find("#txtNum").val(Num);
                    $(item).find("#txtName").val(Name);
                }
            });
        }
    },
    Delete: function (Num) {
        var param = {};
        param["Num"] = Num;
        $.gitAjax({
            url: "/Storage/MeasureAjax/Delete",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "1000") {
                    Measure.PageClick(1, 10);
                } else {
                    $.jBox.tip("删除失败", "error");
                }
            }
        });
    }
}