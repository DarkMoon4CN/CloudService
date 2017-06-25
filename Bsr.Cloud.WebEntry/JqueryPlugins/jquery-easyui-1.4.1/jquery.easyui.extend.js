//扩展easyui表单的验证
$.extend($.fn.validatebox.defaults.rules, {
    //验证汉子
    CHS: {
        validator: function (value) {
            return /^[\u0391-\uFFE5]+$/.test(value);
        },
        message: '只能输入汉字.'
    },
    //移动手机号码验证
    mobile: {//value值为文本框中的值
        validator: function (value) {
            var reg = /^1[3|4|5|7|8|9]\d{9}$/;
            return reg.test(value);
        },
        message: '输入手机号码格式不准确.'
    },
    //固定电话号码验证
    telphone: {//value值为文本框中的值
        validator: function (value) {
            var reg = /^0\d{2}-\d{8}|0\d{2}-\d{7}|0\d{3}-\d{7}|0\d{3}-\d{8}$/;
            return reg.test(value);
        },
        message: '输入号码格式不准确.'
    },
    //国内邮编验证
    zipcode: {
        validator: function (value) {
            var reg = /^[1-9]\d{5}$/;
            return reg.test(value);
        },
        message: '邮编必须是非0开始的6位数字.'
    },
    //用户账号验证
    account: {//param的值为[]中值
        validator: function (value, param) {
            var reg = /^[\u0391-\uFFE5\w]+$/;
            return reg.test(value);
        }, message: '登录名称只允许汉字、英文字母、数字及下划线.'
    },
    QQ: {
        validator: function (value, param) {
            return /^[1-9]\d{4,10}$/.test(value);
        },
        message: 'QQ号码不正确.'
    },
    safepass: {
        validator: function (value, param) {
            return safePassword(value);
        },
        message: '密码由字母和数字组成，至少6位.'
    },
    equalTo: {
        validator: function (value, param) {
            return value == $(param[0]).val();
        },
        message: '两次输入的字符不一至.'
    },
    number: {
        validator: function (value, param) {
            return /^\d+$/.test(value);
        },
        message: '请输入数字.'
    },
    idcard: {
        validator: function (value, param) {
            return idCard(value);
        },
        message: '请输入正确的身份证号码.'
    },
    maxLength: {
        validator: function (value, param) {
            var len = $.trim(value).length;
            return len <= param[0];
        },
        message: '输入内容长度必须介于0和{0}之间.'
    },
    selfemail: {
        validator: function (value, param) {
            var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/;
            return myreg.test(value);
        },
        message: '邮箱格式不正确.'
    },

    //rest服务,重复验证
    remoteRest: {
        validator: function (value, param) {
            var settings = {
                data: "{\"" + param[1] + "\": \"" + value + "\"}",
                async: false,
                success: function (msg) {
                    if (msg.IsRegister == 0 || msg.IsUse == 0) {
                        flag = false;
                        $.data(document.body, "flag", flag);
                    }
                    else {
                        flag = true;
                        $.data(document.body, "flag", flag);
                    }
                },
                error: function (e) {
                    flag = false;
                    $.data(document.body, "flag", flag);
                }
            };
            var msg = BsrCloudServer.ajax(param[0], settings);
            return $.data(document.body, "flag");
        },
        message: '已经注册.'
    },

    //rest服务,修改时的重复验证
    remoteEditRest: {
        validator: function (value, param) {
            if (param[2] != value) {
                var settings = {
                    data: "{\"" + param[1] + "\": \"" + value + "\"}",
                    async: false,
                    success: function (msg) {
                        if (msg.IsRegister == 0 || msg.IsUse == 0) {
                            flag = false;
                            $.data(document.body, "flag", flag);
                        }
                        else {
                            flag = true;
                            $.data(document.body, "flag", flag);
                        }
                    },
                    error: function (e) {
                        flag = false;
                        $.data(document.body, "flag", flag);
                    }
                };
                var msg = BsrCloudServer.ajax(param[0], settings);
                return $.data(document.body, "flag");
            }
            else {
                flag = true;
                $.data(document.body, "flag", flag);
            }
            return $.data(document.body, "flag");
        },
        message: '已经注册.'
    }
});
/* 密码由字母和数字组成，至少6位 */
var safePassword = function (value) {
    return !(/^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/.test(value));
}

var idCard = function (value) {
    if (value.length == 18 && 18 != value.length) return false;
    var number = value.toLowerCase();
    var d, sum = 0, v = '10x98765432', w = [7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2], a = '11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91';
    var re = number.match(/^(\d{2})\d{4}(((\d{2})(\d{2})(\d{2})(\d{3}))|((\d{4})(\d{2})(\d{2})(\d{3}[x\d])))$/);
    if (re == null || a.indexOf(re[1]) < 0) return false;
    if (re[2].length == 9) {
        number = number.substr(0, 6) + '19' + number.substr(6);
        d = ['19' + re[4], re[5], re[6]].join('-');
    } else d = [re[9], re[10], re[11]].join('-');
    if (!isDateTime.call(d, 'yyyy-MM-dd')) return false;
    for (var i = 0; i < 17; i++) sum += number.charAt(i) * w[i];
    return (re[2].length == 9 || number.charAt(17) == v.charAt(sum % 11));
}

