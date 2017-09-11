var grid;

$(function () {
    getMessageGridList();
});

function getMessageGridList() {
    debugger;
    var $datagrid = {};
    var columns = new Array();
    $datagrid.title = "";
    $datagrid.rownumbers = true;
    $datagrid.fitColumns = true;
    $datagrid.nowrap = true;
    $datagrid.loadMsg = "正在加载数据...";
    $.ajax({
        type: 'post',
        datatype: 'json',
        url: '/Home/GetMessageGridList?UsePage=' + "工作台" + '&UseGrid=' + "MessageGridList",
        success: function (returnValue) {
            $datagrid.columns = new Array(createCol(returnValue.columns));
            $('#MessageGridList').datagrid($datagrid);
            $('#MessageGridList').datagrid('reload');

            $('#MessageGridList').datagrid('loadData', returnValue);
        }

    });
}

