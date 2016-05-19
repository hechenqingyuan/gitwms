
var ProductMoveOrder = {
    SelectDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var items = [];
                var tab = h.find("#TempTabInfo");
                $(tab).children("tbody").children("tr").each(function (i, item) {
                    var flag = $(item).find("input[type='checkbox']").attr("checked");
                    if (flag || flag == "checked") {
                        var detail = {};
                        var BarCode = $(item).find("input[type='checkbox']").attr("data-BarCode");
                        var ProductNum = $(item).find("input[type='checkbox']").attr("data-ProductNum");
                        var FromLocalNum = $(item).find("input[type='checkbox']").attr("data-LocalNum");
                        var StorageNum = $(item).find("input[type='checkbox']").attr("data-StorageNum");
                        var BatchNum = $(item).find("input[type='checkbox']").attr("data-BatchNum");
                        var ProductName = $(item).find("input[type='checkbox']").attr("data-ProductName");
                        var ToLocalNum = $(item).find("input[name='LocalNum']").val();
                        var ToLocalName = $(item).find("input[name='LocalName']").val();
                        var Num = $(item).find(".span1").val();

                        detail["BarCode"] = BarCode;
                        detail["ProductNum"] = ProductNum;
                        detail["Num"] = Num;
                        detail["FromLocalNum"] = FromLocalNum;
                        detail["ToLocalNum"] = ToLocalNum;
                        detail["StorageNum"] = StorageNum;
                        detail["BatchNum"] = BatchNum;
                        detail["ProductName"] = ProductName;
                        items.push(detail);
                    }
                });
                if (items.length == 0) {
                    $.jBox.tip("请选择要移库的产品", "warn");
                    return false;
                }
                for (var i = 0; i < items.length; i++) {
                    if (git.IsEmpty(items[i].Num)) {
                        $.jBox.tip("请输入[" + items[i].ProductName + "]要移库的数量", "warn");
                        return false;
                    }
                }

                var param = {};
                param["List"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Move/ProductAjax/LoadProduct",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        ProductMoveOrder.LoadDetail();
                    }
                });
            }
        };
        $.jBox.open("get:/Move/Product/AddProduct", "移库产品", 800, 500, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                ProductMoveOrder.AutoProduct($(item).find("#txtBarCode"), item);
            }
        });
    },
    LoadDetail: function () {
        $.gitAjax({
            url: "/Move/ProductAjax/LoadLocalProduct",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.List != undefined) {
                    var html = "";
                    var json = JSON.parse(result.List);
                    $(json.Data).each(function (i, item) {
                        html += "<tr>";
                        html += "<td>" + item.ProductNum + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.BatchNum + "</td>";
                        html += "<td>" + item.Num + "</td>";
                        html += "<td>" + item.FromLocalName + "</td>";
                        html += "<td>" + item.ToLocalName + "</td>";
                        html += "<td>";
                        html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ProductMoveOrder.Delete('" + item.SnNum + "');\" title=\"删除\"></a>";
                        html += "</td> ";
                        html += "</tr>";
                    });
                    $("#tabInfo").children("tbody").html(html);
                }
            }
        });
    },
    LoadTempLocal: function (SnNum) {
        //加载库存产品信息，弹出框选择产品移库
        var param = {};
        param["SnNum"] = SnNum;
        $.gitAjax({
            url: "/Move/ProductAjax/GetLocalDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.List != undefined) {
                    var html = "";
                    var json = JSON.parse(result.List);
                    $(json.Data).each(function (i, item) {
                        html += "<tr>";
                        html += "<td> ";
                        html += "<input type=\"checkbox\" data-BatchNum=\"" + item.BatchNum + "\" data-StorageNum=\"" + item.StorageNum + "\" data-ProductName=\"" + item.ProductName + "\" data-BarCode=\"" + item.BarCode + "\" data-ProductNum=\"" + item.ProductNum + "\" data-LocalNum=\"" + item.LocalNum + "\"/>";
                        html += "</td> ";
                        html += "<td class=\"inorder_search\">" + item.ProductName + "</td>";
                        html += "<td class=\"inorder_search\">" + item.BarCode + "</td>";
                        html += "<td class=\"inorder_search\">" + item.ProductNum + "</td>";
                        html += "<td class=\"inorder_search\">" + item.BatchNum + "</td>";
                        html += "<td class=\"inorder_search\">" + item.LocalName + "</td>";
                        html += "<td class=\"inorder_search\">" + item.Num + "</td>";
                        html += "<td class=\"inorder_search\"> ";
                        html += "<input type=\"text\"  value=\"\" class=\"span1\"/>";
                        html += "</td> ";
                        html += "<td> ";
                        html += "<input type=\"text\"  name=\"LocalName\" value=\"\" class=\"span2\"/>";
                        html += "<input type=\"hidden\" name=\"LocalNum\"  value=\"\" class=\"span2\"/>";
                        html += "</td> ";
                        html += "</tr>";
                    });
                    $("#TempTabInfo").children("tbody").html(html);

                    $("input[name='LocalName']").each(function () {
                        $(this).LocalDialog({
                            data: undefined, Mult: false, callBack: function (result, self) {
                                $(self).val(result.LocalName);
                                var parent = $(self).parent();
                                $(parent).children("input[name='LocalNum']").val(result.LocalNum);
                            }
                        })
                        ;
                    });

                }
            }
        });
    },
    SelectAll: function (item) {

        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("#TempTabInfo").children("tbody").find("input[type='checkbox']").attr("checked", true);
        }
        else {
            $("#TempTabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    },
    Delete: function (SnNum) {
        var param = {};
        param["SnNum"] = SnNum;
        $.gitAjax({
            url: "/Move/ProductAjax/Delete",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                ProductMoveOrder.LoadDetail();
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
            selectedCallback: function (item) {
                $(target).find("#txtBarCode").val(item.BarCode);
                ProductMoveOrder.LoadTempLocal(item.SnNum);
            }
        });
    },
    Add: function () {
        var OrderNum = $("#txtOrderNum").val();
        var MoveType = $("#ddlMoveType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var OrderTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(MoveType)) {
            $.jBox.tip("请选择移库类型", "warn");
            return false;
        }
        if (git.IsEmpty(ProductType)) {
            $.jBox.tip("请选择产品类型", "warn");
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        param["MoveType"] = MoveType;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["OrderTime"] = OrderTime;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/Move/ProductAjax/Create",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.d == "1006") {
                    $.jBox.tip("请选择要移库的产品", "warn");
                    return false;
                }
                if (git.IsEmpty(OrderNum)) {
                    if (result.d == "Success") {
                        $.jBox.tip("移库单创建成功", "success");
                        ProductMoveOrder.LoadDetail();
                        ProductMoveOrder.Cancel();
                    } else {
                        $.jBox.tip("移库单创建失败", "warn");
                    }
                } else {
                    if (result.d == "Success") {
                        $.jBox.tip("移库单编辑成功", "success");
                    } else {
                        $.jBox.tip("移库单编辑成功", "warn");
                    }
                }
            }
        });
    },
    Cancel: function () {
        $("#txtContractOrder").val("");
        $("#txtRemark").val("");
        $("#txtOrderTime").val("");
    }
};


