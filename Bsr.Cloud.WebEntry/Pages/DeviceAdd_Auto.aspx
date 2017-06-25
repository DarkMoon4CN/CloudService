<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceAdd_Auto.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.DeviceAdd_Auto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>局域网扫描设备</title>
    <style type="text/css">
        .help-block
        {
            font-size: 14px;
            font-style: normal;
        }
    </style>
</head>
<body>
    <div>
        <div class="content-nav">
            <div class="col-xs-6 text-left">
                <span style="white-space: nowrap">局域网内搜索到以下设备 <small>共
                    <label id="deviceAdd_auto_deviceCount">
                        0</label>
                    台</small> </span>
            </div>
            <div class="col-xs-6 text-right">
                <span><a href="#" class="btn btn-link" onclick="autoAddDevice.autoSearchDevice();"><span
                    style="border-right: 0px" class="glyphicon glyphicon-refresh" aria-hidden="true">
                </span>刷新</a></span><span style="border-right: 0px"><a href="#" class="btn btn-link"
                    onclick="setContent($('#content'), 'DeviceAdd.aspx');"> 返回添加初始页面</a></span>
            </div>
        </div>
        <div id="deviceAdd_auto_deviceListDiv">
        </div>
        <div class="col-xs-12" style="text-align: center; margin: 10px;">
            <button type="button" id="btnAdd" onclick="autoAddDevice.addDevice();" class="btn btn-default"
                style="width: 200px;">
                添加</button>
        </div>
    </div>
    <div style="display: none;">
        <div class="template">
            <div class="col-xs-4 col-md-4 col-lg-3">
                <div class="thumbnail">
                    <div data-selected="false" style="height: 20px;">
                        <img style="display: none" src="../Images/icons/icon_Selected.png" />
                    </div>
                    <img deviceimg data-src="../Images/Customer/u79.PNG" style="width: 120px; height: 80px;
                        cursor: pointer" src="../Images/Customer/u79.PNG" />
                    <div style="text-align: center; color: Gray; margin-top: 20px; font-size: 12px;">
                        <p style="font-size: 12px">
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div empty>
            <div class="col-xs-4 col-md-4 col-lg-3">
                <div class="thumbnail">
                    <img style="width: 120px; margin-top: 20px; height: 80px; cursor: pointer" src="../Images/icons/icon_DeviceEmpty.png" />
                    <div style="text-align: center; color: Gray; margin-top: 20px; font-size: 12px;">
                        <p style="font-size: 12px">
                            哎呀，什么也没有
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-md-12 col-lg-12" style="padding-top: 30px; padding-left: 30px">
                <span class="help-block">1.没有搜索到任何设备，请检查设备是否已经正常连接到局域网内？</span> <span class="help-block">
                    2.已经连接至局域内，您可以尝试<a href="#" onclick="autoAddDevice.autoSearchDevice();">刷新</a>试试</span>
                <span class="help-block">3.还是无法正常搜索，那么可以尝试<a href="#" onclick="setContent($('#content'),'DeviceAdd_Manual.aspx');">手动添加</a></span>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var autoAddDevice = {
            //设备数量
            deviceCount: 0,
            //初始化页面
            initPage: function () {
                //设备图标点击事件
                $("#deviceAdd_auto_deviceListDiv").on("click", ".thumbnail [deviceImg]", function () {
                    var devSerial = $(this).closest(".thumbnail").attr("data-devSerial");
                    var selectedMark = $(this).closest(".thumbnail").find("[data-selected]");
                    var isSelected = selectedMark.attr("data-selected");
                    if (isSelected == "true") {
                        selectedMark.attr("data-selected", false);
                        selectedMark.find("img").hide();
                        $(this).closest(".thumbnail").css("border-color", "#ddd");
                    } else {
                        selectedMark.attr("data-selected", true);
                        selectedMark.find("img").show();
                        $(this).closest(".thumbnail").css("border-color", "#30C5FC");
                    }
                });
                autoAddDevice.autoSearchDevice();
            },
            //自动搜索
            autoSearchDevice: function () {
                autoAddDevice.deviceCount = 0;
                $("#deviceAdd_auto_deviceCount").text(autoAddDevice.deviceCount);
                $("#deviceAdd_auto_deviceListDiv").children().remove();
                $("#btnAdd").hide();
                var plugin = DPServer.bsrPlayerPlugin;
                if (plugin && plugin.valid) {
                    $("#deviceAdd_auto_deviceListDiv").css({ "background": "url(../Images/loading.gif) no-repeat center", "min-height": "200px", "width": "100%" });
                    this.timeo = setTimeout(function () {
                        if (autoAddDevice.deviceCount == 0) {
                            $("#deviceAdd_auto_deviceListDiv").css({ "background": "none" }).children().remove();
                            $("#deviceAdd_auto_deviceListDiv").append($("[empty]").clone());
                        }
                    }, 10000);
                    plugin.SearchDevice(5);
                } else {
                    alert("Plugin is not working :(");
                }
            },
            //搜索后往页面添加
            appendDevice: function (device) {
                clearTimeout(this.timeo);
                $("#deviceAdd_auto_deviceListDiv").css({ "background": "none" });
                $("#btnAdd").show();
                if (JSON.parse(device).devtype == 25 || JSON.parse(device).devtype == 27) {
                    BsrCloudServer.Device.isExistSN(JSON.parse(device).devSerial, function (msg) {
                        if (msg.Code == 0 && !msg.IsExist) {
                            var deviceJson = JSON.parse(device);
                            var devtype = deviceJson.devtype;
                            switch (devtype) {
                                case 25: //IPC
                                    $(".template .thumbnail [deviceimg]").attr({ "src": "../Images/Device/IPC.PNG", "data-src": "../Images/Device/IPC.PNG" });
                                    break;
                                case 27: //NVR
                                    $(".template .thumbnail [deviceimg]").attr({ "src": "../Images/Device/DVR.PNG", "data-src": "../Images/Device/DVR.PNG" });
                                    break;
                            }
                            $("#deviceAdd_auto_deviceCount").text(autoAddDevice.deviceCount);
                            $(".template .thumbnail p").text(deviceJson.devSerial);
                            $(".template .thumbnail").attr("data-devSerial", deviceJson.devSerial);
                            $(".template").children().clone().appendTo("#deviceAdd_auto_deviceListDiv");
                            autoAddDevice.deviceCount += 1;
                        }
                    });
                }
            },
            //往数据库中添加设备数据
            addDevice: function () {
                var devSerialList = [];
                var errorCount = 0;
                var successCount = 0;
                $("#deviceAdd_auto_deviceListDiv [data-selected='true']").each(function () {
                    devSerialList.push($(this).closest(".thumbnail").attr("data-devSerial"));
                });
                if (devSerialList.length) {
                    $("body").mask("处理中...");
                    for (var i = 0; i < devSerialList.length; i++) {
                        BsrCloudServer.Device.addDevice(devSerialList[i], function (msg) {
                            if (msg.Code == 0) {
                                successCount += 1;
                            } else {
                                errorCount += 1;
                            }
                            if ((successCount + errorCount) == devSerialList.length) {
                                $("body").unmask();
                                if (errorCount > 0) {
                                    jNotify("成功添加" + successCount + "个设备，失败" + errorCount + "个！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: true });
                                    for (var i = 0; i < devSerialList.length; i++) {
                                        BsrCloudServer.Device.isExistSN(devSerialList[i], function (msg) {
                                            if (msg.Code == 0) {

                                            }
                                        });
                                    }
                                } else if (successCount == devSerialList.length) {
                                    setContent($("#content"), "DeviceList.aspx");
                                }
                            }
                        });
                    }
                } else {
                    jError("请选择要添加的设备！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                }
            }
        };
        $(function () {
            autoAddDevice.initPage();
        });
    </script>
</body>
</html>
