var juploader = null;
var OrderManageProduct = {
    Load: function () {
        OrderManageProduct.AutoCus();
        OrderManageProduct.LoadDetail();
    },
    SelectDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var SnNum = h.find("#hdProductNum").val();
                var BarCode = h.find("#txtBarCode").val();
                var ProductName = h.find("#txtProductName").val();
                var UnitNum = $("#txtUnitNum").val();
                var Size = h.find("#txtSize").val();
                var Price = h.find("#txtPrice").val();
                var UnitName = h.find("#txtUnitNum option:selected").text();
                var Num = h.find("#txtNum").val();
                var Remark = h.find("#txtRemark").val();

                if (git.IsEmpty(Num)) {
                    $.jBox.tip("请输入订单数量", "warn");
                    return false;
                }
                if (isNaN(Num)) {
                    $.jBox.tip("您输入的订单数量必须为数字", "warn");
                    return false;
                }
                if (Num <= 0) {
                    $.jBox.tip("订单数量数必须大于0", "warn");
                    return false;
                }

                var param = {};
                param["SnNum"] = SnNum;
                param["BarCode"] = BarCode;
                param["ProductName"] = ProductName;
                param["Size"] = Size;
                param["Price"] = Price;
                param["UnitName"] = UnitName;
                param["UnitNum"] = UnitNum;
                param["Num"] = Num;
                param["Remark"] = Remark;

                //提交到缓存处理
                $.gitAjax({
                    url: "/Order/OrderAjax/AddProduct",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        OrderManageProduct.LoadDetail();
                    }
                });
            }
        };
        $.jBox.open("get:/Order/OrderManage/AddProduct", "订单信息", 400, 330, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                OrderManageProduct.AutoProduct($(item).find("#txtBarCode"), item);
            }
        });
    },
    LoadDetail: function () {
        $.gitAjax({
            url: "/Order/OrderAjax/LoadProduct",
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
                        html += "<td>" + item.Num + "</td>";
                        html += "<td>" + item.UnitName + "</td>";
                        html += "<td>" + item.Size + "</td>";
                        html += "<td>" + item.Remark + "</td>";
                        html += "<td><a href='javascript:void(0)' onclick=\"OrderManageProduct.DelDetail('" + item.SnNum + "')\">删除</a></td>";
                        html += "</tr>";
                    });
                    $("#tabInfo").children("tbody").html(html);
                }
            }
        });
    },
    DelDetail: function (snNum) {
        var param = {};
        param["SnNum"] = snNum;
        $.gitAjax({
            url: "/Order/OrderAjax/DelDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                OrderManageProduct.LoadDetail();
            }
        });
    },
    AutoCus: function () {
        $("#txtCusNum").autocomplete({
            paramName: "cusName",
            url: '/Client/CustomerAjax/Auto',
            showResult: function (value, data) {
                var row = JSON.parse(value);
                return '<span>' + row.CusNum + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + row.CusName + '</span>';
            },
            onItemSelect: function (item) {

            },
            maxItemsToShow: 5,
            selectedCallback: function (item) {
                $("#txtCusNum").val(item.CusNum);
                $("#txtCusName").val(item.CusName);
                OrderManageProduct.CusNameChange();
            }
        });
    },
    CusNameChange: function () {
        var Local = $("#ddlAddress");
        Local.empty();
        var CusNum = $("#txtCusNum").val();
        var param = {};
        param["CusNum"] = CusNum;
        $.gitAjax({
            url: "/Client/CustomerAjax/GetSelectAddress",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        var option = $("<option>").text(item.Address).val(item.SnNum).attr("data-Contact", item.Contact).attr("data-Phone", item.Phone);
                        Local.append(option);

                        var address = $("#hdAddress").val();
                        if (address != undefined && address != "") {
                            $(Local).children("option").each(function (i, child) {
                                if ($(child).text() == address) {
                                    $(child).attr("selected", true);
                                }
                            });
                        }
                        $(Local).change(function () {
                            var Contact = $(this).find("option:selected").attr("data-Contact");
                            var Phone = $(this).find("option:selected").attr("data-Phone");
                            $("#txtCusPhone").val(Phone);
                            $("#txtContactName").val(Contact);
                        });
                    });
                }
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
        var OrderNum = $("#txtOrderNum").val();
        var OrderType = $("#ddlOrderType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var ContactName = $("#txtContactName").val();
        var CusNum = $("#txtCusNum").val();
        var CusName = $("#txtCusName").val();
        var Address = $("#ddlAddress option:selected").text();
        var CusPhone = $("#txtCusPhone").val();
        var CrateUser = $("#txtCrateUser").val();
        var OrderTime = $("#txtOrderTime").val();
        var SendDate = $("#txtSendDate").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(OrderType)) {
            $.jBox.tip("请选择订单类型", "warn");
            return false;
        }
        if (git.IsEmpty(CusNum)) {
            $.jBox.tip("请填写客户信息", "warn");
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        param["OrderType"] = OrderType;
        param["ContractOrder"] = ContractOrder;
        param["ContactName"] = ContactName;
        param["CusNum"] = CusNum;
        param["CusName"] = CusName;
        param["Address"] = Address;
        param["Phone"] = CusPhone;
        param["CrateUser"] = CrateUser;
        param["OrderTime"] = OrderTime;
        param["SendDate"] = SendDate;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/Order/OrderAjax/Create",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("入库单创建成功", "success");
                    OrderManageProduct.Cancel();
                } else {
                    $.jBox.tip("入库单创建失败", "error");
                }
            }
        });
    },
    CreateBat: function () {
        //批量生成订单
        var items = [];
        $("input[name='order_item']").each(function (i, item) {
            var flag = $(item).attr("checked");
            if (flag == "checked" || flag) {
                var orderNum = $(item).attr("data");
                items.push(orderNum);
            }
        });
        var param = {};
        param["List"] = JSON.stringify(items);
        $.gitAjax({
            url: "/Order/OrderAjax/CreateBat",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("入库单创建成功", "success");
                    ImportFileList.PageClick();
                } else {
                    $.jBox.tip("入库单创建失败", "error");
                }
            }
        });
    },
    Edit: function () {
        var OrderNum = $("#txtOrderNum").val();
        var OrderType = $("#ddlOrderType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var ContactName = $("#txtContactName").val();
        var CusNum = $("#txtCusNum").val();
        var CusName = $("#txtCusName").val();
        var Address = $("#ddlAddress").val();
        var CusPhone = $("#txtCusPhone").val();
        var CrateUser = $("#txtCrateUser").val();
        var OrderTime = $("#txtOrderTime").val();
        var SendDate = $("#txtSendDate").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(OrderType)) {
            $.jBox.tip("请选择订单类型", "warn");
            return false;
        }
        if (git.IsEmpty(CusNum)) {
            $.jBox.tip("请填写客户信息", "warn");
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        param["OrderType"] = OrderType;
        param["ContractOrder"] = ContractOrder;
        param["ContactName"] = ContactName;
        param["CusNum"] = CusNum;
        param["CusName"] = CusName;
        param["Address"] = Address;
        param["Phone"] = CusPhone;
        param["CrateUser"] = CrateUser;
        param["OrderTime"] = OrderTime;
        param["SendDate"] = SendDate;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/Order/OrderAjax/Edit",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("订单编辑成功", "success");
                } else {
                    $.jBox.tip("订单编辑失败", "error");
                }
            }
        });
    },
    Cancel: function () {
        $.gitAjax({
            url: "/Order/OrderAjax/Cancel",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                OrderManageProduct.LoadDetail();
            }
        });
        $("#txtOrderNum").val("");
        $("#ddlOrderStatus").val("");
        $("#txtContactName").val("");
        $("#txtContractOrder").val("");
        $("#txtCusNum").val("");
        $("#txtCusName").val("");
        $("#ddlAddress").text("");
        $("#txtCusPhone").val("");
        $("#txtRemark").val("");
    },
    IsExist: function () {
        var OrderNum = $("#txtOrderNum").val();
        var param = {};
        param["OrderNum"] = OrderNum;
        $.gitAjax({
            url: "/Order/OrderAjax/IsExist",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.Key == "error") {
                    $.jBox.tip("该订单编号已经存在，请重新填写！", "error");
                    $("#txtOrderNum")[0].focus();
                }
            }
        });
    }
};


