var DPServer = {
    //version
    bsrPlayerPlugin: null,
    cversion: "1.0.0.0",
    //检查是否安装插件
    checkPlugin: function () {
        var plugin;
        try {
            if (window.ActiveXObject) {//IE
                plugin = new ActiveXObject("bluesky.BsrPlayerCloud.1");
            } else if (navigator.plugins) {
                plugin = navigator.plugins["BsrPlayerCloudVer"];
            }
        } catch (e) {
            //alert("出错了...\n描述：" + e.description + " \n错误号：" + e.number);
        }
        if (plugin) {
            DPServer.creatBsrPlayerPlugin();
        }
        else {
            if (confirm("需要星际云视频播放插件,是否下载?")) {
                window.location.href = "plugins/BsrPlayerCloudSetup.exe";
            }
        }
    },
    //检测新版本
    isUpdate: function (objver, curver) {
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
    },
    //给插件注册事件
    addEvent: function (obj, name, func) {
        if (obj.attachEvent) {
            obj.attachEvent("on" + name, func);
        } else {
            obj.addEventListener(name, func, false);
        }
    },
    //生成插件DOM
    creatBsrPlayerPlugin: function () {
        if (window.ActiveXObject) {//IE
            DPServer.bsrPlayerPlugin = new ActiveXObject("bluesky.BsrPlayerCloud.1");
            pluginLoaded();
        } else {
            var bsrPlayerPlugin = $("<object>");
            bsrPlayerPlugin.css({ "position": "absolute", "left": "-9999px", "top": "0" }).attr({ "width": "1", "height": "1", "id": "BsrPlayerOCX", "type": "application/x-bsrplayercloud" });
            bsrPlayerPlugin.append("<param name=\"onload\" value=\"pluginLoaded\"/>");
            $("body:first").prepend(bsrPlayerPlugin);
        }
    },
    //局域网搜索回调事件
    deviceSearchCallback: function (device) {
        autoAddDevice.appendDevice(device);
    }
};
(function (DPServer) {
    /*-------窗口相关-------*/
    DPServer.Window = {
        //窗口参数
        wndConfig: [{ "param": "", "sound": false}],
        //保存或者设置窗口参数
        setWndConfig: function (index, param) {
            if (!DPServer.Window.wndConfig[index]) {
                DPServer.Window.wndConfig[index] = param;
            } else {
                if (param == null) {
                    DPServer.Window.wndConfig[index] = null;
                } else {
                    $.extend(true, DPServer.Window.wndConfig[index], param);
                }
            }
            DPServer.Window.onWndSelected('{"currentwnd":' + index + '}');
        },
        //获取窗口配置参数
        getWndConfig: function (index) {
            return DPServer.Window.wndConfig[index];
        },
        //设置多窗格显示模式，支持：1， 4， 6， 9
        setWndMode: function (mode) {
            DPServer.bsrPlayerPlugin.SetWndMode(mode);
        },
        //获取当前多窗格显示模式
        getWndMode: function () {
            var wndMode = DPServer.bsrPlayerPlugin.GetWndMode();
            return wndMode;
        },
        //单击分割窗口回调事件
        onWndSelected: function (options) {
            var curWndIndex = JSON.parse(options).currentwnd;
            var curWndConfig = DPServer.Window.wndConfig[curWndIndex];
            if (curWndConfig) {
                //清晰度
                var subStream = curWndConfig.param.RealStreamModel.SubStream;
                if (subStream == 0) {
                    $("#btnSubstream>a>span").text("清晰度");
                } else if(subStream == 1){
                    $("#btnSubstream>a>span").text("高清");
                }else if(subStream == 2){
                    $("#btnSubstream>a>span").text("标清");
                }else if(subStream == 3){
                    $("#btnSubstream>a>span").text("流畅");
                }
                //伴音
                var sound = curWndConfig.sound;
                if (sound) {
                    $("#btnPlaySound img").attr({"src":"../Images/player/icon_PlaySound_Normal.png","data-src":"../Images/player/icon_PlaySound_Normal.png","data-active-src":"../Images/player/icon_PlaySound_Hover.png"});
                } else {
                    $("#btnPlaySound img").attr({"src":"../Images/player/icon_SoundClosed_Normal.png","data-src":"../Images/player/icon_SoundClosed_Normal.png","data-active-src":"../Images/player/icon_SoundClosed_Hover.png"});
                }
            }
        },
        //全屏
        fullScreen: function () {
            DPServer.bsrPlayerPlugin.SetFullScreen(5);
        },
        //获取一个空闲窗口
        getFirstFreeWndIndex: function () {
            var index = DPServer.bsrPlayerPlugin.GetFirstFreeWndIndex();
            return index;
        },
        //获取当前选中窗口号
        getCurSelectWndIndex: function () {
            var index = DPServer.bsrPlayerPlugin.GetCurSelectWndIndex();
            return index;
        }
    },
    /*-------现场预览播放相关--------*/
    DPServer.Play = {
        //播放 type:0:获取空闲窗口播放,1:获取选中窗口播放
        playOpen: function (param, type) {
            if (DPServer.bsrPlayerPlugin && DPServer.bsrPlayerPlugin.valid) {
                var wIndex;
                if (type == 0) {
                    wIndex = DPServer.Window.getFirstFreeWndIndex();
                } else {
                    wIndex = DPServer.Window.getCurSelectWndIndex();
                    DPServer.bsrPlayerPlugin.PlayClose(wIndex);
                }
                DPServer.bsrPlayerPlugin.PlayOpen(JSON.stringify(param), wIndex);
                var config = { "param": param, "sound": false };
                DPServer.Window.setWndConfig(wIndex, config);
            } else {
                alert("Plugin is not working :(");
            }
        },
        //关闭 type：0：关闭所有 1：关闭当前选中窗口
        playClose: function (type) {
            if (type == 0) {
                var i = 0;
                var mytime = setInterval(function () {
                    DPServer.bsrPlayerPlugin.PlayClose(i);
                    DPServer.Window.setWndConfig(i, null);
                    i++;
                    if (i > 8) {
                        clearInterval(mytime);
                    }
                }, 500);
            } else {
                var index = DPServer.Window.getCurSelectWndIndex();
                DPServer.bsrPlayerPlugin.PlayClose(index);
                DPServer.Window.setWndConfig(index, null);
            }
        },
        //截图
        playCapturePic2: function () {
            var index = DPServer.Window.getCurSelectWndIndex();
            var val = DPServer.bsrPlayerPlugin.PlayCapturePic2(index);
            return val;
        },
        //伴音
        playSound: function () {
            var index = DPServer.Window.getCurSelectWndIndex();
            var curWndConfig = DPServer.Window.wndConfig[index];
            if (curWndConfig) {
                if (curWndConfig.sound) {
                    DPServer.bsrPlayerPlugin.PlaySoundClose(index);
                } else {
                    DPServer.bsrPlayerPlugin.PlaySoundOpen(index);
                }
                DPServer.Window.setWndConfig(index, { "sound": !curWndConfig.sound });
            }
        }
    }
})(DPServer)

$(function () {
    DPServer.checkPlugin();
});

//插件加载完毕后
function pluginLoaded() {
    if (!DPServer.bsrPlayerPlugin) {
        DPServer.bsrPlayerPlugin = document.getElementById("BsrPlayerOCX");
    }
    if (DPServer.bsrPlayerPlugin) {
        //检查最新版本
        var cversion = DPServer.cversion;
        if (cversion) {
            ret = DPServer.isUpdate(DPServer.bsrPlayerPlugin.version, cversion);
            if (ret > 0) {
                if (confirm("星际云视频播放插件有最新版本，是否下载更新？")) {
                    window.location.href = "plugins/BsrPlayerCloudSetup.exe";
                }
            }
        }
        //注册局域网搜索回调事件
        DPServer.addEvent(DPServer.bsrPlayerPlugin, 'deviceSearchCallback', DPServer.deviceSearchCallback);
        //注册选择分割窗口事件
        DPServer.addEvent(DPServer.bsrPlayerPlugin, 'optionsCallback', DPServer.Window.onWndSelected);
    }
    
}
