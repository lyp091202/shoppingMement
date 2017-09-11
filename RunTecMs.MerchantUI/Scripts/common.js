function formatSex(val,row) {
    return val=="0"?"女":"男";
}
function formatStatus(val,row) {
    return val==0?"禁用":"启用";
}
function formatRole(val,row) {
    return val==true?"讲师":"普通客户";
}
function formatistrue(val,row) {

    return val==true?"是":"否";
}
function formatisdistribution(val,row) {

    return val==true?"已分配":"未分配";
}

function formatIsNull(val,row) {

    return val==null?"":val;
}

function formatBank(val,row) {
    if(val=="CFT") {
        return val="工商银行";
    }
    if(val=="CMB_CREDIT") {
        return val="招商银行(信用卡)";
    }

    if(val=="CCB_DEBIT") {
        return val="建设银行(借记卡)";
    }
    if(val=="SPDB_CREDIT") {
        return val="浦发银行(信用卡)";
    }

    if(val=="ICBC_DEBIT") {
        return val="工商银行(借记卡)";
    }
    if(val=="SPDB_DEBIT") {
        return val="浦发银行(借记卡)";
    }
    if(val=="ABC_DEBIT") {
        return val="农业银行(借记卡)";
    }
    if(val=="ABC_DEBIT") {
        return val="农业银行(借记卡)";
    }
    else {
        return val;
    }
}
//文件名
function filename(val,row) {

    if(val!=null&&typeof (val)!="undefined"&&val!=0) {
        if(val.IndexOf('/')>0) {

            var fileName=val.substring(val.lastIndexOf('/')+1);
            return fileName;
        }
        else {
            return val;
        }
    }
    else {
        return val;
    }
}

//展示图片
function DisplayPathPicture(val,row) {
    if(val!=null&&typeof (val)!="undefined"&&val!=0) {
        var arr=val.split(",");
        var str='';
        for(var i=0;i<arr.length;i++) {
            str+='<img  width=50px  style="max-width:90%" src="'+arr[i]+'" title="psb.jpg" alt="psb.jpg"/> '
        }

        return str;
    }
    else {
        return val;
    }
}

//判断截图
function DisplaySomePicture(val,row) {
    if(val!=null&&typeof (val)!="undefined"&&val!=0) {
        if(val.indexOf('/')!= -1) {
            var str='';
            str+='<img  width=300px height=300px  style="max-width:90%" src="'+val+'" title="psb.jpg" alt="psb.jpg"/> '
            return str;
        }
        else {
            return val
        }
    }
    else {
        return val;
    }
}
//formartter<p>
function formatP(val,row) {

    var s=val.replace("<p>",'');
    return s;

}

///播放音频
function DisplayAudio(val,row) {

    if(val!=null&&typeof (val)!="undefined"&&val!=0) {
        var num=val.indexOf('.aac');
        if(num>=0) {

            var url=val.substring(val.indexOf(')')+1)

            var audio='<audio src="'+url+'"  controls="controls"></audio>';

            return audio;

        }
        else {
            return val;
        }
    }


    else {
        return val;
    }
}

//字数太多用。。。代替
function ClipString(val,row) {

    if(val) {

        if(val.length>10) {
            var val=val.substring(0,10)+"...";
        }
    }

    return val;

}

//
function formatterWorkingYears(val,row) {

    if(val>10) {
        var WorkingYears='大于10';
        return WorkingYears;
    }
    return val;
}


function formatDateTwo(val,row) {
    if(val!=null) {
        var date=new Date(parseInt(val.replace("/Date(","").replace(")/",""),10));
        var year=date.getFullYear()===1?"000"+date.getFullYear():date.getFullYear();
        var month=date.getMonth()+1<10?"0"+(date.getMonth()+1):date.getMonth()+1;
        var currentDate=date.getDate()<10?"0"+date.getDate():date.getDate();
        return year+"-"+month+"-"+currentDate;
    }
}
function formatDateTime(val,row) {
    if(val!=null) {
        if(val.indexOf('-')>0) {
            return null;
        }
        var date=new Date(parseInt(val.replace("/Date(","").replace(")/",""),10));
        var year=date.getFullYear();
        var month=date.getMonth()+1<10?"0"+(date.getMonth()+1):date.getMonth()+1;
        var currentDate=date.getDate()<10?"0"+date.getDate():date.getDate();
        var hours=date.getHours()<10?"0"+(date.getHours()):date.getHours();
        var minutes=date.getMinutes()<10?"0"+(date.getMinutes()):date.getMinutes();
        var seconds=date.getSeconds()<10?"0"+date.getSeconds():date.getSeconds();
        return year+"-"+month+"-"+currentDate+" "+hours+":"+minutes+":"+seconds;
    }
}

