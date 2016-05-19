var orderProduct = {
    Load: function () {
        orderProduct.AutoCus();

        $("#txtCusNum").CustomerDialog({
            data: undefined, Mult: false, callBack: function (result) {
                $("#txtCusNum").val(result.CusNum);
                $("#txtCusName").val(result.CusName);
                orderProduct.CusNameChange();
            }
        });
    },
    SelectDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var tab = h.find("#tabInfo2");
                var list = [];
                $(tab).children("tbody").children("tr").each(function (i, item) {
                    var Flag = $(item).find("input[type='checkbox']").attr("checked");
                    var ProductNum = $(item).find("input[name='LocalPro_item']").val();
                    var BatchNum = $(item).find("input[name='LocalPro_item']").attr("data-BatchNum");
                    var LocalNum = $(item).find("input[name='LocalPro_item']").attr("data-LocalNum");
                    var LocalName = $(item).find("input[name='LocalPro_item']").attr("data-LocalName");
                    var Num = $(item).find(".span1").val();
                    if (Flag) {
                        var Product = {};
                        Product["ProductNum"] = ProductNum;
                        Product["BatchNum"] = BatchNum;
                        Product["Num"] = Num;
                        Product["LocalNum"] = LocalNum;
                        Product["LocalName"] = LocalName;
                        list.push(Product);
                    }
                });
                if (list == undefined || list.length == 0) {
                    $.jBox.tip("请选择要出库的项");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(list);
                $.gitAjax({
                    url: "/OutStorage/ProductAjax/AddProduct",
                    type: "post",
                    dataType: "json",
                    data: param,
                    success: function (result) {
                        orderProduct.LoadDetail();
                    }
                });
            }
        };
        $.jBox.open("get:/OutStorage/Product/AddProduct", "出库产品", 800, 450, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                orderProduct.AutoProduct($(item).find("#txtBarCode"), item);
            }
        });
    },
    LoadDetail: function () {
        $.gitAjax({
            url: "/OutStorage/ProductAjax/LoadProduct",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (result.List != undefined) {
                    var html = "";
                    var json = JSON.parse(result.List);
                    $(json.Data).each(function (i, item) {
                        html += "<tr class=\"odd gradeX\">";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.ProductNum + "</td>";
                        html += "<td>" + item.BatchNum + "</td>";
                        html += "<td>" + git.ToDecimal(item.OutPrice,2) + "&nbsp;元</td>";
                        html += "<td>" + git.ToDecimal(item.Amount, 2) + "&nbsp;元</td>";
                        html += "<td>" + item.Size + "</td>";
                        html += "<td>" + item.Num + "</td>";
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
                url: "/OutStorage/ProductAjax/EditNum",
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
            url: "/OutStorage/ProductAjax/DelDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                orderProduct.LoadDetail();
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
                orderProduct.CusNameChange();
            }
        });
    },
    AutoProduct: function (item, target) {
        //选择出库产品自动加载
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
                orderProduct.LoadLocalProduct(target);
            }
        });
    },
    LoadLocalProduct: function (target) {
        //自动加载产品
        var BarCode = $("#txtBarCode").val();
        var param = {};
        param["BarCode"] = BarCode;
        $.gitAjax({
            url: "/OutStorage/ProductAjax/GetLocalProductList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var html = "";
                if (result.Data != undefined) {
                    if (result.Data.List != undefined && result.Data.List.length > 0) {
                        $(result.Data.List).each(function (i, item) {
                            html += "<tr>";
                            html += "<td class=\"inorder_search\"><input type=\"checkbox\" name=\"LocalPro_item\" data-BatchNum=\""+item.BatchNum+"\" data-LocalNum=\""+item.LocalNum+"\" data-LocalName=\""+item.LocalName+"\"  value=\"" + item.ProductNum + "\"/></td>";
                            html += "<td class=\"inorder_search\">" + item.ProductName + "</td>";
                            html += "<td class=\"inorder_search\">" + item.BarCode + "</td>";
                            html += "<td class=\"inorder_search\">" + item.BatchNum + "</td>";
                            html += "<td class=\"inorder_search\">" + item.LocalName + "</td>";
                            html += "<td class=\"inorder_search\">" + item.Num + "</td>";
                            html += "<td class=\"inorder_search\"><input type=\"text\" class=\"span1\" /></td>";
                            html += "</tr>";
                        });
                    }
                }
                $(target).find("#tabInfo2").children("tbody").html(html);
            }
        });
    },
    Add: function () {
        var OrderNum = $("#txtOrderNum").val();
        var OutType = $("#ddlOutType").val();
        var ProductType = $("#ddlProductType").val();
        var ContractOrder = $("#txtContractOrder").val();
        var CusNum = $("#txtCusNum").val();
        var CusName = $("#txtCusName").val();
        var Address = $("#ddlAddress").find("option:selected").text();
        var Phone = $("#txtCusPhone").val();
        var ContactName = $("#txtContactName").val();
        var CrateUser = $("#txtCrateUser").val();
        var OrderTime = $("#txtOrderTime").val();
        var Remark = $("#txtRemark").val();
        if (git.IsEmpty(OutType)) {
            $.jBox.tip("请选择出库单类型", "warn");
            return false;
        }
        if (git.IsEmpty(ProductType)) {
            $.jBox.tip("请选择出库产品类型", "warn");
            return false;
        }
        if (git.IsEmpty(CusNum)) {
            $.jBox.tip("请选择客户", "warn");
            return false;
        }
        var param = {};
        param["OrderNum"] = OrderNum;
        param["OutType"] = OutType;
        param["ProductType"] = ProductType;
        param["ContractOrder"] = ContractOrder;
        param["CusNum"] = CusNum;
        param["CusName"] = CusName;
        param["Address"] = Address;
        param["ContactName"] = ContactName;
        param["CrateUser"] = CrateUser;
        param["Phone"] = Phone;
        param["OrderTime"] = OrderTime;
        param["Remark"] = Remark;
        $.gitAjax({
            url: "/OutStorage/ProductAjax/Create",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                if (git.IsEmpty(OrderNum)) {
                    if (result.Key == "1000") {
                        orderProduct.Cancel();
                        $.jBox.tip("出库单创建成功", "success");
                    } else {
                        $.jBox.tip("出库单创建失败", "error");
                    }
                } else {
                    if (result.Key == "1000") {
                        $.jBox.tip("出库单编辑成功", "success");
                    } else {
                        $.jBox.tip("出库单编辑失败", "error");
                    }
                }
            }
        });
    },
    Cancel: function () {
        $.gitAjax({
            url: "/OutStorage/ProductAjax/Cancel",
            data: undefined,
            type: "post",
            dataType: "json",
            success: function (result) {
                orderProduct.LoadDetail();
            }
        });
        $("#txtContractOrder").val("");
        $("#txtCusNum").val("");
        $("#txtCusName").val("");
        $("#ddlAddress").empty();
        $("#txtCusPhone").val("");
        $("#txtContactName").val("");
        $("#txtRemark").val("");
        $("#txtOrderTime").val("");
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
    SelectAll: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("input[name='LocalPro_item']").attr("checked", true);
        }
        else {
            $("input[name='LocalPro_item']").attr("checked", false);
        }
    },
    SelectAllOrder: function (item) {
        var flag = $(item).attr("checked");
        if (flag || flag == "checked") {
            $("#tabInfo2").children("tbody").children("tr").each(function (i, item) {
                $(item).find("input[type='checkbox']").attr("checked", true);
            });
        }
        else {
            $("#tabInfo2").children("tbody").children("tr").each(function (i, item) {
                $(item).find("input[type='checkbox']").attr("checked", false);
            });
        }
    },
    SelectOrder: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var tab = h.find("#tabInfo2");
                var orderNum = h.find("#txtOrderNum").val();
                var ProductItems = [];
                var QtyItems = [];
                var SnItems = [];
                $(tab).children("tbody").children("tr").each(function (i, item) {
                    var productNum = $(item).find("input[type='checkbox']").val();
                    var flag = $(item).find("input[type='checkbox']").attr("checked");
                    var sn = $(item).find("input[type='checkbox']").attr("SN");
                    var qty = $(item).find("input[type='text']").val();
                    if (flag || flag == "checked") {
                        if (!isNaN(qty) && qty > 0) {
                            ProductItems.push(productNum);
                            QtyItems.push(qty);
                            SnItems.push(sn);
                        }
                    }
                });
                if (ProductItems.length == 0) {
                    $.jBox.tip("请选择要出库的产品", "warn");
                    return false;
                }

                var param = {};
                param["ProductItems"] = JSON.stringify(ProductItems);
                param["QtyItems"] = JSON.stringify(QtyItems);
                param["SnItems"] = JSON.stringify(SnItems);
                $.gitAjax({
                    url: "/OutStorage/ProductAjax/AddOrderDetailProduct",
                    type: "post",
                    dataType: "json",
                    data: param,
                    success: function (result) {
                        orderProduct.LoadDetail();
                    }
                });
            }
        };
        $.jBox.open("get:/OutStorage/Product/OrderDetail", "出库产品", 800, 450, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
                orderProduct.AutoOrder($(item).find("#txtOrderNum"), item);
                $(item).find("#txtOrderNum").keydown(function (e) {
                    if (e.which == 13) {
                        orderProduct.LoadOrderDetail($(item).find("#txtOrderNum").val(), item);
                    }
                });
            }
        });
    },
    AutoOrder: function (item, target) {
        $(item).autocomplete({
            paramName: "productName",
            url: '/OutStorage/ProductAjax/AutoOrder',
            showResult: function (value, data) {
                var row = JSON.parse(value);
                return '<span>' + row.OrderNum + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + row.CusName + '</span>';
            },
            onItemSelect: function (item) {
            },
            maxItemsToShow: 5,
            selectedCallback: function (selectItem) {
                $(target).find("#txtOrderNum").val(selectItem.OrderNum)
                orderProduct.LoadOrderDetail(selectItem.OrderNum, target);
            }
        });
    },
    LoadOrderDetail: function (orderNum, target) {
        var param = {};
        param["orderNum"] = orderNum;
        $.gitAjax({
            url: "/OutStorage/ProductAjax/GetOrderDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var html = "";
                if (result.Data != undefined) {
                    if (result.Data.List != undefined && result.Data.List.length > 0) {
                        $(result.Data.List).each(function (i, item) {
                            html += "<tr>";
                            html += "<td class=\"inorder_search\"><input type=\"checkbox\" SN=\"" + item.SnNum + "\" value=\"" + item.ProductNum + "\"/></td>";
                            html += "<td class=\"inorder_search\">" + item.ProductName + "</td>";
                            html += "<td class=\"inorder_search\">" + item.BarCode + "</td>";
                            html += "<td class=\"inorder_search\">" + item.LocalNum + "</td>";
                            html += "<td class=\"inorder_search\">" + git.ToDecimal(item.Num, 5) + "</td>";
                            html += "<td class=\"inorder_search\">" + git.ToDecimal(item.RealNum, 5) + "</td>";
                            html += "<td class=\"inorder_search\"><input type=\"text\" class=\"span1\" onblur=\"orderProduct.BoxOnblur(this,'" + item.Num + "','" + item.RealNum + "')\" value=\"" + (parseFloat(item.Num) - parseFloat(item.RealNum)) + "\"/></td>";
                            html += "</tr>";
                        });
                    }
                }
                $(target).find("#tabInfo2").children("tbody").html(html);
            }
        });
    },
    BoxOnblur: function (item, num, realNum) {
        var valNum = parseFloat(num) - parseFloat(realNum);
        var val = $(item).val();
        if (isNaN(val) && parseFloat(val) > 0) {
            $.jBox.tip("出库数必须为数字并且大于0", "warn");
            $(item).val(valNum);
        }
        else if (val > valNum) {
            //$.jBox.tip("出库数", "warn");
            $(item).val(valNum);
        }
    }
};


