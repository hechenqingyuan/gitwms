var Sequence = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var TabName = $("#txtTabName").val();
        var Day = $("#txtDay").val();
        var page = {};
        page["PageIndex"] = pageIndex;
        page["PageSize"] = pageSize;
        page["TabName"] = TabName;
        page["Day"] = Day;
        $.gitAjax({
            url: "/UserAjax/Sequence",
            data: page,
            type: "post",
            dataType: "json",
            success: function (result) {
                var data = JSON.parse(result.Data);
                var Html = "";
                $(data).each(function (i, item) {
                    Html += "<tr class=\"odd gradeX\">";
                    
                    Html += "<td>" + item.Num + "</td>";
                    Html += "<td>" + item.MinNum + "</td>";
                    Html += "<td>" + item.MaxNum + "</td>";
                    Html += "<td>" + item.Day + "</td>";
                    Html += "<td>" + item.TabName + "</td>";
                    
                    Html += "</tr>";
                });
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: Sequence.PageClick });
                $("#tabInfo tbody").html(Html);
            }
        });
    }
}

var SNManager = {
    PageClick: function (pageIndex, pageSize) {
        pageSize = pageSize == undefined ? 10 : pageSize;
        var TabName = $("#txtTabName").val();
        var page = {};
        page["PageIndex"] = pageIndex;
        page["PageSize"] = pageSize;
        page["TabName"] = TabName;
        $.gitAjax({
            url: "/UserAjax/SN",
            data: page,
            type: "post",
            dataType: "json",
            success: function (result) {
                var data = JSON.parse(result.Data);
                var Html = "";
                $(data).each(function (i, item) {
                    Html += "<tr class=\"odd gradeX\" data-SN='" + item.SN + "'>";

                    Html += "<td>" + item.TabName + "</td>";
                    
                    Html += "<td><select name='FirstType' class=\"m-wrap small sn_large\">" + SNManager.GetOptions(item.FirstType) + "</select></td>";
                    Html += "<td><input name='FirstRule' type=\"text\" class=\"m-wrap small sn_medium\" value='" + item.FirstRule + "' /></td>";
                    Html += "<td><input name='FirstLength' type=\"text\" class=\"m-wrap small sn_small\" value='" + item.FirstLength + "' /></td>";

                    Html += "<td><input name='JoinChar' type=\"text\" class=\"m-wrap small sn_small\" value='" + item.JoinChar + "' /></td>";

                    Html += "<td><select name='SecondType' class=\"m-wrap small sn_large\">" + SNManager.GetOptions(item.SecondType) + "</select></td>";
                    Html += "<td><input name='SecondRule' type=\"text\" class=\"m-wrap small sn_medium\" value='" + item.SecondRule + "' /></td>";
                    Html += "<td><input name='SecondLength' type=\"text\" class=\"m-wrap small sn_small\" value='" + item.SecondLength + "' /></td>";

                    Html += "<td>";
                    Html += "<a class=\"icon-save\" href=\"javascript:void(0)\" onclick=\"SNManager.Save(this)\" title=\"保存\"></a>&nbsp;&nbsp;";
                    Html += "</td>";

                    Html += "</tr>";
                });
                $("#mypager").pager({ pagenumber: pageIndex, recordCount: result.RowCount, pageSize: pageSize, buttonClickCallback: SNManager.PageClick });
                $("#tabInfo tbody").html(Html);
            }
        });
    },
    GetOptions: function (Type) {
        var html = "";
        html += "<option value='0'>不设置规则</option>";
        for (var i = 0; i < ESequence.length; i++) {
            if (ESequence[i].Value == Type) {
                html += "<option value='" + ESequence[i].Value + "' selected='selected'>" + ESequence[i].Description + "</option>";
            } else {
                html += "<option value='" + ESequence[i].Value + "'>" + ESequence[i].Description + "</option>";
            }
        }
        return html;
    },
    Save: function (item) {
        var parent = $(item).parent().parent();
        var SN = $(parent).attr("data-SN");

        var FirstType = $(parent).find("[name='FirstType']").val();
        var FirstRule = $(parent).find("[name='FirstRule']").val();
        var FirstLength = $(parent).find("[name='FirstLength']").val();

        var JoinChar = $(parent).find("[name='JoinChar']").val();

        var SecondType = $(parent).find("[name='SecondType']").val();
        var SecondRule = $(parent).find("[name='SecondRule']").val();
        var SecondLength = $(parent).find("[name='SecondLength']").val();

        var param = {};
        param["SN"] = SN;
        param["FirstType"] = FirstType;
        param["FirstRule"] = FirstRule;
        param["FirstLength"] = FirstLength;
        param["JoinChar"] = JoinChar;
        param["SecondType"] = SecondType;
        param["SecondRule"] = SecondRule;
        param["SecondLength"] = SecondLength;

        var entity = {};
        entity["entity"] = JSON.stringify(param);
        $.gitAjax({
            url: "/UserAjax/UpdateSn",
            data: entity,
            type: "post",
            dataType: "json",
            success: function (result) {
                $.jBox.tip("保存成功","success");
            }
        });
    }
}