
var CheckOrder = {
    Cancel: function () {
        $("#txtContractOrder").val("");
        $("#txtRemark").val("");
        $("#txtOrderTime").val("");
        $("#txtTargetNum").val("");
    },
    Add: function () {
        var Type = $("#ddlInType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var CreateTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        var TargetNum = $("#txtTargetNum").val();
        var param = {};
        param["Type"] = Type;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["CreateTime"] = CreateTime;
        param["Remark"] = Remark;
        param["TargetNum"] = TargetNum;

        $.gitAjax({
            url: "/Check/ProductAjax/Save",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("盘点单创建成功", "success");
                } else {
                    $.jBox.tip("盘点单创建失败", "error");
                }
            }
        });
    },
    Edit: function () {
        var OrderNum = $("#txtOrderNum").val();
        var Type = $("#ddlInType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var CreateTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();

        var param = {};
        param["OrderNum"] = OrderNum;
        param["Type"] = Type;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["CreateTime"] = CreateTime;
        param["Remark"] = Remark;

        $.gitAjax({
            url: "/Check/ProductAjax/Edit",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("盘点单编辑成功", "success");
                } else {
                    $.jBox.tip("盘点单编辑失败", "error");
                }
            }
        });
    },
    ShowDialog: function (type) {
        var submit = function (v, h, f) {
            if (v == 1) {
                var values = [];
                var items = h.find("input[type='checkbox']");
                $(items).each(function (i, item) {
                    var flag = $(item).attr("checked");
                    if (flag == "checked" || flag) {
                        values.push($(item).val());
                    }
                });
                if (values.length == 0) {
                    $.jBox.tip("请选择要盘点的产品", "error");
                    return false;
                }
                var param = {};
                param["ProductItems"] = JSON.stringify(values);
                $.gitAjax({
                    url: "/Check/ProductAjax/AddProduct",
                    type: "post",
                    dataType: "json",
                    data: param,
                    success: function (result) {
                        if ("edit" == type) {
                            CheckOrder.LoadEditDetail();
                        } else {
                            CheckOrder.LoadDetail();
                        }
                    }
                });
            }
            else if (v == 3) {
                var productNum = h.find("#txtProduct").val();
                var cateNum = h.find("#ddlCategory").val();
                var param = {};
                param["productNum"] = productNum;
                param["cateNum"] = cateNum;

                $.gitAjax({
                    url: "/Check/ProductAjax/AddAll",
                    type: "post",
                    dataType: "json",
                    data: param,
                    success: function (result) {
                        if ("edit" == type) {
                            CheckOrder.LoadEditDetail();
                        } else {
                            CheckOrder.LoadDetail();
                        }
                    }
                });
            }
        };
        $.jBox.open("get:/Product/Goods/Dialog", "选择产品", 850, 500, {
            buttons: { "全部": 3, "确定": 1, "关闭": 2 }, submit: submit, loaded: function (h) {
            }
        });
    },
    LoadDetail: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var param = {};
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;
        $.gitAjax({
            url: "/Check/ProductAjax/LoadDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = JSON.parse(result.Data);
                var Html = "";
                if (json.List != undefined && json.List.length > 0) {
                    $(json.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>" + item.SnNum + "</td>";
                        Html += "<td>" + item.BarCode + "</td>";
                        Html += "<td>" + git.GetStrSub(item.ProductName, 20) + "</td>";
                        Html += "<td>" + item.Size + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: CheckOrder.LoadDetail });
            }
        });
    },
    LoadEditDetail: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var param = {};
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;
        $.gitAjax({
            url: "/Check/ProductAjax/LoadDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = JSON.parse(result.Data);
                var Html = "";
                if (json.List != undefined && json.List.length > 0) {
                    $(json.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>" + item.SnNum + "</td>";
                        Html += "<td>" + item.BarCode + "</td>";
                        Html += "<td>" + git.GetStrSub(item.ProductName, 20) + "</td>";
                        Html += "<td>" + item.Size + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "<td>";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"CheckOrder.Delete('" + item.SnNum + "')\" title=\"删除\"></a>";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: CheckOrder.LoadDetail });
            }
        });
    },
    Delete: function (targetNum) {
        var param = {};
        param["targetNum"] = targetNum;
        $.gitAjax({
            url: "/Check/ProductAjax/Delete",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                CheckOrder.LoadEditDetail(1, 10);
            }
        });
    }
};