var OutStore = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var status = $("#btnStatusGroup").find(".disabled").val();
        var OrderNum = $("#txtOutStoreNum").val();
        var CusName = $("#txtCustomer").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var order = $("#txtOrderNum").val(); //客户订单
        var OutType = $("#ddlOutType").val();
        var planNum = $("#txtPlanOrder").val();
        var ReprtNum = $("#txtReprtNum").val();
        var param = {};
        param["Status"] = status;
        param["OrderNum"] = OrderNum;
        param["CusName"] = CusName;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
        param["order"] = order;
        param["OutType"] = OutType;
        param["planNum"] = planNum;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;
        $.gitAjax({
            url: "/OutStorage/ProductManagerAjax/GetList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {

                var Html = "";
                if (result.Data != undefined) {
                    var json = JSON.parse(result.Data);

                    if (json.List != undefined && json.List.length > 0) {
                        $(json.List).each(function (i, item) {
                            Html += "<tr class=\"odd gradeX\">";
                            Html += "<td><input type=\"checkbox\" value=\"" + item.OrderNum + "\"/></td>";
                            Html += "<td>" + item.OrderNum + "</td>";
                            Html += "<td>" + git.GetEnumDesc(EOutType, item.OutType) + "</td>";
                            Html += "<td>" + item.CusName + "</td>";
                            Html += "<td>" + item.ContractOrder + "</td>";
                            Html += "<td>" + item.Num + "</td>";
                            Html += "<td>" + git.ToDecimal(item.Amount,2) + "&nbsp;元</td>";
                            Html += "<td>" + git.GetEnumDesc(EAudite, item.Status) + "</td>";
                            Html += "<td>" + git.GetEnumDesc(EOpType, item.OperateType) + "</td>";
                            Html += "<td>" + item.CreateUserName + "</td>";
                            Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                            Html += "<td>";
                            if (item.Status == EAuditeJson.Wait || item.Status == EAuditeJson.NotPass) {
                                Html += "<a class=\"icon-edit\" href='/OutStorage/Product/Edit?orderNum=" + item.OrderNum + "' title=\"编辑\"></a>&nbsp;&nbsp;";
                            }
                            Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"OutStore.Delete('" + item.OrderNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";
                            Html += "<a class=\"icon-eye-open\" href=\"javascript:void(0)\" onclick=\"OutStore.Audite(1,'" + item.OrderNum + "')\" title=\"查看\"></a>&nbsp;&nbsp;";

                            if (item.Status == EAuditeJson.Wait) {
                                Html += "<a class=\"icon-ok\" href=\"javascript:void(0)\" onclick=\"OutStore.Audite(2,'" + item.OrderNum + "')\" title=\"审核\"></a>&nbsp;&nbsp;";
                            }
                            //if (item.OutType == EOutTypeJson.Sell) {
                            //    Html += "<a class=\" icon-print\" href='/OutStorage/Product/Print?orderNum=" + item.OrderNum + "' title=\"打印\"></a>&nbsp;&nbsp;";
                            //}
                            Html += "<a class=\" icon-print\" href='/Report/Manager/Show?ReportNum=" + ReprtNum + "&OrderNum=" + item.OrderNum + "' title=\"打印\"></a>&nbsp;&nbsp;";

                            Html += "</td>";
                            Html += "</tr>";
                        });
                    }
                }

                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: OutStore.PageClick });
            }
        });
    },
    ToExcel: function () {
        var ProductName = $("#txtProduct").val();
        var CateNum = $("#ddlCategory").val();
        var param = {};
        param["ProductName"] = ProductName;
        param["CateNum"] = CateNum;
        $.gitAjax({
            url: "/OutStorage/ProductManagerAjax/ToExcel", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
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
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", true);
        }
        else {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    },
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            OutStore.PageClick(1, 10);
        });
    },
    Edit: function (OrderNum) {
        snNum = snNum == undefined ? "" : snNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var SnNum = h.find("#txtSnNum").val();
                var BarCode = h.find("#txtBarCode").val();
                var ProductName = h.find("#txtProductName").val();
                var CateNum = h.find("#ddlCategory option:selected").val();
                var CateName = h.find("#ddlCategory option:selected").html();
                var MinNum = h.find("#txtMinNum").val();
                var MaxNum = h.find("#txtMaxNum").val();
                var AvgPrice = h.find("#txtAvgPrice").val();
                var InPrice = h.find("#txtInPrice").val();
                var OutPrice = h.find("#txtOutPrice").val();
                var NetWeight = h.find("#txtNetWeight").val();
                var GrossWeight = h.find("#txtGrossWeight").val();
                var Unit = h.find("#ddlUnit option:selected").val();
                var UnitName = h.find("#ddlUnit option:selected").html();
                var Size = h.find("#txtSize").val();
                var Description = h.find("#txtDescripte").val();
                var StorageNum = h.find("#ddlStorage  option:selected").val();
                var DefaultLocal = h.find("#ddlLocal  option:selected").val();
                var CusNum = h.find("#ddlCustomer option:selected").val();
                var CusName = h.find("#ddlCustomer option:selected").html();
                if (BarCode == undefined || BarCode == "") {
                    $.jBox.tip("请输出条码编号", "warn");
                    return false;
                }
                if (ProductName == undefined || ProductName == "") {
                    $.jBox.tip("请输出产品名称", "warn");
                    return false;
                }
                if (CateNum == undefined || CateNum == "") {
                    $.jBox.tip("请输出产品类型", "warn");
                    return false;
                }
                if (MinNum == undefined || MinNum == "") {
                    $.jBox.tip("请输出预警值下线", "warn");
                    return false;
                }
                if (MaxNum == undefined || MaxNum == "") {
                    $.jBox.tip("请输出预警值上线", "warn");
                    return false;
                }
                if (AvgPrice == undefined || AvgPrice == "") {
                    $.jBox.tip("请输出平均价格", "warn");
                    return false;
                }
                if (Unit == undefined || Unit == "") {
                    $.jBox.tip("请输出单位", "warn");
                    return false;
                }
                if (InPrice == undefined || InPrice == "") {
                    $.jBox.tip("请输出进口价格", "warn");
                    return false;
                }
                if (OutPrice == undefined || OutPrice == "") {
                    $.jBox.tip("请输出出口价格", "warn");
                    return false;
                }
                if (NetWeight == undefined || NetWeight == "") {
                    $.jBox.tip("请输出净重", "warn");
                    return false;
                }
                if (GrossWeight == undefined || GrossWeight == "") {
                    $.jBox.tip("请输出毛重", "warn");
                    return false;
                }
                var param = {};
                param["SnNum"] = SnNum;
                param["BarCode"] = BarCode;
                param["ProductName"] = ProductName;
                param["CateNum"] = CateNum;
                param["CateName"] = CateName;
                param["MinNum"] = MinNum;
                param["MaxNum"] = MaxNum;
                param["AvgPrice"] = AvgPrice;
                param["InPrice"] = InPrice;
                param["OutPrice"] = OutPrice;
                param["NetWeight"] = NetWeight;
                param["GrossWeight"] = GrossWeight;
                param["Unit"] = Unit;
                param["UnitName"] = UnitName;
                param["Size"] = Size;
                param["Description"] = Description;
                param["StorageNum"] = StorageNum;
                param["DefaultLocal"] = DefaultLocal;
                param["CusNum"] = CusNum;
                param["CusName"] = CusName;
                $.gitAjax({
                    url: "/OutStorage/ProductAjax/EditProduct", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (SnNum == undefined || SnNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            OutStore.PageClick(1);
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
        if (git.IsEmpty(snNum)) {
            $.jBox.open("get:/OutStore/Product/Detail", "添加产品", 500, 460, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/OutStore/Product/Detail?snNum=" + snNum, "编辑产品", 500, 460, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (OrderNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["OrderNum"] = OrderNum;
                $.gitAjax({
                    url: "/OutStorage/ProductManagerAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            OutStore.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
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
                    $.jBox.tip("请选择要删除的出库单", "warn");
                    return false;
                }
                var param = {};
                param["list"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/OutStorage/ProductManagerAjax/DeleteBatch", type: "post", data: param, success: function (result) {
                        if (result.d == "Success") {
                            OutStore.PageClick(1);
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
                        url: "/OutStorage/ProductManagerAjax/Audit",
                        data: param,
                        type: "post",
                        dataType: "json",
                        success: function (result) {
                            if (result.d != undefined) {
                                if (result.d == "1000") {
                                    $.jBox.tip("出库单审核成功", "success");
                                    OutStore.PageClick(1, 20);
                                } else if (result.d == "1001") {
                                    $.jBox.tip("出库单不存在", "warn");
                                } else if (result.d == "1002") {
                                    $.jBox.tip("出库单已经审核", "warn");
                                } else if (result.d == "1003") {
                                    $.jBox.tip("出库产品不存在", "warn");
                                } else if (result.d == "1004") {
                                    $.jBox.tip("出库数不满足要求", "warn");
                                }
                            } else {
                                $.jBox.tip("出库单审核失败", "warn");
                            }
                        }
                    });
                }
            }
        };
        if (flag == 1) {
            $.jBox.open("get:/OutStorage/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "出库单详细", 800, 410, { buttons: { "关闭": 3 }, submit: submit });
        } else if (flag == 2) {
            $.jBox.open("get:/OutStorage/Product/Detail?flag=" + flag + "&orderNum=" + orderNum, "出库单审核", 800, 410, { buttons: { "审核通过": 1, "审核不通过": 2, "关闭": 3 }, submit: submit });
        }
    },
    SearchEvent: function () {
        $("#btnHSearch").click(function () {
            var flag = $("#divHSearch").css("display");
            if (flag == "none") {
                $("#divHSearch").slideDown("slow");
            } else {
                $("#divHSearch").slideUp("slow");
            }
        });
    }
};