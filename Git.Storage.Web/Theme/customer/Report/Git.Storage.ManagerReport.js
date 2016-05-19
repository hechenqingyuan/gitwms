
var ReportManage = {
    UploadCheck: function () {
        var juploader = null;
        $.jUploader.setDefaults({
            cancelable: true, // 可取消上传
            allowedExtensions: ['xls', 'xlsx', "frl", "txt", "doc", "frx"], // 
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
                        $("#txtFileName").val(response.fileUrl);
                        $.jBox.closeTip();
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
    Save: function () {

        ReportManage.EditMetadata();
        var ReportNum = $("#txtReportNum").val();
        var ReportName = $("#txtReportName").val();
        var ReportType = $("#ddlReportType").val();
        var Remark = $("#txtRemark").val();
        var DataSource = $("#txtDataSource").val();
        var DsType = $("#ddlDsType").val();
        var FileName = $("#txtFileName").val();
        if (git.IsEmpty(DataSource)) {
            $.jBox.tip("请输入数据源", "warn");
            return false;
        }
        
        var param = {};
        param["ReportNum"] = ReportNum;
        param["ReportName"] = ReportName;
        param["ReportType"] = ReportType;
        param["Remark"] = Remark;
        param["DataSource"] = DataSource;
        param["DsType"] = DsType;
        param["FileName"] = FileName;

        var entity = {};
        entity["entity"] = JSON.stringify(param);
        $.gitAjax({
            url: "/Report/ManagerAjax/Create",
            type: "post",
            data: entity,
            success: function (result) {
                if (result.Key == "1000") {
                    $.jBox.tip(result.Value, "success");
                } else {
                    $.jBox.tip(result.Value, "warn");
                }
            }
        });
    },
    GetMetadata: function () {
        //获得存储过程元数据
        var ProceName = $("#txtDataSource").val();
        var DsType = $("#ddlDsType").val();
        if (DsType == EDataSourceTypeJson.Procedure) {
            var param = {};
            param["ProceName"] = ProceName;
            $.gitAjax({
                url: "/Report/ManagerAjax/GetMetadata",
                type: "post",
                data: param,
                success: function (result) {
                    ReportManage.LoadData();
                }
            })
        }
    },
    LoadData: function () {
        var GetElement = function (value) {
            var html = "";
            html += "<select class=\"span11\" name='ParamElement'>";
            for (var i = 0; i < EElementType.length; i++) {
                if (value == EElementType[i].Value) {
                    html += "<option value=\"" + EElementType[i].Value + "\" selected=\"selected\">" + EElementType[i].Description + "</option>";
                } else {
                    html += "<option value=\"" + EElementType[i].Value + "\">" + EElementType[i].Description + "</option>";
                }
            }
            html += "</select>";

            return html;
        }
        var SetTable = function (result) {
            if (result.List != undefined) {
                var list = JSON.parse(result.List);
                var html = "";
                $(list).each(function (i, item) {
                    html += "<tr data-ParamNum=\"" + item.ParamNum + "\">";
                    html += "<td>" + item.ParamName + "</td>";
                    html += "<td><input type=\"text\" name=\"ShowName\" class=\"span11\" value=\"" + item.ShowName + "\" /></td>";
                    html += "<td>" + item.ParamType + "</td>";
                    html += "<td><input type=\"text\" name=\"ParamData\" class=\"span11\" value=\"" + item.ParamData + "\" /></td>";
                    html += "<td><input type=\"text\" name=\"DefaultValue\" class=\"span11\" value=\"" + item.DefaultValue + "\" /></td>";
                    html += "<td>" + GetElement(item.ParamElement) + "</td>";
                    html += "<td>";
                    html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ReportManage.Delete('" + item.ParamNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";
                    html += "</td>";
                    html += "</tr>";
                });
                $("#tabInfo").children("tbody").html(html);
            }
        }

        var param = {};
        $.gitAjax({
            url: "/Report/ManagerAjax/LoadParam",
            type: "post",
            data: param,
            success: function (result) {
                SetTable(result);
            }
        })
    },
    EditMetadata: function () {
        var list = [];
        $("#tabInfo").children("tbody").find("tr").each(function (i, item) {
            var ReportParams = {};
            ReportParams["ParamNum"] = $(item).attr("data-ParamNum");
            ReportParams["ShowName"] = $(item).find("input[name='ShowName']").val();
            ReportParams["ParamData"] = $(item).find("input[name='ParamData']").val();
            ReportParams["DefaultValue"] = $(item).find("input[name='DefaultValue']").val();
            ReportParams["ParamElement"] = $(item).find("select[name='ParamElement']").val();
            list.push(ReportParams);
        });
        if (list.length == 0) {
            return true;
        }
        var param = {};
        param["list"] = JSON.stringify(list);
        $.gitAjax({
            url: "/Report/ManagerAjax/EditMetadata",
            type: "post",
            data: param,
            async: false,
            success: function (result) {

            }
        })
    },
    Change: function () {
        var DsType = $("#ddlDsType").val();
        if (EDataSourceTypeJson.SQL == DsType) {
            $("#btnAddParam").removeClass("disabled");
        } else {
            $("#btnAddParam").addClass("disabled");
        }
    },
    ShowDialog: function () {
        var submit = function (v, h, f) {
            if (v == true) {
                var ParamName = h.find("#txtParamName").val();
                var ShowName = h.find("#txtShowName").val();
                var ParamType = h.find("#ddlParamType").val();
                var ParamData = h.find("#txtParamData").val();
                var DefaultValue = h.find("#txtDefaultValue").val();
                var ParamElement = h.find("#ddlParamElement").val();
                var entity = {};
                entity["ParamName"] = ParamName;
                entity["ShowName"] = ShowName;
                entity["ParamType"] = ParamType;
                entity["ParamData"] = ParamData;
                entity["DefaultValue"] = DefaultValue;
                entity["ParamElement"] = ParamElement;
                var param = {};
                param["entity"] = JSON.stringify(entity);
               
                //提交到缓存处理
                $.gitAjax({
                    url: "/Report/ManagerAjax/AddParam",
                    data: param,
                    type: "post",
                    dataType: "json",
                    success: function (result) {
                        ReportManage.LoadData();
                    }
                });
            }
        };
        $.jBox.open("get:/Report/Manager/AddParam", "新增参数", 400, 410, {
            buttons: { "确定": true, "关闭": false }, submit: submit, loaded: function (item) {
            }
        });
    },
    Delete: function (ParamNum) {
        var param = {};
        param["ParamNum"] = ParamNum;
        //提交到缓存处理
        $.gitAjax({
            url: "/Report/ManagerAjax/DeleteParam",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                ReportManage.LoadData();
            }
        });
    }
}


var ReportListManager = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var status = $("#btnStatusGroup").find(".disabled").val();
        var ReportName = $("#txtReportName").val();
        var ReportType = $("#ddlOrderType").val();

        var param = {};
        param["Status"] = status;
        param["ReportName"] = ReportName;
        param["ReportType"] = ReportType;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Report/ManagerAjax/GetList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var html = "";
                if (result.Data != undefined) {
                    var json = JSON.parse(result.Data);

                    $(json).each(function (i, item) {
                        html += "<tr>";
                        html += "<td>";
                        html += "<input type=\"checkbox\" name=\"report_item\" class=\"checkboxes\"  value=\"" + item.ReportNum + "\"/>";
                        html += "</td>";
                        html += "<td>" + item.ReportNum + "</td>";
                        html += "<td>" + item.ReportName + "</td>";
                        html += "<td>" + git.GetEnumDesc(EReportType, item.ReportType) + "</td>";
                        html += "<td>" + git.GetEnumDesc(EBool, item.Status) + "</td>";
                        html += "<td>" + item.Remark + "</td>";
                        html += "<td>" + git.GetEnumDesc(EDataSourceType, item.DsType) + "</td>";
                        html += "<td title=\""+item.DataSource+"\">" + git.GetStrSub(item.DataSource,10) + "</td>";
                        html += "<td title=\"" + item.FileName + "\">" + git.GetStrSub(item.FileName,15) + "</td>";
                        html += "<td>";
                        html += "<a class=\"icon-remove\" href=\"javascript:void(0)\" onclick=\"ReportListManager.Delete('" + item.ReportNum + "')\" title=\"删除\"></a>&nbsp;&nbsp;";
                        html += "<a class=\"icon-edit\" href=\"/Report/Manager/Edit?ReportNum=" + item.ReportNum + "\" title=\"编辑\"></a>&nbsp;&nbsp;";
                        html += "<a class=\"icon-bar-chart\" href=\"/Report/Manager/Show?ReportNum=" + item.ReportNum + "\" title=\"查看报表\"></a>&nbsp;&nbsp;";
                        html += "<a class=\"icon-picture\" href=\"/Report/Manager/Designer?ReportNum=" + item.ReportNum + "\" title=\"设计报表\"></a>&nbsp;&nbsp;";
                        html += "</td>";
                        html += "</tr>";
                    });

                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: ReportListManager.PageClick });
                $(".widget-title").find("input[type='checkbox']").attr("checked", false);
            }
        });
    },
    TabClick: function () {
        $("#btnStatusGroup").children("button").click(function () {
            $("#btnStatusGroup").children("button").removeClass("disabled");
            $(this).addClass("disabled");
            ReportListManager.PageClick(1, 10);
        });
    },
    Delete: function (ReportNum) {
        var param = {};
        param["ReportNum"] = ReportNum;
        $.gitAjax({
            url: "/Report/ManagerAjax/Delete",
            type: "post",
            data: param,
            async: false,
            success: function (result) {
                ReportListManager.PageClick(1,10);
            }
        });
    },
    DeleteBatch: function () {
        var items = [];
        $("#tabInfo").children("tbody").find("input[type='checkbox'][name='report_item']").each(function (i, item) {
            var flag = $(item).attr("checked");
            if (flag || flag == "checked") {
                var ReportNum = $(item).val();
                items.push(ReportNum);
            }
        });

        var param = {};
        param["list"] = JSON.stringify(items);
        $.gitAjax({
            url: "/Report/ManagerAjax/DeleteBatch",
            type: "post",
            data: param,
            async: false,
            success: function (result) {
                ReportListManager.PageClick(1, 10);
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
    Search: function () {
        var list = [];
        $("#tbSearch").find("input").each(function (i, item) {
            var ReportParams = {};
            ReportParams["ParamName"] = $(item).attr("id");
            ReportParams["DefaultValue"] = $(item).val();
            list.push(ReportParams);
        });
        var ReportNum = $("#txtReportNum").val();
        var json = JSON.stringify(list);
        json = escape(json);
        window.location.href = "/Report/Manager/Show?ReportNum=" + ReportNum + "&SearchValues=" + json;
    }
}
