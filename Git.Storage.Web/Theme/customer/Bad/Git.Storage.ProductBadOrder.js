
var ProductBadOrder = {
    OnkeyPress: function (num, obj) {
        var v = obj.value;
        if (isNaN(v)) {
            obj.value = "1";
        }
        if (v > num) {
            $.jBox.tip("报损数量不能大于库存数!", 'error');
            obj.value = num;
        }
    },
    SelectDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var tab = h.find("#TempTabInfo");
                var items = [];
                $(tab).children("tbody").children("tr").each(function (i, item) {
                    var flag = $(item).find("input[type='checkbox']").attr("checked");
                    if (flag || flag == "checked") {
                        var Num = $(item).find("input[type='text']").val();
                        var Sn = $(item).find("input[type='checkbox']").attr("Sn");
                        var StorageNum = $(item).find("input[type='checkbox']").attr("StorageNum");
                        var LocalNum = $(item).find("input[type='checkbox']").attr("LocalNum");
                        var ProductNum = $(item).find("input[type='checkbox']").attr("ProductNum");
                        var BarCode = $(item).find("input[type='checkbox']").attr("BarCode");
                        var LocalQty = $(item).find("input[type='checkbox']").attr("Num");
                        var BatchNum = $(item).find("input[type='checkbox']").attr("BatchNum");
                        var LocalProduct = {};
                        LocalProduct["Sn"] = Sn;
                        LocalProduct["StorageNum"] = StorageNum;
                        LocalProduct["LocalNum"] = LocalNum;
                        LocalProduct["ProductNum"] = ProductNum;
                        LocalProduct["BarCode"] = BarCode;
                        LocalProduct["Num"] = LocalQty;
                        LocalProduct["Qty"] = Num;
                        LocalProduct["BatchNum"] = BatchNum;
                        items.push(LocalProduct);
                    }
                });
                if (items.length == 0) {
                    $.jBox.tip("请选择要报损的产品", "warn");
                    return false;
                }
                var flag = false;
                for (var i = 0; i < items.length; i++) {
                    if (isNaN(items[i].Qty)) {
                        flag = true;
                    }
                }
                if (flag) {
                    $.jBox.tip("报算数量必须为数字且大于0", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Bad/ProductAjax/LoadProduct",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        ProductBadOrder.LoadDetail();
                    }
                });
            }
        };
        $.jBox.open("get:/Bad/Product/AddProduct", "报损产品", 800, 500, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                ProductBadOrder.AutoProduct($(item).find("#txtBarCode"), item);
            }
        });
    },
    LoadDetail: function () {
        $.gitAjax({
            url: "/Bad/ProductAjax/LoadLocalProduct",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.List != undefined) {
                    var html = "";
                    var json = JSON.parse(result.List);
                    $(json.Data).each(function (i, item) {
                        html += "<tr  class=\"odd gradeX\">";
                        html += "<td>" + item.ProductNum + "</td>";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.BatchNum + "</td>";
                        html += "<td>" + item.FromLocalName + "</td>";
                        html += "<td>" + item.Num + "</td>";
                        html += "<td>";
                        html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ProductBadOrder.Delete('" + item.SnNum + "');\" title=\"删除\"></a>";
                        html += "</td> ";
                        html += "</tr>";
                    });
                    $("#tabInfo").children("tbody").html(html);
                }
            }
        });
    },
    LoadTempLocal: function (SnNum) {
        var param = {};
        param["SnNum"] = SnNum;
        $.gitAjax({
            url: "/Bad/ProductAjax/GetLocalDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.List != undefined) {
                    var html = "";
                    var json = JSON.parse(result.List);
                    $(json.Data).each(function (i, item) {
                        html += "<tr  class=\"odd gradeX\">";
                        html += "<td class=\"inorder_search\"> ";
                        html += "<input type=\"checkbox\" name=\"product_item\" Sn=\"" + item.Sn + "\" StorageNum=\"" + item.StorageNum + "\" LocalNum=\"" + item.LocalNum + "\" ProductNum=\"" + item.ProductNum + "\" BarCode=\"" + item.BarCode + "\" Num=\"" + item.Num + "\" BatchNum=\"" + item.BatchNum + "\"/>";
                        html += "</td> ";
                        html += "<td class=\"inorder_search\">" + item.ProductName + "</td>";
                        html += "<td class=\"inorder_search\">" + item.BarCode + "</td>";
                        html += "<td class=\"inorder_search\">" + item.LocalName + "</td>";
                        html += "<td class=\"inorder_search\">" + item.BatchNum + "</td>";
                        html += "<td class=\"inorder_search\">" + item.Num + "</td>";
                        html += "<td class=\"inorder_search\"> ";
                        html += '<input type="text" value="1" class=\"span1\">';
                        html += "</td> ";
                        html += "</tr>";
                    });
                    $("#TempTabInfo").children("tbody").html(html);
                }
            }
        });
    },
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("input[name='product_item']").attr("checked", true);
        }
        else {
            $("input[name='product_item']").attr("checked", false);
        }
    },
    Delete: function (SnNum) {
        var param = {};
        param["SnNum"] = SnNum;
        $.gitAjax({
            url: "/Bad/ProductAjax/Delete",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                ProductBadOrder.LoadDetail();
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
                ProductBadOrder.LoadTempLocal(item.SnNum);
            }
        });
    },
    Add: function () {
        var BadType = $("#ddlBadType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var CrateUser = $("#txtCrateUser").val();
        var OrderTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(BadType)) {
            $.jBox.tip("请选择报损类型", "warn");
            return false;
        }
        if (git.IsEmpty(ProductType)) {
            $.jBox.tip("请选择入库产品类型", "warn");
            return false;
        }

        var param = {};
        param["BadType"] = BadType;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["CrateUser"] = CrateUser;
        param["OrderTime"] = OrderTime;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/Bad/ProductAjax/Create",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.d != undefined && result.d == "Success") {
                    $.jBox.tip("报损单创建成功", "success");
                    ProductBadOrder.LoadDetail();
                    ProductBadOrder.Cancel();
                    
                } else {
                    $.jBox.tip("报损单创建失败", "error");
                }
            }
        });
    },
    Edit: function () {
        var OrderNum = $("#txtOrderNum").val();
        var BadType = $("#ddlBadType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var CrateUser = $("#txtCrateUser").val();
        var OrderTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(BadType)) {
            $.jBox.tip("请选择报损类型", "warn");
            return false;
        }
        if (git.IsEmpty(ProductType)) {
            $.jBox.tip("请选择入库产品类型", "warn");
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        param["BadType"] = BadType;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["CrateUser"] = CrateUser;
        param["OrderTime"] = OrderTime;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/Bad/ProductAjax/Edit",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.d != undefined && result.d == "Success") {
                    $.jBox.tip("报损单编辑成功", "success");
                } else {
                    $.jBox.tip("报损单编辑失败", "error");
                }
            }
        });
    },
    Cancel: function () {
        $("#txtContractOrder").val("");
        $("#txtOrderTime").val("");
        $("#txtRemark").val("");
    }
};


