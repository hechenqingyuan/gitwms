
var ReturnOrder = {
    Load: function () {
        $("#txtContractOrder").keydown(function (e) {
            if (e.keyCode == 13) {
                ReturnOrder.LoadOrder();
            }
        });
    },
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", true);
        } else {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    },
    LoadOrder: function () {
        var OrderNum = $("#txtContractOrder").val();
        if (git.IsEmpty(OrderNum)) {
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        $.gitAjax({
            url: "/Returns/ProductAjax/Load",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.d != undefined && result.d == "1001") {
                    $.jBox.tip("该单据已经在退货中，请勿重新提交", "warn");
                    return;
                }
                if (result.d != undefined && result.d == "1002") {
                    $.jBox.tip("该单据没有审核通过，不能处理申请退货", "warn");
                    return;
                }

                $("#txtCusNum").val(result.CusNum);
                $("#txtCusName").val(result.CusName);
                $("#txtAddress").val(result.Address);
                $("#txtCusPhone").val(result.Phone);
                $("#txtContactName").val(result.Contact);

                if (result.data != undefined) {
                    var json = JSON.parse(result.data);
                    var html = "";
                    $(json.List).each(function (i, item) {
                        html += "<tr class=\"odd gradeX\">";
                        html += "<td><input type=\"checkbox\" BatchNum=\"" + item.BatchNum + "\" ProductNum=\"" + item.ProductNum + "\" BarCode=\"" + item.BarCode + "\" LocalNum=\"" + item.LocalNum + "\" SnNum=\"" + item.SnNum + "\" /></td>";
                        html += "<td>" + item.ProductNum + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.BatchNum + "</td>";
                        html += "<td>" + item.LocalName + "</td>";
                        html += "<td>" + item.Num + "&nbsp;-&nbsp;" + item.BackNum + "&nbsp;=&nbsp;" + (item.Num - item.BackNum) + "</td>";
                        html += "<td><input type='text' class='input-small inde' data-max=\"" + (item.Num - item.BackNum) + "\"/></td>";
                        html += "</tr>";
                    });
                    $("#tabInfo").children("tbody").html(html);
                }
            }
        });
    },
    Create: function () {
        var OrderNum = $("#txtOrderNum").val();
        var CusNum = $("#txtCusNum").val();
        var CusName = $("#txtCusName").val();
        var Address = $("#txtAddress").val();
        var Phone = $("#txtCusPhone").val();
        var Contact = $("#txtContactName").val();
        var CreateTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        var ContractOrder = $("#txtContractOrder").val();
        if (git.IsEmpty(ContractOrder)) {
            $.jBox.tip("请输入关联单号", "error");
            return false;
        }
        if (git.IsEmpty(CusNum)) {
            $.jBox.tip("输入的关联单号有误", "error");
            return false;
        }

        var items = $("#tabInfo").find("tr");
        var list = [];
        $(items).each(function (i, item) {
            var flag = $(item).find("input[type='checkbox']").attr("checked");
            if (flag == "checked" || flag) {
                var ProductNum = $(item).find("input[type='checkbox']").attr("ProductNum");
                var BarCode = $(item).find("input[type='checkbox']").attr("BarCode");
                var LocalNum = $(item).find("input[type='checkbox']").attr("LocalNum");
                var BatchNum = $(item).find("input[type='checkbox']").attr("BatchNum");
                var BackNum = $(item).find("input[type='text']").val();
                var MaxNum = $(item).find("input[type='text']").attr("data-max");

                if (!isNaN(BackNum) && BackNum > 0) {
                    if (BackNum <= MaxNum) {
                        var detail = {};
                        detail["ProductNum"] = ProductNum;
                        detail["BarCode"] = BarCode;
                        detail["LocalNum"] = LocalNum;
                        detail["BackNum"] = BackNum;
                        detail["BatchNum"] = BatchNum;
                        list.push(detail);
                    }
                }
            }
        });
        if (list.length == 0) {
            $.jBox.tip("请选择退货的产品", "error");
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        param["CusNum"] = CusNum;
        param["CusName"] = CusName;
        param["Address"] = Address;
        param["Phone"] = Phone;
        param["Contact"] = Contact;
        param["CreateTime"] = CreateTime;
        param["Remark"] = Remark;
        param["ContractOrder"] = ContractOrder;
        param["list"] = JSON.stringify(list);
        $.gitAjax({
            url: "/Returns/ProductAjax/Create",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {

                if (OrderNum == undefined || OrderNum == "") {
                    if (result.d != undefined) {
                        if (result.d == "1000") {
                            $.jBox.tip("退货单申请成功", "success");
                        }
                        if (result.d == "1001") {
                            $.jBox.tip("退货单申请失败", "error");
                        }
                        if (result.d == "1002") {
                            $.jBox.tip("退货单修改成功", "success");
                        }
                        if (result.d == "1003") {
                            $.jBox.tip("退货单修改失败", "error");
                        }
                    } else {
                        $.jBox.tip("操作失败,请联系系统管理员", "error");
                    }
                } else {
                    if (result.d == "Success") {
                        $.jBox.tip("退货单修改成功", "success");
                    } else {
                        $.jBox.tip("退货单修改失败", "error");
                    }
                }
            }
        });
    }
};



