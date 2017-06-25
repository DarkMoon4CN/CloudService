<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChannelManage.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.ChannelManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>通道管理</title>
    <link href="../JqueryPlugins/zTree_v3.5/css/zTreeStyle/metro.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .ztree
        {
            padding: 0px;
        }
        .ztree li ul
        {
            margin: 0;
            padding: 0;
        }
        .ztree li a
        {
            width: 100%;
            line-height: 40px;
            overflow: visible;
            margin-bottom: 1px;
        }
        .ztree li a.level0
        {
            background-color: #f8f8f8;
        }
        .ztree li a.level1
        {
            background-color: #fbfbfb;
        }
        .ztree li span.ico_docu.button
        {
            vertical-align: middle;
        }
        .ztree li span.ico_open.button
        {
            vertical-align: middle;
        }
        .ztree li span.ico_close.button
        {
            vertical-align: middle;
        }
        #groupTree.ztree li span.button {background-image:url("../Images/left_menuForOutLook.png"); *background-image:url("../Images/left_menuForOutLook.gif")}
        #groupTree.ztree li span.button.switch.level0 {width: 20px; height:20px}
        #groupTree.ztree li span.button.switch.level1 {width: 20px; height:20px}
        #groupTree.ztree li span.button.noline_open {background-position: 0 0;}
        #groupTree.ztree li span.button.noline_close {background-position: -18px 0;}
        #groupTree.ztree li span.button.noline_open.level0 {background-position: 0 -18px;}
        #groupTree.ztree li span.button.noline_close.level0 {background-position: -18px -18px;}
        #channelTree.ztree li span.icon
        {
            margin-left: 10px;
        }
        #channelTree.ztree li span.button.iconEdit
        {
            background: url(../../Images/icons/icon_Edit_Normal.png) no-repeat scroll 0 0 transparent;
            vertical-align: middle;
        }
        #channelTree.ztree li span.button.iconEdit:hover
        {
            background: url(../../Images/icons/icon_Edit_Hover.png) no-repeat scroll 0 0 transparent;
        }
        #channelTree.ztree li span.button.iconLock
        {
            vertical-align: top;
            vertical-align: middle;
        }
        #channelTree.ztree li span.button.iconLock.locked
        {
            background: url(../../Images/icons/icon_lock_Normal.png) no-repeat scroll 0 0 transparent;
            vertical-align: top;
            vertical-align: middle;
        }
        #channelTree.ztree li span.button.iconLock.locked:hover
        {
            background: url(../../Images/icons/icon_Lock_Hover.png) no-repeat scroll 0 0 transparent;
        }
        #channelTree.ztree li span.button.iconLock.unlock
        {
            background: url(../../Images/icons/icon_Unlock_Normal.png) no-repeat scroll 0 0 transparent;
            vertical-align: top;
            vertical-align: middle;
        }
        #channelTree.ztree li span.button.iconLock.unlock:hover
        {
            background: url(../../Images/icons/icon_Unlock_Hover.png) no-repeat scroll 0 0 transparent;
        }
        #channelTree.ztree li span.button.iconSetting
        {
            background: url(../../Images/icons/icon_Setting_Normal.png) no-repeat scroll 0 0 transparent;
            vertical-align: top;
            vertical-align: middle;
        }
    </style>
