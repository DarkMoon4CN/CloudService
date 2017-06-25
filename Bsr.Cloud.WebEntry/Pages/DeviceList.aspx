<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceList.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Pages.DeviceList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="col-xs-12 content-nav">
        <div class="col-xs-6 text-left">
            <span >已添加设备列表 </span> <small>共
                <label id="DeviceList_deviceCount">
                    0</label>
                台</small>
        </div>
        <div class="col-xs-6 text-right">
            <span><a href="#" class="btn btn-link" id="btnReSearch" onclick="DeviceList.getDeviceState();return false;">
                刷新设备状态</a></span> <span style="border-right: 0px"><a href="#" class="btn btn-link"
                    onclick="DeviceList.redirectAddDevice();">添加新设备</a></span>
        </div>
    </div>
    <div id="DeviceList_deviceListDiv" style=" padding:0px 5px 0px 5px;">
    </div>
    <!--模板-->
    <div style="display: none">
        <div id="addDeviceTemplate">
            <div class="col-sm-4 col-md-3">
                <div class="thumbnail">
                    <div style="height: 35px">
                    </div>
                    <img data-src="../Images/icons/icon_AddDevice_Normal.png" style="width: 112px; height: 84px;
                        cursor: pointer;" src="../Images/icons/icon_AddDevice_Normal.png" onclick="DeviceList.redirectAddDevice();" />
                    <div class="caption">
                        <div class="deviceName" style="text-align: center">
                            <label>
                                添加新设备
                            </label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="deviceListTemplate">
            <div class="col-sm-4 col-md-3" data-deviceid="0">
                <div class="thumbnail">
                    <div style="height: 35px">
                        <input type="checkbox" style="float: left; display: none" />
                        <span class="deviceState" style="float: right">在线</span>
                    </div>
                    <img class="deviceImg" data-src="../Images/Customer/u79.PNG" style="width: 112px;
                        height: 84px" src="../Images/Customer/u79.PNG" />
                    <div class="caption">
                        <div class="deviceName" style="text-align: center">
                            <label>
                            </label>
                            <div class="input-group input-group-sm" style="display: none; height: 15px">
                                <input type="text" class="form-control" />
                                <span class="input-group-btn">
                                    <button class="btn btn-default" type="button" onclick="DeviceList.saveEdit(this);">
                                        <span class="glyphicon glyphicon-ok"></span>
                                    </button>
                                    <button class="btn btn-default" type="button" onclick="DeviceList.editDevice(this);">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="caption">
                    <div style="text-align: right">
                        <a href="#" class="icon-btn icon-btn-edit" onclick="DeviceList.editDevice(this);return false;">
                        </a><a href="#" class="icon-btn icon-btn-del" onclick="DeviceList.delteDevice(this);return false;">
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var DeviceList = {
            //初始化设备列表
            initDevice: function () {
                $("#DeviceList_deviceListDiv").children().remove();
                BsrCloudServer.Device.getDevice(function (msg) {
                    if (msg.Code == 0) {
                        var deviceList = msg.deviceResponseList;
                        if (deviceList && deviceList.length) {
                            for (var i = 0; i < deviceList.length; i++) {
                                if (deviceList[i].IsAuthorize == 1) {
                                    continue;
                                }
                                switch (deviceList[i].HardwareType) {//设备硬件类型
                                    case 3:
                                        $("#deviceListTemplate .deviceImg").attr("src", "../Images/Device/IPC.PNG");
                                        break;
                                    case 4, 5, 6:
                                        $("#deviceListTemplate .deviceImg").attr("src", "../Images/Device/DVR.PNG");
                                        break;
                                }

                                $("#deviceListTemplate .deviceName label").text(deviceList[i].DeviceName); //设备名称
                                $("#deviceListTemplate .deviceName input[type='text']").val(deviceList[i].DeviceName);
                                if (deviceList[i].BPServerDeviceState) {
                                    $("#deviceListTemplate .deviceState").text("在线").attr("class", "deviceState label label-success"); //设备状态
                                } else {
                                    $("#deviceListTemplate .deviceState").text("离线").attr("class", "deviceState label label-default"); //设备状态
                                }
                                $("#deviceListTemplate [data-deviceid]").attr("data-deviceid", deviceList[i].DeviceId); //设备ID
                                $("#deviceListTemplate [data-deviceid]").attr("id", "device" + deviceList[i].DeviceId); //给设备div赋id
                                $("#deviceListTemplate").children().clone().appendTo($("#DeviceList_deviceListDiv"));
                                $("#deviceListTemplate [data-deviceid]").removeAttr("id"); //移除设备模板div的id
                                $("#DeviceList_deviceCount").text(i + 1);
                            }
                            $("#addDeviceTemplate").children().clone().appendTo($("#DeviceList_deviceListDiv"));
                            //获取设备状态
                            DeviceList.getDeviceState();
                        } else {
                            //批量删除按钮禁用,并跳转无设备页
                            //$("#btnMultiDelDevice").addClass("disabled");
                            setContent($("#content"), "DeviceAdd.aspx");
                        }
                    }
                });
            },
            //删除设备
            delteDevice: function (obj) {
                if (confirm("确定删除吗?")) {
                    var deviceId = $(obj).closest("[data-deviceid]").attr("data-deviceid");
                    $(obj).closest("[data-deviceid]").mask("处理中...");
                    BsrCloudServer.Device.delteDevice(deviceId, function (msg) {
                        if (msg.Code == 0) {
                            DeviceList.initDevice();
                            jSuccess("设备删除成功！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                        else {
                            jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                        $(obj).closest("[data-deviceid]").unmask();
                    });
                }
            },
            //编辑设备
            editDevice: function (obj) {
                var deviceDiv = $(obj).closest("[data-deviceid]").attr("id");
                var deviceId = $(obj).closest("[data-deviceid]").attr("data-deviceid");
                var isEdit = $("#" + deviceDiv + " .deviceName label").is(":visible");
                if (isEdit) {
                    $("#" + deviceDiv + " .deviceName label").hide();
                    $("#" + deviceDiv + " .deviceName div").show();
                } else {
                    $("#" + deviceDiv + " .deviceName label").show();
                    $("#" + deviceDiv + " .deviceName div").hide();
                }
            },
            //保存设备名称
            saveEdit: function (obj) {
                var deviceId = $(obj).closest("[data-deviceid]").attr("data-deviceid");
                var deviceDiv = $(obj).closest("[data-deviceid]").attr("id");
                var newDeviceName = $("#" + deviceDiv + " .deviceName input[type='text']").val();
                if (!newDeviceName) { return false; }
                $("#" + deviceDiv).mask("处理中...");
                BsrCloudServer.Device.updateDeviceName(deviceId, newDeviceName, function (msg) {
                    if (msg.Code == 0) {
                        $("#" + deviceDiv + " .deviceName label").text(newDeviceName).show();
                        $("#" + deviceDiv + " .deviceName div").hide();
                        $("#" + deviceDiv).unmask();
                        jSuccess("数据保存成功！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    } else {
                        $("#" + deviceDiv).unmask();
                        jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                });
            },
            //获取设备状态
            getDeviceState: function () {
                jNotify("请稍后...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                var deviceIdList = [];
                $("#DeviceList_deviceListDiv [data-deviceid]").each(function () {
                    deviceIdList.push($(this).attr("data-deviceid"));
                });
                BsrCloudServer.Device.getServerDeviceState(deviceIdList, function (msg) {
                    if (msg.Code == 0) {
                        var deviceState = msg.deviceState;
                        for (var i = 0; i < deviceState.length; i++) {
                            if (deviceState[i].State == 1) {
                                $("#device" + deviceState[i].DeviceId + " .deviceState").text("在线").attr("class", "deviceState label label-success");
                            } else {
                                $("#device" + deviceState[i].DeviceId + " .deviceState").text("离线").attr("class", "deviceState label label-default");
                            }
                        }
                        if ($.jNotify) {
                            $.jNotify._close();
                        }
                    } else {
                        if ($.jNotify) {
                            $.jNotify._close();
                        }
                        jError("获取设备在线状态失败，" + msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                });
            },
            //跳转添加设备页面
            redirectAddDevice: function () {
                setContent($("#content"), "DeviceAdd.aspx");
            }
        };
        $(function () {
            DeviceList.initDevice();
        });
    </script>
</body>
</html>
