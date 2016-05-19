
var Customer = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var CusNum = $("#txtCusNum").val();
        var CusType = $("#ddlCusType").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["CusNum"] = CusNum;
        param["CusType"] = CusType;
        $.gitAjax({
            url: "/CustomerAjax/GetCustomerList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td><input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" value=\"" + item.CusNum + "\"/></td>";
                        Html += "<td>" + item.CusNum + "</td>";
                        Html += "<td>" + item.CusName + "</td>";
                        Html += "<td>" + item.Phone + "</td>";
                        Html += "<td>" + item.Fax + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>";
                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Customer.Add('" + item.CusNum + "','','')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Customer.Delete('" + item.CusNum + "','')\" title=\"删除\"></a>";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Customer.PageClick });
            }
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
    Add: function (CusNum, SnNum, Address) {
        CusNum = CusNum == undefined ? "" : CusNum;
        SnNum = SnNum == undefined ? "" : SnNum;
        Address = Address == undefined ? "" : Address;
        //选择新增地址
        var submit = function (v, h, f) {
            if (v == 0) {
                if (SnNum == undefined || SnNum == "") {
                    $.jBox.open("get:/Client/Customer/Address", "新增地址", 390, 250, { buttons: { "确定": 0, "关闭": 1 }, submit: submitCusAddress });
                    return false;
                } else {
                    $.jBox.open("get:/Client/Customer/Address?CusNum=" + CusNum, "编辑地址", 390, 250, { buttons: { "确定": 0, "关闭": 1 }, submit: submitCusAddress });
                    return false;
                }
            } else if (v == 1) {
                var CusNum = h.find("#txtCusNum").val();
                var CusName = h.find("#txtCusName").val();
                var CusType = h.find("#ddlCusType").val();
                var Fax = h.find("#txtFax").val();
                var Email = h.find("#txtEmail").val();
                var Phone = h.find("#txtPhone").val();
                var Remark = h.find("#txtRemark").val();
                if (CusName == undefined || CusName == "") {
                    $.jBox.tip("请输入客户名称", "warn");
                    return false;
                }
                CusType = git.IsEmpty(CusType) ? "0" : CusType;
                var param = {};
                param["CusNum"] = CusNum;
                param["CusName"] = CusName;
                param["Fax"] = Fax;
                param["Email"] = Email;
                param["Phone"] = Phone;
                param["Remark"] = Remark;
                param["CusType"] = CusType;
                $.gitAjax({
                    url: "/CustomerAjax/AddCustomer", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (CusNum == undefined || CusNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Customer.PageClick(1, 10);
                            return true;
                        } else {
                            if (CusNum == undefined || CusNum == "") {
                                $.jBox.tip("添加失败", "error");
                            }
                            else {
                                $.jBox.tip("编辑失败", "error");
                            }
                        }
                    }
                });
            }
            else if (v == 2) {
                Customer.RemoveCache();
            }
        }
        var submitCusAddress = function (v, h, f) {
            if (v == 0) {
                var SnNum = h.find("#txtSnNum").val();
                var Address = h.find("#txtAddress").val();
                var Contact = h.find("#txtContact").val();
                var Phone = h.find("#txtPhone").val();
                if (Address == undefined || Address == "") {
                    $.jBox.tip("请输入地址信息", "warn");
                    return false;
                }
                var param = {};
                param["SnNum"] = SnNum;
                param["Address"] = Address;
                param["Contact"] = Contact;
                param["Phone"] = Phone;
                $.gitAjax({
                    url: "/CustomerAjax/AddAddress", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (SnNum == undefined || SnNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Customer.LoadAddress(CusNum);
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

            }
        }
        //新增客户
        if (CusNum == "" && Address == "") {
            $.jBox.open("get:/Client/Customer/Add", "新增客户", 730, 400, {
                buttons: { "新增地址": 0, "确定": 1, "关闭": 2 }, submit: submit
                , loaded: function () { Customer.RemoveCache(); }
            });
        }//编辑地址
        else if (Address == "Address") {
            $.jBox.open("get:/Client/Customer/Address?SnNum=" + SnNum, "编辑地址", 390, 250,
                { buttons: { "确定": 0, "关闭": 1 }, submit: submitCusAddress });

        }//编辑客户
        else {
            Customer.RemoveCache();
            $.jBox.open("get:/Client/Customer/Add?CusNum=" + CusNum, "编辑客户", 730, 400, {
                buttons: { "新增地址": 0, "确定": 1, "关闭": 2 },
                submit: submit, loaded: function () { Customer.LoadAddress(CusNum); }
            });
        }
    },
    //加载地址信息
    LoadAddress: function (CusNum) {
        var param = {};
        param["CusNum"] = CusNum;
        $.gitAjax({
            url: "/CustomerAjax/GetAddList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>" + item.SnNum + "</td>";
                        Html += "<td>" + item.Address + "</td>";
                        Html += "<td>" + item.Contact + "</td>";
                        Html += "<td>" + item.Phone + "</td>";
                        Html += "<td>";

                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Customer.Add('" + CusNum + "','" + item.SnNum + "','Address')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Customer.DelCusAddress('" + CusNum + "','" + item.SnNum + "')\" title=\"删除\"></a>";
                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tempCusAddress tbody").html(Html);
            }
        });
    },
    //删除客户信息
    Delete: function (CusNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["CusNum"] = CusNum;
                $.gitAjax({
                    url: "/CustomerAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Customer.PageClick(1, 10);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    //删除地址
    DelCusAddress: function (cusNum, snNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["snNum"] = snNum;
                param["cusNum"] = cusNum;
                $.gitAjax({
                    url: "/CustomerAjax/DelCusAddress", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Customer.LoadAddress(cusNum);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    //移除缓存
    RemoveCache: function () {
        $.gitAjax({
            url: "/CustomerAjax/RemoveCache", type: "post", data: "", success: function (result) {

            }
        });
    }


}




