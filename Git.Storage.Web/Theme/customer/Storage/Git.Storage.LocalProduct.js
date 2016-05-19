var LocalProduct = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var ProductName = $("#txtProduct").val();
        var CateNum = $("#ddlCategory").val();
        var param = {};
        param["PageIndex"] = pageIndex;
        param["PageSize"] = pageSize;
        param["ProductName"] = ProductName;
        param["CateNum"] = CateNum;
        $.gitAjax({
            url: "/Storage/LocalProductAjax/GetList",
            data: param,
            type: "post",
            dataType: "json",
            success: function (result) {
                var json = result;
                var Html = "";
                if (json.Data != undefined && json.Data.List != undefined && json.Data.List.length > 0) {
                    $(json.Data.List).each(function (i, item) {
                        Html += "<tr class=\"odd gradeX\">";
                        Html += "<td><input type=\"checkbox\" name=\"user_item\" class=\"checkboxes\" data=\"" + item.SnNum + "\"/></td>";
                        Html += "<td>" + item.SnNum + "</td>";
                        Html += "<td>" + item.BarCode + "</td>";
                        Html += "<td>" + unescape(item.ProductName) + "</td>";
                        Html += "<td>" + item.Display + "</td>";
                        Html += "<td>" + item.Size + "</td>";
                        Html += "<td>" + item.CateName + "</td>";
                        Html += "<td>" + item.UnitName + "</td>";
                        Html += "<td>" + unescape(item.Remark) + "</td>";
                        Html += "<td>" + item.LocalName + "</td>";
                        Html += "<td><input type=\"text\" class=\"input-small inde\" value=\"" + item.LocalProductNum + "\"/></td>";
                        Html += "</tr>";
                    });
                }
                $("#tabInfo tbody").html(Html);
                $(".group-checkable").attr("checked",false);
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: json.RowCount, pageSize: pageSize, buttonClickCallback: LocalProduct.PageClick });
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
    Add: function () {
        var QtyItems = [];
        var SnNumItems = [];
        $("#tabInfo").children("tbody").children("tr").each(function (i, item) {
            var productNum = $(item).find("input[type='checkbox']").val();
            var flag = $(item).find("input[type='checkbox']").attr("checked");
            var snNum = $(item).find("input[type='checkbox']").attr("data");
            if (flag || flag == "checked") {
                var qty = $(item).find("input[type='text']").val();
                SnNumItems.push(snNum);
                QtyItems.push(qty);
            }
        });
        if (QtyItems.length == 0) {
            $.jBox.tip("请选择要初始化库存的产品！", "warn");
            return false;
        }
        var param = {};
        param["QtyItems"] = JSON.stringify(QtyItems);
        param["SnNumItems"] = JSON.stringify(SnNumItems);
        $.gitAjax({
            url: "/Storage/LocalProductAjax/Add", type: "post", data: param, success: function (result) {
                if (result.d == "success") {
                    $.jBox.tip("保存成功", "success");
                    LocalProduct.PageClick(1);
                } else {
                    $.jBox.tip("保存失败", "error");
                }
            }
        });
    }


}