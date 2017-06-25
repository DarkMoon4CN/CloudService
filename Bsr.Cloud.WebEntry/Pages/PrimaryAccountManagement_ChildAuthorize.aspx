<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrimaryAccountManagement_ChildAuthorize.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.PrimaryAccountManagement_ChildAuthorize" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta content="text/html; charset=UTF-8" http-equiv="Content-Type" />
    <link href="../JqueryPlugins/zTree_v3.5/css/zTreeStyle/metro.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .ztree
        {
            height: 400px;
            font-size: 10pt;
            font-family: "Microsoft Yahei" ,Verdana,Simsun, "Segoe UI Web Light" , "Segoe UI Light" , "Segoe UI Web Regular" , "Segoe UI" , "Segoe UI Symbol" , "Helvetica Neue" ,Arial;
        }
        .ztree li
        {
            white-space: normal;
        }
        .ztree li ul
        {
            margin: 0;
            padding: 0;
        }
        .ztree li
        {
            line-height: 25px;
        }
        .ztree li a
        {
            width: 70%;
        }
        .ztree li a.level0
        {
            background-color: #D4D4D4;
            border-bottom: 1px solid white;
        }
    </style>
</head>
<body>
    <!-- 子账户权限 -->
    <div>
        <div class='col-md-1 col-md-offset-10' style='padding-top: 8px;'>
            <button type='button' class='btn btn-sm btn-primary' data-dismiss='modal' onclick='btnSaveAuthorizeSub();return false;'>
                保存</button>
        </div>
        <div class="col-md-12">
            <label class="checkbox-inline">
                <input type="checkbox" id="showMySpace" name="inlineChkCustomerToSee" value="option1">
                显示我的空间
            </label>
            <label class="checkbox-inline">
                <input type="checkbox" id="showEventMessage" name="inlineChkCustomerToSee" value="option2">
                显示消息事件
            </label>
            <label class="checkbox-inline">
                <input type="checkbox" id="showCloudVideo" name="inlineChkCustomerToSee" value="option3">
                显示云视频
            </label>
        </div>
        <div class="col-md-12" style="margin-top: 12px;">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#videoChannel" role="tab" data-toggle="tab">
                    视频通道</a></li>
                <li role="presentation"><a href="#videoChannelOther" role="tab" data-toggle="tab">报警输入</a></li>
                <li role="presentation"><a href="#videoChannelOther" role="tab" data-toggle="tab">...</a></li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content">
                <div role="tabpanel" class="tab-pane active" id="videoChannel">
                    <ul id='myAuthorizeTree' class='ztree'>
                    </ul>
                </div>
                <div role="tabpanel" class="tab-pane" id="videoChannelOther">
                </div>
                <div role="tabpanel" class="tab-pane">
                </div>
            </div>
        </div>
    </div>
    <script src="../JqueryPlugins/zTree_v3.5/js/jquery.ztree.all-3.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            createTree();
            var AuthorizeNodes = [];
            var result = BsrCloudServer.Customer.GetAuthorizeSubCustomer($("#hideSelectSubAccountID").val());
            if (result.authorizeSubResponse != null) {
                $("[name='inlineChkCustomerToSee']").removeAttr("checked");
                if (result.authorizeSubResponse.subCustomerPermissionList == null || result.authorizeSubResponse.subCustomerPermissionList.length == 0) {
                    $("[name='inlineChkCustomerToSee']").removeAttr("checked");
                }
                else {
                    for (var i = 0; i < result.authorizeSubResponse.subCustomerPermissionList.length; i++) {
                        var key = result.authorizeSubResponse.subCustomerPermissionList[i].PermissionName;
                        var name = result.authorizeSubResponse.subCustomerPermissionList[i].IsEnable;
                        switch (key) {
                            case "MySpace":
                                if (name.toString() == "1") {
                                    $("#showMySpace").attr("checked", "checked");
                                }
                                else {
                                    $("#showMySpace").removeAttr("checked");
                                }
                                break;
                            case "EventMessage":
                                if (name.toString() == "1") {
                                    $("#showEventMessage").attr("checked", "checked");
                                }
                                else {
                                    $("#showEventMessage").removeAttr("checked");
                                }
                                break;
                            case "CloudVideo":
                                if (name.toString() == "1") {
                                    $("#showCloudVideo").attr("checked", "checked");
                                }
                                else {
                                    $("#showCloudVideo").removeAttr("checked");
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                if (result.authorizeSubResponse.authorizeDeviceResponseList != null && result.authorizeSubResponse.authorizeDeviceResponseList.length > 0) {
                    for (var i = 0; i < result.authorizeSubResponse.authorizeDeviceResponseList.length; i++) {
                        var flag = false;
                        for (var j = 0; j < result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse.length; j++) {
                            var strJson = new Object();
                            strJson.id = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelId;
                            strJson.pId = result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId;
                            strJson.name = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName;
                            if (result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].IsEnable == 1) {
                                strJson.chkDisabled = false;
                            }
                            else {
                                strJson.chkDisabled = true;
                            }
                            if (result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse == null ||
                                    result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse.length == 0) {
                                //strJson += ",\"Video\": 0" + ",\"Playback\": 0 }";
                                strJson.Video = 0;
                                strJson.Playback = 0;
                                strJson.checked = false;
                            }
                            else {
                                strJson.checked = true;
                                flag = true;
                                var ChannelPermission = "";
                                for (var k = 0; k < result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse.length; k++) {
                                    if ("" == ChannelPermission) {
                                        ChannelPermission = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse[k].PermissionName;
                                    }
                                    else {
                                        ChannelPermission += result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse[k].PermissionName;
                                    }
                                }
                                if (ChannelPermission.indexOf("Video") >= 0) {
                                    strJson.Video = 1;
                                }
                                else {
                                    strJson.Video = 0;
                                }
                                if (ChannelPermission.indexOf("Playback") >= 0) {
                                    strJson.Playback = 1;
                                } else {
                                    strJson.Playback = 0;
                                }
                            }

                            AuthorizeNodes.push(strJson);
                        }
                        AuthorizeNodes.push({ "id": result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId, "pId": 0, "name": result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceName, isParent: true, open: true, "checked": flag });
                    }
                }
            }
            $("#myAuthorizeTree").empty();
            $.fn.zTree.init($("#myAuthorizeTree"), setting, AuthorizeNodes);
            treeObj = $.fn.zTree.getZTreeObj("myAuthorizeTree");
        });

        //创建树 
        var setting;
        function createTree() {
            //树设置
            setting = {
                data: {
                    simpleData: {
                        enable: true
                    },
                    keep: {
                        parent: false,
                        leaf: true
                    }
                },
                check: {
                    enable: true,
                    chkStyle: "checkbox",
                    chkboxType: { "Y": "ps", "N": "ps" }
                },
                view: {
                    showIcon: true,
                    showLine: false,
                    showTitle: false,
                    selectedMulti: false,
                    addDiyDom: addSelfDiyDom
                },
                treeNode: {
                    nocheck: false,
                    checked: true
                },
                callback: {
                    onCheck: zTreeOnCheck
                }
            };
        }


        // 每次点击 checkbox 或 radio后触发该事件
        function zTreeOnCheck(event, treeId, treeNode) {
            if (treeNode.isParent) {
                if (treeNode.checked) {
                    for (var i = 0; i < treeNode.children.length; i++) {
                        if (!treeNode.children[i].chkDisabled) {
                            $("[name='chkPermission_" + treeNode.children[i].id + "']").prop("checked", true);
                            treeNode.children[i].Video = 1;
                            //treeNode.children[i].Playback = 1;
                        }
                    }
                }
                else {
                    for (var i = 0; i < treeNode.children.length; i++) {
                        $("[name='chkPermission_" + treeNode.children[i].id + "']").prop("checked", false);
                        treeNode.children[i].Video = 0;
                        //treeNode.children[i].Playback = 0;
                    }
                }
            }
            else {
                if (treeNode.checked) {
                    $("[name='chkPermission_" + treeNode.id + "']").prop("checked", true);
                    treeNode.Video = 1;
                    //treeNode.Playback = 1;
                }
                else {
                    $("[name='chkPermission_" + treeNode.id + "']").prop("checked",false);
                    treeNode.Video = 0;
                    //treeNode.Playback = 0;
                }
            }
        }

        function addSelfDiyDom(treeId, treeNode) {
            if (!treeNode.isParent) {
                var aObj = $("#" + treeNode.tId + "_a");
                var btnGroup = $("<div style='float:right;width: 150px;' ></div>");
                var editStr = ""; //disabled
                if (treeNode.chkDisabled) {
                    editStr = "<label class='checkbox-inline'> " +
                        "<input type='checkbox' id='chkSP_" + treeNode.id + "' value='option1' disabled='disabled' name='chkPermission_" + treeNode.id + "' />现场视频</label>"
                         + "<label class='checkbox-inline'  style='margin-left: 40px;'>" +
                        "<input type='checkbox' id='chkHF_" + treeNode.id + "' value='option2' disabled='disabled' name='chkPermission_1" + treeNode.id + "' />回放</label>";
                } else {
                    if (treeNode.Video == 0) {
                        editStr = "<label class='checkbox-inline'> " +
                        "<input type='checkbox' id='chkSP_" + treeNode.id + "' value='option1' name='chkPermission_" + treeNode.id + "' />现场视频</label>";

                    } else {
                        editStr = "<label class='checkbox-inline'> " +
                        "<input type='checkbox' id='chkSP_" + treeNode.id + "' value='option1' name='chkPermission_" + treeNode.id + "' checked='checked' />现场视频</label>";
                    }
                    if (treeNode.Playback == 0) {
                        editStr += "<label class='checkbox-inline'  style='margin-left: 40px;'>" +
                        "<input type='checkbox' id='chkHF_" + treeNode.id + "' value='option2' name='chkPermission_1" + treeNode.id + "' />回放</label>";
                    } else {
                        editStr += "<label class='checkbox-inline'  style='margin-left: 40px;'>" +
                        "<input type='checkbox' id='chkHF_" + treeNode.id + "' value='option2' name='chkPermission_1" + treeNode.id + "' checked='checked' />回放</label>";
                    }
                }

                btnGroup.append(editStr);
                aObj.append(btnGroup);

                var chkSP = $("#chkSP_" + treeNode.id);
                if (chkSP) {
                    chkSP.bind("click", function () {
                        var flag = true;
                        $("[name='chkPermission_" + treeNode.id + "']").each(function () {
                            if ($(this)[0].checked) {
                                flag = false;
                                //treeNode.checked = true;
                                treeObj.checkNode(treeNode, true, false);
                            }
                        });
                        if (flag) {
                            treeObj.checkNode(treeNode, false, false);
                        }
                        //更改选中状态状态的值
                        if ($(chkSP)[0].checked) {
                            treeNode.Video = 1;
                            if (!treeNode.getParentNode().checked) {
                                treeObj.checkNode(treeNode.getParentNode(), true, false);
                            }
                        }
                        else {
                            treeNode.Video = 0;
                            if (treeNode.getParentNode().checked) {
                                var flag_checked = false;
                                for (var i = 0; i < treeNode.getParentNode().children.length; i++) {
                                    if (treeNode.getParentNode().children[i].checked) {
                                        flag_checked = true;
                                    }
                                }
                                if (!flag_checked) {
                                    treeObj.checkNode(treeNode.getParentNode(), false, false);
                                }
                            }
                        }
                    });
                }
                var chkHF = $("#chkHF_" + treeNode.id);
                if (chkHF) {
                    chkHF.bind("click", function () {
                        var flag = true;
                        $("[name='chkPermission_" + treeNode.id + "']").each(function () {
                            if ($(this)[0].checked) {
                                flag = false;
                                treeObj.checkNode(treeNode, true, false);
                            }
                        });
                        if (flag) {
                            treeObj.checkNode(treeNode, false, false);
                        }
                        //更改选中状态状态的值
                        if ($(chkHF)[0].checked) {
                            treeNode.Playback = 1;
                            if (!treeNode.getParentNode().checked) {
                                treeObj.checkNode(treeNode.getSelectedNodes(), true, false);
                            }
                        }
                        else {
                            treeNode.Playback = 0;
                            if (treeNode.getParentNode().checked) {
                                var flag_checked = false;
                                for (var i = 0; i < treeNode.getParentNode().children.length; i++) {
                                    if (treeNode.getParentNode().children[i].checked) {
                                        flag_checked = true;
                                    }
                                }
                                if (!flag_checked) {
                                    treeObj.checkNode(treeNode.getParentNode(), false, false);
                                }
                            }
                        }
                    });
                }
            }
        }

    </script>
</body>
</html>