var ProductBadManager = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var status = $("#btnStatusGroup").find(".disabled").val();
        var badType = $("#ddlBadType").val();
        var orderNum = $("#txtOrderNum").val();
        var productType = $("#ddlProductType").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();

        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["BadType"] = badType;
        param["ProductType"] = productType;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Bad/ProductManagerAjax/GetList",
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
                            html += "<td><input type=\"checkbox\" name=\"user_item\" value=\"" + item.OrderNum + "\"/></td>";
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + git.GetEnumDesc(EBadType, item.BadType) + "</td>";
                            html += "<td>" + item.ContractOrder + "</td>";
                            html += "<td>" + item.Num + "</td>";
                            html += "<td>" + git.GetEnumDesc(EAudite, item.Status) + "</td>";
                            html += "<td>" + git.GetEnumDesc(EOpType, item.OperateType) + "</td>";
                            html += "<td>" + item.CreateUser + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            html += "<td>";
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-edit\" href=\"/Bad/Product/Edit?orderNum=" + item.OrderNum + "\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            }
                            html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ProductBadManager.Delete('" + item.OrderNum + "');\" title=\"删除\"></a>&nbsp;&nbsp;";
                            html += "<a class=\"icon-eye-open\" href=\"javascript:void(0)\" onclick=\"ProductBadManager.Audite(1,'" + item.OrderNum + "')\" title=\"查看\"></a>&nbsp;&nbsp;";
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-ok\" href=\"javascript:void(0)\" onclick=\"ProductBadManager.Audite(2,'" + item.OrderNum + "')\" title=\"审核\"></a>&nbsp;&nbsp;";
                            }
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: ProductBadManager.PageClick });
            }
        });
    },
    ToExcel: function () {
        var status = $("#btnStatusGroup").find(".disabled").val();
        var badType = $("#ddlBadType").val();
        var orderNum = $("#txtOrderNum").val();
        var productType = $("#ddlProductType").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();

        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["BadType"] = badType;
        param["ProductType"] = productType;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
       
        $.gitAjax({
            url: "/Bad/ProductManagerAjax/ToExcel", type: "post", data: param, success: function (result) {
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
            ProductBadManager.PageClick(1, 10);
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
                    $.jBox.tip("请选择要删除的报损单", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Bad/ProductManagerAjax/DeleteBatch", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            ProductBadManager.PageClick(1);
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
                url: "/Bad/ProductManagerAjax/Delete",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    if (result.d != undefined && result.d == "Success") {
                        ProductBadManager.PageClick(1, 10);
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
                var reason = h.find("#txtReason").val();
                var param = {};
                var status = 0;
                if (v == 1) {
                    status = 2
                } else if (v == 2) {
                    status = 3;
                } else if (v == 3) {
                    //关闭浮动层
                }
                param["OrderNum"] = orderNum;
                param["Status"] = status;
                param["Reason"] = reason;
                if (v == 1 || v == 2) {
                    $.gitAjax({
                        url: "/Bad/ProductManagerAjax/Audit",
                        data: param,
                        type: "post",
                        dataType: "json",
                        success: function (result) {
                            if (result.d != undefined && result.d == "1000") {
                                $.jBox.tip("操作成功", "success");
                                ProductBadManager.PageClick(1, 10);
                            } else {
                                $.jBox.tip("操作失败", "error");
                            }
                        }
                    });
                }
            }
        };
        if (flag == 1) {
            $.jBox.open("get:/Bad/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "报损单详细", 800, 500, { buttons: { "关闭": false }, submit: submit });
        } else if (flag == 2) {
            $.jBox.open("get:/Bad/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "报损单审核", 800, 500, { buttons: { "审核通过": 1, "审核不通过": 2, "关闭": 3 }, submit: submit });
        }
    }
};
