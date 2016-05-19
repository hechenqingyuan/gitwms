
var CustomerReport = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Report/ReportAjax/CustomerDetailList",
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
                        html += "<td>" + item.CusName + "</td>";
                        html += "<td>" + git.GetEnumDesc(ECusType, item.CusType) + "</td>";
                        html += "<td>" + item.Phone + "</td>";
                        html += "<td>" + item.Fax + "</td>";
                        html += "<td>" + git.JsonToDateTimeymd(item.CreateTime) + "</td>";
                        html += "<td>";
                        html += item.Num;
                        html += "</td>";
                        html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: CustomerReport.PageClick });
            }
        });
    },
    //显示订单数量排名前十的客户
    CustomerReportTOP10: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Report/ReportAjax/CustomerReportTOP10",
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
                        html += "<td>" + item.CusName + "</td>";
                        html += "<td>" + git.GetEnumDesc(ECusType, item.CusType) + "</td>";
                        html += "<td>" + item.Num + "</td>";
                        html += "</tr>";
                    });
                }
                $("#tabInfoDetail tbody").html(html);

                var so = new SWFObject("/Theme/plugins/ampie/ampie.swf", "InStorageAmpie", "100%", "400", "8", "#FFFFFF");
                so.addVariable("path", "/Theme/plugins/ampie/");
                so.addVariable("settings_file", encodeURIComponent("/Theme/plugins/ampie/kampie_settings.xml"));
                so.addVariable("chart_data", encodeURIComponent(result.InStorageData));
                so.write("ampieDIV");
            }
        });
    },
    TabClick: function () {
        $("#liStatusGroup").children("li").click(function () {
            $("#liStatusGroup").children("li").removeClass("active");
            $(this).addClass("active");
            CustomerReport.PageClick(1, 10);
            CustomerReport.CustomerReportTOP10(1, 10);
        });
    }
};