var ReturnManager = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();

        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Returns/ProductManagerAjax/GetList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var html = "";
                if (result.Data != undefined) {
                    var json = JSON.parse(result.Data);
                    if (json.List != undefined && json.List.length > 0) {
                        $(json.List).each(function (i, item) {
                            html += "<tr>";
                            html += "<td><input type=\"checkbox\" name='return_item' value='" + item.OrderNum + "'/></td>";
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + git.GetEnumDesc(EReturnType, item.ReturnType) + "</td>";
                            html += "<td>" + item.Num + "</td>";
                            html += "<td>" + item.ContractOrder + "</td>";
                            html += "<td>" + git.GetEnumDesc(EAudite, item.Status) + "</td>";
                            html += "<td>" + item.CreateUserName + "</td>";
                            html += "<td>" + git.GetEnumDesc(EOpType, item.OperateType) + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            html += "<td>";
                            if (item.Status == EAuditeJson.Wait || item.Status == EAuditeJson.NotPass) {
                                html += "<a class=\"icon-edit\" href=\"/Returns/Product/Edit?OrderNum=" + item.OrderNum + "\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            }
                            html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ReturnManager.Delete('" + item.OrderNum + "');\" title=\"删除\"></a>&nbsp;&nbsp;";
                            html += "<a class=\"icon-eye-open\" href=\"javascript:void(0)\" onclick=\"ReturnManager.Audite(1,'" + item.OrderNum + "')\" title=\"查看\"></a>&nbsp;&nbsp;";
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-ok\" href=\"javascript:void(0)\" onclick=\"ReturnManager.Audite(2,'" + item.OrderNum + "')\" title=\"审核\"></a>&nbsp;&nbsp;";
                            }
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: ReturnManager.PageClick });
                $("#chbSelectAll").attr("checked", false);
            }
        });
    },
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            ReturnManager.PageClick(1, 10);
        });
    },
    Delete: function (orderNum) {
        var param = {};
        param["OrderNum"] = orderNum;
        $.gitAjax({
            url: "/Returns/ProductManagerAjax/Delete",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.d != undefined) {
                    ReturnManager.PageClick(1);
                } else {
                    $.jBox.tip("操作失败,请联系系统管理员", "error");
                }
            }
        });
    },
    Audite: function (flag, orderNum) {
        // flag 1是查看详细 2是审核
        var submit = function (v, h, f) {
            if (flag == 2) {
                var Reason = h.find("#txtReason").val();
                var param = {};
                var status = 0;
                if (v == 1) {
                    status = 2
                } else if (v == 2) {
                    status = 3;
                }
                if (v != 3) {
                    param["OrderNum"] = orderNum;
                    param["Status"] = status;
                    param["Reason"] = Reason;
                    $.gitAjax({
                        url: "/Returns/ProductManagerAjax/Audit",
                        data: param,
                        type: "post",
                        dataType: "json",
                        success: function (result) {
                            if (result.d != undefined && result.d == "1000") {
                                $.jBox.tip("退货单审核成功", "success");
                                ReturnManager.PageClick(1, 10);
                            } else {
                                $.jBox.tip("退货单审核失败", "warn");
                            }
                        }
                    });
                }
            }
        };
        if (flag == 1) {
            $.jBox.open("get:/Returns/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "退货单详细", 800, 410, { buttons: { "关闭": 3 }, submit: submit });
        } else if (flag == 2) {
            $.jBox.open("get:/Returns/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "退货单审核", 800, 410, { buttons: { "审核通过": 1, "审核不通过": 2, "关闭": 3 }, submit: submit });
        }
    },
    Edit: function () {
        var OrderNum = $("#txtOrderNum").val();
        if (git.IsEmpty(OrderNum)) {
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        $.gitAjax({
            url: "/Returns/ProductAjax/Edit",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.d != undefined && result.d == "1001") {
                    $.jBox.tip("该退货单已经处理，不能重复编辑", "warn");
                    return;
                }
                if (result.data != undefined) {
                    var json = JSON.parse(result.data);
                    var html = "";
                    $(json.List).each(function (i, item) {
                        html += "<tr class=\"odd gradeX\">";
                        html += "<td><input type=\"checkbox\" ProductNum=\"" + item.ProductNum + "\" BarCode=\"" + item.BarCode + "\" LocalNum=\"" + item.LocalNum + "\" SnNum=\"" + item.SnNum + "\" /></td>";
                        html += "<td>" + item.ProductNum + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.LocalName + "</td>";
                        html += "<td>" + item.Num + "&nbsp;-&nbsp;" + item.BackNum + "&nbsp;=&nbsp;" + (item.Num - item.BackNum) + "</td>";
                        html += "<td><input type='text' class='input-small inde' data-max=\"" + (item.Num - item.BackNum) + "\" value=\"" + item.Qty + "\"/></td>";
                        html += "</tr>";
                    });
                    $("#tabInfo").children("tbody").html(html);
                }
            }
        });
    },
    DeleteBatch: function () {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var items = [];
                $("#tabInfo").children("tbody").find("input[type='checkbox']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var orderNum = $(item).val();
                        items.push(orderNum);
                    }
                });
                if (items.length == 0) {
                    $.jBox.tip("请选择要删除的退货单", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Returns/ProductManagerAjax/DeleteBatch", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            ReturnManager.PageClick(1);
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
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", true);
        }
        else {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    },
    ToExcel: function () {
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();

        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;

        $.gitAjax({
            url: "/Returns/ProductManagerAjax/ToExcel", type: "post", data: param, success: function (result) {
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
    }
};