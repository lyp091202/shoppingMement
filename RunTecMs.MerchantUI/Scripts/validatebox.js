
var SelNull="--请选择--";

//扩展日历验证
$.extend($.fn.calendar.defaults, {
    validator: function (date) {
        var now = new Date,
            cur = formatDate(now),
            d = formatDate(date);
        return d <= cur;
    }
});

 $.extend($.fn.textbox.defaults,{
   width: 173
 });
  $.extend($.fn.numberbox.defaults,{
   width: 173
 });
 $.extend($.fn.datebox.defaults,{
   width: 173
 });

//扩展EasyUI 合并行
$.extend($.fn.datagrid.methods, {
    autoMergeCells: function (jq, fields) {
        return jq.each(function () {
            var target = $(this);
            if (!fields) {
                fields = target.datagrid("getColumnFields");
            }
            var rows = target.datagrid("getRows");
            var i = 0,
            j = 0,
            temp = {};
            for (i; i < rows.length; i++) {
                var row = rows[i];
                j = 0;
                for (j; j < fields.length; j++) {
                    var field = fields[j];
                    var tf = temp[field];
                    if (!tf) {
                        tf = temp[field] = {};
                        tf[row[field]] = [i];
                    } else {
                        var tfv = tf[row[field]];
                        if (tfv) {
                            tfv.push(i);
                        } else {
                            tfv = tf[row[field]] = [i];
                        }
                    }
                }
            }
            $.each(temp, function (field, colunm) {
                $.each(colunm, function () {
                    var group = this;

                    if (group.length > 1) {
                        var before,
                        after,
                        megerIndex = group[0];
                        for (var i = 0; i < group.length; i++) {
                            before = group[i];
                            after = group[i + 1];
                            if (after && (after - before) == 1) {
                                continue;
                            }
                            var rowspan = before - megerIndex + 1;
                            if (rowspan > 1) {
                                target.datagrid('mergeCells', {
                                    index: megerIndex,
                                    field: field,
                                    rowspan: rowspan
                                });
                            }
                            if (after && (after - before) != 1) {
                                megerIndex = after;
                            }
                        }
                    }
                });
            });
        });
    },

    // 设置某单元格为编辑状态
        editCell: function (jq, param) {
            return jq.each(function () {
                var opts = $(this).datagrid('options');
                var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
                //获取列的字段为“”时改为编辑器
                for (var i = 0; i < fields.length; i++) {
                    var col = $(this).datagrid('getColumnOption', fields[i]);
                    col.editor1 = col.editor;
                    if (fields[i] != param.field) {
                        col.editor = null;
                    }
                }
                $(this).datagrid('beginEdit', param.index);
                for (var i = 0; i < fields.length; i++) {
                    var col = $(this).datagrid('getColumnOption', fields[i]);
                    col.editor = col.editor1;
                }
            });
        },

           // 设置某单元格为编辑状态
        editCell: function (jq, param) {
            return jq.each(function () {
                var opts = $(this).datagrid('options');
                var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
                //获取列的字段为“”时改为编辑器
                for (var i = 0; i < fields.length; i++) {
                    var col = $(this).datagrid('getColumnOption', fields[i]);
                    col.editor1 = col.editor;
                    if (fields[i] != param.field) {
                        col.editor = null;
                    }
                }
                $(this).datagrid('beginEdit', param.index);
                for (var i = 0; i < fields.length; i++) {
                    var col = $(this).datagrid('getColumnOption', fields[i]);
                    col.editor = col.editor1;
                }
            });
        }

});

//拓展添加编辑和移除编辑
$.extend($.fn.datagrid.methods, {
    addEditor: function (jq, param) {
        if (param instanceof Array) {
            $.each(param, function (index, item) {
                var e = $(jq).datagrid('getColumnOption', item.field);
                e.editor = item.editor;
            });
        } else {
            var e = $(jq).datagrid('getColumnOption', param.field);
            e.editor = param.editor;
        }
    },
    removeEditor: function (jq, param) {
        if (param instanceof Array) {
            $.each(param, function (index, item) {
                var e = $(jq).datagrid('getColumnOption', item);
                e.editor = {};
            });
        } else {
            var e = $(jq).datagrid('getColumnOption', param);
            e.editor = {};
        }
    }
});


