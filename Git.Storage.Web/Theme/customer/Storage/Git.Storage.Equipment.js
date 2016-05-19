
var Equipment = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var EquipmentName = $("#txtEquipmentName").val();
        var Status = $("#ddlStatus").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["EquipmentName"] = EquipmentName;
        param["Status"] = Status;
        $.gitAjax({
            url: "/EquipmentAjax/GetEquipmentList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td><input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" value=\"" + item.SnNum + "\"/></td>";
                        Html += "<td>" + (i + 1) + "</td>";
                        Html += "<td>" + item.SnNum + "</td>";
                        Html += "<td>" + item.EquipmentName + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EBool, item.IsImpower) + "</td>";
                        Html += "<td>" + item.Flag + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EEquipmentStatus, item.Status) + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>";
                        
                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Equipment.Add('" + item.SnNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Equipment.Delete('" + item.SnNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";

                        if (item.IsImpower == 0) {
                            Html += "<a class=\"icon-lock\" href=\"javascript:void(0)\" onclick=\"Equipment.Empower('" + item.SnNum + "')\" title=\"授权\"></a>";
                        } else {

                        }
                        Html += "</td>";
                        Html += "</tr>";
                    });
                } else {
                    Html += " <tr id=\"zero\"> <td colspan='10'> <center>没有找到记录!<center>  </td>  </tr>";
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Equipment.PageClick });
            }
        });
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
    Add: function (SnNum) {
        SnNum = SnNum == undefined ? "" : SnNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var SnNum = h.find("#txtSnNum").val();
                var EquipmentName = h.find("#txtEquipmentName").val();
                var Flag = h.find("#txtFlag").val();
                var IsImpower = h.find("#chbIsImpower").attr("checked");
                var Status = h.find("#ddlStatus").val();
                var Remark = h.find("#txtRemark").val();
                if (EquipmentName == undefined || EquipmentName == "") {
                    $.jBox.tip("请输入设备名", "warn");
                    return false;
                }
                if (IsImpower == "checked") {
                    if (Flag == undefined || Flag == "") {
                        $.jBox.tip("请输入授权符", "warn");
                        return false;
                    }
                    IsImpower = 1;//授权
                }
                else {
                    IsImpower = 0;//不授权
                }
                if (Status == undefined || Status == "") {
                    $.jBox.tip("请选择状态", "warn");
                    return false;
                }
                var param = {};
                param["SnNum"] = SnNum;
                param["EquipmentName"] = EquipmentName;
                param["Flag"] = Flag;
                param["IsImpower"] = IsImpower;
                param["Status"] = Status;
                param["Remark"] = Remark;

                $.gitAjax({
                    url: "/EquipmentAjax/AddEquipment", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (SnNum == undefined || SnNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Equipment.PageClick(1, 10);
                            return true;
                        } else {
                            if (SnNum == undefined || SnNum == "") {
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
        if (SnNum == undefined || SnNum == "") {
            $.jBox.open("get:/Storage/Equipment/Add", "添加设备", 500, 250, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Storage/Equipment/Add?SnNum=" + SnNum, "编辑设备", 500, 250, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Empower: function (SnNum) {
        SnNum = SnNum == undefined ? "" : SnNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var SnNum = h.find("#txtSnNum").val();
                var Flag = h.find("#txtFlag").val();
                var EquipmentName = h.find("#txtEquipmentName").val();
                var status = h.find("#hdStatus").val();
                if (Flag == undefined || Flag == "") {
                    $.jBox.tip("请输入授权符", "warn");
                    return false;
                }
                var param = {};
                param["SnNum"] = SnNum;
                param["EquipmentName"] = EquipmentName;
                param["Flag"] = Flag;
                param["IsImpower"] = 1;
                param["Status"] = status;
                $.gitAjax({
                    url: "/EquipmentAjax/AddEquipment", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            $.jBox.tip("授权成功", "success");
                            Equipment.PageClick(1, 10);
                            return true;
                        } else {
                            $.jBox.tip("授权失败", "error");
                        }
                    }
                });
                return true;
            } else {
                return true;
            }
        }
        $.jBox.open("get:/Storage/Equipment/Empower?SnNum=" + SnNum, "设备授权", 300, 200, { buttons: { "确定": true, "关闭": false }, submit: submit });

    },
    Delete: function (SnNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["SnNum"] = SnNum;
                $.gitAjax({
                    url: "/EquipmentAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Equipment.PageClick(1, 10);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    }
};

//checkbox选中之后授权标示符可以输入，否则不能输入
function IsTrue(item) {
    var flag = $(item).attr("checked");
    if (flag == "checked") {
        $("input[name='txtFlag']").removeAttr("disabled");
    }
    else {
        $("input[name='txtFlag']").attr("disabled", "disabled");
        $("input[name='txtFlag']").val("");
    }
}


