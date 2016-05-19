

var Location = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var StorageName = $("#ddlStorageName").val();
        var LocalName = $("#txtLocalName").val();
        var LocalType = $("#ddlLocalType").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["StorageName"] = StorageName;
        param["LocalName"] = LocalName;
        param["LocalType"] = LocalType;
        $.gitAjax({
            url: "/LocationAjax/GetList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td><input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" value=\"" + item.LocalNum + "\"/></td>";
                        Html += "<td>" + (i + 1) + "</td>";
                        Html += "<td>" + item.LocalNum+ "</td>";
                        Html += "<td>" + item.LocalName + "</td>";
                        Html += "<td>" + item.StorageName + "</td>";
                        Html += "<td>" + git.GetEnumDesc(ELocalType, item.LocalType) + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EBool, item.IsForbid) + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EBool, item.IsDefault) + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>";
                        
                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Location.Add('" + item.LocalNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Location.Delete('" + item.LocalNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";

                        if (item.IsForbid == 1) {
                            Html += "<a class=\"icon-lock\" href=\"javascript:void(0)\" onclick=\"Location.Audit('" + item.LocalNum + "',0)\" title=\"禁用\"></a>";
                        }
                        else {
                            Html += "<a class=\"icon-unlock\" href=\"javascript:void(0)\" onclick=\"Location.Audit('" + item.LocalNum + "',1)\" title=\"解除\"></a>";
                        }
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Location.PageClick });
            }
        });
    },
    ToExcel: function () {
        var StorageName = $("#ddlStorageName").val();
        var LocalName = $("#txtLocalName").val();
        var LocalType = $("#ddlLocalType").val();
        var param = {};
        param["StorageName"] = StorageName;
        param["LocalName"] = LocalName;
        param["LocalType"] = LocalType;
        $.gitAjax({
            url: "/LocationAjax/ToExcel", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
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
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("input[name='user_item']").attr("checked", true);
        }
        else {
            $("input[name='user_item']").attr("checked", false);
        }
    },
    Add: function (LocalNum) {
        LocalNum = LocalNum == undefined ? "" : LocalNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var LocalNum = h.find("#txtLocalNum").val();
                var LocalBarCode = h.find("#txtLocalBarCode").val();
                var LocalName = h.find("#txtLocalName").val();
                var StorageNum = h.find("#ddlStorage").val();
                var LocalType = h.find("#ddlLocalType").val();
                var IsDefault = h.find("#ddlIsDefault").val();
                //var Remark = h.find("#txtRemark").val();
                if (LocalName == undefined || LocalName == "") {
                    $.jBox.tip("请输入库位名称", "warn");
                    return false;
                }
                if (StorageNum == undefined || StorageNum == "") {
                    $.jBox.tip("请选择仓库", "warn");
                    return false;
                }
                if (LocalType == undefined || LocalType == "") {
                    $.jBox.tip("请输入库位类型", "warn");
                    return false;
                }
                if (LocalBarCode == undefined || LocalBarCode == "") {
                    $.jBox.tip("请输入库位条码", "warn");
                    return false;
                }           
                if (IsDefault == undefined || IsDefault == "") {
                    $.jBox.tip("是否为默认值", "warn");
                    return false;
                }
                var param = {};
                param["LocalNum"] = LocalNum;
                param["LocalName"] = LocalName;
                param["LocalBarCode"] = LocalBarCode;
                param["StorageNum"] = StorageNum;
                param["LocalType"] = LocalType;
                param["IsDefault"] = IsDefault;
                //param["Remark"] = Remark;
                $.gitAjax({
                    url: "/LocationAjax/Add", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (LocalNum == undefined || LocalNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Location.PageClick(1);
                            return true;
                        } else {
                            if (LocalNum == undefined || LocalNum == "") {
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
        if (LocalNum == undefined || LocalNum == "") {
            $.jBox.open("get:/Storage/Location/Add", "添加库位", 500, 250, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Storage/Location/Add?LocalNum=" + LocalNum, "编辑库位", 500, 250, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (LocalNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["LocalNum"] = LocalNum;
                $.gitAjax({
                    url: "/LocationAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Location.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    } ,
    Audit: function (LocalNum, IsForbid) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["LocalNum"] = LocalNum;
                param["IsForbid"] = IsForbid;
                $.gitAjax({
                    url: "/LocationAjax/Audit", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Location.PageClick(1);
                        } else {
                            $.jBox.tip("操作失败", "error");
                        }
                    }
                });
            }
        };
        if (IsForbid == 0) {
            $.jBox.confirm("确定要禁用吗？", "提示", submit);
        }
        else if (IsForbid == 1) {
            $.jBox.confirm("确定要解除吗？", "提示", submit);
        }
    }
};