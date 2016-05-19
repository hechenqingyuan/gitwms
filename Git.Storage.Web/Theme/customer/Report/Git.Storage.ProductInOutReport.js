
var ProductInOutReport = {
    PageClick: function (pageIndex, pageSize) {
        pageIndex = pageIndex == undefined ? 1 : pageIndex;
        pageSize = pageSize == undefined ? 10 : pageSize;
        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        param["PageSize"] = pageSize;
        param["PageIndex"] = pageIndex;

        $.gitAjax({
            url: "/Report/ReportAjax/ProductInOutReportList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        html += "<tr>";
                        html += "<td>" + item.ProductName + "</td>";
                        html += "<td>" + item.BarCode + "</td>";
                        html += "<td>" + item.SnNum + "</td>";
                        html += "<td>" + item.Size + "</td>";
                        html += "<td>" + item.InStorageNum + "</td>";
                        html += "<td>" + item.OutStorageNum + "</td>";
                        html += "<td>" + git.ToDecimal(item.InStorageNumPCT, 2) + "%</td>";
                        html += "<td>" + git.ToDecimal(item.OutStorageNumPCT, 2) + "%</td>";
                        //html += "<td>";
                        //html += "<a href=\"javascript:void(0)\" onclick=\"ProductInOutReport.Detail('" + item.SnNum + "')\">查看</a>&nbsp;&nbsp;";
                        //html += "</td>";
                        html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(html);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: ProductInOutReport.PageClick });
            }
        });
    },
    //绑定饼图数据
    BindPieData: function () {
        var queryTime = $("#liStatusGroup").find(".active").find("input:hidden").val();
        var param = {};
        param["QueryTime"] = queryTime;
        $.gitAjax({
            url: "/Report/ReportAjax/BindPieData",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var so = new SWFObject("/Theme/plugins/ampie/ampie.swf", "InStorageAmpie", "100%", "400", "8", "#FFFFFF");
                so.addVariable("path", "/Theme/plugins/ampie/");
                so.addVariable("settings_file", encodeURIComponent("/Theme/plugins/ampie/kampie_settings.xml"));
                so.addVariable("chart_data", encodeURIComponent(result.InStorageData));
                so.write("InStorageAmpieDIV");
                var so = new SWFObject("/Theme/plugins/ampie/ampie.swf", "OutStorageAmpie", "100%", "400", "8", "#FFFFFF");
                so.addVariable("path", "/Theme/plugins/ampie/");
                so.addVariable("settings_file", encodeURIComponent("/Theme/plugins/ampie/kampie_settings.xml"));
                so.addVariable("chart_data", encodeURIComponent(result.OutStorageData));
                so.write("OutStorageAmpieDIV");
            }
        });
    },
    TabClick: function () {
        $("#liStatusGroup").children("li").click(function () {
            $("#liStatusGroup").children("li").removeClass("active");
            $(this).addClass("active");
            ProductInOutReport.PageClick(1, 10);
            ProductInOutReport.BindPieData();
        });
    },
    Detail: function (snNum) {
        snNum = snNum == undefined ? "" : snNum;
        $.jBox.open("get:/Report/Report/GoodsDetail?snNum=" + snNum, "产品明细", 500, 430, { buttons: { "关闭": false } });
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