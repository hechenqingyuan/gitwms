
var UITree = function () {

    var Tree={
        LeftSource:undefined,
        RightSource:undefined,
        ToRight:function(){
            $("#tree_2").html("");
            if (Tree.RightSource) {
                var parentItem="";
                $(Tree.RightSource).each(function(i,parent){
                    if(parent.ParentNum==undefined || parent.ParentNum==""){
                        parentItem+="<li>";
                        parentItem+='<a class="tree-toggle" href="javascript:void(0);" data-ResNum="'+parent.ResNum+'" data-ParentNum="'+parent.ParentNum+'">'+parent.ResName+'</a>';
                        var flagExists=false;
                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ParentNum==parent.ResNum){
                                flagExists=true;
                                break;
                            }
                        }

                        if(flagExists){
                            parentItem+='<ul class="branch in">';
                            $(Tree.RightSource).each(function(j,child){
                                if(child.ParentNum==parent.ResNum){
                                    flagExists=false;
                                    for(var i=0;i<Tree.RightSource.length;i++){
                                        if(Tree.RightSource[i].ParentNum==child.ResNum){
                                            flagExists=true;
                                            break;
                                        }
                                    }
                                    if(flagExists){
                                        parentItem+='<a class="tree-toggle" href="javascript:void(0);" data-ResNum="'+child.ResNum+'" data-ParentNum="'+child.ParentNum+'">'+child.ResName+'</a>';
                                        parentItem+='<ul class="branch in">';
                                        $(Tree.RightSource).each(function(index,grandson){
                                            if(grandson.ParentNum==child.ResNum){
                                                parentItem+='<li><a href="javascript:void(0);" data-ResNum="'+grandson.ResNum+'" data-ParentNum="'+grandson.ParentNum+'"><i class="icon-arrow-left"></i>'+grandson.ResName+'</a></li>';
                                            }
                                        });
                                        parentItem+='</ul>';
                                    }else{
                                        parentItem+='<a href="javascript:void(0);" data-ResNum="'+child.ResNum+'" data-ParentNum="'+child.ParentNum+'"><i class="icon-arrow-left"></i>'+child.ResName+'</a>';
                                    }
                                }
                            });
                            parentItem+='</ul>';
                        }

                        parentItem+="</li>";
                    }
                });
                $("#tree_2").append(parentItem);
            }

            //绑定操作
            $("#tree_2").find("a").dblclick(function(){
                    var ResNum=$(this).attr("data-ResNum");
                    var ParentNum=$(this).attr("data-ParentNum");
                    var ResName=$(this).text();
                    
                    var delList=[];
                    if (ParentNum==undefined || ParentNum=="") {
                        //第一级
                        var flagExists=false;
                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ResNum==ResNum){
                                flagExists=true;
                                break;
                            }
                        }

                        if(!flagExists){
                            var entity={};
                            entity["ResNum"]=ResNum;
                            entity["ParentNum"]=ParentNum;
                            entity["ResName"]=ResName;
                            Tree.LeftSource.push(entity);
                            delList.push(ResNum);
                        }

                        //第二级
                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ParentNum==ResNum){
                                flagExists=false;
                                for(var j=0;j<Tree.LeftSource.length;j++){
                                    if(Tree.LeftSource[j].ResNum==Tree.RightSource[i].ResNum){
                                        flagExists=true;
                                        break;
                                    }
                                }

                                if(!flagExists){
                                    var entity={};
                                    entity["ResNum"]=Tree.RightSource[i].ResNum;
                                    entity["ParentNum"]=Tree.RightSource[i].ParentNum;
                                    entity["ResName"]=Tree.RightSource[i].ResName;
                                    Tree.LeftSource.push(entity);
                                    delList.push(Tree.RightSource[i].ResNum);
                                }

                                //判断第三级
                                for(var index=0;index<Tree.RightSource.length;index++){
                                    if(Tree.RightSource[index].ParentNum==Tree.RightSource[i].ResNum){

                                        flagExists=false;
                                        for(var j=0;j<Tree.LeftSource.length;j++){
                                            if(Tree.LeftSource[j].ResNum==Tree.RightSource[index].ResNum){
                                                flagExists=true;
                                                break;
                                            }
                                        }

                                        if(!flagExists){
                                            var entity={};
                                            entity["ResNum"]=Tree.RightSource[index].ResNum;
                                            entity["ParentNum"]=Tree.RightSource[index].ParentNum;
                                            entity["ResName"]=Tree.RightSource[index].ResName;
                                            Tree.LeftSource.push(entity);
                                            delList.push(Tree.RightSource[index].ResNum);
                                        }

                                    }
                                }
                            }
                        }


                        var delIndex=undefined;
                        for(var index=0;index<Tree.RightSource.length;index++){
                            if(Tree.RightSource[index].ResNum==ResNum){
                                delIndex=index;
                            }
                        }
                        if(delIndex!=undefined){
                            Tree.RightSource.splice(delIndex,1);
                        }

                    }
                    else{
                        var parent=undefined;
                        var root=undefined;
                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ResNum==ParentNum){
                                parent= Tree.RightSource[i];
                                break;
                            }
                        }
                        if(parent!=undefined && parent.ParentNum!=undefined && parent.ParentNum!=""){
                            for(var i=0;i<Tree.RightSource.length;i++){
                                if(Tree.RightSource[i].ResNum==parent.ParentNum){
                                    root= Tree.RightSource[i];
                                    break;
                                }
                            }
                        }

                        var flagExists=false;
                        if(root!=undefined){
                            for(var j=0;j<Tree.LeftSource.length;j++){
                                if(Tree.LeftSource[j].ResNum==root.ResNum){
                                    flagExists=true;
                                    break;
                                }
                            }

                            if(!flagExists){
                                var entity={};
                                entity["ResNum"]=root.ResNum;
                                entity["ParentNum"]=root.ParentNum;
                                entity["ResName"]=root.ResName;
                                Tree.LeftSource.push(entity);
                            }
                        }
                        flagExists=false;
                        if(parent!=undefined){
                            for(var j=0;j<Tree.LeftSource.length;j++){
                                if(Tree.LeftSource[j].ResNum==parent.ResNum){
                                    flagExists=true;
                                    break;
                                }
                            }

                            if(!flagExists){
                                var entity={};
                                entity["ResNum"]=parent.ResNum;
                                entity["ParentNum"]=parent.ParentNum;
                                entity["ResName"]=parent.ResName;
                                Tree.LeftSource.push(entity);
                            }
                        }

                        //添加当前点击对象
                        var entity={};
                        entity["ResNum"]=ResNum;
                        entity["ParentNum"]=ParentNum;
                        entity["ResName"]=ResName;
                        Tree.LeftSource.push(entity);
                        delList.push(ResNum);

                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ParentNum==ResNum){
                                flagExists=false;
                                for(var j=0;j<Tree.LeftSource.length;j++){
                                    if(Tree.LeftSource[j].ResNum==Tree.RightSource[i].ResNum){
                                        flagExists=true;
                                        break;
                                    }
                                }
                                if(!flagExists){
                                    var entity={};
                                    entity["ResNum"]=Tree.RightSource[i].ResNum;
                                    entity["ParentNum"]=Tree.RightSource[i].ParentNum;
                                    entity["ResName"]=Tree.RightSource[i].ResName;
                                    Tree.LeftSource.push(entity);
                                    delList.push(Tree.RightSource[i].ResNum);
                                }
                            }
                        }
                    }

                    function del(list){
                        for(var i=0;i<list.length;i++){
                            var ResNum=list[i];
                            var delIndex=undefined;
                            for(var index=0;index<Tree.RightSource.length;index++){
                                if(Tree.RightSource[index].ResNum==ResNum){
                                    delIndex=index;
                                }
                            }
                            if(delIndex!=undefined){
                                Tree.RightSource.splice(delIndex,1);
                            }
                        }
                    }

                    //删除原始数据
                    del(delList);

                    if(parent!=undefined){
                        var flag=false;
                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ParentNum==parent.ResNum){
                                flag=true;
                                break;
                            }
                        }
                        if(!flag){
                            var delIndex=undefined;
                            for(var index=0;index<Tree.RightSource.length;index++){
                                if(Tree.RightSource[index].ResNum==parent.ResNum){
                                    delIndex=index;
                                }
                            }
                            if(delIndex!=undefined){
                                Tree.RightSource.splice(delIndex,1);
                            }
                        }
                    }

                    if(root!=undefined){
                        var flag=false;
                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ParentNum==root.ResNum){
                                flag=true;
                                break;
                            }
                        }
                        if(!flag){
                            var delIndex=undefined;
                            for(var index=0;index<Tree.RightSource.length;index++){
                                if(Tree.RightSource[index].ResNum==root.ResNum){
                                    delIndex=index;
                                }
                            }
                            if(delIndex!=undefined){
                                Tree.RightSource.splice(delIndex,1);
                            }
                        }
                    }

                    Tree.ToLeft();
                    Tree.ToRight();
                });
        },
        ToLeft:function(){
            $("#tree_1").html("");
            if (Tree.LeftSource) {
                var parentItem="";
                $(Tree.LeftSource).each(function(i,parent){
                    if(parent.ParentNum==undefined || parent.ParentNum==""){
                        parentItem+="<li>";
                        parentItem+='<a class="tree-toggle" href="javascript:void(0);" data-ResNum="'+parent.ResNum+'" data-ParentNum="'+parent.ParentNum+'">'+parent.ResName+'</a>';

                        var flagExists=false;
                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ParentNum==parent.ResNum){
                                flagExists=true;
                                break;
                            }
                        }

                        if(flagExists){
                            parentItem+='<ul class="branch in">';
                            $(Tree.LeftSource).each(function(j,child){
                                if(child.ParentNum==parent.ResNum){
                                    flagExists=false;
                                    for(var i=0;i<Tree.LeftSource.length;i++){
                                        if(Tree.LeftSource[i].ParentNum==child.ResNum){
                                            flagExists=true;
                                            break;
                                        }
                                    }
                                    if(flagExists){
                                        parentItem+='<a class="tree-toggle" href="javascript:void(0);" data-ResNum="'+child.ResNum+'" data-ParentNum="'+child.ParentNum+'">'+child.ResName+'</a>';
                                        parentItem+='<ul class="branch in">';
                                        $(Tree.LeftSource).each(function(index,grandson){
                                            if(grandson.ParentNum==child.ResNum){
                                                parentItem+='<li><a href="javascript:void(0);" data-ResNum="'+grandson.ResNum+'" data-ParentNum="'+grandson.ParentNum+'"><i class="icon-arrow-right"></i>'+grandson.ResName+'</a></li>';
                                            }
                                        });
                                        parentItem+='</ul>';
                                    }else{
                                        parentItem+='<a href="javascript:void(0);" data-ResNum="'+child.ResNum+'" data-ParentNum="'+child.ParentNum+'"><i class="icon-arrow-right"></i>'+child.ResName+'</a>';
                                    }
                                }
                            });
                            parentItem+='</ul>';
                        }
                        parentItem+="</li>";
                    }
                });
                $("#tree_1").append(parentItem);

                $("#tree_1").find("a").dblclick(function(){
                    var ResNum=$(this).attr("data-ResNum");
                    var ParentNum=$(this).attr("data-ParentNum");
                    var ResName=$(this).text();
                    
                    var delList=[];
                    if (ParentNum==undefined || ParentNum=="") {
                        //第一级
                        var flagExists=false;
                        for(var i=0;i<Tree.RightSource.length;i++){
                            if(Tree.RightSource[i].ResNum==ResNum){
                                flagExists=true;
                                break;
                            }
                        }

                        if(!flagExists){
                            var entity={};
                            entity["ResNum"]=ResNum;
                            entity["ParentNum"]=ParentNum;
                            entity["ResName"]=ResName;
                            Tree.RightSource.push(entity);
                            delList.push(ResNum);
                        }

                        //第二级
                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ParentNum==ResNum){
                                flagExists=false;
                                for(var j=0;j<Tree.RightSource.length;j++){
                                    if(Tree.RightSource[j].ResNum==Tree.LeftSource[i].ResNum){
                                        flagExists=true;
                                        break;
                                    }
                                }

                                if(!flagExists){
                                    var entity={};
                                    entity["ResNum"]=Tree.LeftSource[i].ResNum;
                                    entity["ParentNum"]=Tree.LeftSource[i].ParentNum;
                                    entity["ResName"]=Tree.LeftSource[i].ResName;
                                    Tree.RightSource.push(entity);
                                    delList.push(Tree.LeftSource[i].ResNum);
                                }

                                //判断第三级
                                for(var index=0;index<Tree.LeftSource.length;index++){
                                    if(Tree.LeftSource[index].ParentNum==Tree.LeftSource[i].ResNum){

                                        flagExists=false;
                                        for(var j=0;j<Tree.RightSource.length;j++){
                                            if(Tree.RightSource[j].ResNum==Tree.LeftSource[index].ResNum){
                                                flagExists=true;
                                                break;
                                            }
                                        }

                                        if(!flagExists){
                                            var entity={};
                                            entity["ResNum"]=Tree.LeftSource[index].ResNum;
                                            entity["ParentNum"]=Tree.LeftSource[index].ParentNum;
                                            entity["ResName"]=Tree.LeftSource[index].ResName;
                                            Tree.RightSource.push(entity);
                                            delList.push(Tree.LeftSource[index].ResNum);
                                        }

                                    }
                                }
                            }
                        }

                        var delIndex=undefined;
                        for(var index=0;index<Tree.LeftSource.length;index++){
                            if(Tree.LeftSource[index].ResNum==ResNum){
                                delIndex=index;
                            }
                        }
                        if(delIndex!=undefined){
                            Tree.LeftSource.splice(delIndex,1);
                        }
                        
                    }
                    else{
                        var parent=undefined;
                        var root=undefined;
                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ResNum==ParentNum){
                                parent= Tree.LeftSource[i];
                                break;
                            }
                        }
                        if(parent!=undefined && parent.ParentNum!=undefined && parent.ParentNum!=""){
                            for(var i=0;i<Tree.LeftSource.length;i++){
                                if(Tree.LeftSource[i].ResNum==parent.ParentNum){
                                    root= Tree.LeftSource[i];
                                    break;
                                }
                            }
                        }

                        var flagExists=false;
                        if(root!=undefined){
                            for(var j=0;j<Tree.RightSource.length;j++){
                                if(Tree.RightSource[j].ResNum==root.ResNum){
                                    flagExists=true;
                                    break;
                                }
                            }

                            if(!flagExists){
                                var entity={};
                                entity["ResNum"]=root.ResNum;
                                entity["ParentNum"]=root.ParentNum;
                                entity["ResName"]=root.ResName;
                                Tree.RightSource.push(entity);
                            }
                        }
                        flagExists=false;
                        if(parent!=undefined){
                            for(var j=0;j<Tree.RightSource.length;j++){
                                if(Tree.RightSource[j].ResNum==parent.ResNum){
                                    flagExists=true;
                                    break;
                                }
                            }

                            if(!flagExists){
                                var entity={};
                                entity["ResNum"]=parent.ResNum;
                                entity["ParentNum"]=parent.ParentNum;
                                entity["ResName"]=parent.ResName;
                                Tree.RightSource.push(entity);
                            }
                        }

                        //添加当前点击对象
                        var entity={};
                        entity["ResNum"]=ResNum;
                        entity["ParentNum"]=ParentNum;
                        entity["ResName"]=ResName;
                        Tree.RightSource.push(entity);
                        delList.push(ResNum);

                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ParentNum==ResNum){
                                flagExists=false;
                                for(var j=0;j<Tree.RightSource.length;j++){
                                    if(Tree.RightSource[j].ResNum==Tree.LeftSource[i].ResNum){
                                        flagExists=true;
                                        break;
                                    }
                                }
                                if(!flagExists){
                                    var entity={};
                                    entity["ResNum"]=Tree.LeftSource[i].ResNum;
                                    entity["ParentNum"]=Tree.LeftSource[i].ParentNum;
                                    entity["ResName"]=Tree.LeftSource[i].ResName;
                                    Tree.RightSource.push(entity);
                                    delList.push(Tree.LeftSource[i].ResNum);
                                }
                            }
                        }
                    }

                    function del(list){
                        for(var i=0;i<list.length;i++){
                            var ResNum=list[i];
                            var delIndex=undefined;
                            for(var index=0;index<Tree.LeftSource.length;index++){
                                if(Tree.LeftSource[index].ResNum==ResNum){
                                    delIndex=index;
                                }
                            }
                            if(delIndex!=undefined){
                                Tree.LeftSource.splice(delIndex,1);
                            }
                        }
                    }

                    //删除原始数据
                    del(delList);

                    if(parent!=undefined){
                        var flag=false;
                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ParentNum==parent.ResNum){
                                flag=true;
                                break;
                            }
                        }
                        if(!flag){
                            var delIndex=undefined;
                            for(var index=0;index<Tree.LeftSource.length;index++){
                                if(Tree.LeftSource[index].ResNum==parent.ResNum){
                                    delIndex=index;
                                }
                            }
                            if(delIndex!=undefined){
                                Tree.LeftSource.splice(delIndex,1);
                            }
                        }
                    }

                    if(root!=undefined){
                        var flag=false;
                        for(var i=0;i<Tree.LeftSource.length;i++){
                            if(Tree.LeftSource[i].ParentNum==root.ResNum){
                                flag=true;
                                break;
                            }
                        }
                        if(!flag){
                            var delIndex=undefined;
                            for(var index=0;index<Tree.LeftSource.length;index++){
                                if(Tree.LeftSource[index].ResNum==root.ResNum){
                                    delIndex=index;
                                }
                            }
                            if(delIndex!=undefined){
                                Tree.LeftSource.splice(delIndex,1);
                            }
                        }
                    }

                    Tree.ToLeft();
                    Tree.ToRight();
                });
            }
        },
        SetData:function(){
            var RoleNum=$("#hdRoleNum").val();
            var param={};
            param["RoleNum"]=RoleNum;
            $.gitAjax({
                url: "/ResAjax/GetTreeSource",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    Tree.RightSource=JSON.parse(result.ListAlloted);
                    Tree.LeftSource=JSON.parse(result.ListNotAlloted);
                    Tree.ToLeft();
                    Tree.ToRight();
                }
            });
        }
    };

    return {
        init: function () {
            Tree.SetData();
        },
        Save: function () {
            
            var list = [];
            if (Tree.RightSource!=undefined) {
                $(Tree.RightSource).each(function (i, item) {
                    var ResNum = item.ResNum;
                    list.push(ResNum);
                });
            }
            var RoleNum = $("#hdRoleNum").val();
            if (list.length == 0) {
                $.jBox.tip("请选择要分配的权限", "warn");
                return false;
            }
            console.log(list);
            var param = {};
            param["List"] = JSON.stringify(list);
            param["roleNum"] = RoleNum;
            $.gitAjax({
                url: "/ResAjax/Save",
                data: param,
                type: "post",
                dataType: "json",
                success: function (result) {
                    if (result.d != undefined && result.d == "success") {
                        $.jBox.tip("权限设置成功", "success");
                    } else {
                        $.jBox.tip("权限设置失败", "error");
                    }
                }
            });
        }
    };
}();
