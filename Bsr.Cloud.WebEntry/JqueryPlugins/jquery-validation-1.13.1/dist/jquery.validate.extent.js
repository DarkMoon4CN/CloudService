//特殊符号
jQuery.validator.addMethod("symbol", function (value, element) {
    return this.optional(element) || /[^\.@]+$/.test(value);
}, "不允许包含.@");
// 字符验证
jQuery.validator.addMethod("string", function (value, element) {
    return this.optional(element) || /[^\u4E00-\u9FA5]+$/.test(value);
}, "不允许包含中文");
//字母数字
jQuery.validator.addMethod("alnum", function (value, element) {
    return this.optional(element) || /^(?!\d*$)/.test(value);
}, "用户名不能为纯数字");
//密码
jQuery.validator.addMethod("password", function (value, element) {
    return safePassword(value);
}, "密码至少包含英文和数字，至少6位");
// 手机号码验证       
jQuery.validator.addMethod("mobilephone", function (value, element) {
    var length = value.length;
    var mobile = /^((\+?86)|(\(\+86\)))?1\d{10}$/;
    return this.optional(element) || mobile.test(value);
}, "请正确填写您的手机号码");
// 验证码
jQuery.validator.addMethod("verfykey", function (value, element) {
    return this.optional(element) || /^\d{6}$/.test(value);
}, "验证码为6位数字");
// 身份证号码验证       
jQuery.validator.addMethod("isIdCardNo", function (value, element) {
    return this.optional(element) || isIdCardNo(value);
}, "请正确输入您的身份证号码");
// 电话验证
jQuery.validator.addMethod("telephone", function (value, element) {
    var tel = /^\d{3,4}-?\d{7,9}$/;
    return this.optional(element) || (tel.test(value));
}, "请正确填写您的固定电话(如010-88888888)");

jQuery.validator.addMethod("check", function (value, element) {
    return $("#email").val() != "" || $("#mobile").val() != "";
}, "手机和邮箱必须一个有值");

jQuery.validator.addMethod("isExistedUser", function (value, element) {
    var result = false;
    BsrCloudServer.Customer.isRegister(value, function (msg) {
        if (msg && msg.Code == 0) {
            result = (msg.IsRegister == 1);
        }
    });
    $.ajaxSetup({ async: true }); // 恢复异步
    return result;
}, "帐号不存在");

jQuery.validator.addMethod("isExistedLogname", function (value, element) {
    var result = false;
    BsrCloudServer.Customer.isRegister(value, function (msg) {
        if (msg && msg.Code == 0) {
            result = (msg.IsRegister == 0);
        }
    });
    $.ajaxSetup({ async: true }); // 恢复异步
    return result;
}, "已经注册");

//param为0时代表手机验证，1时为邮箱验证
jQuery.validator.addMethod("isExistedLognameByEdit", function (value, element, param) {
    var result = false;
    var rlt = BsrCloudServer.Customer.getCustomerInfo();
    if (rlt && rlt.Code == 0) {
        if (param[1].toString() == "0") {//手机验证
            if (value == rlt.customerReponse.ReceiverCellPhone) {
                result = true;
            }
            else {
                BsrCloudServer.Customer.isRegister(value, function (msg) {
                    if (msg && msg.Code == 0) {
                        result = (msg.IsRegister == 0);
                    }
                });
                $.ajaxSetup({ async: true }); // 恢复异步
            }
        } else if (param[1].toString() == "1") {//邮箱
            if (value == rlt.customerReponse.ReceiverEmail) {
                result = true;
            }
            else {
                BsrCloudServer.Customer.isRegister(value, function (msg) {
                    if (msg && msg.Code == 0) {
                        result = (msg.IsRegister == 0);
                    }
                });
                $.ajaxSetup({ async: true }); // 恢复异步
            }
        }
    }
    return result;
}, "已经注册");

//param为0时代表手机验证，1时为邮箱验证,
jQuery.validator.addMethod("isExistedMainLognameByEdit", function (value, element, param) {
    var result = false;
    var rlt = BsrCloudServer.Customer.GetSinglePrimaryCustomer(param[2]);
    if (rlt && rlt.Code == 0) {
        if (param[1].toString() == "0") {//手机验证
            if (value == rlt.customerReponse.ReceiverCellPhone) {
                result = true;
            }
            else {
                BsrCloudServer.Customer.isRegister(value, function (msg) {
                    if (msg && msg.Code == 0) {
                        result = (msg.IsRegister == 0);
                    }
                });
                $.ajaxSetup({ async: true }); // 恢复异步
            }
        } else if (param[1].toString() == "1") {//邮箱
            if (value == rlt.customerReponse.ReceiverEmail) {
                result = true;
            }
            else {
                BsrCloudServer.Customer.isRegister(value, function (msg) {
                    if (msg && msg.Code == 0) {
                        result = (msg.IsRegister == 0);
                    }
                });
                $.ajaxSetup({ async: true }); // 恢复异步
            }
        }
    }
    return result;
}, "已经注册");