var OrderManage = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var planNum = $("#txtPlanNum").val();
        var orderType = $("#ddlOrderType").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();

        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["PlanNum"] = planNum;
        param["OrderType"] = orderType;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Order/OrderManageAjax/GetList",
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
                            html += "<td>";
                            html += "<input type=\"checkbox\" name=\"order_item\" class=\"checkboxes\"  data=\"" + item.OrderNum + "\" value=\"" + item.OrderNum + "\"/>";
                            html += "</td>";
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + item.BarCode + "</td>";
                            html += "<td>" + item.ProductName + "</td>";
                            html += "<td>" + git.GetEnumDesc(EOrderType, item.OrderType) + "</td>";
                            html += "<td>" + item.CusName + "</td>";
                            html += "<td>" + item.ContractOrder + "</td>";
                            html += "<td>" + item.Num + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.OrderTime) + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.SendDate) + "</td>";
                            html += "<td>" + git.GetEnumDesc(EOrderStatus, item.Status) + "</td>";
                            html += "<td>";
                            if (item.AuditeStatus == EAuditeJson.Wait || item.AuditeStatus == EAuditeJson.NotPass) {
                                html += "<a href=\"/Order/OrderManage/Edit?orderNum=" + item.OrderNum + "\">编辑</a>&nbsp;&nbsp;";
                            }
                            html += "<a href=\"javascript:void(0)\" onclick=\"OrderManage.Delete('" + item.OrderNum + "');\">删除</a>&nbsp;&nbsp;";
                            html += "<a href=\"javascript:void(0)\" onclick=\"OrderManage.Audite(1,'" + item.OrderNum + "')\">查看</a>&nbsp;&nbsp;";
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: OrderManage.PageClick });
                $(".widget-title").find("input[type='checkbox']").attr("checked", false);
            }
        });
    },
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            OrderManage.PageClick(1, 10);
        });
    },
    Delete: function (orderNum) {
        if (!git.IsEmpty(orderNum)) {
            var param = {};
            param["OrderNum"] = orderNum;
            $.gitAjax({
                url: "/Order/OrderManageAjax/Delete",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    if (result.d != undefined && result.d == "Success") {
                        OrderManage.PageClick(1, 10);
                    }
                }
            });
        }
    },
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", true);
        } else {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    },
    BatchDel: function () {
        //批量删除
        var submit = function (v, h, f) {
            if (v == "ok") {
                var items = [];
                $("#tabInfo").children("tbody").find("input[type='checkbox']").each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag || flag == "checked") {
                        var cateNum = $(item).val();
                        items.push(cateNum);
                    }
                });
                if (items.length == 0) {
                    $.jBox.tip("请选择要删除的订单", "warn");
                    return false;
                }
                var param = {};
                param["List"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Order/OrderManageAjax/BatchDel", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            $.jBox.tip("删除成功", "success");
                            OrderManage.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        }
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
                        url: "/Order/OrderAjax/Audite",
                        data: param,
                        type: "post",
                        dataType: "json",
                        success: function (result) {
                            if (result.d != undefined && result.d == "1000") {
                                $.jBox.tip("操作成功", "success");
                            } else {
                                $.jBox.tip("操作失败", "warn");
                            }
                            OrderManage.PageClick(1, 10);
                        }
                    });
                }
            }
        };
        if (flag == 1) {
            $.jBox.open("get:/Order/OrderManage/Detail?flag=" + flag + "&orderNum=" + orderNum, "订单详细", 660, 410, { buttons: { "关闭": 3 }, submit: submit });
        } else if (flag == 2) {
            $.jBox.open("get:/Order/OrderManage/Detail?flag=" + flag + "&orderNum=" + orderNum, "订单审核", 660, 410, { buttons: { "审核通过": 1, "审核不通过": 2, "关闭": 3 }, submit: submit });
        }
    },
    ToExcel: function () {
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var orderType = $("#ddlOrderType").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();

        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["OrderType"] = orderType;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;

        $.gitAjax({
            url: "/Order/OrderManageAjax/ToExcel", type: "post", data: param, success: function (result) {
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
    }
};



var ImportFileList = {
    PageClick: function () {
        $.gitAjax({
            url: "/Order/OrderManageAjax/ImportFileList",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                var html = "";
                if (result.Data != undefined) {
                    var json = JSON.parse(result.Data);
                    if (json.List != undefined && json.List.length > 0) {
                        $(json.List).each(function (i, item) {
                            html += "<tr>";
                            if (item.StatusLable != "已导入" && !git.IsEmpty(item.ProductNum)) {
                                html += "<td><input type='checkbox' name='order_item' data='" + item.OrderNum + "' value='" + item.OrderNum + "'/></td>";
                            } else {
                                html += "<td></td>";
                            }
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + item.CusName + "</td>";
                            if (git.IsEmpty(item.ProductNum)) {
                                html += "<td>" + item.BarCode + "</td>";
                                html += "<td title='请先将该产品录入到系统中'>" + item.ProductName + "<b style='color:red;'>【新增产品】<b></td>";
                            } else {
                                html += "<td>" + item.BarCode + "</td>";
                                html += "<td>" + item.ProductName + "</td>";
                            }
                            html += "<td>" + item.Num + "</td>";
                            html += "<td>" + item.UnitName + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.OrderTime) + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.SendDate) + "</td>";
                            html += "<td>" + item.StatusLable + "</td>";
                            html += "<td>";
                            html += "<a href=\"javascript:void(0)\" onclick=\"ImportFileList.Delete('" + item.OrderNum + "');\">删除</a>&nbsp;&nbsp;";
                            if (item.StatusLable != "已导入") {
                                html += "<a href=\"/Order/OrderManage/EditImportFile?orderNum=" + item.OrderNum + "\">生成订单</a>&nbsp;&nbsp;";
                            }
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);

            }
        });
    },
    Delete: function (orderNum) {
        if (!git.IsEmpty(orderNum)) {
            var param = {};
            param["OrderNum"] = orderNum;
            $.gitAjax({
                url: "/Order/OrderManageAjax/DeleteImportFileList",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    if (result.d != undefined && result.d == "Success") {
                        ImportFileList.PageClick(1, 10);
                    }
                }
            });
        }
    },
    Clear: function (orderNum) {
        $.gitAjax({
            url: "/Order/OrderManageAjax/Clear",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                ImportFileList.PageClick(1, 10);
            }
        });
    },
    UploadLoad: function () {
        $.jUploader.setDefaults({
            cancelable: true,
            allowedExtensions: ['xls', 'xlsx', 'csv'],
            messages: {
                upload: '导入分类',
                cancel: '取消',
                emptyFile: "{file} 为空，请选择一个文件.",
                invalidExtension: "只允许上传 {extensions} 文件.",
                onLeave: "文件正在上传，如果你现在离开，上传将会被取消。"
            }
        });
        if (juploader == null) {
            juploader = $.jUploader({
                button: 'txtImport',
                action: '/Common/Upload',
                onUpload: function (fileName) {
                    jBox.tip('正在上传 ' + fileName + ' ...', 'loading');
                },
                onComplete: function (fileName, response) {
                    if (response.success) {

                        jBox.tip('正在处理文件 ...', 'loading');
                        var param = {};
                        param["Url"] = response.fileUrl;
                        $.gitAjax({
                            url: "/Order/OrderManageAjax/doImportFile", type: "post", data: param, success: function (result) {
                                if (result.d != "") {
                                    window.setTimeout(function () { $.jBox.tip(result.d, 'error'); }, 600);
                                } else {
                                    ImportFileList.PageClick(1);
                                    window.setTimeout(function () {
                                        $.jBox.tip("导入订单信息成功", 'success');
                                    }, 600);
                                }
                            }
                        });
                    } else {
                        jBox.tip('上传失败', 'error');
                    }
                },
                showMessage: function (message) {
                    jBox.tip(message, 'error');
                },
                onCancel: function (fileName) {
                    jBox.tip(fileName + ' 上传取消。', 'info');
                }
            });
        }
    }
}