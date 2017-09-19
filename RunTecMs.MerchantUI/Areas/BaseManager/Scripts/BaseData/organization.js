$(function () {
    // 公司名文本框
    $('#Name').textbox({
        required: true,
        validType: 'CheckName',
    });
    EasyuiTag();
    //公司、部门下拉列表
    Common.Compy_DepCombo();
    // 获取地址
    var EditDepInfo= $.cookie('cookieDepInfo');
     if(EditDepInfo ==null || EditDepInfo =="" || EditDepInfo =="null"){
        Common.Adress();
     }
    // 获取编辑公司的信息
    GetCompanyInfo();
    GetDepartInfo();
});

// 创建
function EasyuiTag() {
    // 公司列表
    var $Companydatagrid = {};
    var columns = new Array();
    $Companydatagrid.title = "";
    $Companydatagrid.toolbar = '#toolbar';
    $Companydatagrid.striped = true;
    $Companydatagrid.rownumbers = true;
    $Companydatagrid.fitColumns = true;
    $Companydatagrid.singleSelect = true;
    $Companydatagrid.nowrap = true;
    $Companydatagrid.loadMsg = "正在加载数据...";
    $.ajax({
        type: 'post',
        datatype: 'json',
        url: '/BaseManager/Organization/GetCompanyList?UsePage=' + "BMB0101" + '&UseGrid=' + "CompanyInfoList",
        success: function (returnValue) {
            $Companydatagrid.columns = new Array(createCol(returnValue.columns));
            $('#CompanyInfoList').datagrid($Companydatagrid);
            $('#CompanyInfoList').datagrid('reload');

            $('#CompanyInfoList').datagrid('loadData', returnValue);
        }
    });

    // 部门列表
    var $Depdatagrid = {};
    $Depdatagrid.title = "";
    $Depdatagrid.toolbar = '#toolbar';
    $Depdatagrid.striped = true;
    $Depdatagrid.rownumbers = true;
    $Depdatagrid.fitColumns = true;
    $Depdatagrid.singleSelect = true;
    $Depdatagrid.nowrap = true;
    $Depdatagrid.loadMsg = "正在加载数据...";
    $.ajax({
        type: 'post',
        datatype: 'json',
        url: '/BaseManager/Organization/GetDepartList?UsePage=' + "BMB0102" + '&UseGrid=' + "CompanyInfoList",
        success: function (returnValue) {
            $Depdatagrid.columns = new Array(createCol(returnValue.columns));
            $('#DepartmentTable').datagrid($Depdatagrid);
            $('#DepartmentTable').datagrid('reload');

            $('#DepartmentTable').datagrid('loadData', returnValue);
        }
    });
};

// 点击查询按钮
function CompanySeek(){
     $form=$('#companyform');
     $table = $('#CompanyInfoList');
     Common.SeekTable($form,$table);
};


// 上传公司logo
function upCompanylogo(){
    var compId=$('#companyId').val();
    var type = "Company";
    var src = "/BaseManager/OrgAjax/UploadLogoWindow/";
    Common.uplogoPic(compId,type,src);
};

    //子窗口调用，给logo图片修改src值（值来自子窗口）
    function UploadImg(value){
        $("#hideImagePath").val(value);
        if($("#upLogo").length>0)
        {
            $("#upLogo").attr("src","");
            $("#upLogo").attr("src",value+"?t="+Math.random());
        }
        else
        {
            $("#upLogo").attr("src",value);
        }
    };


// 编辑公司信息（获取信息）
function GetCompanyInfo(){
    var EditCompInfo= $.cookie('cookieInfo');
    if(EditCompInfo!=null && EditCompInfo!=""){
        $.cookie('cookieInfo', '',{expiress:1,path:'/'});
        $('#submit').css('display','none');
        $('#clearInfo').css('display','none');
        $('#EditInfo').css('display','inline-block');
        $('#Name').textbox({
            required : true,
            validType : 'length[0,20]',
        });
        var cookieData= eval('('+EditCompInfo+')'); 

        $.ajax({
                 type: 'post',
                 data: cookieData,
                 datatype: 'json',
                 cache: false,
                 url: '/BaseManager/Organization/GetCompanyInfo/',
                 success: function (data, textStatus, xmlHttpRequest) {
                     if (data !=null ) {
                       rowData=data.comInfo;
                        //编辑信息填充
                        EditComDatafill(rowData);
                     }
                     else {
                         $.messager.alert('提示', '失败', 'info');
                     }
                 },
                 error: function (xmlHttpRequest, textStatus, errorThrown) {
                }
          })
    }
    else{
            $('#EditInfo').css('display','none');
            $('#submit').css('display','inline-block');
            $('#clearInfo').css('display','inline-block');

    }
    };

