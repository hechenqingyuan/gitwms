var maxPageIndex = 1;
$("#hdIndex").val("1");
var InStorageReport = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Report/ReportAjax/InStorageReport",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var html = "";
                var htmlDetail = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        html += "<tr>";
                        html += "<td>" + item.OrderNum + "</td>";
                        html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        html += "<td>" + item.SupName + "</td>";
                        html += "<td>" + item.Num + "</td>";
                        html += "<td>" + item.Amount + "</td>";
                        html += "<td>";
                        html += "<a href=\"javascript:void(0)\" onclick=\"InStorageReport.Audite(1,'" + item.OrderNum + "')\">查看</a>&nbsp;&nbsp;";
                        html += "</td>";
                        html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: InStorageReport.PageClick });

            }
        });
    },
    //根据时间段显示入库的情况以及图表情况
    PageClickDetail: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Report/ReportAjax/InStorageReportDetail",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        html += "<tr>";
                        html += "<td>" + parseInt(i + 1) + "</td>";
                        html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        html += "<td>" + item.Num + "</td>";
                        html += "<td>" + item.Amount + "&nbsp;&nbsp;" + "</td>";
                        html += "</tr>";
                    });
                }
                $("#tabInfoDetail tbody").html(html);
                $("#mypagerDetail").minpager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: InStorageReport.PageClickDetail });

            }
        });
    },
    //饼图数据
    InStorageAmpie: function (index) {
        var hdIndex = $("#hdIndex").val();
        var pageIndex = hdIndex;
        if (index == -1 && hdIndex <= 1) {
            $.jBox.tip("已经到起始页！", "warn");
            return false;
        }
        else if (index == -1 && hdIndex > 1) {
            pageIndex = parseInt(hdIndex) - 1;
            $("#hdIndex").val(pageIndex);
        }
        else if (index == 1 && hdIndex > maxPageIndex) {
            $("#hdIndex").val(parseInt(hdIndex) - 1);
            $.jBox.tip("已经到最后一页！", "warn");
            return false;
        }
        else if (index == 1 && hdIndex <= maxPageIndex) {
            pageIndex = parseInt(hdIndex) + 1;
            $("#hdIndex").val(pageIndex);
        }


        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        param["PageIndex"] = pageIndex;
        if (1 <= pageIndex && pageIndex <= maxPageIndex) {
            $.gitAjax({
                url: "/Report/ReportAjax/InStorageAmpie",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    var so = new SWFObject("/Theme/plugins/amline/amline.swf", "InStorageAmpie", "100%", "400", "8", "#FFFFFF");
                    so.addVariable("path", "/Theme/plugins/amline/");
                    so.addVariable("settings_file", encodeURIComponent("/Theme/plugins/amline/amline_settings.xml"));
                    so.addVariable("chart_data", encodeURIComponent(result.InStorageData));
                    so.write("InStorageAmlineDIV");
                    maxPageIndex = Math.ceil(parseInt(result.RowCount) / 10);
                }
            });
        }
    },
    TabClick: function () {
        $("#liStatusGroup").children("li").click(function () {
            $("#liStatusGroup").children("li").removeClass("active");
            $(this).addClass("active");
            $("#hdIndex").val("1");
            maxPageIndex = 1;
            InStorageReport.PageClick(1, 10);
            InStorageReport.PageClickDetail(1, 10);
            InStorageReport.InStorageAmpie(0);
        });
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
                            InStorageReport.PageClick(1, 10);
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
    }
};