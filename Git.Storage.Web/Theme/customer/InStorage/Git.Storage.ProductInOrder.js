
var orderProduct = {
    Load: function () {
        orderProduct.AutoSup();
        orderProduct.LoadDetail();

        $("#txtSupNum").SupplierDialog({
            data: undefined, Mult: false, callBack: function (result) {
                $("#txtSupNum").val(result.SupNum);
                $("#txtSupName").val(result.SupName);
                $("#txtContactName").val(result.ContactName);
                $("#txtSupPhone").val(result.Phone);
            }
        });
    },
    SelectDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var SnNum = h.find("#hdProductNum").val();
                var BarCode = h.find("#txtBarCode").val();
                var ProductName = h.find("#txtProductName").val();
                var Size = h.find("#txtSize").val();
                var Price = h.find("#txtPrice").val();
                var LocalQty = h.find("#txtLocalQty").val();
                var ProductBatch = h.find("#txtProductBatch").val();
                var LocalNum = h.find("#txtLocalNum").val();
                var Num = h.find("#txtNum").val();
                if (git.IsEmpty(BarCode)) {
                    $.jBox.tip("请输入入库的产品", "warn");
                    return false;
                }
                if (git.IsEmpty(Num)) {
                    $.jBox.tip("请输入入库产品数量", "warn");
                    return false;
                }
                if (isNaN(Num)) {
                    $.jBox.tip("您输入的入库产品数量必须为数字", "warn");
                    return false;
                }
                if (Num <= 0) {
                    $.jBox.tip("入库产品数必须大于0", "warn");
                    return false;
                }
                if (git.IsEmpty(LocalNum)) {
                    $.jBox.tip("请选择入库库位", "warn");
                    return false;
                }
                var param = {};
                param["SnNum"] = SnNum;
                param["BarCode"] = BarCode;
                param["ProductName"] = ProductName;
                param["Size"] = Size;
                param["Price"] = Price;
                param["LocalQty"] = LocalQty;
                param["ProductBatch"] = ProductBatch;
                param["LocalNum"] = LocalNum;
                param["Num"] = Num;
                //提交到缓存处理
                $.gitAjax({
                    url: "/InStorage/ProductAjax/AddProduct",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        orderProduct.LoadDetail();
                    }
                });
            }
        };
        $.jBox.open("get:/InStorage/Product/AddProduct", "入库产品", 400, 410, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                orderProduct.AutoProduct($(item).find("#txtBarCode"), item);
                $(item).find("#txtBarCode").ProductDialog({
                    data: undefined, Mult: false, callBack: function (result) {
                        $(item).find("#txtBarCode").val(result.BarCode);
                        $(item).find("#txtProductName").val(result.ProductName);
                        $(item).find("#txtSize").val(unescape(result.Size));
                        $(item).find("#txtPrice").val(git.ToDecimal(result.InPrice,2));
                        $(item).find("#txtLocalQty").val(result.Num);
                        $(item).find("#hdProductNum").val(result.SnNum);
                        $(item).find("#spanUnitName1").text(result.UnitName);
                        $(item).find("#spanUnitName2").text(result.UnitName);
                    }
                });
                $(item).find("#txtLocalName").LocalDialog({
                    data: undefined, Mult: false, callBack: function (result) {
                        $(item).find("#txtLocalName").val(result.LocalName);
                        $(item).find("#txtLocalNum").val(result.LocalNum);
                    }
                });
            }
        });
    },
    LoadDetail: function () {
        $.gitAjax({
            url: "/InStorage/ProductAjax/LoadProduct",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.List != undefined) {
                    var html = "";
                    var json = JSON.parse(result.List);
                    $(json.Data).each(function (i, item) {
                        html += "<tr sn='" + item.SnNum + "'>";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.Size + "</td>";
                        html += "<td>" + item.BatchNum + "</td>";
                        html += "<td>" + git.ToDecimal(item.InPrice,2)+ "&nbsp;元</td>";
                        html += "<td>" + item.Num + "</td>";
                        html += "<td>" + git.ToDecimal(item.TotalPrice, 2) + "&nbsp;元</td>";
                        html += "<td>" + item.LocalName + "</td>";
                        html += "<td>";
                        html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"orderProduct.EditNum('" + item.SnNum + "'," + item.Num + ")\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"orderProduct.DelDetail('" + item.SnNum + "')\" title=\"删除\"></a>";
                        html += "</td>";
                        html += "</tr>";
                    });
                    $("#tabInfo").children("tbody").html(html);
                }
            }
        });
    },
    EditNum: function (snNum, num) {
        var html = "<div style='padding:10px;'>数量：<input type='text' id='txtNum' name='txtNum' value='" + num + "' /></div>";
        var submit = function (v, h, f) {
            if (f.txtNum == '' || isNaN(f.txtNum)) {
                $.jBox.tip("请输入数量。", 'error', { focusId: "txtNum" });
                return false;
            }
            var param = {};
            param["SnNum"] = snNum;
            param["num"] = f.txtNum;
            $.gitAjax({
                url: "/InStorage/ProductAjax/EditNum",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    orderProduct.LoadDetail();
                }
            });
            return true;
        };
        $.jBox(html, { title: "修改数量", submit: submit });
    },
    DelDetail: function (snNum) {
        var param = {};
        param["SnNum"] = snNum;
        $.gitAjax({
            url: "/InStorage/ProductAjax/DelDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                orderProduct.LoadDetail();
            }
        });
    },
    AutoSup: function () {
        $("#txtSupNum").autocomplete({
            paramName: "supName",
            url: '/Client/SupplierAjax/Auto',
            showResult: function (value, data) {
                var row = JSON.parse(value);
                return '<span>' + row.SupNum + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + row.SupName + '</span>';
            },
            onItemSelect: function (item) {

            },
            maxItemsToShow: 5,
            selectedCallback: function (item) {
                $("#txtSupNum").val(item.SupNum);
                $("#txtSupName").val(item.SupName);
                $("#txtContactName").val(item.ContactName);
                $("#txtSupPhone").val(item.Phone);
            }
        });
    },
    AutoProduct: function (item, target) {
        $(item).autocomplete({
            paramName: "productName",
            url: '/Product/GoodsAjax/AutoProduct',
            showResult: function (value, data) {
                var row = JSON.parse(value);
                return '<span>' + row.BarCode + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + row.ProductName + '</span>';
            },
            onItemSelect: function (item) {

            },
            maxItemsToShow: 5,
            selectedCallback: function (selectItem) {
                $(target).find("#txtBarCode").val(selectItem.BarCode);
                $(target).find("#txtProductName").val(selectItem.ProductName);
                $(target).find("#txtSize").val(selectItem.Size);
                $(target).find("#txtPrice").val(selectItem.InPrice);
                $(target).find("#txtLocalQty").val(selectItem.Num);
                $(target).find("#hdProductNum").val(selectItem.SnNum);
                $(target).find("#spanUnitName1").text(selectItem.UnitName);
                $(target).find("#spanUnitName2").text(selectItem.UnitName);
                var param = {};
                param["productNum"] = selectItem.SnNum;
                $.gitAjax({
                    url: "/InStorage/ProductAjax/GetLocalNum",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        $(target).find("#txtLocalQty").val(result.Sum);
                    }
                });
            }
        });
    },
    Add: function () {
        var InType = $("#ddlInType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var SupNum = $("#txtSupNum").val();
        var SupName = $("#txtSupName").val();
        var ContactName = $("#txtContactName").val();
        var Phone = $("#txtSupPhone").val();
        var OrderTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(InType)) {
            $.jBox.tip("请选择入库单类型", "warn");
            return false;
        }
        if (git.IsEmpty(ProductType)) {
            $.jBox.tip("请选择入库产品类型", "warn");
            return false;
        }
        if (git.IsEmpty(SupNum)) {
            $.jBox.tip("请选择供应商", "warn");
            return false;
        }

        var param = {};
        param["InType"] = InType;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["SupNum"] = SupNum;
        param["SupName"] = SupName;
        param["ContactName"] = ContactName;
        param["Phone"] = Phone;
        param["OrderTime"] = OrderTime;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/InStorage/ProductAjax/Create",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("入库单创建成功", "success");
                    orderProduct.Cancel();
                } else {
                    $.jBox.tip("入库单创建失败", "error");
                }
            }
        });
    },
    Edit: function () {
        var InType = $("#ddlInType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var SupNum = $("#txtSupNum").val();
        var SupName = $("#txtSupName").val();
        var ContactName = $("#txtContactName").val();
        var Phone = $("#txtSupPhone").val();
        var OrderTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        var orderNum = $("#txtOrderNum").val();
        if (git.IsEmpty(InType)) {
            $.jBox.tip("请选择入库单类型", "warn");
            return false;
        }
        //if (git.IsEmpty(ProductType)) {
        //    $.jBox.tip("请选择入库产品类型", "warn");
        //    return false;
        //}
        if (git.IsEmpty(SupNum)) {
            $.jBox.tip("请选择供应商", "warn");
            return false;
        }

        var param = {};
        param["OrderNum"] = orderNum;
        param["InType"] = InType;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["SupNum"] = SupNum;
        param["SupName"] = SupName;
        param["ContactName"] = ContactName;
        param["Phone"] = Phone;
        param["OrderTime"] = OrderTime;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/InStorage/ProductAjax/Edit",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("入库单编辑成功", "success");
                } else {
                    $.jBox.tip("入库单编辑失败", "error");
                }
            }
        });
    },
    Cancel: function () {
        $.gitAjax({
            url: "/InStorage/ProductAjax/Cancel",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                orderProduct.LoadDetail();
            }
        });
        $("#txtContractOrder").val("");
        $("#txtSupNum").val("");
        $("#txtSupName").val("");
        $("#txtContactName").val("");
        $("#txtSupPhone").val("");
        $("#txtOrderTime").val("");
        $("#txtRemark").val("");
    }
};


