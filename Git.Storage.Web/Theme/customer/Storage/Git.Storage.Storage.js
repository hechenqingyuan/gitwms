var Storage = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var StorageName = $("#txtStorageName").val();
        var StorageType = $("#ddlStorageType").val();
        var IsForbid = $("#ddlIsForbid").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["StorageName"] = StorageName;
        param["StorageType"] = StorageType;
        param["IsForbid"] = IsForbid;
        $.gitAjax({
            url: "/StoreAjax/GetList",
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
                        if (item.StorageNum != "DSM_0000" && item.StorageNum != "DSP_0000") {
                            Html += "<input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" value=\"" + item.StorageNum + "\"/>";
                        }
                        Html += "</td>";
                        Html += "<td>" + (i + 1) + "</td>";
                        Html += "<td>" + item.StorageNum + "</td>";
                        Html += "<td>" + item.StorageName + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EStorageType,item.StorageType) + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EBool,item.IsForbid) + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EBool,item.IsDefault) + "</td>";
                        Html += "<td>" + item.Remark + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>";
                        if (item.StorageNum != "DSM_0000" && item.StorageNum != "DSP_0000") {

                            Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Storage.Add('" + item.StorageNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Storage.Delete('" + item.StorageNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";

                            if (item.IsForbid == 1) {
                                Html += "<a class=\"icon-lock\" href=\"javascript:void(0)\" onclick=\"Storage.Audit('" + item.StorageNum + "',0)\" title=\"禁用\"></a>";
                            }
                            else {
                                Html += "<a class=\"icon-unlock\" href=\"javascript:void(0)\" onclick=\"Storage.Audit('" + item.StorageNum + "',1)\" title=\"解除\"></a>";
                            }
                        }
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Storage.PageClick });
            }
        });
    },
    ToExcel: function () {
        var StorageName = $("#txtStorageName").val();
        var StorageType = $("#ddlStorageType").val();
        var IsForbid = $("#ddlIsForbid").val();
        var param = {};
        param["StorageName"] = StorageName;
        param["StorageType"] = StorageType;
        param["IsForbid"] = IsForbid;
        $.gitAjax({
            url: "/StoreAjax/ToExcel", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
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
    
        //var jsonParam = '{ "status":' + status + ',"orderNum":"' + orderNum + '","begin":"' + begin + '","end":"' + end + '","cusName":"' + cusName + '"}';
        //jQuery.WebService("/Plan/SearchOrder.aspx/ToExcel", jsonParam, function (result) {
        //    $.jBox.closeTip();
        //    jQuery.jBox.info(result.d, '提示');

        //}, function () {
        //    $.jBox.tip("正在导出数据...", 'loading');
        //}, function () {
        //    $.jBox.tip("导出数据错误!", 'error');
        //});
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
    Add: function (StorageNum) {
        StorageNum = StorageNum == undefined ? "" : StorageNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var StorageNum = h.find("#txtStorateNum").val();
                var StorageName = h.find("#txtStorageName").val();
                var Length = h.find("#txtLength").val();
                var Width = h.find("#txtWidth").val();
                var Height = h.find("#txtHeight").val();
                var Action = h.find("#txtAction").val();
                var StorageType = h.find("#ddlStorageType").val();
                var Status = h.find("#ddlStatus").val();
                var IsDefault = h.find("#ddlIsDefault").val();
                var Remark = h.find("#txtRemark").val();
                IsDefault = IsDefault == undefined ? 1 : 0;
                if (StorageName == undefined || StorageName == "") {
                    $.jBox.tip("请输入仓库名", "warn");
                    return false;
                }
                var param = {};
                param["StorageNum"] = StorageNum;
                param["StorageName"] = StorageName;
                param["Length"] = Length;
                param["Width"] = Width;
                param["Height"] = Height;
                param["Action"] = Action;
                param["StorageType"] = StorageType;
                param["Status"] = Status;
                param["IsDefault"] = IsDefault;
                param["Remark"] = Remark;
                $.gitAjax({
                    url: "/StoreAjax/Add", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (StorageNum == undefined || StorageNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Storage.PageClick(1);
                            return true;
                        } else {
                            if (StorageNum == undefined || StorageNum == "") {
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
        if (StorageNum == undefined || StorageNum == "") {
            $.jBox.open("get:/Storage/Store/Add", "添加仓库", 500, 250, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Storage/Store/Add?StorageNum=" + StorageNum, "编辑仓库", 500, 250, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (StorageNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["StorageNum"] = StorageNum;
                $.gitAjax({
                    url: "/StoreAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Storage.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit); 
    },
    Audit: function (StorageNum,IsForbid) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["StorageNum"] = StorageNum;
                param["IsForbid"] = IsForbid;
                $.gitAjax({
                    url: "/StoreAjax/Audit", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Storage.PageClick(1);
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
        else if (IsForbid == 1)
        {
            $.jBox.confirm("确定要解除吗？", "提示", submit);
        }
    }
};