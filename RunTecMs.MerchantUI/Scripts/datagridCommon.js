
var comInfo="", depInfo="",businessInfo="";
var SelNull="--请选择--";
$(function () {
    // dataGrid列动态加载
    $.extend($.fn.datagrid.methods, {
        createHeader: function (jq, opts) {
            function buildHeader(headerContainer, columnsDefine, frozenHeader) {
                //如果列配置为空，直接返回
                if (!columnsDefine) {
                    return;
                }
                $(headerContainer).show(); //标题显示
                $(headerContainer).empty(); //清空原有内容
                //生成table的dom对象，添加到header所在的层
                var t = $("<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\"><tbody></tbody></table>").appendTo(headerContainer);
                //columns设置格式[[...],[...],[...]],第一个子数组生成表格的一行
                for (var i = 0; i < columnsDefine.length; i++) {
                    var tr = $("<tr></tr>").appendTo($("tbody", t));
                    var cols = columnsDefine[i];
                    for (var j = 0; j < cols.length; j++) {
                        var col = cols[j]; //列设置col
                        var attr = "";
                        if (col.rowspan) {//跨行设置
                            attr += "rowspan=\"" + col.rowspan + "\" ";
                        }
                        if (col.colspan) {//跨列设置
                            attr += "colspan=\"" + col.colspan + "\" ";
                        }
                        var td = $("<td " + attr + "></td>").appendTo(tr); //生成td,设置属性
                        //是否在第一列添加checkbox
                        if (col.checkbox) {
                            td.attr("field", col.field);
                            $("<div class=\"datagrid-header-check\"></div>").html("<input type=\"checkbox\"/>").appendTo(td);
                        } else {
                            //如果设置了field字段
                            if (col.field) {
                                td.attr("field", col.field);
                                td.append("<div class=\"datagrid-cell\"><span></span><span class=\"datagrid-sort-icon\"></span></div>");
                                $("span", td).html(col.title);
                                $("span.datagrid-sort-icon", td).html("&nbsp;");
                                var cell = td.find("div.datagrid-cell");
                                if (col.resizable == false) {
                                    cell.attr("resizable", "false");
                                }
                                col.boxWidth = $.boxModel ? (col.width - (cell.outerWidth() - cell.width())) : col.width;
                                cell.width(col.boxWidth);
                                cell.css("text-align", (col.align || "left"));
                            } else {
                                $("<div class=\"datagrid-cell-group\"></div>").html(col.title).appendTo(td);
                            }
                        }
                        //隐藏表格
                        if (col.hidden) {
                            td.hide();
                        }
                    }
                }
                //是否显示行号
                if (frozenHeader && opts.rownumbers) {
                    var td = $("<td rowspan=\"" + opts.frozenColumns.length + "\"><div class=\"datagrid-header-rownumber\"></div></td>");
                    if ($("tr", t).length == 0) {
                        td.wrap("<tr></tr>").parent().appendTo($("tbody", t));
                    } else {
                        td.prependTo($("tr:first", t));
                    }
                }
            };
            return jq.each(function () {
                var dc = $.data(this, "datagrid").dc;
                var headerContainer1 = dc.view1.children("div.datagrid-header");
                var headerContainer2 = dc.view2.children("div.datagrid-header");
                var header1 = headerContainer1.children("div.datagrid-header-inner"); //view1的header，行号标题，一般为空
                var header2 = headerContainer2.children("div.datagrid-header-inner"); //表格的header，显示title
                buildHeader(header1, opts.frozenColumns, true); //生成冻结表头
                buildHeader(header2, opts.columns, false); //生成表头
                header1.css("display", opts.showHeader ? "block" : "none");
                header2.css("display", opts.showHeader ? "block" : "none");
            });
        }
    });

    if($('#GoBack').length>0){
     var parurl=document.referrer;
       if(parurl.indexOf("Home") > 0){
         $('#GoBack').css('display','none');
        }
        else{
         $('#GoBack').css('display','inline-block');
        }
    }
     
     if($('#customer-dg').length>0 || $('#affiliationTable').length>0){
         $.ajax({
         type : 'post',
         url : '/OrganizationManager/OrgAjax/DepartmentCombo/',
         dataType : 'json',
         success : function(data){
         comInfo=data.comList;
          depInfo=data.depList;
         }
        });
        }
});

