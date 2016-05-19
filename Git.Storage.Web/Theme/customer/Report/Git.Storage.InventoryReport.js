var InventoryBook = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var change = $("#ddlChange").val();
        var searchKey = $("#txtProduct").val();
        var beginTime = $("#txtBeginTime").val();
        var endTime = $("#txtEndTime").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["SearchKey"] = searchKey;
        param["Change"] = change;
        param["beginTime"] = beginTime;
        param["endTime"] = endTime;
        $.gitAjax({
            url: "/Report/ReportAjax/InventoryList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                var tempProductNum = "";
                var rowIndex = 0;
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    tempProductNum = json.Data.List[0].ProductNum;
                    var array = new Array();
                    $(json.Data.List).each(function (i, item) {
                        if (item.ProductNum == tempProductNum) {
                            rowIndex++;
                        }
                        if (item.ProductNum != tempProductNum || parseFloat(json.Data.List.length) == parseFloat(i + 1)) {
                            array[tempProductNum] = rowIndex;
                            rowIndex = 1;
                        }
                        tempProductNum = item.ProductNum;
                    });
                    var index = 0;
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        if (index == 0) {
                            index = parseFloat(array[item.ProductNum]);
                            Html += "<td style='vertical-align:middle;' rowspan='" + index + "'>" + item.ProductNum + "</td>";
                            Html += "<td style='vertical-align:middle;' rowspan='" + index + "'>" + item.BarCode + "</td>";
                            Html += "<td style='vertical-align:middle;' rowspan='" + index + "'>" + item.ProductName + "</td>";
                        }
                        Html += "<td>" + item.Num + "</td>";
                        Html += "<td>" + git.GetEnumDesc(EChange, item.Type) + "</td>";
                        Html += "<td>" + item.ContactOrder + "</td>";
                        Html += "<td>" + item.FromLocalName + "</td>";
                        Html += "<td>" + item.ToLocalName + "</td>";
                        Html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        Html += "<td>" + item.UserName + "</td>";
                        Html += "</tr>";
                        index--;
                    });

                }
                //Html += "<tr class=\"odd gradeX\">";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td></td>";
                //Html += "<td>总计：</td>";
                //Html += "<td>" + result.AllNum + "</td>";
                //Html += "<td></td>";
                //Html += "<td>" + result.AllTotalPrice + "</td>";

                //Html += "</tr>";
                $("#tabInfo tbody").html(Html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: InventoryBook.PageClick });
            }
        });
    },
    ToExcel: function () {
        var change = $("#ddlChange").val();
        var searchKey = $("#txtProduct").val();
        var param = {};
        param["SearchKey"] = searchKey;
        param["Change"] = change;
        $.gitAjax({
            url: "/Report/ReportAjax/ToInventoryReportExcel", type: "post", data: param, success: function (result) {
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