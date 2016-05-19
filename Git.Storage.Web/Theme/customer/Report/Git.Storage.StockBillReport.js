var StockBillReport = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var localName = $("#txtLocalName").val();
        var localType = $("#ddlLocalType").val();
        var productName = $("#txtProduct").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["LocalName"] = localName;
        param["ProductName"] = productName;
        param["LocalType"] = localType;
        $.gitAjax({
            url: "/Report/ReportAjax/StockBillList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td>" + item.LocalName + "</td>";
                        Html += "<td>" + git.GetEnumDesc(ELocalType, item.LocalType) + "</td>";
                        Html += "<td>" + item.ProductNum + "</td>";
                        Html += "<td>" + item.BarCode + "</td>";
                        Html += "<td title='" + item.ProductName + "'>" + git.GetStrSub(item.ProductName,25) + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "<td title='"+item.Size+"'>" + git.GetStrSub(item.Size,25) + "</td>";
                        Html += "<td>" + item.MinNum + "</td>";
                        Html += "<td>" + item.MaxNum + "</td>";
                        Html += "<td>" + item.Num + "</td>";
                        //Html += "<td>" + item.AvgPrice + "</td>";
                        //Html += "<td>" + item.TotalPrice + "</td>";
                        Html += "</tr>";
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
                Html += "<td></td>";
                Html += "<td>总计：</td>";
                Html += "<td>" + result.AllNum + "</td>";
                //Html += "<td></td>";
                //Html += "<td>" + result.AllTotalPrice + "</td>";

                Html += "</tr>";
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: StockBillReport.PageClick });
            }
        });
    },
    ToExcel: function () {
        var localName = $("#txtLocalName").val();
        var ProductName = $("#txtProduct").val();
        var localType = $("#ddlLocalType").val();
        var param = {};
        param["LocalName"] = localName;
        param["ProductName"] = ProductName;
        param["LocalType"] = localType;
        $.gitAjax({
            url: "/Report/ReportAjax/ToStockBilReportExcel", type: "post", data: param, success: function (result) {
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