var InStorageManager = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var supNum = $("#txtSupplier").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var InType = $("#ddlInType").val();
        var planNum = $("#txtPlanNum").val();
        var ReprtNum = $("#txtReprtNum").val();
        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["SupName"] = supNum;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;
        param["InType"] = InType;
        param["planNum"] = planNum;
        $.gitAjax({
            url: "/InStorage/ProductManagerAjax/GetList",
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
                            html += "<td><input type=\"checkbox\" value=\"" + item.OrderNum + "\"/></td>";
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + git.GetEnumDesc(EInType, item.InType) + "</td>";
                            html += "<td>" + item.SupName + "</td>";
                            html += "<td>" + item.ContractOrder + "</td>";
                            html += "<td>" + item.Num + "</td>";
                            html += "<td>" + git.ToDecimal(item.Amount,2) + "&nbsp;元</td>";
                            html += "<td>" + git.GetEnumDesc(EAudite, item.Status) + "</td>";
                            html += "<td>" + item.CreateUserName + "</td>";
                            html += "<td>" + git.GetEnumDesc(EOpType, item.OperateType) + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            html += "<td>";
                            if (item.Status == EAuditeJson.Wait || item.Status == EAuditeJson.NotPass) {
                                html += "<a class=\"icon-edit\" href=\"/InStorage/Product/Edit?orderNum=" + item.OrderNum + "\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            }
                            html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"InStorageManager.Delete('" + item.OrderNum + "');\" title=\"删除\"></a>&nbsp;&nbsp;";
                            html += "<a class=\"icon-eye-open\" href=\"javascript:void(0)\" onclick=\"InStorageManager.Audite(1,'" + item.OrderNum + "')\" title=\"查看\"></a>&nbsp;&nbsp;";
                            
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-ok\" href=\"javascript:void(0)\" onclick=\"InStorageManager.Audite(2,'" + item.OrderNum + "')\" title=\"审核\"></a>&nbsp;&nbsp;";
                            }

                            html += "<a class=\"icon-print\" href=\"/Report/Manager/Show?ReportNum=" + ReprtNum + "&OrderNum=" + item.OrderNum + "\" title=\"打印\"></a>&nbsp;&nbsp;";

                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: InStorageManager.PageClick });
            }
        });
    },
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            InStorageManager.PageClick(1, 10);
        });
    },
    Delete: function (orderNum) {
        if (!git.IsEmpty(orderNum)) {
            var param = {};
            param["OrderNum"] = orderNum;
            $.gitAjax({
                url: "/InStorage/ProductManagerAjax/Delete",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    if (result.d != undefined && result.d == "Success") {
                        InStorageManager.PageClick(1, 10);
                    }
                }
            });
        }
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
                    $.jBox.tip("请选择要删除的入库单", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/InStorage/ProductManagerAjax/DeleteBatch", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            InStorageManager.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    Audite: function (flag, orderNum) {
        // flag 1是查看详细 2是审核
        var submit = function (v, h, f) {
            if (flag == 1) {

            } else if (flag == 2) {
                var param = {};
                var status = 0;
                if (v == 1) {
                    status = 2
                } else if (v == 2) {
                    status = 3;
                }
                if (v != 3) {
                    var reason = h.find("#txtReason").val();
                    param["OrderNum"] = orderNum;
                    param["Status"] = status;
                    param["Reason"] = reason;
                    $.gitAjax({
                        url: "/InStorage/ProductManagerAjax/Audite",
                        data: param,
                        type: "post",
                        dataType: "json",
                        success: function (result) {
                            if (result.d != undefined && result.d == "1000") {
                                $.jBox.tip("操作成功", "success");
                            } else {
                                $.jBox.tip("操作失败", "warn");
                            }
                            InStorageManager.PageClick(1, 10);
                        }
                    });
                }
            }
        };
        if (flag == 1) {
            $.jBox.open("get:/InStorage/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "入库单详细", 660, 410, { buttons: { "关闭": 3 }, submit: submit });
        } else if (flag == 2) {
            $.jBox.open("get:/InStorage/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "入库单审核", 660, 410, { buttons: { "审核通过": 1, "审核不通过": 2, "关闭": 3 }, submit: submit });
        }
    },
    ToExcel: function () {
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var supNum = $("#txtSupplier").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["SupName"] = supNum;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;

        $.gitAjax({
            url: "/InStorage/ProductManagerAjax/ToExcel", type: "post", data: param, success: function (result) {
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
    },
    AllToExcel: function () {
        var param = {};
        $.gitAjax({
            url: "/InStorage/ProductManagerAjax/ToExcel", type: "post", data: param, success: function (result) {
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
    },
    SearchEvent: function () {
        $("#btnHSearch").click(function () {
            var flag = $("#divHSearch").css("display");
            if (flag == "none") {
                $("#txtHOrderNum").val("");
                $("#txtHBeginTime").val("");
                $("#txtHEndTime").val("");
                $("#divHSearch").slideDown("slow");
            } else {
                $("#divHSearch").slideUp("slow");
            }
        });
    },
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", true);
        }
        else {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    }
};