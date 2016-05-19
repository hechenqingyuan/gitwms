var juploader = null;
var Supplier = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var SupNum = $("#txtSupNum").val();
        var SupName = $("#txtSupName").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["SupNum"] = SupNum;
        param["SupName"] = SupName;

        $.gitAjax({
            url: "/SupplierAjax/GetSupplierList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td><input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" data=\"" + item.SupNum + "\" value=\"" + item.SupNum + "\"/></td>";
                        Html += "<td>" + item.SupNum + "</td>";
                        Html += "<td>" + git.GetEnumDesc(ESupType, item.SupType) + "</td>";
                        Html += "<td title="+item.SupName+">" + git.GetStrSub(item.SupName,8) + "</td>";
                        Html += "<td>" + item.Phone + "</td>";
                        Html += "<td>" + item.Fax + "</td>";
                        Html += "<td>" + item.Email + "</td>";
                        Html += "<td>" + item.ContactName + "</td>";
                        Html += "<td title="+item.Address+">" + git.GetStrSub(item.Address,10) + "</td>";
                        Html += "<td title=" + item.Description + ">" + git.GetStrSub(item.Description, 10) + "</td>";
                        Html += "<td>";

                        Html += "<a class=\"icon-edit\" href=\"javascript:void(0)\" onclick=\"Supplier.Add('" + item.SupNum + "')\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        Html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"Supplier.Delete('" + item.SupNum + "')\" title=\"删除\"></a>";

                        Html += "</td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Supplier.PageClick });
            }
        });
    },
    ToExcel: function () {
        var SupNum = $("#txtSupNum").val();
        var SupName = $("#txtSupName").val();
        var param = {};
        param["SupNum"] = SupNum;
        param["SupName"] = SupName;
        $.gitAjax({
            url: "/SupplierAjax/ToExcel", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
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
                    param["SupNum"] = ids;
                    $.gitAjax({
                        url: "/SupplierAjax/BatchDel", type: "post", data: param, success: function (result) {
                            if (result.d == "success") {
                                Supplier.PageClick(1);
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
    Add: function (SupNum) {
        SupNum = SupNum == undefined ? "" : SupNum;
        var submit = function (v, h, f) {
            if (v == true) {
                var SupName = h.find("#txtSupName").val();
                var Phone = h.find("#txtPhone").val();
                var Fax = h.find("#txtFax").val();
                var Email = h.find("#txtEmail").val();
                var ContactName = h.find("#txtContactName").val();
                var Address = h.find("#txtAddress").val();
                var Description = h.find("#txtDescription").val();
                var SupType = h.find("#ddlSupType").val();

                if (SupName == undefined || SupName == "") {
                    $.jBox.tip("请输入供应商名称", "warn");
                    return false;
                }
                if (SupType == undefined || SupType == "") {
                    $.jBox.tip("请选择供应商类型", "warn");
                    return false;
                }
                var param = {};
                param["SupNum"] = SupNum;
                param["SupName"] = SupName;
                param["Phone"] = Phone;
                param["Fax"] = Fax;
                param["Email"] = Email;
                param["ContactName"] = ContactName;
                param["Address"] = Address;
                param["Description"] = Description;
                param["SupType"] = SupType;

                $.gitAjax({
                    url: "/SupplierAjax/AddSupplier", type: "post", data: { "entity": JSON.stringify(param) }, success: function (result) {
                        if (result.d == "success") {
                            if (SupNum == undefined || SupNum == "") {
                                $.jBox.tip("添加成功", "success");
                            } else {
                                $.jBox.tip("编辑成功", "success");
                            }
                            Supplier.PageClick(1, 10);
                            return true;
                        } else {
                            if (SupNum == undefined || SupNum == "") {
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
        if (SupNum == undefined || SupNum == "") {
            $.jBox.open("get:/Client/Supplier/AddSupplier", "添加供应商", 500, 300, { buttons: { "确定": true, "关闭": false }, submit: submit });
        } else {
            $.jBox.open("get:/Client/Supplier/AddSupplier?SupNum=" + SupNum, "编辑供应商", 500, 300, { buttons: { "确定": true, "关闭": false }, submit: submit });
        }
    },
    Delete: function (SupNum) {
        var submit = function (v, h, f) {
            if (v == 'ok') {
                var param = {};
                param["SupNum"] = SupNum;
                $.gitAjax({
                    url: "/SupplierAjax/Delete", type: "post", data: param, success: function (result) {
                        if (result.d == "success") {
                            Supplier.PageClick(1, 10);
                        } else {
                            $.jBox.tip("删除失败", "error");
                        }
                    }
                });
            }
        };
        $.jBox.confirm("确定要删除吗？", "提示", submit);
    },
    UploadLoad: function () {
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
                button: 'txtImport', // 这里设置按钮id
                action: '/upload.ashx', // 这里设置上传处理接口
                // 开始上传事件
                onUpload: function (fileName) {
                    jBox.tip('正在上传 ' + fileName + ' ...', 'loading');
                },

                // 上传完成事件
                onComplete: function (fileName, response) {
                    // response是json对象，格式可以按自己的意愿来定义，例子为： { success: true, fileUrl:'' }

                    if (response.success) {
                        var submit = function (v, h, f) {
                            if (v == true) {
                                jBox.tip('正在处理文件 ...', 'loading');
                                var param = {};
                                param["Url"] = response.fileUrl;
                                $.gitAjax({
                                    url: "/SupplierAjax/doImportFile", type: "post", data: param, success: function (result) {
                                        if (result.d != "") {
                                            window.setTimeout(function () { $.jBox.tip(result.d, 'error'); }, 600);
                                        } else {
                                            Supplier.PageClick(1);
                                            window.setTimeout(function () {
                                                $.jBox.tip("导入供应商信息成功", 'success');
                                            }, 600);
                                        }
                                    }
                                });
                             
                            } else {
                                window.setTimeout(function () { $.jBox.tip("已取消导入数据", 'success'); }, 600);

                            }

                            return true;
                        };

                        $.jBox.confirm("确定要导入该文件中供应商数据吗，确认将会清空库中原有数据？", "确认", submit, { buttons: { '确定': true, '取消': false } });


                    } else {
                        jBox.tip('上传失败', 'error');
                    }
                },

                // 系统信息显示（例如后缀名不合法）
                showMessage: function (message) {
                    jBox.tip(message, 'error');
                },

                // 取消上传事件
                onCancel: function (fileName) {
                    jBox.tip(fileName + ' 上传取消。', 'info');
                }
            });
        }

    }
};