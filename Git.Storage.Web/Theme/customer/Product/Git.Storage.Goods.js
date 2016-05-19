
var Category = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var cateName = $("#txtCategory").val();
        var param = {};
        param["pageIndex"] = pageIndex;
        param["pageSize"] = pageSize;
        param["cateName"] = cateName;
        $.gitAjax({
            url: "/Product/GoodsAjax/CateList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.list != undefined && json.list.data != undefined && json.list.data.length > 0) {
                    $(json.list.data).each(function (i, item) {
                        Html += "<tr>";
                        Html += "<td><input type=\"checkbox\" value=\"" + item.CateNum + "\"/></td>";
                        Html += "<td>" + item.CateNum + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "<td>" + item.CreateUser + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>" + item.Remark + "</td>";
                        Html += "<td>";

                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Category.Add('" + item.CateNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Category.Delete('" + item.CateNum + "')\" title=\"删除\"></a>";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Category.PageClick });
            }
        });
    },
    Add: function (cateNum) {
        cateNum = cateNum == undefined ? "" : cateNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var num = h.find("#txtCategoryCode").val();
                var name = h.find("#txtCategoryName").val();
                var remark = h.find("#txtRemark").val();
                if (git.IsEmpty(name)) {
                    $.jBox.tip("请输入产品类别名称", "warn");
                    return false;
                }
                var param = {};
                param["num"] = num;
                param["name"] = name;
                param["remark"] = remark;
                $.gitAjax({
                    url: "/Product/GoodsAjax/Edit", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            if (cateNum == undefined || cateNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Category.PageClick(1);
                            return true;
                        } else {
                            if (cateNum == undefined || cateNum == "") {
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
        };
        if (git.IsEmpty(cateNum)) {
            $.jBox.open("get:/Product/Goods/Add", "添加产品类别", 350, 200, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Product/Goods/Add?cateNum=" + cateNum, "编辑产品类别", 350, 200, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }

    },
    Delete: function (cateNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["cateNum"] = cateNum;
                $.gitAjax({
                    url: "/Product/GoodsAjax/DelCate", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            $.jBox.tip("删除成功", "success");
                            Category.PageClick(1);
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
        } else {
            $("#tabInfo").children("tbody").find("input[type='checkbox']").attr("checked", false);
        }
    },
    DelBat: function () {
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
                    $.jBox.tip("请选择要删除的类型", "warn");
                    return false;
                }
                var param = {};
                param["List"] = JSON.stringify(items);
                $.gitAjax({
                    url: "/Product/GoodsAjax/DelBat", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            $.jBox.tip("删除成功", "success");
                            Category.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        }
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    Load: function () {
        $("#txtCategory").keydown(function (e) {
            if (e.keyCode == 13) {
                Category.PageClick(1, 10);
            }
        });
    }
};

var Product = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var ProductName = $("#txtProduct").val();
        var CateNum = $("#ddlCategory").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["ProductName"] = ProductName;
        param["CateNum"] = CateNum;
        $.gitAjax({
            url: "/GoodsAjax/GetList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td><input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" data=\"" + item.SnNum + "\" value=\"" + item.SnNum + "\"/></td>";
                        Html += "<td>" + item.SnNum + "</td>";
                        Html += "<td>" + item.BarCode + "</td>";
                        Html += "<td>" + item.ProductName + "</td>";
                        Html += "<td>" + item.MinNum + "</td>";
                        Html += "<td>" + item.MaxNum + "</td>";
                        Html += "<td>" + item.AvgPrice + "</td>";
                        Html += "<td>" + item.Size + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "<td>" + item.UnitName + "</td>";
                        Html += "<td title='"+item.Description+"'>" +git.GetStrSub(item.Description,10) + "</td>";
                        Html += "<td>";
                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Product.Edit('" + item.SnNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Product.Delete('" + item.SnNum + "')\" title=\"删除\"></a>";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Product.PageClick });
            }
        });
    },
    AutoProduct: function (item, target) {
        $(item).autocomplete({
            paramName: "productName",
            url: '/Product/GoodsAjax/AutoOutsourcing',
            showResult: function (value, data) {
                var row = JSON.parse(value);

                return '<span>' + row.BarCode + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + row.ProductName + '</span>';
            },
            onItemSelect: function (item) {

            },
            maxItemsToShow: 5,
            selectedCallback: function (selectItem) {
                $(target).find("#txtSnNum").val(selectItem.SnNum);
                $(target).find("#txtChildBarCod").val(selectItem.BarCode);
                $(target).find("#txtChildName").val(selectItem.ProductName);
                var param = {};
                param["SnNum"] = selectItem.SnNum;
                $.gitAjax({
                    url: "/Product/GoodsAjax/GetUnit",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        $(target).find("#ddlUnit").val(result.Unit);
                        $(target).find("#txtRate").val(result.Rate);
                    }
                });
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
            url: "/GoodsAjax/ToExcel", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
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
    Edit: function (snNum) {
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
                    $.jBox.tip("请输入条码编号", "warn");
                    return false;
                }
                if (ProductName == undefined || ProductName == "") {
                    $.jBox.tip("请输入产品名称", "warn");
                    return false;
                }
                if (CateNum == undefined || CateNum == "") {
                    $.jBox.tip("请输入产品类型", "warn");
                    return false;
                }
                /*if (MinNum == undefined || MinNum == "") {
                    $.jBox.tip("请输入预警值下线", "warn");
                    return false;
                }
                if (MaxNum == undefined || MaxNum == "") {
                    $.jBox.tip("请输入预警值上线", "warn");
                    return false;
                }*/
                //if (AvgPrice == undefined || AvgPrice == "") {
                //    $.jBox.tip("请输入平均价格", "warn");
                //    return false;
                //}
                if (Unit == undefined || Unit == "") {
                    $.jBox.tip("请输入单位", "warn");
                    return false;
                }
                AvgPrice=git.IsEmpty(AvgPrice)?0:AvgPrice;
                InPrice=git.IsEmpty(InPrice)?0:InPrice;
                OutPrice=git.IsEmpty(OutPrice)?0:OutPrice;
                MinNum=git.IsEmpty(MinNum)?0:MinNum;
                MaxNum=git.IsEmpty(MaxNum)?0:MaxNum;
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
                param["UnitNum"] = Unit;
                param["UnitName"] = UnitName;
                param["Size"] = Size;
                param["Description"] = Description;
                param["StorageNum"] = StorageNum;
                param["DefaultLocal"] = DefaultLocal;
                param["CusNum"] = CusNum;
                param["CusName"] = CusName;
                $.gitAjax({
                    url: "/GoodsAjax/EditProduct", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (SnNum == undefined || SnNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Product.PageClick(1);
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
            $.jBox.open("get:/Product/Goods/Detail", "添加产品", 500, 380, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Product/Goods/Detail?snNum=" + snNum, "编辑产品", 500, 380, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (SnNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["SnNum"] = SnNum;
                $.gitAjax({
                    url: "/GoodsAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Product.PageClick(1);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    BatchDel: function () {
        var chklist = $("#tabInfo tbody tr").find("input:checked");
        var ids = "";
        $.each(chklist, function (index, item) {
            ids += $(item).attr("data") + ",";
        });
        if (ids.length > 0) {
            var submit = function (v, h, f) {
                if (v == 'ok') {
                    var param = {};
                    param["SnNum"] = ids;
                    $.gitAjax({
                        url: "/GoodsAjax/BatchDel", type: "post", data: param, success: function (result) {
                            if (result.d == "success") {
                                Product.PageClick(1);
                            } else {
                                $.jBox.tip("删除失败", "error");
                            }
                        }
                    });
                }
            };
            $.jBox.confirm("确定要删除吗？", "提示", submit);
        }
        else {
            $.jBox.tip("请至少选择一条数据!", 'info');
        }
    },
    DdlChange: function (DefaultLocal) {
        var Local = $("#ddlLocal");
        Local.empty();
        var StorageNum = $("#ddlStorage").val();
        var param = {};
        param["StorageNum"] = StorageNum;
        $.gitAjax({
            url: "/GoodsAjax/GetLocal",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        if (DefaultLocal != undefined || DefaultLocal != "") {
                            if (DefaultLocal == item.LocalNum) {
                                var option = $("<option selected='selected'>").text(item.LocalName).val(item.LocalNum)
                                Local.append(option);
                            }
                            else {
                                var option = $("<option>").text(item.LocalName).val(item.LocalNum)
                                Local.append(option);
                            }
                        }
                        else {
                            var option = $("<option>").text(item.LocalName).val(item.LocalNum)
                            Local.append(option);
                        }
                    });
                }
            }
        });
    }
};