/*
*  datagrid 获取正在编辑状态的行，使用如下：
*  $('#id').datagrid('getEditingRowIndexs'); //获取当前datagrid中在编辑状态的行编号列表
*/
$.extend($.fn.datagrid.methods, {
    getEditingRowIndexs: function (jq) {
        var rows = $.data(jq[0], "datagrid").panel.find('.datagrid-row-editing');
        var indexs = [];
        rows.each(function (i, row) {
            var index = row.sectionRowIndex;
            if (indexs.indexOf(index) == -1) {
                indexs.push(index);
            }
        });
        return indexs;
    }
});


$.extend($.fn.textbox.defaults.rules, {
    //验证公司名
    CheckName: {
        validator: function (value, param) {
            var flag = false;
            //var Reph = /^([a-zA-Z\u4e00-\u9fa5]+){1,20}$/;
            //if (!Reph.test(value)) {
            if (value.length>20) {
                $.fn.textbox.defaults.rules.CheckName.message = "公司名称不能超过20个字";
                return false;
            }
            $.ajax({
                type: 'post',
                cache: false,
                dataType: 'json',
                data: { name: value },
                url: "/OrganizationManager/OrgAjax/TestCompanyName/",
                async: false,
                success: function (data, textStatus, xmlHttpRequest) {
                    if (data.status == "001") {
                        $.fn.textbox.defaults.rules.CheckName.message = "公司名不能为空";
                    }
                    if (data.status == "002") {
                        $.fn.textbox.defaults.rules.CheckName.message = "公司名已存在";
                    }
                    else {
                        flag = true;
                    }
                }
            });
            if (flag) {
                $('#Name').removeClass('validatebox-invalid');
            }
            return flag;
        }
    },

    //验证员工用户名
    CheckEmpName: {
        validator: function (value, param) {
            var flag = false;
            var Reph = /^([a-zA-Z0-9]+){5,12}$/;
            if (!Reph.test(value)) {
                $.fn.textbox.defaults.rules.CheckEmpName.message = "只允许英文字母、数字,至少5位";
                return false;
            }
            $.ajax({
                type: 'post',
                cache: false,
                dataType: 'json',
                data: { name: value },
                url: "/OrganizationManager/OrgAjax/TestEmploginName/",
                async: false,
                success: function (data, textStatus, xmlHttpRequest) {
                    if (data.status == "001") {
                        $.fn.textbox.defaults.rules.CheckEmpName.message = "登录名不能为空";
                    }
                    if (data.status == "002") {
                        $.fn.textbox.defaults.rules.CheckEmpName.message = "登录名已存在";
                    }
                    else {
                        flag = true;
                    }
                }
            });
            if (flag) {
                $('#loginName').removeClass('validatebox-invalid');
            }
            return flag;
        }
    },

    // 验证旧密码
    testoldPwd :{
        validator: function (value, param) {
           var flag = false;
            $.ajax({
                type: 'post',
                cache: false,
                dataType: 'json',
                data: { oldPwd: value },
                url: "/Ajax/TestOldPwd/",
                async: false,
                success: function (data, textStatus, xmlHttpRequest) {
                    if (data.success == false) {
                        $.fn.textbox.defaults.rules.testoldPwd.message = data.msg;
                    }
                    else {
                        flag = true;
                    }
                }
            });
            if (flag) {
                $('#oldpwd').removeClass('validatebox-invalid');
            }
            return flag;
        }
    },

});

