/*
* Date: 2014-11-12
*/
//version
var cversion = "4.0.0.0";
//addressType default 5
var AddressType = "5";
//
var ChannelCount = 1;
function IsInstall() {
    var ua = navigator.browserLanguage;
    if (ua) {
        var plugin = navigator.plugins["bluesky.BsrPlayerCloud.1"];

        plugin = navigator.mimeTypes["application/x-bsrplayercloud"];
        if (plugin) {
            // Contoso control is installed and enabled
            return plugin;
        } else {
            try {
                plugin = new ActiveXObject("bluesky.BsrPlayerCloud.1");
                return plugin;
            } catch (e) {
                //is not installed or disabled
                return;
            }
        }
    }

    if (ua == undefined) {
        var mimetype = navigator.mimeTypes["application/x-bsrplayercloud"];
        if (mimetype) {
            return mimetype;
        }
    }

    return;
}

function getplugin() {
    return document.getElementById('bsrWebUI');
}

function IsUpdate(objver, curver) {
    var objv = new String(objver);
    var objstrarray = objv.split(".");
    var curv = new String(curver);
    var curstrarray = curv.split(".");
    var maxlen = Math.max(objstrarray.length, curstrarray.length);

    var result, so, sc;
    for (var i = 0; i < maxlen; i++) {
        so = objstrarray[i];
        sc = curstrarray[i];
        if (sc > so) {
            result = 1;
        }
        else if (sc < so) {
            result = -1;
        }
        else {
            result = 0;
        }

        if (result !== 0) {
            return result;
        }
    }

    return result;
}

function Init() {
    var ret = IsInstall();
    var obj = getplugin();

    if (ret !== undefined) {
        var str = obj.version;
        ret = IsUpdate(str, cversion);
        if (ret > 0) {
            //install new version
            //window.location.href = "/update.html";
            //

        }
        else {
            //load
        }
    }
    else {
        window.location.href = "plugins/BsrPlayerCloudSetup.exe";
    }
    addTestEvent();

}



function plugin0() {
    return document.getElementById('bsrWebUI');
}
plugin = plugin0;
function addEvent(obj, name, func) {
    if (obj.attachEvent) {
        obj.attachEvent("on" + name, func);
    } else {
        obj.addEventListener(name, func, false);
    }
}

function load() {
    addEvent(plugin(), 'test', function () {
        alert("Received a test event from the plugin.")
    });
}
function pluginLoaded() {
    // 加载
    // alert("Plugin loaded!");
}

function alarmDisplay(txt, count) {
    //        var sHTML = "<p>";
    //        sHTML = sHTML + txt + count;
    //        sHTML = sHTML + "</p>";
    //        ScriptDiv.insertAdjacentHTML("afterBegin", sHTML);

    var stalrm = document.getElementById("plaslm");
    stalrm.innerHTML += "<br />";
    stalrm.innerHTML += "报警状态：" + txt;

}

function addTestEvent() {
    addEvent(plugin(), 'echo', alarmDisplay);
}

function testEvent() {
    plugin().testEvent();
}

function pluginValid() {
    if (plugin().valid) {
        alert(plugin().echo("This plugin seems to be working!"));
    } else {
        alert("Plugin is not working :(");
    }
}

function playopen() {
    //alert("open");
    var PlayIPAddressFlag = document.getElementById("PlayIPAddress").value;

    ChannelCount = document.getElementById("ChannelCount").value;

    //PlayIPAddressFlag = "NR8108HOOO5102466700513603";
    //192.168.15.3

    for (var i = 1; i <= ChannelCount; i++) {

        url = '{"DestinationIP" : "", "LoginModel" : { "Address" : "' + PlayIPAddressFlag + '",  "AddressType" : "' + AddressType + '",' +
        		    '"CmdPort" : "3721","DataPort" : "3720", "DeviceType" : "Bsr.LimitDevice", "Password" : "123456",' +
        		    '"UserName" : "admin" }, "RealStreamModel" : { "Channel" : "' + i + '", "SubStream" : 0, "TransProc" : 0 },' +
        		    '"RestServIp" : "192.168.8.85", "RestServPort" : 8006, "cond" : { "count" : 4 }, "playerType" : 2,' +
        		    '"real" : { "channel" : 1, "ip\" : 0, "subStream" : 0, "transMode" : 1, "transProc" : 1  } }'

        //alert(url);
        if (plugin().valid) {
            plugin().PlayOpen(url, i - 1);

        } else {
            alert("Plugin is not working :(");
        }
    }

}

function playclose() {

    //alert("close");
    for (var i = 1; i <= ChannelCount; i++) {
        if (plugin().valid) {
            plugin().PlayClose(i - 1);
        } else {
            alert("Plugin is not working :(");
        }
    }
}

//    function setalarm() {
//       // alert("setalarm");

//        if (plugin().valid) {
//            plugin().SetAlarm("192.168.8.239", 3000);
//        } else {
//            alert("Plugin is not working :(");
//        }
//    }
function selectChange(obj) {
    addressModel = document.getElementById("PlayIPAddress");
    if (obj.value == "0") {
        addressModel.value = "192.168.15.3";
    }
    if (obj.value == "5") {
        addressModel.value = "NR8108HOOO5102466700513603";
    }
    AddressType = obj.value;
}
var flag = 0;
function setalarm() {
    //alert("setalarm");
    var stalrm = document.getElementById("plaslm");


    flag++;
    if (flag % 2 == 1) {
        stalrm.innerHTML += "开启中..";
        document.getElementById("playsetalarm").value = "关闭报警";
    } else {
        document.getElementById("playsetalarm").value = "开启报警";
        stalrm.innerHTML += "关闭中..";
    }


    if (plugin().valid) {
        plugin().SetAlarm("192.168.8.239", 3000);

    } else {
        alert("Plugin is not working :(");
    }
}