</head>
<body>
    <div>
        <div class="content-nav">
            <div class="col-xs-12 text-left">
                <span>通道管理</span>
            </div>
        </div>
        <div style="margin: 40px">
            <div class="col-xs-6" style="text-align: center; vertical-align: middle">
                <div class="col-xs-2">
                    <img src="../Images/icons/icon_AddGroup_Normal.png" /></div>
                <div class="input-group col-xs-8 ">
                    <input type="text" class="form-control" placeholder="添加分组" id="txtGroupName" maxlength="32">
                    <span class="input-group-btn">
                        <button class="btn btn-default" type="button" onclick="channelManage.group.add();">
                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                        </button>
                    </span>
                </div>
            </div>
            <div class="col-xs-6" style="text-align: center; vertical-align: middle">
                <div class="col-xs-2">
                    <img src="../Images/icons/icon_Search_Normal.png" /></div>
                <div class="input-group col-xs-8 ">
                    <input type="text" class="form-control" placeholder="通道查询" id="txtChannelSearch">
                    <span class="input-group-btn">
                        <button class="btn btn-default" type="button" onclick="channelManage.channel.search();">
                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                        </button>
                    </span>
                </div>
            </div>
            <div class="col-xs-12" style="text-align: center; height: 43px" id="groupingTip">
                <span class="help-block">提示：可以通过拖拽方式快速将设备分配给某个分组（1个通道仅支持1个组，但1个组可以同时包含多个不同通道）<a href="#"
                    style="font-style: normal" id="btnMove" onclick="channelManage.channel.moveToGroup();return false;"><img
                        src="../Images/icons/icon_Relate_Normal.png" />建立关联</a></span>
            </div>
        </div>
        <div>
            <div style="float: left; width: 50%; height: 500px; border-right: 1px dashed #EBEBEB;">
                <ul id="groupTree" class="ztree" style="width: 95%">
                </ul>
                <div style="height: 40px; background-color: #F8F8F8; margin: 1px 0px 1px 0px; width: 95%;">
                    <a href="#" style="display: block; width: 100%; height: 40px; text-decoration: none;
                        padding: 0px 10px 10px 10px; color: #333333;" onclick="channelManage.group.addRoot();">
                        <span>
                            <img src="../Images/icons/icon_AddGroup_Shortcut_Normal.png" />新增分组</span></a>
                </div>
            </div>
            <div style="float: left; width: 50%; height: 500px">
                <div style="height: 40px; background-color: #F8F8F8; margin: 0px 0px 0px 15px">
                    <a href="#" id="btnShowChannel" style="display: block; width: 100%; text-decoration: none;
                        color: #333333">
                        <img src="../Images/icons/Listbox_Display.png" />
                        <span>设备列表</span></a>
                </div>
                <ul id="channelTree" class="ztree list-group" style="width: 100%; padding: 1px 0px 0px 15px">
                </ul>
            </div>
        </div>
    </div>
    <script src="../JqueryPlugins/zTree_v3.5/js/jquery.ztree.all-3.5.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/zTree_v3.5/js/jquery.ztree.exhide-3.5.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        var channelManage = {
            group: {
                groupTree: null,
                init: function () {
                    //树设置
                    var setting = {
                        data: {
                            simpleData: { enable: true },
                            keep: {
                                parent: true,  //锁定父节点,防止拖拽后变为子节点
                                leaf: true
                            }
                        },
                        view: {
                            showIcon: false,
                            showLine: false,
                            selectedMulti: false, //ctrl多选
                            addDiyDom: addDiyDom,
                            addHoverDom: addHoverDom,
                            removeHoverDom: removeHoverDom
                        },
                        edit: {
                            enable: true,
                            showRemoveBtn: false,
                            showRenameBtn: false,
                            drag: {
                                auotExpandTrigger: true,
                                isCopy: false,
                                isMove: true,
                                prev: false,
                                next: false
                            }
                        },
                        callback: {
                            beforeDrop: channelManage.group.drop,  //拖拽前处理
                            beforeRename: channelManage.group.rename, //重命名前处理
                            beforeRemove: channelManage.group.remove,  //移除
                            onClick: channelManage.group.onClick
                        }
                    };
                    var zNodes = [];
                    var groupTreeObj = $("#groupTree");
                    groupTreeObj.empty();
                    $.fn.zTree.init(groupTreeObj, setting, zNodes);
                    channelManage.group.groupTree = $.fn.zTree.getZTreeObj("groupTree");

                    //追加组
                    BsrCloudServer.ResourceGroup.getChannelGroup(function (msg) {
                        if (msg.Code == 0) {
                            var resourceGroupList = msg.resourceGroupList;
                            var newNodes = [];
                            for (var i = 0; i < resourceGroupList.length; i++) {
                                newNodes.push({ id: resourceGroupList[i].ResourceGroupId, pId: resourceGroupList[i].ParentResourceGroupId, name: resourceGroupList[i].ResourceGroupName, isParent: true, open: true });
                            }
                            channelManage.group.groupTree.addNodes(null, newNodes);
                            appendChannel();
                        } else {
                            jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                    });
                    //追加已分组通道
                    function appendChannel() {
                        //获取组的根节点
                        var rootNodes = channelManage.group.groupTree.getNodesByFilter(function (node) { return (node.level == 0); });
                        for (var i = 0; i < rootNodes.length; i++) {
                            //加载已分组通道
                            BsrCloudServer.ResourceGroup.getChannelListByGroupId(rootNodes[i].id, function (msg) {
                                if (msg.Code == 0) {
                                    var channelList = msg.responseGroupChannelList;
                                    for (var j = 0; j < channelList.length; j++) {
                                        var parentNode = channelManage.group.groupTree.getNodesByFilter(function (node) { return (node.id == channelList[j].ResourceGroupId && node.isParent == true); }, true);
                                        var newNodes = { id: channelList[j].ChannelId, pId: channelList[j].ResourceGroupId, name: channelList[j].ChannelName, isParent: false, icon: '../Images/icons/channel.png' };
                                        channelManage.group.groupTree.addNodes(parentNode, newNodes, true);
                                    }
                                }
                                else {
                                    jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                }
                            });
                        }
                    }

                    //添加自定义图标,把收缩按钮和小图标都放到a标签中
                    function addDiyDom(treeId, treeNode) {
                        var spaceWidth = 10;
                        var aObj = $("#" + treeNode.tId + "_a");
                        var switchObj = $("#" + treeNode.tId + "_switch");
                        switchObj.remove();
                        if (treeNode.isParent) {
                            aObj.prepend(switchObj);
                        } else {
                            aObj.prepend("<span style='display: inline-block;width:20px;'></span>");
                        }
                        if (treeNode.level <= 1) {
                            var spaceStr = "<span style='display: inline-block;width:20px;'></span>";
                            aObj.prepend(spaceStr);
                        }
                        if (treeNode.level > 1) {
                            var spaceStr = "<span style='display: inline-block;width:" + (20 + spaceWidth * treeNode.level) + "px;'></span>";
                            aObj.prepend(spaceStr);
                        }
                    }
                    //添加组删除和重名称
                    function addHoverDom(treeId, treeNode) {
                        if ($("#diyHoverBtn_" + treeNode.id).length > 0) return;
                        var aObj = $("#" + treeNode.tId + "_a");
                        var removeObj = $("<span class='icon button' ></span>");
                        var editObj = $("<span  class='icon button' ></span>");
                        removeObj.css({ "background-image": "url('../../Images/icons/icon_Delete_Normal.png')", "background-position": "0px 0px", "vertical-align": "middle", "margin-right": "10px" });
                        editObj.css({ "background-image": "url('../../Images/icons/icon_Edit_Normal.png')", "background-position": "0px 0px", "vertical-align": "middle", "margin-right": "10px" });

                        var managerObj = $("<div></div>");
                        managerObj.attr("id", "diyHoverBtn_" + treeNode.id);
                        managerObj.css({ "heigth": "30px", "float": "right", "display": "block" });
                        managerObj.append(editObj).append(removeObj);
                        aObj.append(managerObj);
                        var btn = $("#diyHoverBtn_" + treeNode.id);
                        if (btn) {
                            $("#diyHoverBtn_" + treeNode.id + " span:eq(0)").bind("click", function () { channelManage.group.groupTree.editName(treeNode); });
                            $("#diyHoverBtn_" + treeNode.id + " span:eq(1)").bind("click", function () { channelManage.group.groupTree.removeNode(treeNode, true); });
                        }
                    }
                    function removeHoverDom(treeId, treeNode) {
                        $("#diyHoverBtn_" + treeNode.id).unbind().remove();
                    }
                    //单击
                    function groupClick() {

                    }
                },
                add: function () {
                    //添加分组前检查
                    function checkNewGroup() {
                        var newNodeName = $("#txtGroupName").val();
                        var selectNodes = channelManage.group.groupTree.getSelectedNodes();
                        if (!newNodeName || newNodeName == $("#txtGroupName").attr("placeholder")) {
                            jError("请输入分组名称！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            $("#txtGroupName").focus();
                            return false;
                        } else if (selectNodes.length == 0) {
                            var rootNodes = channelManage.group.groupTree.getNodesByParam("level", "0", null);
                            if (rootNodes && rootNodes.length) {
                                for (var i = 0; i < rootNodes.length; i++) {
                                    if (rootNodes[i].name == newNodeName) {
                                        $("#txtGroupName").focus();
                                        jError("注意：同一个目录下不能重复分组！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                        return false;
                                    }
                                }
                            }
                        } else if (selectNodes.length != 0) {
                            if (!selectNodes[0].isParent) {
                                jError("注意：当前选中为设备节点,不能分组！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                return false;
                            }
                            if (selectNodes[0].children && selectNodes[0].children.length) {
                                var childrenNodes = selectNodes[0].children;
                                for (i = 0; i < childrenNodes.length; i++) {
                                    if (childrenNodes[i].name == newNodeName && childrenNodes[i].isParent) {
                                        jError("注意：该目录下已存在该组！", {
                                            VerticalPosition: 'top',
                                            HorizontalPosition: 'center',
                                            ShowOverlay: false
                                        });
                                        return false;
                                    }
                                }
                            }
                        }
                        return true;
                    }
                    var newNodeName = $("#txtGroupName").val();
                    var selectNodes = channelManage.group.groupTree.getSelectedNodes();
                    if (checkNewGroup()) {
                        var msg = BsrCloudServer.ResourceGroup.addRGourpByName(newNodeName, (selectNodes && selectNodes.length) ? selectNodes[0].id : "0");
                        if (msg) {
                            if (msg.Code == 0) {
                                jSuccess("添加分组成功！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                channelManage.group.init();
                            } else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            }
                        }
                    }
                },
                addRoot: function () {
                    var nodes = channelManage.group.groupTree.getNodesByParam("level", 0);
                    var newNodeName = "新建分组";
                    if (nodes.length > 0) {
                        newNodeName = ("新建分组" + nodes.length);
                    }
                    var newNode = { name: newNodeName, isParent: true };
                    $("#txtGroupName").val(newNodeName);
                    var selectedNodes = channelManage.group.groupTree.getSelectedNodes();
                    if (selectedNodes.length > 0) {
                        channelManage.group.groupTree.cancelSelectedNode(selectedNodes[0]);
                    }
                    channelManage.group.add();
                },
                drop: function (treeId, treeNodes, targetNode, moveType, isCopy) {
                    var result = false;
                    if (!targetNode) {
                        var rootNodes = channelManage.group.groupTree.getNodesByParam("level", "0", null);
                        if (rootNodes && rootNodes.length) {
                            for (var i = 0; i < rootNodes.length; i++) {
                                for (var j = 0; j < treeNodes.length; j++) {
                                    if (rootNodes[i].name == treeNodes[j].name) {
                                        jError("注意：该目录下已存在该组！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                        return false;
                                    }
                                }
                            }
                        }

                    } else {
                        var targetNode_children = targetNode.children;
                        if (treeNodes.length > 0 && targetNode_children) {
                            for (var i = 0; i < treeNodes.length; i++) {
                                for (var j = 0; j < targetNode_children.length; j++) {
                                    if (treeNodes[i].name == targetNode_children[j].name) {
                                        jError("注意：该目录下已存在该组！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    for (var i = 0; i < treeNodes.length; i++) {
                        if (treeNodes[i].isParent) {//拖拽组
                            var msg = BsrCloudServer.ResourceGroup.moveResourceGroup(treeNodes[i].id, targetNode ? targetNode.id : 0);
                            if (msg) {
                                if (msg.Code == 0) { channelManage.group.init(); }
                                else { jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false }); }
                            }
                        } else {//拖拽通道   
                            if (targetNode) {
                                var msg = BsrCloudServer.ResourceGroup.moveChannelListByResourceGroupId(targetNode.id, [treeNodes[i].id]);
                                if (msg) {
                                    if (msg.Code == 0) { channelManage.group.init(); }
                                    else { jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false }); }
                                }
                            }
                        }
                    }
                    return false;
                },
                rename: function (treeId, treeNode, newName, isCancel) {
                    if (isCancel) { return true; }
                    var treeNode_brothers = channelManage.group.groupTree.getNodesByFilter(function (node) { return node.level == treeNode.level && node.isParent });
                    if (!$.trim(newName)) {
                        jError("注意：名称不能为空!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        $(".rename").focus();
                    }
                    if (treeNode_brothers.length > 1) {
                        for (i = 0; i < treeNode_brothers.length; i++) {
                            if (treeNode_brothers[i] != treeNode && treeNode_brothers[i].name == newName) {
                                jError("注意：名称不能重复!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                $(".rename").focus();
                                return false;
                            }
                        }
                    }
                    //重命名组
                    if (treeNode.isParent) {
                        BsrCloudServer.ResourceGroup.updateResourceGroupName(treeNode.id, newName, function (msg) {
                            if (msg) {
                                if (msg.Code == 0) {
                                    jSuccess("保存成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                    channelManage.group.groupTree.cancelEditName(newName);
                                }
                                else {
                                    jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                    channelManage.group.groupTree.cancelEditName();
                                }
                            }
                        });
                    }
                    //在分组中重命名通道
                    else {
                        BsrCloudServer.Channel.updateChannelName(treeNode.id, newName, function (msg) {
                            if (msg.Code == 0) {
                                jSuccess("保存成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                channelManage.group.groupTree.cancelEditName(newName);
                                groupOrChannelOnRename(treeId, treeNode);
                            }
                            else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                channelManage.group.groupTree.cancelEditName();
                            }
                        }
                );
                    }
                    return false;
                },
                remove: function (treeId, treeNode) {
                    //删除组
                    if (treeNode.isParent) {
                        var msg = BsrCloudServer.ResourceGroup.deleteResourceGroup(treeNode.id);
                        if (msg) {
                            if (msg.Code == 0) {
                                jSuccess("删除组成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                channelManage.group.groupTree.removeNode(treeNode);
                                channelManage.checkIsGrouped();
                            }
                            else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            }
                        }
                    } else {
                        //删除组下通道
                        var msg = BsrCloudServer.ResourceGroup.deleteGroupChanne(treeNode.id, treeNode.getParentNode().id);
                        if (msg.Code == 0) {
                            jSuccess("移除通道成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            channelManage.group.groupTree.removeNode(treeNode);
                            channelManage.checkIsGrouped();
                        }
                        else {
                            jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }

                    }
                    return false;
                },
                onClick: function (event, treeId, treeNode, clickFlag) {
                    channelManage.group.groupTree.getNodesByFilter(function (node) {
                        $("#" + node.tId + "_a").css({ "border-left": "0", "margin-left": "0px" });
                    });
                    $("#" + treeNode.tId + "_a").css({ "border-left": "4px solid #30C5FC", "margin-left": "-4px" });
                }
            },
            channel: {
                channelTree: null,
                init: function () {
                    var setting = {
                        data: {
                            simpleData: {
                                enable: true
                            },
                            keep: {
                                parent: true,  //锁定父节点,防止拖拽后变为子节点
                                leaf: true
                            }
                        },
                        check: {
                            enable: true
                        },
                        view: {
                            dblClickExpand: false,
                            showIcon: false,
                            showLine: false,
                            addDiyDom: addDiyDom,
                            addHoverDom: addHoverDom,
                            removeHoverDom: removeHoverDom
                        },
                        edit: {
                            enable: true,
                            showRenameBtn: false,
                            showRemoveBtn: false,
                            drag: {
                                auotExpandTrigger: true,
                                isCopy: true,
                                isMove: true,
                                prev: false,
                                next: false,
                                inner: false
                            }
                        },
                        callback: {
                            beforeRename: channelManage.channel.rename,
                            beforeDrop: channelManage.channel.drop,
                            onClick: function (event, treeId, treeNode, clickFlag) { channelManage.channel.channelTree.expandNode(treeNode); }
                        }
                    };
                    var zNodes = [];
                    var channelTreeObj = $("#channelTree");
                    channelTreeObj.children().remove();
                    $.fn.zTree.init(channelTreeObj, setting, zNodes);
                    channelManage.channel.channelTree = $.fn.zTree.getZTreeObj("channelTree");

                    BsrCloudServer.Device.getDevice(function (msg) {
                        if (msg.Code == 0) {
                            var deviceList = msg.deviceResponseList;
                            for (var i = 0; i < deviceList.length; i++) {
                                var newNodes = [{ id: deviceList[i].DeviceId, pId: 0, name: (deviceList[i].IsAuthorize ? deviceList[i].DeviceName + "(授权设备)" : deviceList[i].DeviceName), isParent: true, open: true}];
                                channelManage.channel.channelTree.addNodes(null, newNodes);
                            }
                            BsrCloudServer.Channel.getChannelByPage(999, 1, function (msg) {
                                if (msg.Code == 0) {
                                    var channelList = msg.groupChannelResponseList;
                                    for (var j = 0; j < channelList.length; j++) {
                                        var parentNode = channelManage.channel.channelTree.getNodesByFilter(function (node) { return (node.id == channelList[j].DeviceId && node.isParent == true); }, true);
                                        var newNodes = { id: channelList[j].ChannelId, pId: channelList[j].DeviceId, name: channelList[j].ChannelName, isParent: false, isEnable: channelList[j].IsEnable, isAuthorize: channelList[j].IsAuthorize };
                                        channelManage.channel.channelTree.addNodes(parentNode, newNodes);
                                    }
                                    channelManage.checkIsGrouped();

                                } else {
                                    jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                }
                            });
                        } else {
                            jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                    });
                    //添加自定义按钮
                    function addDiyDom(treeId, treeNode) {
                        var spaceWidth = 10;
                        var swithObj = $("#" + treeNode.tId + "_switch");
                        var checkObj = $("#" + treeNode.tId + "_check").css("margin-left", "30px");
                        var icoObj = $("#" + treeNode.tId + "_ico");
                        var aObj = $("#" + treeNode.tId + "_a");
                        swithObj.remove();
                        checkObj.remove();
                        if (treeNode.level > 0) {
                            var spaceStr = "<span style='display: inline-block;width:" + (spaceWidth * treeNode.level) + "px;'></span>";
                            aObj.prepend(spaceStr);
                        }
                        aObj.prepend(checkObj)
                    }
                    //添加鼠标滑动按钮
                    function addHoverDom(treeId, treeNode) {
                        if ($("#diyHoverBtn_" + treeNode.id).length > 0) return;
                        var aObj = $("#" + treeNode.tId + "_a");
                        if (!treeNode.isParent) {
                            //添加操作按钮
                            var btnGroup = $("<div style='float:right;' ></div>").attr("id", "diyHoverBtn_" + treeNode.id);
                            var editStr = "<span class='icon button iconEdit' id='editBtn_" + treeNode.id + "' title='编辑通道名称'></span>";
                            var unlockStr = "<span class='icon button iconLock unlock' id='lockBtn_" + treeNode.id + "' title='冻结/启用'></span>";
                            var lockedStr = "<span class='icon button iconLock locked' id='lockBtn_" + treeNode.id + "' title='冻结/启用'></span>";
                            var settingStr = "<span class='icon button iconSetting' id='setupBtn_" + treeNode.id + "' title='高级设置'></span>";
                            if (!treeNode.isAuthorize) {
                                if (treeNode.isEnable) {
                                    btnGroup.append(editStr).append(unlockStr);
                                } else {
                                    btnGroup.append(editStr).append(lockedStr);
                                }
                            }
                            aObj.append(btnGroup);

                            var btnEdit = $("#editBtn_" + treeNode.id);
                            if (btnEdit) btnEdit.bind("click", function () { channelManage.channel.channelTree.editName(treeNode); });

                            var btnLock = $("#lockBtn_" + treeNode.id);
                            if (btnLock) btnLock.bind("click", function () { channelManage.channel.lock(treeNode) });
                        }

                    }
                    function removeHoverDom(treeId, treeNode) {
                        $("#diyHoverBtn_" + treeNode.id).unbind().remove();
                    }
                },
                rename: function (treeId, treeNode, newName, isCancel) {
                    if (isCancel) { return true; }
                    else {
                        if (!$.trim(newName)) {
                            jError("注意：通道名称不能为空！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            $(".rename").focus();
                        }
                        var msg = BsrCloudServer.Channel.updateChannelName(treeNode.id, newName, function (msg) {
                            if (msg.Code == 0) {
                                jSuccess("保存成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                channelTree_Menu.cancelEditName(newName);
                                groupOrChannelOnRename(treeId, treeNode);
                            } else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                channelManage.channel.channelTree.cancelEditName();
                            }
                        });
                    }
                    return false;
                },
                drop: function (treeId, treeNodes, targetNode, moveType, isCopy) {
                    if (!targetNode) {
                        return false;
                    }
                    channelManage.channel.moveToGroup(targetNode.id);
                    return false;
                },
                moveToGroup: function (groupId) {
                    if (!groupId) {
                        var selectedNodes = channelManage.group.groupTree.getSelectedNodes();
                        if (selectedNodes.length > 0 && selectedNodes[0].isParent) {
                            groupId = selectedNodes[0].id;
                        } else {
                            jError("请先选择分组！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            return false;
                        }

                    }
                    var checkedNodes = channelManage.channel.channelTree.getCheckedNodes(true);
                    if (checkedNodes.length == 0) {
                        jError("请先选择要分组的设备通道！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        return false;
                    }
                    var channelIdList = [];
                    for (var i = 0; i < checkedNodes.length; i++) {
                        if (!checkedNodes[i].isParent) {
                            channelIdList.push(checkedNodes[i].id);
                        }
                    }
                    if (channelIdList.length) {
                        var msg = BsrCloudServer.ResourceGroup.moveChannelListByResourceGroupId(groupId, channelIdList);
                        if (msg.Code == 0) {
                            for (var i = 0; i < checkedNodes.length; i++) {
                                //移动成功后设置复选框不可选
                                channelManage.channel.channelTree.checkNode(checkedNodes[i], false);
                                channelManage.channel.channelTree.setChkDisabled(checkedNodes[i], true);
                            }
                            jSuccess("保存成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            channelManage.group.init();
                        }
                        else {
                            jErroe(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                    }
                },
                lock: function (treeNode) {
                    var btnEdit = $('#lockBtn_' + treeNode.id);
                    if (treeNode.isEnable) {
                        BsrCloudServer.Channel.enableChannel(treeNode.id, 0, function (msg) {
                            if (msg) {
                                if (msg.Code == 0) {
                                    treeNode.isEnable = 0;
                                    btnEdit.removeClass("unlock").addClass("locked");
                                } else {
                                    jError(treeNode.name + "禁用失败，" + msg.Message, {
                                        VerticalPosition: 'top',
                                        HorizontalPosition: 'center',
                                        ShowOverlay: false
                                    });
                                }
                            }
                        });
                    } else {
                        BsrCloudServer.Channel.enableChannel(treeNode.id, 1, function (msg) {
                            if (msg) {
                                if (msg.Code == 0) {
                                    treeNode.isEnable = 1;
                                    btnEdit.removeClass("locked").addClass("unlock");
                                } else {
                                    jError(treeNode.name + "启用失败，" + msg.Message, {
                                        VerticalPosition: 'top',
                                        HorizontalPosition: 'center',
                                        ShowOverlay: false
                                    });
                                }
                            }
                        });
                    }
                    return false;
                },
                search: function () {
                    $("#channelTree").parent().mask("处理中...");
                    var searchValue = $.trim($("#txtChannelSearch").val());
                    if (searchValue && searchValue != $("#txtChannelSearch").attr("placeholder")) {
                        channelManage.channel.channelTree.hideNodes(channelManage.channel.channelTree.getNodesByParam("isParent", false));
                        var nodes = channelManage.channel.channelTree.getNodesByFilter(function (node) {
                            return (node.name.indexOf(searchValue) > -1);
                        });
                        channelManage.channel.channelTree.showNodes(nodes);
                    } else {
                        channelManage.channel.channelTree.showNodes(channelManage.channel.channelTree.getNodesByParam("isParent", false));
                    }
                    setTimeout(function () {
                        $("#channelTree").parent().unmask();
                    }, 300);

                }
            },
            checkIsGrouped: function () {
                var groupChannelNodes = channelManage.group.groupTree.getNodesByParam("isParent", false);
                var channelNodes = channelManage.channel.channelTree.getNodesByParam("isParent", false);
                for (var i = 0; i < channelNodes.length; i++) {
                    channelManage.channel.channelTree.setChkDisabled(channelNodes[i], false, true, true);
                    for (var j = 0; j < groupChannelNodes.length; j++) {
                        if (channelNodes[i].id == groupChannelNodes[j].id) {
                            channelManage.channel.channelTree.checkNode(channelNodes[i], false);
                            channelManage.channel.channelTree.setChkDisabled(channelNodes[i], true, true, true);
                        }
                    }
                }
            },
            onRenamed: function (treeId, treeNode) {
                var name = treeNode.name;
                var node;
                if (treeId == "groupTree") {
                    if (!treeNode.isParent) {
                        node = channelManage.channel.channelTree.getNodesByFilter(function (node) {
                            return (node.isParent == false && node.id == treeNode.id);
                        }, true);
                    }
                } else {
                    node = channelManage.group.groupTree.getNodesByFilter(function (node) {
                        return (node.isParent == false && node.id == treeNode.id);
                    }, true);
                }
                if (node) {
                    node.name = name;
                    $("#" + node.tId + "_span").text(name);
                }
                return true;
            }
        };

        $(function () {
            channelManage.group.init();
            channelManage.channel.init();
            $("#btnShowChannel").click(function () {
                if ($("#channelTree").is(":visible")) {
                    $("#channelTree").slideUp("normal", function () {
                        $("#groupingTip").children().hide();
                        $(this).find("img").attr("src", "../Images/icons/Listbox_Hide.png");
                    });
                } else {
                    $("#channelTree").slideDown("normal", function () {
                        $("#groupingTip").children().show();
                        $(this).find("img").attr("src", "../Images/icons/Listbox_Display.png");
                    });
                }
            });
            //绑定通道修改
            $(document).on({
                "keypress": function (event) {
                    if (event.keyCode == "13") {
                        channelManage.channel.search();
                    }
                }
            }, "#txtChannelSearch");
            //绑定通道修改
            $(document).on({
                "keypress": function (event) {
                    if (event.keyCode == "13") {
                        channelManage.group.add();
                    }
                }
            }, "#txtGroupName");

            if ($("#txtGroupName,#txtChannelSearch").placeholder) {
                $("#txtGroupName,#txtChannelSearch").placeholder();
            }
        });

        /**-----------通道和组公用方法-------------**/
        //获取根节点
        function getRootNode(node) {
            if (node.level == 0) {
                return node;
            } else {
                var node = node.getParentNode();
                if (node.level == 0) {
                    return node;
                } else {
                    return getRootNode(node);
                }
            }
        }
    </script>
</body>
</html>