//param为0时代表手机验证，1时为邮箱验证,
jQuery.validator.addMethod("isExistedSubLognameByEdit", function (value, element, param) {
    var result = false;
    var rlt = BsrCloudServer.Customer.GetSingleSubCustomer(param[2]);
    if (rlt && rlt.Code == 0) {
        if (param[1].toString() == "0") {//手机验证
            if (value == rlt.customerReponse.ReceiverCellPhone) {
                result = true;
            }
            else {
                BsrCloudServer.Customer.isRegister(value, function (msg) {
                    if (msg && msg.Code == 0) {
                        result = (msg.IsRegister == 0);
                    }
                });
                $.ajaxSetup({ async: true }); // 恢复异步
            }
        } else if (param[1].toString() == "1") {//邮箱
            if (value == rlt.customerReponse.ReceiverEmail) {
                result = true;
            }
            else {
                BsrCloudServer.Customer.isRegister(value, function (msg) {
                    if (msg && msg.Code == 0) {
                        result = (msg.IsRegister == 0);
                    }
                });
                $.ajaxSetup({ async: true }); // 恢复异步
            }
        }
    }
    return result;
}, "已经注册");

//增加身份证验证
function isIdCardNo(num) {
    var factorArr = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1);
    var parityBit = new Array("1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2");
    var varArray = new Array();
    var intValue;
    var lngProduct = 0;
    var intCheckDigit;
    var intStrLen = num.length;
    var idNumber = num;
    // initialize
    if ((intStrLen != 15) && (intStrLen != 18)) {
        return false;
    }
    // check and set value
    for (i = 0; i < intStrLen; i++) {
        varArray[i] = idNumber.charAt(i);
        if ((varArray[i] < '0' || varArray[i] > '9') && (i != 17)) {
            return false;
        } else if (i < 17) {
            varArray[i] = varArray[i] * factorArr[i];
        }
    }
    if (intStrLen == 18) {
        //check date
        var date8 = idNumber.substring(6, 14);
        if (isDate8(date8) == false) {
            return false;
        }
        // calculate the sum of the products
        for (i = 0; i < 17; i++) {
            lngProduct = lngProduct + varArray[i];
        }
        // calculate the check digit
        intCheckDigit = parityBit[lngProduct % 11];
        // check last digit
        if (varArray[17] != intCheckDigit) {
            return false;
        }
    }
    else {        //length is 15
        //check date
        var date6 = idNumber.substring(6, 12);
        if (isDate6(date6) == false) {
            return false;
        }
    }
    return true;
}

/* 密码由字母和数字组成，至少6位 */
var safePassword = function (value) {
    //return !(/^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\"]*)|.{0,5})$|\s/.test(value));
    return (/^(?=.*[0-9].*)(?=.*[a-zA-Z].*)[\S]{6,32}$/.test(value));
    //return !(/^[a-zA-Z][a-zA-Z0-9]{5,12}$|\s/.test(value));
    //return (/^[a-zA-Z][a-zA-Z0-9]{5,12}$|\s/.test(value));
}

function isDate6(sDate) {
    if (!/^[0-9]{6}$/.test(sDate)) {
        return false;
    }
    var year, month, day;
    year = sDate.substring(0, 4);
    month = sDate.substring(4, 6);
    if (year < 1700 || year > 2500) return false
    if (month < 1 || month > 12) return false
    return true
}
/**
* 判断是否为“YYYYMMDD”式的时期
*
*/
function isDate8(sDate) {
    if (!/^[0-9]{8}$/.test(sDate)) {
        return false;
    }
    var year, month, day;
    year = sDate.substring(0, 4);
    month = sDate.substring(4, 6);
    day = sDate.substring(6, 8);
    var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    if (year < 1700 || year > 2500) return false
    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
    if (month < 1 || month > 12) return false
    if (day < 1 || day > iaMonthDays[month - 1]) return false
    return true
}