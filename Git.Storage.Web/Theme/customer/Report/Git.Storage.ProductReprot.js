var Product = {
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
            url: "/ReportAjax/GetList",
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
                        Html += "<td>" + item.AvgPrice + "</td>";
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
                Html += "<td></td>";
                Html += "<td></td>";
                Html += "<td>总计：</td>";
                Html += "<td>" + totalLocalProductNum + "</td>";
                Html += "<td>" + totalInStorageNum + "</td>";
                Html += "<td>" + totalOutStorageNum + "</td>";
                Html += "<td>" + totalBadReportNum + "</td>";
                Html += "</tr>";
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: Product.PageClick });
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
                $.jBox.info(result.d, "提示");
                return true;
            }
        });
        return true;
    }

};