var CheckManager = {
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
        param["BeginTime"] = beginTime;
        param["EndTime"] = endTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Check/ProductManagerAjax/GetList",
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
                            html += "<input type='checkbox' name='check_item' value='" + item.OrderNum + "'/>";
                            html += "</td>";
                            html += "<td>" + item.OrderNum + "</td>";
                            html += "<td>" + git.GetEnumDesc(ECheckType, item.Type) + "</td>";
                            html += "<td>" + item.ContractOrder + "</td>";
                            html += "<td>" + git.GetEnumDesc(EAudite, item.Status) + "</td>";
                            html += "<td>" + item.CreateUserName + "</td>";
                            html += "<td>" + git.GetEnumDesc(EOpType, item.OperateType) + "</td>";
                            html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            html += "<td>";
                            if (item.Status == EAuditeJson.Wait && item.IsComplete == EBoolJson.No) {
                                html += "<a class=\"icon-edit\" href=\"/Check/Product/Edit?orderNum=" + item.OrderNum + "\" title=\"编辑\"></a>&nbsp;&nbsp;";
                            }

                            html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"CheckManager.Delete('" + item.OrderNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";
                            html += "<a class=\"icon-eye-open\" href='/Check/Product/Detail?OrderNum=" + item.OrderNum + "' title=\"盘差\"></a>&nbsp;&nbsp;";

                            if (item.IsComplete == EBoolJson.No) {
                                html += "<a class=\"icon-upload\" href='/Check/Product/Upload?OrderNum=" + item.OrderNum + "' title=\"复核\"></a>&nbsp;&nbsp;";
                            }
                            if (item.Status == EAuditeJson.Wait) {
                                html += "<a class=\"icon-ok\" href='/Check/Product/Audite?OrderNum=" + item.OrderNum + "' title=\"审核\"></a>&nbsp;&nbsp;";
                                html += "<a class=\"icon-download\" href=\"javascript:void(0)\" onclick=\"CheckAudite.ToCheckExcel('" + item.OrderNum + "')\" title=\"下载盘点单Excel\"></a>&nbsp;&nbsp;";

                            }
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: CheckManager.PageClick });
                $("#chbSelectAll").attr("checked", false);
            }
        });
    },
    Delete: function (orderNum) {
        var param = {};
        param["orderNum"] = orderNum;
        $.gitAjax({
            url: "/Check/ProductManagerAjax/Delete",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.d == "Success") {
                    CheckManager.PageClick(1, 10);
                } else {
                    $.jBox.tip("删除失败,请联系管理员", "warn");
                }
            }
        });
    },
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            CheckManager.PageClick(1, 10);
        });
    },
    Load: function () {
        CheckManager.TabClick();
        CheckManager.PageClick(1, 10);
    },
    DifPageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var orderNum = $("#txtOrderNum").val();
        var Status = $("#txtStatus").val();
        var IsComplete = $("#txtIsComplete").val();
        var param = {};
        param["OrderNum"] = orderNum;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Check/ProductManagerAjax/GetCheckDif",
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
                            html += "<select class='input-small'>";
                            html += item.LocalName
                            html += "</select>";
                            html += "</td>"
                            //html += "<td>" + item.LocalName + "</td>";
                            html += "<td>" + item.ProductNum + "</td>";
                            html += "<td>" + item.BarCode + "</td>";
                            html += "<td>" + item.ProductName + "</td>";
                            html += "<td>" + item.BatchNum + "</td>";
                            if (IsComplete == EBoolJson.Yes) {
                                html += "<td>" + item.LocalQty + "</td>";
                                html += "<td>" + item.FirstQty + "</td>";
                                html += "<td>" + (item.DifQty) + "</td>";
                            } else {
                                html += "<td>X</td>";
                                html += "<td>X</td>";
                                html += "<td>X</td>";
                            }
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: CheckManager.DifPageClick });
            }
        });
    },
    Check: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var orderNum = $("#txtOrderNum").val();
        var Status = $("#txtStatus").val();
        var param = {};
        param["OrderNum"] = orderNum;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Check/ProductManagerAjax/GetCheckDif",
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
                            html += "<select class='input-small' name='ddlLocalNum' >";
                            html += item.LocalName
                            html += "</select>";
                            html += "</td>"
                            //html += "<td>" + item.LocalName + "</td>";
                            html += "<td>" + item.ProductNum + "</td>";
                            html += "<td>" + item.BarCode + "</td>";
                            html += "<td>" + item.ProductName + "</td>";
                            html += "<td>" + item.BatchNum + "</td>";
                            html += "<td>X</td>";
                            html += "<td data-id='" + item.ID + "' BatchNum='" + item.BatchNum + "' LocalNum='" + item.LocalNum + "' ProductNum='" + item.ProductNum + "' BarCode='" + item.BarCode + "'><input type='text' name='txtFirstQty' class='input-small inde' value='" + item.FirstQty + "'/>&nbsp;&nbsp;<a class='btn_link' href='javascript:void(0)'>确定</a></td>";
                            html += "<td>X</td>";
                            html += "<td>";
                            html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"CheckAudite.DeleteCheckData('" + item.ID + "');\" title=\"删除\"></a>&nbsp;&nbsp;";
                            html += "</td>";
                            html += "</tr>";
                        });
                    }
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: CheckManager.Check });
                $(".btn_link").click(function () {
                    CheckAudite.CheckData(this);
                });
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
                    $.jBox.tip("请选择要删除的盘点单", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Check/ProductManagerAjax/DeleteBatch", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            CheckManager.PageClick(1, 10);
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
        var status = $("#btnStatusGroup").find(".disabled").val();
        var orderNum = $("#txtOrderNum").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var param = {};
        param["Status"] = status;
        param["OrderNum"] = orderNum;
        param["BeginTime"] = beginTime;
        param["EndTime"] = endTime;

        $.gitAjax({
            url: "/Check/ProductManagerAjax/ToExcel", type: "post", data: param, success: function (result) {
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

var CheckAudite = {
    Audite: function (status) {
        //status 2 审核通过 3 审核失败 其他 取消
        if (status == 0) {
            window.location.href = "/Check/Product/List";
        }
        var OrderNum = $("#txtOrderNum").val();
        var param = {};
        param["OrderNum"] = OrderNum;
        param["Status"] = status;
        $.gitAjax({
            url: "/Check/ProductManagerAjax/Audite",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.d == "1000") {
                    $.jBox.tip("操作成功", "success");
                } else {
                    $.jBox.tip("操作失败", "error");
                }
            }
        });
    },
    CheckData: function (item) {
        var OrderNum = $("#txtOrderNum").val();
        var parent = $(item).parent();
        var ProductNum = $(parent).attr("ProductNum");
        var BarCode = $(parent).attr("BarCode");
        var BatchNum = $(parent).attr("BatchNum");
        var ID = $(parent).attr("data-id");
        var Qty = $(parent).find("input[name='txtFirstQty']").val();
        var LocalNum = $(parent).parent().find("select[name='ddlLocalNum']").find("option:selected").val();
        var LocalName = $(parent).parent().find("select[name='ddlLocalNum']").find("option:selected").html();
        if (isNaN(Qty)) {
            $.jBox.tip("请输入盘点数量!", "warn");
            return false;
        }
        var param = {};
        param["ID"] = ID;
        param["OrderNum"] = OrderNum;
        param["ProductNum"] = ProductNum;
        param["BarCode"] = BarCode;
        param["Qty"] = Qty;
        param["BatchNum"] = BatchNum;
        param["LocalNum"] = LocalNum;
        param["LocalName"] = LocalName;
        $.gitAjax({
            url: "/Check/ProductAjax/CheckData",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("提交成功", "success");
                } else {
                    $.jBox.tip("提交失败", "error");
                }
            }
        });
    },
    Complete: function () {
        var OrderNum = $("#txtOrderNum").val();
        var param = {};
        param["OrderNum"] = OrderNum;
        $.gitAjax({
            url: "/Check/ProductManagerAjax/Complete",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("操作成功", "success");
                } else {
                    $.jBox.tip("操作失败", "error");
                }
            }
        });
    },
    ToCheckExcel: function (orderNum) {
        var param = {};
        param["orderNum"] = orderNum;
        $.gitAjax({
            url: "/Check/ProductManagerAjax/ToCheckExcel", type: "post", data: param, success: function (result) {
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
    UploadCheck: function () {
        var juploader = null;
        $.jUploader.setDefaults({
            cancelable: true, // 可取消上传
            allowedExtensions: ['xls', 'xlsx'], // 只允许上传txt文本
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
                button: 'btnUpload',
                action: '/Common/Upload',
                onUpload: function (fileName) {
                    jBox.tip('正在上传 ' + fileName + ' ...', 'loading');
                },
                onComplete: function (fileName, response) {
                    if (response.success) {
                        jBox.tip('正在处理文件 ...', 'loading');
                        var param = {};
                        param["Url"] = response.fileUrl;
                        param["OrderNum"] = $("#txtOrderNum").val();
                        $.gitAjax({
                            url: "/Check/ProductManagerAjax/CheckData", type: "post", data: param, success: function (result) {
                                if (result.Key != undefined && result.Key == "1000") {
                                    $.jBox.tip(result.Value, "success");
                                    CheckManager.Check(1, 5);
                                } else {
                                    $.jBox.tip("上传数据失败", "error");
                                }
                            }
                        });
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
    },
    SelectDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var ProductNum = h.find("#hdProductNum").val();
                var BarCode = h.find("#txtBarCode").val();
                var ProductName = h.find("#txtProductName").val();
                var LocalNum = h.find("#txtLocalNum").val();
                var LocalName = h.find("#txtLocalName").val();
                var FirstQty = h.find("#txtNum").val();
                var BatchNum = h.find("#txtProductBatch").val();
                var OrderNum = $("#txtOrderNum").val();
                var param = {};
                param["ProductNum"] = ProductNum;
                param["BarCode"] = BarCode;
                param["ProductName"] = ProductName;
                param["LocalNum"] = LocalNum;
                param["LocalName"] = LocalName;
                param["FirstQty"] = FirstQty;
                param["BatchNum"] = BatchNum;
                param["OrderNum"] = OrderNum;
                $.gitAjax({
                    url: "/Check/ProductManagerAjax/AddCheckData",
                    type: "post",
                    dataType: "json",
                    data: param,
                    success: function (result) {
                        if (result.Key == "1000") {
                            $.jBox.tip("新增成功", "success");
                            CheckManager.Check(1, 5);
                        } else {
                            $.jBox.tip("新增失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.open("get:/Check/Product/AddProduct", "新增盘点数据", 400, 410, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {

                CheckAudite.AutoProduct($(item).find("#txtBarCode"), item);

                $(item).find("#txtBarCode").ProductDialog({
                    data: undefined, Mult: false, callBack: function (result) {
                        $(item).find("#txtBarCode").val(result.BarCode);
                        $(item).find("#txtProductName").val(result.ProductName);
                        $(item).find("#txtSize").val(unescape(result.Size));
                        $(item).find("#txtPrice").val(git.ToDecimal(result.InPrice, 2));
                        $(item).find("#txtLocalQty").val(result.Num);
                        $(item).find("#hdProductNum").val(result.SnNum);
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
    DeleteCheckData: function (id) {
        var param = {};
        param["id"] = id;
        $.gitAjax({
            url: "/Check/ProductManagerAjax/DeleteCheckData",
            type: "post",
            dataType: "json",
            data: param,
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip("删除成功", "success");
                    CheckManager.Check(1, 5);
                } else {
                    $.jBox.tip("删除失败", "error");
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
    }
};