//填充公司信息
function EditComDatafill(rowData){
      $('#CompanyPanel').panel({title:'修改 '+rowData.Name+' 的信息' });
        rowData.RegistrationTime = formatDateTwo(rowData.RegistrationTime);
        rowData.CompanyCode=$.trim(rowData.CompanyCode);
        rowData.Remark=$.trim(rowData.Remark);
        rowData.CompanyDescription=$.trim(rowData.CompanyDescription);
        Common.PartAdress(rowData.RegistrationAddr);
             if(rowData.CompanyPicName!="" && rowData.CompanyPicName!=null) {
                $("#upLogo").attr("src",rowData.CompanyPicName);
                $('#hideImagePath').val(rowData.CompanyPicName);
             }
        $('#formCompany').form('load', rowData);
    }

// 编辑部门信息（获取信息）
function GetDepartInfo(){
     var EditDepInfo= $.cookie('cookieDepInfo');
     if(EditDepInfo!=null && EditDepInfo!=""){
        $.cookie('cookieDepInfo', '',{expiress:1,path:'/'});
        $('#depsubmit').css('display','none');
        $('#depclearInfo').css('display','none');
        $('#EditDepInfo').css('display','inline-block');
        var cookieData= eval('('+EditDepInfo+')'); 

        $.ajax({
                 type: 'post',
                 data: cookieData,
                 datatype: 'json',
                 cache: false,
                 url: '/BaseManager/Organization/GetDepartList/',
                 success: function (data, textStatus, xmlHttpRequest) {
                     if (data !=null ) {
                       rowData=data.departInfo;
                        //编辑信息填充
                        EditDepDatafill(rowData);
                     }
                     else {
                         $.messager.alert('提示', '失败', 'info');
                     }
                 },
                 error: function (xmlHttpRequest, textStatus, errorThrown) {
                }
          })
        
    }
    else{
            $('#EditDepInfo').css('display','none');
            $('#depsubmit').css('display','inline-block');
            $('#depclearInfo').css('display','inline-block');

    }
    };

//填充编辑部门信息
function EditDepDatafill(rowData){
    $('#DepartPanel').panel({title:'修改 '+rowData.Name+' 的信息' });
        
           $('#subDeparty').combotree({
               url: '/BaseManager/OrgAjax/DepartCombo?id=' + rowData.CompanyID,
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
              }
           });
           $('#subCompany').combobox('setValue',rowData.CompanyID);
        
       if(rowData.ParentDepID !="" && rowData.ParentDepID !=null){
           var parId=rowData.CompanyID +"_"+rowData.ParentDepID;
           $('#subDeparty').combotree('setValue',parId);
           }
           

            if(rowData.DepPicName!="" && rowData.DepPicName!=null) {
                $("#upLogo").attr("src",rowData.DepPicName);
                $('#hideImagePath').val(rowData.DepPicName);
             }
        $('#formDepart').form('load', rowData);
    };

// 提交表单前传值
function GetValue(){
     $adress=$('#Adress');
     Common.GetAdress($adress);
};

// 部门提交表单前传值
function DepGetValue(){
    var subDep=$('#subDeparty').combotree('getValue');
            if(subDep.lastIndexOf("_")>0){
                var pdepId=subDep.split('_');
                var pdepIndex=subDep.split('_').length-1;
                $('#parentId').val(pdepId[pdepIndex]);
            }
            else{
                $('#parentId').val(subDep);
            }
};

// 提交表单按钮
function submitForm(){
    $('#formCompany').form('submit',{
        url: "/BaseManager/Organization/AddCompanyInfo/",
        onSubmit:function(){
            GetValue();
            return $(this).form('enableValidation').form('validate');
        },
        success:function(data){
        var dataobj=eval('('+data+')');
            if (dataobj.status == true) {
        $.messager.show({
            title : '提示', 
            msg : '添加成功',
            timeout : 2000,
        });
            clearForm();
        }
        else {
        $.messager.alert('提示', '添加失败', 'info');
        }
        }
                
    });
};

// 清除表单按钮
function clearForm(){
    $('#formCompany').form('reset');
};

// 提交修改表单按钮
function EditForm(){
    $.messager.confirm('提示','确定要修改信息？',function(r){
        if(r){
            $('#formCompany').form('submit',{
                url: '/BaseManager/Organization/UpdateCompany/',
            onSubmit:function(){
                GetValue(); 
                return $(this).form('enableValidation').form('validate');
            },
            success:function(data){
                var dataobj=eval('('+data+')');
                if(dataobj.status==true){
                $.messager.show({
                title:'提示',
                msg:'更新成功',
                timeout:2000,
                });
                }
                else{
                if(dataobj.status=="002"){
                $.messager.alert('提示','公司名称已存在','info');
                }
                else{
                $.messager.alert('提示','更新失败','info');
                }
                }
            },
        });
        }
    }); 
};