var ProductMoveManager = {
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
            url: "/Move/ProductManagerAjax/GetList",
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
                            html += "<td><input type=\"checkbox\" name=\"user_item\" data=\"" + item.OrderNum + "\" value=\"" + item.OrderNum + "\"/></td>";
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + git.GetEnumDesc(EMoveType, item.MoveType) + "</td>";
                            html += "<td>" + item.ContractOrder + "</td>";
                            html += "<td>" + item.Num + "</td>";
                            html += "<td>" + item.CreateUserName + "</td>";
                            html += "<td>" + git.GetEnumDesc(EAudite, item.Status) + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            html += "<td>";
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-edit\" href=\"/Move/Product/Edit?orderNum=" + item.OrderNum + "\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            }
                            html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ProductMoveManager.Delete('" + item.OrderNum + "');\" title=\"删除\"></a>&nbsp;&nbsp;";
                            html += "<a class=\"icon-eye-open\" href=\"javascript:void(0)\" onclick=\"ProductMoveManager.Audite(1,'" + item.OrderNum + "')\" title=\"查看\"></a>&nbsp;&nbsp;";
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-ok\" href=\"javascript:void(0)\" onclick=\"ProductMoveManager.Audite(2,'" + item.OrderNum + "')\" title=\"审核\"></a>&nbsp;&nbsp;";
                            }
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: ProductMoveManager.PageClick });
            }
        });
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
            url: "/Move/ProductManagerAjax/ToExcel", type: "post", data: param, success: function (result) {
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
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            ProductMoveManager.PageClick(1, 10);
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
    BatchDel: function () {
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
                    $.jBox.tip("请选择要删除的移库单", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Move/ProductManagerAjax/DeleteBatch", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            ProductMoveManager.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    Delete: function (orderNum) {
        if (!git.IsEmpty(orderNum)) {
            var param = {};
            param["OrderNum"] = orderNum;
            $.gitAjax({
                url: "/Move/ProductManagerAjax/Delete",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    if (result.d != undefined && result.d == "Success") {
                        ProductMoveManager.PageClick(1, 10);
                    }
                }
            });
        }
    },
    Audite: function (flag, orderNum) {
        // flag 1是查看详细 2是审核
        var submit = function (v, h, f) {
            if (flag == 1) {

            } else if (flag == 2) {
                var status = 0;
                if (v == 1) {
                    status = 2
                } else if (v == 2) {
                    status = 3;
                }
                if (v != 3) {
                    var reason = h.find("#txtReason").val();
                    var param = {};
                    param["OrderNum"] = orderNum;
                    param["Status"] = status;
                    param["Reason"] = reason;
                    $.gitAjax({
                        url: "/Move/ProductManagerAjax/Audit",
                        data: param,
                        type: "post",
                        dataType: "json",
                        success: function (result) {
                            if (result.d != undefined && result.d == "1000") {
                                $.jBox.tip("移库单审核成功", "success");
                                ProductMoveManager.PageClick(1, 10);
                            } else {
                                $.jBox.tip("移库单审核失败", "warn");
                            }
                        }
                    });
                }
            }
        };
        if (flag == 1) {
            $.jBox.open("get:/Move/Product/Detail?flag=1&orderNum=" + orderNum, "移库单详细", 800, 500, { buttons: { "关闭": 3 }, submit: submit });
        } else if (flag == 2) {
            $.jBox.open("get:/Move/Product/Detail?flag=2&orderNum=" + orderNum, "移库单审核", 800, 500, { buttons: { "审核通过": 1, "审核不通过": 2, "关闭": 3 }, submit: submit });
        }
    }
};