var Common =
{
    // 点击查询按钮的form提交
    SeekTable: function (seekform, seektable) {
        seekform.form('submit', {
            onSubmit: function (para) {
                var isValid = $(this).form('validate');
                if (!isValid) {
                    return false;
                }
            },
            success: function (data) {
                seektable.datagrid({ queryParams: $(this).serializeObject() });
            }
        })
    },

    // 上传logo图片
    uplogoPic: function (upId, upType, src) {
        $("#AddFrame").attr("src", src);
        $("#AddDiv").css("display", "bloak");
        $('#AddDiv').dialog({
            modal: true,
            title: "选择图片",
            resizable: true,
            height: 540,
            width: 700,
            buttons: [{
                text: '上传',
                handler: function () {
                    //调用子窗口的方法
                    var childWindow = $("#AddFrame")[0].contentWindow; //获取子窗体的window对象
                    // 给子窗口的方法传参
                    childWindow.ChildImgpath(upId, upType);
                    $("#AddDiv").dialog('close');
                }
            }, {
                text: '关闭',
                handler: function () {
                    $("#AddDiv").dialog('close');
                }
            }]
        });
    },

    // 地址（省、市、县）下拉列表
    Adress: function () { 

    if($('#province').length>0){
         // 获取省份(直辖市)信息
    function GetProvince() {
        var provSelector = $("#province");
        //provSelector.empty();
        var array=[];
        var arrProvince = provinceInfo;
   array.push("[");
    for (var provinceIndex in arrProvince) {
        if (provinceIndex==arrProvince.length-1) {
            array.push("{"+"name:'"+arrProvince[provinceIndex]["name"]+"',value:"+provinceIndex+"}");
    }
    else {
         array.push("{"+"name:'"+arrProvince[provinceIndex]["name"]+"',value:"+provinceIndex+"},");
    }

                }
    array.push("]");
    return array.join("");
    };

    // 获取指定省份(直辖市)的城市(辖区或县)信息
    function GetCity(provinceName) {
        var provSelector = $("#province");
        var citySelector = $("#city");
        var arrCity=[],arry=[];
        for (var provinceIndex in provinceInfo) {
            if (provinceInfo[provinceIndex]["name"] == provinceName) {
               arry = provinceInfo[provinceIndex]["sub"];
              break;
            }
        }
        return arry;
        //citySelector.empty();//移除城市框的内容
    };

    // 获取指定城市(辖区或县)的地区信息
    function GetArea(provinceName, cityName) {
        var areaSelector = $("#area");
        var arrCity, arrArea;
        for (var provinceIndex in provinceInfo) {
            if (provinceInfo[provinceIndex]["name"] == provinceName) {
                arrCity = provinceInfo[provinceIndex]["sub"];
                for (var cityIndex in arrCity) {
                    if (arrCity[cityIndex]["name"] == cityName) {
                        arrArea = arrCity[cityIndex]["sub"];
                        break;
                    }
                }
            }
        }
        return arrArea;
    };

     /**
    * 地址选择框
    */
   //地址
    $('#province').combobox({
          width:170,
          textField:'name',
          valueField:'value',
          panelHeight : 'auto',
          data:eval("("+GetProvince()+")"),
          value: SelNull,
          editable:false,
          onSelect: function(record){
          $('#area').combobox({
              value:'',
          });
          var city=GetCity(record.name);
          $('#city').combobox({
                 valueField:'name',
                 textField:'name',
                 panelHeight : 'auto',
                 data:city,
                 value:SelNull,
              });
           }
     });

      $('#city').combobox({
            width:170,
            textField:'name',
            valueField:'name',
            panelHeight : 'auto',
            editable:false,
            setValue:1,
            data:GetCity($('#province').combobox('getText')),
            onSelect:function(record){
            var parentPro=$('#province').combobox('getText');
            var Area=GetArea(parentPro,record.name);
            $('#area').combobox({
                  textFiled:'name',
                  panelHeight : 'auto',
                  valueField:'name',
                  data:Area,
                  value:SelNull,
              });
            }
      });

       $('#area').combobox({
            width:170,
            editable:false,
            textField:'name',
            valueField:'name',
       });
      }
    },

    // 拼接地址（用'-'连接）
    GetAdress: function(adress){
     if($('#province').combobox('getText')!=SelNull && $('#province').combobox('getText')!=""){
     
        var city=$('#city').combobox('getText');
             if(city!="--请选择--" && city!=""){
                city='-'+$('#city').combobox('getText');
             }
             else
             {
                city='';
             }
             if($('#area').combobox('getText')==SelNull || $('#area').combobox('getText')==''){
                var area='';
             }else{
                var area='-'+$('#area').combobox('getText');
             }
             if($.trim($('#detAdress').textbox('getText'))==""){
                var det='';
             }else{
                var det='-'+$('#detAdress').textbox('getText');
             }
             var Adress=$('#province').combobox('getText') + city + area + det;
             adress.val(Adress);
            }
    },

    // 分解地址内容
    PartAdress : function(Adress){
           if(Adress!="" && Adress!=null){
           if(Adress.indexOf("-")>0){
               var spAddress=Adress.split('-');
               $('#province').textbox('setText',spAddress[0]);
               $('#city').textbox('setText',spAddress[1]);
               $('#area').textbox('setText',spAddress[2]);
               var detadr=spAddress[3];
               if(spAddress.length > 4){
                for (var i = 4; i < spAddress.length; i++) {
                   detadr+='-'+spAddress[i];
                }
               }
               $('#detAdress').textbox('setText',detadr);
           }
           else{
               $('#province').textbox('setText',Adress);
           }
          }
        else{
           $('#province').textbox('setText','');
        }
    },

    // 公司、部门下拉列表(添加部门时使用，需判断级别)
    Compy_DepCombo: function(){
   //选择部门所属
   var csign=0;
    $('#subCompany').combobox({
         width : 175,
         url: '/OrganizationManager/OrgAjax/CompanyCombo/',
         valueField : 'CompanyID',
         textField : 'Name',
         panelHeight : 'auto',
         editable : false,
         onLoadSuccess :function(){
             if(csign==0){
                 var data = $('#subCompany').combobox('getData');
                   data.unshift(0,{'CompanyID':0,'Name': SelNull});
                   csign++;
                   $(this).combobox('loadData',data);
                }
//                 $(this).combobox('setValue', 0)
             },
         onSelect : function(record){
           $('#subDeparty').combotree({
                url: '/OrganizationManager/OrgAjax/DepartCombo?id='+record.CompanyID,
                value:SelNull,
                loadFilter: function (rows) {
                for(var i=0;i<rows.length;i++){
                    if(rows[i].PreID==""){
                        rows[i].DepName=rows[i].CompanyName;
                     }
                }
                var filter = utils.filterProperties(rows, ['ID as id', 'DepName as text', 'PreID as pid']);
                var data = utils.toTreeData(filter, 'id', 'pid', "children");
                return data;
            },
            onClick: function(node){
              if(node.id.lastIndexOf("_")<0){
                $('#subDeparty').combotree('clear');
             }
            },
            onSelect : function(item){
                    var parent = item;
                    var tree = $('#subDeparty').combotree('tree');
                    var path = new Array();
                    do {  
                        path.unshift(parent.text);
                        var parent = tree.tree('getParent', parent.target);
                        } while (parent);
              $('#deplevel').val(path.length);
//                    var pathStr = '';
//                    for (var i = 0; i < path.length; i++) {
//                        pathStr += path[i];  
//                        if (i < path.length - 1) {
//                            pathStr += ' - ';
//                        }
//                    }
//                var parentTree=pathStr;
            }
           })
         }
    });
    $('#subDeparty').combotree({
        width : 175,
     });
    },

      //角色的下拉列表
    Role_JobCombo :function(){
      var rolesign=0, jobsign=0;
        $('#subRole').combobox({
             width : 175,
             url: '/OrganizationManager/OrgAjax/RoleCombo/',
             valueField : 'RoleID',
             panelHeight : 'auto',
             textField : 'Name',
             onLoadSuccess :function(){
             if(rolesign==0){
                 var data = $(this).combobox('getData');
                   data.unshift(0,{'RoleID':0,'Name':SelNull});
                   rolesign++;
                   $(this).combobox('loadData',data);
                }
                 //$(this).combobox('setValue', 0)

             }
        });
        $('#subJob').combobox({
             width : 175,
             panelHeight : 'auto',
             url: '/OrganizationManager/OrgAjax/JobCombo/',
             valueField : 'JobID',
             textField : 'Name',
             onLoadSuccess :function(){
             if(jobsign==0){
                 var data = $(this).combobox('getData');
                   data.unshift(0,{'JobID':0,'Name':SelNull});
                   jobsign++;
                   $(this).combobox('loadData',data);
                }
                 //$(this).combobox('setValue', 0)
             }
        });
    },

    // 业务的下拉列表
    BusinessCombo : function($business){
        $business.combobox({
             width : 175,
             panelHeight : 'auto',
             url: '/OrganizationManager/OrgAjax/BusinessCombo/',
             valueField : 'BusinessID',
             textField : 'Name',
             editable : false,
             onSelect:function(){
               if($('#formRalation').length>0 && $('#CompId').val()!=0){
                 currentCustSeek();
               }
               
               if($business.is($('#subBusiness')) && $('#formOwnership').length>0 && $('#CompId').val()!=0){
                 formerShipSeek();
               }
             }
        });
    },


    // 添加员工时公司、部门下拉列表
    AddEmpCompy_DepCombo: function(){
    var comsign=0;
   //选择部门所属
    $('#subCompany').combobox({
         width : 175,
         url: '/OrganizationManager/OrgAjax/CompanyCombo/',
         valueField : 'CompanyID',
         textField : 'Name',
         panelHeight : 'auto',
         editable : false,
         onLoadSuccess :function(){
             if(comsign==0){
                 var data = $('#subCompany').combobox('getData');
                   data.unshift(0,{'CompanyID':0,'Name': SelNull});
                   comsign++;
                   $(this).combobox('loadData',data);
                }
                 //$(this).combobox('setValue', 0)
             },
         onSelect : function(record){
           $('#subDeparty').combotree({
                url: '/OrganizationManager/OrgAjax/DepartCombo?id='+record.CompanyID,
                loadFilter: function (rows) {
//                for(var i=0;i<rows.length;i++){
//                    if(rows[i].PreID==""){
//                        rows[i].DepName=rows[i].CompanyName;
//                    }
//                }
                for(var i=0;i<rows.length;i++){
                    if(rows[i].PreID==""){
                        rows[i].DepName=rows[i].CompanyName;
                     }
                }
                var filter = utils.filterProperties(rows, ['ID as id', 'DepName as text', 'PreID as pid']);
                var data = utils.toTreeData(filter, 'id', 'pid', "children");
                return data;
            },
            onClick: function(node){
              if(node.id.lastIndexOf("_")<0){
                $('#subDeparty').combotree('clear');
                //$('#subDeparty').combotree('setValue',0)
             }
            }
           });
//           $('#subRole').combotree({
//                width : 175,
//           })
         }
    });
    $('#subDeparty').combotree({
        width : 175,
        value: SelNull,
     });
    },

    // 获取所选部门ID值并赋给隐藏域
    GetDepartyID: function(depId){
        var subDep=$('#subDeparty').combotree('getValue');
                if(subDep.lastIndexOf("_")>0){
                  var pdepId=subDep.split('_');
                  var pdepIndex=subDep.split('_').length-1;
                  depId.val(pdepId[pdepIndex]);
                }
                else{
                depId.val("");
                }
    },

  //根据公司ID和部门ID来对应combotree中部门的value
  PutCompany_depart : function(CompId,DepId){
     if(DepId !="" && DepId !=null){
           var parId=CompId +"_"+DepId;
           $('#subDeparty').combotree({
                url: '/OrganizationManager/OrgAjax/DepartCombo?id='+CompId,
                loadFilter: function (rows) {
                var filter = utils.filterProperties(rows, ['ID as id', 'DepName as text', 'PreID as pid']);
                var data = utils.toTreeData(filter, 'id', 'pid', "children");
                return data;
            },
            onSelect : function(item){
                    var parent = item;
                    var tree = $('#subDeparty').combotree('tree');
                    var path = new Array();
                    do {  
                        path.unshift(parent.text);
                        var parent = tree.tree('getParent', parent.target);
                    } while (parent);
                $('#deplevel').val(path.length);
              }
           });
           $('#subDeparty').combotree('setValue',parId);
        }
  },

  //// 员工combotree
  EmployeeCombotree:function ($emptree){
    $emptree.combotree({
        url: '/OrganizationManager/OrgAjax/CompDepartEmpTree/',
        value :SelNull,
        loadFilter: function (rows) {
            var treeData = [];
            if (rows) {
                for(var i=0;i<rows.length;i++){
                    if(rows[i].PreID==""){
                        rows[i].EmployeeTrueName=rows[i].CompanyName;
                    }
                    else{
                        if(rows[i].EmployeeID == 0){
                            rows[i].EmployeeTrueName=rows[i].DepName;
                    }
                    }
                }
                 var filter = utils.filterProperties(rows, ['ID as id', 'EmployeeTrueName as text', 'PreID as pid', 'EmployeeID as empId']);
                 treeData = utils.toTreeData(filter, 'id', 'pid', "children");
             }
             return treeData;
          },
         onClick: function (node) {
             var subDep=node.id,compId=0,depId=0,empId=0;
             var empName="";
                if(subDep.lastIndexOf("_")>0){
                  var pdepId=subDep.split('_');
                  var pdepIndex=subDep.split('_').length;
                  if(pdepIndex==2){
                    if(node.empId==0){
                         compId=pdepId[0];
                         depId=pdepId[1];
                    }
                    else{
                        compId=pdepId[0];
                         empId=pdepId[1];
                         empName=node.text;
                    }
                  }
                  else{
                   if(pdepIndex==3){
                         compId=pdepId[0];
                         depId=pdepId[1];
                         empId=pdepId[2];
                         empName=node.text;
                   }
                }
                }
                else{
                 compId=subDep;
                }
                if(empId==0){
                    $emptree.combotree('clear');
                }
                if($emptree.is($('#newEmployee'))){
                     $('#newCompId').val(compId);
                     $('#newDepId').val(depId);
                     $('#newEmpId').val(empId);
                }
                else{
                     $('#CompId').val(compId);
                     $('#depID').val(depId);
                     $('#EmpName').val(empName);
                     $('#empId').val(empId);
                }
               if($('#formRalation').length>0){
                 currentCustSeek();
               }
               if($('#formOwnership').length>0 && $emptree.is($('#Employee')) ){
                 formerShipSeek();
               }
         }

        });
    },

    //获取 公司-部门-员工 树形结构
    ComDelEmpTree:function($tree,$table){
         $tree.tree({
        url: '/OrganizationManager/OrgAjax/CompDepartEmpTree/',
         onLoadSuccess : function(node, data) {
            $tree.tree('collapseAll');
           },
        loadFilter: function (rows) {
            var treeData = [];
            if (rows) {
                for(var i=0;i<rows.length;i++){
                    if(rows[i].PreID==""){
                        rows[i].EmployeeTrueName=rows[i].CompanyName;
                    }
                    else{
                        if(rows[i].EmployeeID == 0){
                            rows[i].EmployeeTrueName=rows[i].DepName;
                     }
                    }
                }
                 var filter = utils.filterProperties(rows, ['ID as id', 'EmployeeTrueName as text', 'PreID as pid', 'EmployeeID as empId']);
                 treeData = utils.toTreeData(filter, 'id', 'pid', "children");
             }
             return treeData;
          },
         onClick: function (node) {
             var subDep=node.id,compId=0,depId=0,empId=0;
             var empName="";
                if(subDep.lastIndexOf("_")>0){
                  var pdepId=subDep.split('_');
                  var pdepIndex=subDep.split('_').length;
                  if(pdepIndex==2){
                    if(node.empId==0){
                         compId=pdepId[0];
                         depId=pdepId[1];
                    }
                    else{
                        compId=pdepId[0];
                         empId=pdepId[1];
                         empName=node.text;
                    }
                  }
                  else{
                   if(pdepIndex==3){
                         compId=pdepId[0];
                         depId=pdepId[1];
                         empId=pdepId[2];
                         empName=node.text;
                   }
                  }
                }
                else{
                 compId=subDep;
                }
             $table.datagrid('load', { CompanyId:compId, DepartId: depId, EmployeeName : empName });
         }
    });
    },

    // 清除下拉列表
    ComboClear : function($combo){
     if($combo.combobox('getValue')==0){
       $combo.combobox('clear');
     }
    },
   //公共提示框
    messager: function (result,msg) { //如果保存成功后的返回操作
        //result为请求处理后的返回值
        if (result) {
            $.messager.show({
                title: '系统消息',
                msg: "查询"+msg+"页面信息不存在，请修改查询条件后再查询",
                showType: 'slide',
               timeout:3000,
               width:200,
               height:120
            });
        }
    }
}

