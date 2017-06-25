<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeviceAdd_Manual.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.DeviceAdd_Manual" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>手动添加设备</title>
</head>
<body>
    <div>
        <div class="content-nav">
            <div class="col-xs-6 text-left">
                <span>主动添加设备</span>
            </div>
            <div class="col-xs-6 text-right">
                <a href="#" class="btn btn-link" onclick="setContent($('#content'), 'DeviceAdd.aspx');">
                    返回添加初始页面</a>
            </div>
        </div>
        <div style="margin: 30px">
            <div>
                <form class="form-horizontal" role="form">
                <div class="form-group">
                    <label for="txtSN" class="col-md-1 control-label">
                        设备SN码</label>
                    <div class="col-md-4">
                        <div class="input-group col-xs-12 ">
                            <input type="text" class="form-control" maxlength="64" id="txtSN">
                            <span class="input-group-btn">
                                <button class="btn btn-default" type="button" onclick="manualAddDevice.addDevice();">
                                    <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                </button>
                            </span>
                        </div>
                        <span class="help-block">SN码位于设备机身后侧或包装箱条形码附近</span>
                    </div>
                </div>
                </form>
            </div>
            <div style="margin-top: 40px; display: none">
                <span class="help-block">成功添加设备列表</span>
                <div id="deviceList">
                </div>
            </div>
            <div class="col-xs-12" style="text-align: center; margin-top: 60px;">
                <button type="button" id="btnComplete" onclick="setContent($('#content'), 'DeviceList.aspx');"
                    class="btn btn-default" style="width: 200px;">
                    完成</button>
            </div>
        </div>
    </div>
    <div id="template" style="display: none">
        <div class="col-xs-4 col-md-4 col-lg-3">
            <div class="thumbnail">
                <img deviceimg data-src="../Images/Customer/u79.PNG" style="width: 120px; height: 80px;
                    margin-top: 20px; cursor: pointer" src="../Images/Customer/u79.PNG" />
                <div style="text-align: center; color: Gray; margin-top: 20px; font-size: 12px;">
                    <p style="font-size: 12px">
                        M2114B10015102466701377625
                    </p>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var manualAddDevice = {
            //添加设备
            addDevice: function () {
                var sn = $.trim($("#txtSN").val());
                if (!sn) {
                    jError("提示：请输入设备SN号！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    return false;
                } else {
                    $("body").mask("处理中...");
                    BsrCloudServer.Device.addDevice(sn, function (msg) {
                        $("body").unmask();
                        if (msg.Code == 0) {
                            jSuccess("设备添加成功", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            manualAddDevice.appendDevice(sn);
                        } else {
                            jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                    });
                }
            },
            //追加添加的设备
            appendDevice: function (serial) {
                BsrCloudServer.Device.searchDevice(serial, function (msg) {
                    if (msg.Code == 0 && msg.deviceList.length) {
                        var device = msg.deviceList[0];
                        switch (device.HardwareType) {//设备硬件类型
                            case 3:
                                $("#template [deviceimg]").attr("src", "../Images/Device/IPC.PNG");
                                break;
                            case 4, 5, 6:
                                $("#template [deviceimg]").attr("src", "../Images/Device/DVR.PNG");
                                break;
                        }
                        $("#template .thumbnail p").text(device.SerialNumber);
                        $("#template").children().clone().appendTo($("#deviceList"));
                        $("#deviceList").parent().show();
                    }
                });
            }
        };
    </script>
</body>
</html>
