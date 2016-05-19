var ProductReport = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var ProductName = $("#txtProduct").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["ProductName"] = ProductName;
        param["BeginTime"] = beginTime;
        param["EndTime"] = endTime;
        var totalLocalProductNum = 0;
        var totalInStorageNum = 0;
        var totalOutStorageNum = 0;
        var totalBadReportNum = 0;
        $.gitAjax({
            url: "/Report/ReportAjax/ProductReportList",
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
                        Html += "<td>" + item.BarCode + "</td>";
                        Html += "<td>" + item.ProductName + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "<td>" + item.MinNum + "</td>";
                        Html += "<td>" + item.MaxNum + "</td>";
                        Html += "<td>" + item.Size + "</td>";
                        //Html += "<td>" + item.AvgPrice + "</td>";
                        Html += "<td>" + item.LocalProductNum + "</td>";
                        Html += "<td>" + item.InStorageNum + "</td>";
                        Html += "<td>" + item.OutStorageNum + "</td>";
                        Html += "<td>" + item.BadReportNum + "</td>";
                        Html += "</tr>";

                        totalLocalProductNum = item.TotalLocalProductNum;
                        totalInStorageNum = item.TotalInStorageNum;
                        totalOutStorageNum = item.TotalOutStorageNum;
                        totalBadReportNum = item.TotalBadReportNum;
                    });
                }
                Html += "<tr class=\"odd gradeX\">";
                Html += "<td></td>";
                Html += "<td></td>";
                Html += "<td></td>";
                Html += "<td></td>";
                Html += "<td></td>";
                //Html += "<td></td>";
                Html += "<td></td>";
                Html += "<td>总计：</td>";
                Html += "<td>" + totalLocalProductNum + "</td>";
                Html += "<td>" + totalInStorageNum + "</td>";
                Html += "<td>" + totalOutStorageNum + "</td>";
                Html += "<td>" + totalBadReportNum + "</td>";
                Html += "</tr>";
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: ProductReport.PageClick });
            }
        });
    },
    ToExcel: function () {
        var ProductName = $("#txtProduct").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var param = {};
        param["ProductName"] = ProductName;
        param["BeginTime"] = beginTime;
        param["EndTime"] = endTime;
        $.gitAjax({
            url: "/Report/ReportAjax/ToProductReportExcel", type: "post", data: param, success: function (result) {
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
    }

};