// 由公司ID获取公司名
 function Comformatter(val){
      if (val == 0) {
                return ;
            }
       for (var i = 0; i < comInfo.length; i++) {
           if (comInfo[i].CompanyID == val) {
                   return comInfo[i].Name;
              }
         }
    };

// 由部门ID获取部门名
function Depformatter(val){
    if (val == 0) {
            return ;
        }
    for (var i = 0; i < depInfo.length; i++) {
        if (depInfo[i].DepID == val) {
                return depInfo[i].Name;
            }
        }
};

// 获取动态列
function createCol(arr) {

    var columns = new Array();
   
    $.each(arr, function (i, item) {
        if (item.sortable == "1") {
            var sortable = true;
        }
        else {
            var sortable = false;
        }
        if (item.checkbox == "1") {
            var checkbox = true;
        }
        else {
            var checkbox = false;
        }
        if (item.hidden == "1") {
            var Hidden = true;
        }
        else {
            var Hidden = false;
        }
        if (item.resizable == "1") {
            var resizable = true;
        }
        else {
            var resizable = false;
        }

        columns.push({ "field": item.field, "title": item.title, "width": item.width, "sortable": sortable, "checkbox": checkbox, "hidden": Hidden, "align": item.align, "halign": item.halign, "rowspan": item.rowspan, "colspan": item.colspan, "orderType": item.orderType, "resizable": resizable });
    });

    return columns;
}