function formatOnlyDate(val) {
    if(val=='/Date(-62135596800000)/') {
        return '';
    }

    if(val!=null&&typeof (val)!="undefined"&&val!=0) {
        var date=new Date(parseInt(val.replace("/Date(","").replace(")/",""),10));
        var y=date.getFullYear();
        var m=date.getMonth()+1;
        var d=date.getDate();
        return y+"-"+(m<10?("0"+m):m)+"-"+(d<10?("0"+d):d);
    }
    return val;
}

function formatDate(date) {
    if(date!=null) {
        var y=date.getFullYear();
        var m=date.getMonth()+1;
        var d=date.getDate();
        return y+"-"+(m<10?("0"+m):m)+"-"+(d<10?("0"+d):d);
    }
}
function getUrlParam(name) {
    var reg=new RegExp("(^|&)"+name+"=([^&]*)(&|$)");
    var r=window.location.search.substr(1).match(reg);
    if(r!=null) return unescape(r[2]);return null;
}
function onSelect(date) {
    $("#endtime").datebox("enable");
    $("#endtime").datebox("reset");
}

function isObjectValueEqual(a,b) {
    // Of course, we can do it use for in 
    // Create arrays of property names
    var aProps=Object.getOwnPropertyNames(a);
    var bProps=Object.getOwnPropertyNames(b);

    // If number of properties is different,
    // objects are not equivalent
    if(aProps.length!=bProps.length) {
        return false;
    }

    for(var i=0;i<aProps.length;i++) {
        var propName=aProps[i];

        // If values of same property are not equal,
        // objects are not equivalent
        if(a[propName]!==b[propName]) {
            return false;
        }
    }

    // If we made it this far, objects
    // are considered equivalent
    return true;
}

Messager={


    MFF0001: function(msg,result) {

        //result为请求处理后的返回值
        if(result) {
            $.messager.show({
                title: '系统消息',
                msg: "查询"+msg+"页面信息不存在，请修改查询条件后再查询",
                showType: 'slide',
                timeout: 3000,
                width: 200,
                height: 120
            });
        }
    },

    MFF0002: function(msg) {
        $.messager.alert("提示","没有选择数据，请选择要"+msg+"的数据！");
        return;
    },

    MFF0003: function(msg) {

        $.messager.alert("提示","多条数据被选择，请选择1条数据进行"+msg+"！");
        return;
    },

    MFF0004: function(msg) {

        $.messager.alert("提示","该数据已经被"+msg+"，如需修改请联系系统管理员。");
        return;
    },

    MFF0005: function() {

        $.messager.confirm('删除提示','确定要删除选中的数据吗？',function(row) {
            if(row) {
                return true;
            }
        });
    },
    MFF0006: function() {

        $.messager.show({
            title: '系统消息',
            msg: "输入的用户名或密码不正确，请重新输入。",
            showType: 'slide',
            timeout: 3000,
            width: 200,
            height: 120
        });
    },
    MFF0007: function() {

        $.messager.show({
            title: '系统消息',
            msg: "输入的验证码不正确，请重新输入。",
            showType: 'slide',
            timeout: 3000,
            width: 200,
            height: 120
        });

    },

    MFF0008: function(msg) {

        $.messager.alert("提示",msg+"不能为空，请输入。");
        return;

    },
    MFF0009: function() {
        $.messager.confirm('提示','确定要提交修改后的的数据吗？',function(row) {
            if(row) {
                return true;
            }
        });
    },

    MFF0010: function() {
        $.messager.alert("提示","该数据已经被修改，如需修改请在新数据基础上修改！");
        return;
    }
}

var MoneyType={
    RMB: 'RMB',
    Facoin: '发贝'
}

function show(title,msg) {
    $.messager.show({
        title: title,
        msg: msg,
        showType: 'show'
    });
}
;(function($) {
    $.fn.extend({
        //获取收入来源
        getRevenueSource: function() {
            $(this).combobox({
                method: "get",
                url: "/Ajax/GetRevenueSource",
                valueField: "id",
                textField: "name",
                value: '0',
                onLoadError: function(rep) {
                    document.write(rep.responseText);
                }
            });
        },
        //获取结算状态
        getPaymentStatus: function() {
            $(this).combobox({
                method: "get",
                url: "/Ajax/GetPaymentStatus/",
                valueField: "id",
                textField: "name",
                value: '0',
                onLoadError: function(rep) {
                    document.write(rep.responseText);
                }
            });
        },
        //获取结算状态
        getLecturePaymentStatus: function() {
            $(this).combobox({
                method: "get",
                url: "/Ajax/GetLecturePaymentStatus/",
                valueField: "id",
                textField: "name",
                value: '0',
                onLoadError: function(rep) {
                    document.write(rep.responseText);
                }
            });
        },
        //获取时间间隔
        getInterval: function() {
            $(this).combobox({
                method: "get",
                url: "/Ajax/GetInterval/",
                valueField: "id",
                textField: "name",
                value: '0',
                onLoadError: function(rep) {
                    document.write(rep.responseText);
                }
            });
        }
    });
})(jQuery);