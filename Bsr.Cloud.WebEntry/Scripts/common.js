var globalJS = {
    //获取浏览器版本
    getBrowser: function () {
        var Sys = {};
        var ua = navigator.userAgent.toLowerCase();
        var s;
        (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
        (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
        (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

        //以下进行测试
        if (Sys.ie) return ('IE ' + Sys.ie);
        if (Sys.firefox) return ('Firefox ' + Sys.firefox);
        if (Sys.chrome) return ('Chrome ' + Sys.chrome);
        if (Sys.opera) return ('Opera ' + Sys.opera);
        if (Sys.safari) return ('Safari ' + Sys.safari);
        return "other";
    },
    //获取url参数
    getUrlParam: function (name, url) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (url) {
            r = url.search.substr(1).match(reg);
        }
        if (r != null)
            return unescape(r[2]);
        return null;
    },
    getHost: function (url) {
        //获取域名
        //host2 = document.domain;
        if (url) {
            return url.host;
        } else
            return window.location.host;
    },
    //解决浏览器缓存
    timestamp: function (url) {
        //var getTimestamp=Math.random();
        var getTimestamp = new Date().getTime();
        if (url.indexOf("?") > -1) {
            url = url + "&timestamp=" + getTimestamp
        } else {
            url = url + "?timestamp=" + getTimestamp
        }
        return url;
    },
    /*Javascript中暂停功能的实现*/
    Pause: function (obj, iMinSecond) {
        if (window.eventList == null)
            window.eventList = new Array();
        var ind = -1;
        for (var i = 0; i < window.eventList.length; i++) {
            if (window.eventList[i] == null) {
                window.eventList[i] = obj;
                ind = i;
                break;
            }
        }
        if (ind == -1) {
            ind = window.eventList.length;
            window.eventList[ind] = obj;
        }
        setTimeout("globalJS.GoOn(" + ind + ")", iMinSecond);
    },
    GoOn: function (ind) {
        var obj = window.eventList[ind];
        window.eventList[ind] = null;
        if (obj.NextStep)
            obj.NextStep();
        else
            obj();
    },
    GetStringRealLength: function (str) {
        ///<summary>获得字符串实际长度，中文2，英文1</summary>
        ///<param name="str">要获得长度的字符串</param>
        var realLength = 0, len = str.length, charCode = -1;
        for (var i = 0; i < len; i++) {
            charCode = str.charCodeAt(i);
            if (charCode >= 0 && charCode <= 128) realLength += 1;
            else realLength += 2;
        }
        return realLength;
    },
    //屏蔽页面backspace事件
    rmBackspaceE: function (e) {
        var ev = e || window.event; //获取event对象   
        var obj = ev.target || ev.srcElement; //获取事件源   
        var t = obj.type || obj.getAttribute('type'); //获取事件源类型  
        //获取作为判断条件的事件类型
        var vReadOnly = obj.getAttribute('readonly');
        //处理null值情况
        vReadOnly = (vReadOnly == "") ? false : vReadOnly;
        //当敲Backspace键时，事件源类型为密码或单行、多行文本的，
        //并且readonly属性为true或enabled属性为false的，则退格键失效
        var flag1 = (ev.keyCode == 8 && (t == "password" || t == "text" || t == "textarea")
                    && vReadOnly == "readonly") ? true : false;
        //当敲Backspace键时，事件源类型非密码或单行、多行文本的，则退格键失效
        var flag2 = (ev.keyCode == 8 && t != "password" && t != "text" && t != "textarea")
                    ? true : false;
        //判断
        if (flag2) {
            return false;
        }
        if (flag1) {
            return false;
        }
    }
}

// 对Date的扩展，将 Date 转化为指定格式的String
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
// 例子： 
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423 
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
Date.prototype.Format = function (fmt) { //author: meizz 
    var o = {
        "M+": this.getMonth() + 1, //月份 
        "d+": this.getDate(), //日 
        "h+": this.getHours(), //小时 
        "m+": this.getMinutes(), //分 
        "s+": this.getSeconds(), //秒 
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
        "S": this.getMilliseconds() //毫秒 
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

var request = {
    myspace: "MySpace.aspx",
    divice: {
        list: "DeviceList.aspx",
        add: {
            select: "DeviceAdd.aspx",
            auto: "DeviceAdd_Auto.aspx",
            manual: "DeviceAdd_Manual.aspx"
        }
    },
    channel: "ChannelManage.aspx",
    userManager: "PrimaryAccountManagement.aspx"
};