//扩展easyui表单的验证
$.extend($.fn.validatebox.defaults.rules, {
    //验证汉字
    chs: {
        validator: function(value) {
            var reg = /^[\u0391-\uFFE5]+$/;
            return reg.test(value);
        },
        message: "只能输入汉字"
    },
    //移动手机号码验证
    mobile: {
        validator: function(value) {
            var reg = /^1[3|4|5|8][0-9]\d{8}$/;
            return reg.test(value);
        },
        message: "请输入您正确的手机号码"
    },
    //远程验证登录名
    remoteMobile: {
        validator: function(value) {
            var flag = false;
            $.ajax({
                type: 'post',
                cache: false,
                dataType: "json",
                data: { mobile: value },
                url: "/Ajax/IsExistMobile",
                async: false,
                success: function(data) {
                    flag = !data.success ? false : true;
                }
            });
            return flag;
        },
        message: "手机号已经存在,请更换"
    },
    //国内邮编验证
    zipCode: {
        validator: function(value) {
            var reg = /^[0-9]\d{5}$/;
            return reg.test(value);
        },
        message: "邮政编码必须是6位数，以0开始"
    },
    //数字
    number: {
        validator: function(value) {
            var reg = /^[0-9]*$/;
            return reg.test(value);
        },
        message: "请输入数字"
    },

    // 职位
    CheckCombo: {
      validator:function(value){
        if(value==SelNull){
         return false;
        }
        else{
          return true;
        }
      },
      message: "请选择"
    },




    //密码
    safePassword: {
        validator: function(value) {
            var reg = /^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\"\"]*)|.{0,5})$|\s/;
            return !reg.test(value);
        },
        message: "密码由字母和数字组成，至少6位"

    },
    //是否相等
    equalTo: {
        validator: function(value, param) {
            return value === $(param[0]).val();
        },
        message: "两次输入的字符不一致"
    },
    //真实姓名
    trueName: {
        validator: function(value) {
            var reg = /^([a-zA-Z\u4e00-\u9fa5]+){3,12}$/;
            return reg.test(value);
        },
        message: "只允许汉字、英文字母,至少3位"
    },
    //登录名
    loginName: {
        validator: function(value) {
            var reg = /^([a-zA-Z0-9]+){5,12}$/;
            return reg.test(value);
        },
        message: "只允许英文字母、数字,长度介于{5，12}直接"
    },
    //远程验证登录名
//    remoteLoginName: {
//        validator: function(value) {
//            var flag = false;
//            var reg = /^([a-zA-Z0-9]+){5,12}$/;
//            if (!reg.test(value)) {
//                $.fn.textbox.defaults.rules.remoteLoginName.message = "只允许英文字母、数字,至少5位";
//            } else {
//                flag = true;
//            }
//            if (flag) {
//                $.ajax({
//                    type: 'post',
//                    cache: false,
//                    dataType: "json",
//                    data: { loginName: value },
//                    url: "/Ajax/IsExistLoginName",
//                    async: false,
//                    success: function(data) {
//                        flag = !data.success ? false : true;
//                    }
//                });
//            }

//            return flag;
//        },
//        message: "用户名已经存在,请更换"
//    },
    //员工代码
    staffCode: {
        validator: function(value) {
            var reg = /^[a-zA-Z0-9]{3,12}$/;
            return reg.test(value);
        },
        message: "只允许英文字母、数字,长度3-12位"
    },
    //员工性别
    verifyGender: {
        validator: function(value) {
            var reg = /^(["男"|"女"]+)$/;
            return reg.test(value);
        },
        message: "性别不正确"
    },
    //QQ号码
    qq: {
        validator: function(value) {
            var reg = /^[0-9]{5,20}$/;
            return reg.test(value);
        },
        message: "QQ号码格式不正确,至少5位"
    },
    //微信号
    webChat: {
        validator: function(value) {
            var reg = /^[a-zA-Z\d_]{5,20}$/;
            return reg.test(value);
        },
        message: "微信号格式不正确,至少5位"
    },
    //是否启用
    isEnable: {
        validator: function(value) {
            var reg = /^(["启用"|"禁用"]+)$/;
            return reg.test(value);
        },
        message: "状态不正确"
    },

    //身份证号
    idCard: {
        validator: function(value) {
            var reg = /^\d{6}(18|19|20)?\d{2}(0[1-9]|1[12])(0[1-9]|[12]\d|3[01])\d{3}(\d|X)$/;
            return reg.test(value);
        },
        message: "身份证号不正确"
    },
    //到服务器端验证
    customRemote: {
        validator: function(value, param) {
            var params = {};
            params[param[1]] = value;
            $.post(param[0], params, function(data) {
                if (data.success) {
                    $.fn.validatebox.defaults.rules.customRemote.message = param[2];
                    return false;
                }
                return true;
            });

        },
        message: ""
    },
    startDate: {
        validator: function (value, param) {
            var end = $(param[0]).datebox("getValue");
            var d1 = formatDate($.fn.datebox.defaults.parser(end)),
                d2 = formatDate($.fn.datebox.defaults.parser(value));
            return d2 <= d1;

        },
        message: "开始日期要小于结束日期"
    },
    endDate: {
        validator: function (value, param) {
            var start = $(param[0]).datebox("getValue");
            var d1 = formatDate($.fn.datebox.defaults.parser(start)),
                d2 = formatDate($.fn.datebox.defaults.parser(value));
            return d1 <= d2;

        },
        message: "结束日期要大于开始日期"
    },
    equalDate: {
        validator: function (value, param) {
            var d1 = $(param[0]).datebox("getValue");
            return value >= d1;
        },
        message: "结束日期不能早于开始日期"
    }

});