// 删除公司
function DeleteCompany(){
    var selRow = $('#CompanyInfoList').datagrid('getSelected');
    if(selRow){
        var compId=selRow.CompanyID;
        $.messager.confirm('提示','确认要删除这个公司的所有信息吗？',function(r){
            if(r){
                $.ajax({
                 type: 'post',
                 data: { companyId: compId},
                 datatype: 'json',
                 cache: false,
                 url: '/BaseManager/Organization/DestoryCompany/',
                 success: function (data, textStatus, xmlHttpRequest) {
                     if (data.status == true) {
                         $.messager.alert('提示', '删除成功', 'info');
                         var index = $('#CompanyInfoList').datagrid('getRowIndex', selRow);
                         $('#CompanyInfoList').datagrid('deleteRow', index);
                     }
                     else {
                         $.messager.alert('提示', '删除失败', 'info');
                     }
                 },
                 error: function (xmlHttpRequest, textStatus, errorThrown) {
        }
                });
            }
        })
    }
    else{
    $.messager.alert('提示','没有选择数据','warning');
    }
};

// 修改公司信息
function EditCompany(){
    var row = $('#CompanyInfoList').datagrid('getSelected');
    if(row){
        var data={compId : row.CompanyID};
        $.cookie('cookieInfo',JSON.stringify(data),{expiress:1,path:'/'});
        window.location.href = '/BaseManager/Organization/AddCompany';
    }
    else {
        $.messager.alert('提示', '请选择一行数据', 'warning');
    }
};

// 添加公司信息
function AddCompany() {
    window.location.href = '/BaseManager/Organization/AddCompany';
};


/**
* 部门页面
*/

// 查询部门信息
function DepartSeek(){
$form=$('#departForm');
    $table=$('#DepartmentTable');
    Common.SeekTable($form,$table);
};

// 上传部门logo
function upDepartlogo(){
    var depId=$('#depId').val();
    var type = "Department";
    var src = "/BaseManager/OrgAjax/UploadLogoWindow/";
    Common.uplogoPic(depId,type,src);
}

// 删除部门
function DeleteDepartment(){
    var selRow = $('#DepartmentTable').datagrid('getSelected');
    if(selRow){
        var depId=selRow.DepID;
        $.messager.confirm('提示','确认要删除这个部门的所有信息吗？',function(r){
            if(r){
                $.ajax({
                 type: 'post',
                 data: { departId: depId},
                 datatype: 'json',
                 cache: false,
                 url: '/BaseManager/Organization/DestoryDepart/',
                 success: function (data, textStatus, xmlHttpRequest) {
                     if (data.status == true) {
                         $.messager.alert('提示', '删除成功', 'info');
                          var index = $('#DepartmentTable').datagrid('getRowIndex', selRow);
                           $('#DepartmentTable').datagrid('deleteRow', index);
                     }
                     else {
                         $.messager.alert('提示', '删除失败', 'info');
                     }
                 },
                 error: function (xmlHttpRequest, textStatus, errorThrown) {
        }
                });
            }
        })
    }
    else{
    $.messager.alert('提示','没有选择数据','warning');
    }
};

// 点击编辑部门
function EditDepartment(){
    var row= $('#DepartmentTable').datagrid('getSelected');
    if(row){
        var data={editDepId : row.DepID};
        $.cookie('cookieDepInfo',JSON.stringify(data),{expiress:1,path:'/'});
        window.location.href = '/BaseManager/Organization/AddDepartment?DepartId=' + row.DepID;
    }
        else {
            $.messager.alert('提示', '请选择一行数据', 'warning');
    }
};

// 部门提交表单按钮
function submitDepForm(){
    $('#formDepart').form('submit',{
        url: "/BaseManager/Organization/AddDepInfo/",
            onSubmit:function(){
            DepGetValue();
                return $(this).form('enableValidation').form('validate');
            },
            success:function(data){
            var dataobj=eval('('+data+')');
                if (dataobj.status == true) {
            $.messager.show({
                title : '提示', 
                msg : '添加成功',
                timeout : 2000,
            });
                clearDepForm();
            }
            else {
            if(dataobj.status=="002"){
                $.messager.alert('提示','部门已存在','info');
                }
                else{
                $.messager.alert('提示', '添加失败', 'info');
                }
            }
            }
                
        });
};

// 清空添加部门表单
function clearDepForm(){
    $('#formDepart').form('reset');
};

// 编辑部门信息
function EditDepForm(){
    $.messager.confirm('提示','确定要修改信息？',function(r){
        if(r){
            $('#formDepart').form('submit',{
                url: '/BaseManager/Organization/UpdateDepartment/',
            onSubmit:function(){
                DepGetValue(); 
                return $(this).form('enableValidation').form('validate');
            },
            success:function(data){
                var dataobj=eval('('+data+')');
                if(dataobj.status==true){
                $.messager.show({
                title:'提示',
                msg:'更新成功',
                timeout:2000,
                });
                }
                else{
                if(dataobj.status=="002"){
                $.messager.alert('提示','部门名称已存在','info');
                }
                else{
                $.messager.alert('提示','更新失败','info');
                }
                }
            },
        });
        }
    }); 
};
