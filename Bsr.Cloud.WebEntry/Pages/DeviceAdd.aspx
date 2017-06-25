<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceAdd.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Pages.DeviceAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>添加设备总页面</title>
</head>
<body>
    <div id="deviceAdd">
        <div id="deviceAdd-top">
            <a href="#" onclick="deviceAdd.redirectDeviceList();return false;">返回设备列表</a>
        </div>
        <div id="deviceAdd-tip">
        </div>
        <div id="deviceAdd-type" class="col-xs-12">
            <div class="col-xs-5 col-xs-offset-1">
                <img src="../Images/icons/icon_DeviceSearch.png" onclick="deviceAdd.redirectAdd_atuo();" />
                <div>
                    局域网扫描添加</div>
            </div>
            <div class="col-xs-5">
                <img src="../Images/icons/icon_DeviceSN.png" onclick="deviceAdd.redirectAdd_manual();" />
                <div>
                    序列号添加</div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var deviceAdd = {
            //初始化页面
            initPage: function () {
                BsrCloudServer.Device.getDevice(function (msg) {
                    if (msg.Code == 0) {
                        var deviceList = msg.deviceResponseList;
                        if (deviceList && deviceList.length) {
                            $("#deviceAdd-top").children().show();
                            $("#deviceAdd-tip").text("您可以通过下面以下方式进行添加");
                        } else {
                            $("#deviceAdd-top").children().hide();
                            $("#deviceAdd-tip").text("您还没有添加任何一台设备，可以通过下面任意一种方式进行添加");
                        }
                    } else {
                        $("#deviceAdd-top").children().hide();
                        $("#deviceAdd-tip").text("您还没有添加任何一台设备，可以通过下面任意一种方式进行添加");
                    }
                });
            },
            redirectDeviceList: function () {
                setContent($("#content"), "DeviceList.aspx");
            },
            redirectAdd_atuo: function () {
                setContent($("#content"), "DeviceAdd_Auto.aspx");
            },
            redirectAdd_manual: function () {
                setContent($("#content"), "DeviceAdd_Manual.aspx");
            }
        };
        $(function () {
            deviceAdd.initPage();
        });
    </script>
</body>
</html>
