<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VideoWeb.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Pages.VideoWeb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>视频播放</title>
    <link href="../Styles/VideoWeb.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div>
        <div style="padding-bottom: 5px">
            <div class="col-md-8 col-sm-8">
                <div id="bsrWebUIDiv" style="position: relative">
                </div>
                <div id="playerMenu" style="display: none">
                    <div class="btn-group btn-group-sm">
                        <button type="button" id="btnPlayStop" class="btn btn-default" onclick="DPServer.Play.playClose(1);">
                            <span class="glyphicon glyphicon-stop" style="color: red"></span>停止
                        </button>
                        <button type="button" id="btnPlaySound" class="btn btn-default" onclick="DPServer.Play.playSound();">
                            <span class="glyphicon glyphicon-volume-off"></span>伴音
                        </button>
                        <button type="button" id="btnPlayCapturePic" class="btn btn-default" onclick="VideoWeb.playCapturePic();">
                            <span class="glyphicon glyphicon-camera"></span>截图
                        </button>
                        <div class="btn-group btn-group-sm">
                            <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">
                                画面分割 <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="#" onclick="DPServer.Window.setWndMode(1);"><span class="glyphicon glyphicon-film">
                                </span>一画面</a></li>
                                <li><a href="#" onclick="DPServer.Window.setWndMode(4);"><span class="glyphicon glyphicon-th-large">
                                </span>四画面</a></li>
                                <li><a href="#" onclick="DPServer.Window.setWndMode(6);"><span class="glyphicon glyphicon-th-large">
                                </span>四画面</a></li>
                                <li><a href="#" onclick="DPServer.Window.setWndMode(9);"><span class="glyphicon glyphicon-th">
                                </span>九画面</a></li>
                            </ul>
                        </div>
                    </div>
                    <div class="btn-group btn-group-sm pull-right">
                        <div class="btn-group btn-group-sm" id="changeSubstream">
                            <button type="button" id="btnPlaySubstream" class="btn btn-default dropdown-toggle"
                                data-toggle="dropdown">
                                <span>清晰度</span> <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li><a href="#" onclick="VideoWeb.changeSubStream(1);"><span class="glyphicon glyphicon-sd-video">
                                </span>流畅</a></li>
                                <li><a href="#" onclick="VideoWeb.changeSubStream(0);"><span class="glyphicon glyphicon-hd-video">
                                </span>高清</a></li>
                            </ul>
                        </div>
                        <button type="button" class="btn btn-default" onclick="DPServer.Window.fullScreen();">
                            <span class="glyphicon glyphicon-fullscreen"></span>全屏
                        </button>
                    </div>
                </div>
                <div>
                    <ul id="playerMenu2">
                        <li><a onclick="DPServer.Play.playClose(1);">
                            <img src="../Images/player/icon_PlayStop_Normal.png" alt="停止" title="停止" data-src="../Images/player/icon_PlayStop_Normal.png"
                                data-active-src="../Images/player/icon_PlayStop_Hover.png" /></a> </li>
                        <li><a onclick="DPServer.Play.playSound();" id="btnPlaySound">
                            <img src="../Images/player/icon_PlaySound_Normal.png" alt="伴音" title="伴音" data-src="../Images/player/icon_PSoundClosed_Normal.png"
                                data-active-src="../Images/player/icon_Closed_Hover.png" /></a> </li>
                        <li><a onclick="VideoWeb.playCapturePic();">
                            <img src="../Images/player/icon_PlaySnap_Normal.png" alt="设置通道封面" title="设置通道封面"
                                data-src="../Images/player/icon_PlaySnap_Normal.png" data-active-src="../Images/player/icon_PlaySnap_Hover.png" /></a>
                        </li>
                    </ul>
                    <ul id="playerMenu3">
                        <li><a onclick="DPServer.Window.fullScreen();">
                            <img src="../Images/player/icon_PlayFullScreen_Normal.png" alt="全屏" title="全屏" data-src="../Images/player/icon_PlayFullScreen_Normal.png"
                                data-active-src="../Images/player/icon_PlayFullScreen_Hover.png" /></a>
                        </li>
                        <li>
                            <div class="btn-group dropup" id="btnSplitScreen">
                                <a data-toggle="dropdown">
                                    <img src="../Images/player/icon_Play4Video_Normal.png" alt="画面分割" title="画面分割" data-src="../Images/player/icon_Play4Video_Normal.png"
                                        data-active-src="../Images/player/icon_Play4Video_Hover.png" /></a>
                                <ul id="splitScreenMenu" class="dropdown-menu" role="menu" style="min-width: 100px;
                                    left: -20px">
                                    <li><a onclick="DPServer.Window.setWndMode(9);">
                                        <img src="../Images/player/icon_Play9Video_Normal.png" data-src="../Images/player/icon_Play9Video_Normal.png"
                                            data-active-src="../Images/player/icon_Play9Video_Hover.png" />
                                    </a></li>
                                    <li><a onclick="DPServer.Window.setWndMode(6);">
                                        <img src="../Images/player/icon_Play6Video_Normal.png" data-src="../Images/player/icon_Play6Video_Normal.png"
                                            data-active-src="../Images/player/icon_Play6Video_Hover.png" />
                                    </a></li>
                                    <li><a onclick="DPServer.Window.setWndMode(4);">
                                        <img src="../Images/player/icon_Play4Video_Normal.png" data-src="../Images/player/icon_Play4Video_Normal.png"
                                            data-active-src="../Images/player/icon_Play4Video_Hover.png" />
                                    </a></li>
                                    <li><a onclick="DPServer.Window.setWndMode(1);">
                                        <img src="../Images/player/icon_Play1Video_Normal.png" data-src="../Images/player/icon_Play1Video_Normal.png"
                                            data-active-src="../Images/player/icon_Play1Video_Hover.png" />
                                    </a></li>
                                </ul>
                            </div>
                        </li>
                        <li>
                            <div class="btn-group dropup" id="btnSubstream">
                                <a data-toggle="dropdown"><span>清晰度</span></a>
                                <ul id="subStreamMenu" class="dropdown-menu" role="menu">
                                    <li><a onclick="VideoWeb.changeSubStream(1);"><span>高清</span> </a></li>
                                    <li><a onclick="VideoWeb.changeSubStream(2);"><span>标清</span> </a></li>
                                    <li><a onclick="VideoWeb.changeSubStream(3);"><span>流畅</span> </a></li>
                                </ul>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="col-md-4 col-sm-4">
                <div id="video-channelList">
                </div>
            </div>
        </div>
    </div>
    <!--设备和通道展示模板-->
    <div id="template" style="display: none">
        <div id="template-img-type">
            <div class="col-xs-12">
                <div class="channel-item-img channelItem " style="width: 260px;" data-channleid="0">
                    <div>
                        <img data-src="../channelImage/small/default.jpg" src="../channelImage/small/default.jpg" alt="通道截图" onerror="javascript:this.src='../channelImage/small/default.jpg';"  />
                        <a class="play play64" href="#" onclick="VideoWeb.playButtonClick(this);return false;">
                        </a>
                    </div>
                    <div class="channel-footer">
                        <div class="channelName col-xs-8" style="padding-left: 5px">
                            <p style="padding-top: 5px; width:100%; float:left">
                                通道名称</p>
                            <div class="input-group input-group-sm" style="display: none">
                                <input type="text" class="form-control" placeholder="通道名称">
                                <span class="input-group-btn">
                                    <button class="btn btn-default" type="button" onclick="MySpace.saveChannelName(this);">
                                        <span class="glyphicon glyphicon-ok"></span>
                                    </button>
                                    <button class="btn btn-default" type="button" onclick="VideoWeb.editChannelName(this);">
                                        <span class="glyphicon glyphicon-remove"></span>
                                    </button>
                                </span>
                            </div>
                        </div>
                        <div class="col-xs-4" style="text-align: right; padding-top: 5px">
                            <a href="#" class="icon-btn icon-btn-edit" title="修改通道名称" onclick="VideoWeb.editChannelName(this);return false;">
                            </a><a href="#" class="icon-btn icon-btn-zoom" title="放大通道截图" onclick="VideoWeb.zoomPic(this);return false;">
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 图片弹出模态框 -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel"
        aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="channelName">
                        通道名称</h4>
                </div>
                <div class="modal-body">
                    <img src="../channelImage/default.jpg" id="channelImg" style="width: 100%; height: 550px" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var VideoWeb = {
            channelCount: 0, currentPage: 1, pageSize: 999, isGroup: 0, selectedChannelId: 0,
            //初始化
            initChannel: function () {
                $("#video-channelList").children().remove();
                if (MySpace.channelShowType == 0) {
                    VideoWeb.initChannelImgView();
                } else {
                    VideoWeb.initChannelListView();
                }

            },
            //初始化列表模式通道
            initChannelListView: function () {
                var ul = $("<ul>").attr("id", "groupAndChannelTree").addClass("ztree").css("height", "590px");
                //树设置
                var setting = {
                    data: {
                        simpleData: {
                            enable: true
                        }
                    },
                    view: {
                        showIcon: false,
                        showLine: false,
                        addDiyDom: function (treeId, treeNode) {
                            var spaceWidth = 10;
                            var aObj = $("#" + treeNode.tId + "_a");
                            var switchObj = $("#" + treeNode.tId + "_switch");
                            var spanObj = $("#" + treeNode.tId + "_span");
                            spanObj.ellipsis({ maxWidth: 120, maxLine: 1 });
                            switchObj.remove();
                            if (treeNode.isParent) {
                                aObj.prepend(switchObj);
                            } else {
                                aObj.prepend("<span style='display: inline-block;width:21px;height:21px'></span>");
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
                    },
                    callback: {
                        onDblClick: function (event, treeId, treeNode) {
                            if (!treeNode.isParent) {
                                VideoWeb.playOpen(treeNode.id);
                            }
                        }
                    }
                };
                var zNodes = [];
                $.fn.zTree.init(ul, setting, zNodes);
                $("#video-channelList").append(ul);
                var treeObj = $.fn.zTree.getZTreeObj("groupAndChannelTree");

                BsrCloudServer.ResourceGroup.getChannelGroup(function (msg) {
                    if (msg.Code == 0) {
                        var resourceGroupList = msg.resourceGroupList;
                        var newNodes = [];
                        for (var i = 0; i < resourceGroupList.length; i++) {
                            newNodes.push({ id: resourceGroupList[i].ResourceGroupId, pId: resourceGroupList[i].ParentResourceGroupId, name: resourceGroupList[i].ResourceGroupName, isParent: true, open: true });
                        }
                        treeObj.addNodes(null, newNodes, true);
                        treeObj.addNodes(null, { name: "未分组通道", id: 0, isParent: true, open: false }, true);
                        appendChannel();
                    } else {
                        jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                });
                function appendChannel() {
                    //获取组的根节点
                    var rootNodes = treeObj.getNodesByFilter(function (node) { return (node.level == 0 && node.name.indexOf("未分组") == -1); });
                    for (var i = 0; i < rootNodes.length; i++) {
                        //加载已分组通道
                        BsrCloudServer.ResourceGroup.getChannelListByGroupId(rootNodes[i].id, function (msg) {
                            if (msg.Code == 0) {
                                var channelList = msg.responseGroupChannelList;
                                for (var j = 0; j < channelList.length; j++) {
                                    var parentNode = treeObj.getNodesByFilter(function (node) { return (node.id == channelList[j].ResourceGroupId && node.isParent == true); }, true);
                                    var newNodes = { id: channelList[j].ChannelId, pId: channelList[j].ResourceGroupId, name: channelList[j].ChannelName, isParent: false, icon: 'Images/icons/channel.png' };
                                    treeObj.addNodes(parentNode, newNodes, true);
                                }
                            }
                            else {
                                jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            }
                        });
                    }
                    //加载未分组通道
                    BsrCloudServer.Channel.getChannelByPage(999, 0, function (msg) {
                        //获取未分组节点
                        var parentNode = treeObj.getNodesByFilter(function (node) { return (node.id == 0 && node.level == 0 && node.name.indexOf("未分组") > -1); }, true);
                        if (msg.Code == 0) {
                            var channelList = msg.groupChannelResponseList;
                            var newNodes = [];
                            for (var j = 0; j < channelList.length; j++) {
                                newNodes.push({ id: channelList[j].ChannelId, pId: channelList[j].ResourceGroupId, name: channelList[j].ChannelName, isParent: false, icon: 'Images/icons/channel.png' });
                            }
                            treeObj.addNodes(parentNode, newNodes);
                        }
                        else {
                            jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                    }, { isGroup: 1, resouceGroupId: 0 });
                }
            },
            //初始化视图模式通道
            initChannelImgView: function () {
                var selectedGroupId = $("#txtCurrentGroup").attr("data-groupid");
                jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                //获取分页通道
                var paras = { keyWord: "", isGroup: MySpace.isGroup, resouceGroupId: selectedGroupId };
                var startCount = (VideoWeb.channelCount == 0 ? 0 : (VideoWeb.pageSize * (VideoWeb.currentPage - 1)));
                BsrCloudServer.Channel.getChannelByPage(VideoWeb.pageSize, startCount, function (msg) {
                    if (msg.Code == 0) {
                        var channelList = msg.groupChannelResponseList;
                        if (!channelList.length) {
                            $("#video-channelList").append(" <div class=\"col-xs-12 col-xs-12\"><h3 class=\"text-warning\">当前组下还没有通道</h3></div>");
                        } else {
                            for (var i = 0; i < channelList.length; i++) {
                                //授权设备屏蔽修改
                                if (channelList[i].IsAuthorize == 1) {
                                    $("#template-img-type .icon-btn-edit").hide();
                                } else {
                                    $("#template-img-type .icon-btn-edit").show();
                                }
                                $("#template-img-type .channelItem").attr({ "data-channelId": channelList[i].ChannelId, "id": "channelItem" + channelList[i].ChannelId });
                                $("#template-img-type img").attr({ "src": "../channelImage/small/" + channelList[i].ImagePath, "data-src": "../channelImage/" + channelList[i].ImagePath })
                                .css({ "width": "100%", "height": "165px" });
                                $("#template-img-type .channel-item-img .channelName p").text(channelList[i].ChannelName).attr("title", channelList[i].ChannelName);
                                $("#template-img-type .channel-item-img .channelName p").ellipsis({maxWidth:120, maxLine: 1 });
                                $("#template-img-type .channel-item-img .channelName input").val(channelList[i].ChannelName);
                                $("#template-img-type").children().clone().appendTo($("#video-channelList"));
                            }
                        }
                        $.jNotify._close();
                    } else {
                        jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                }, paras);
            },
            //初始化播放窗口
            initPlayWindow: function () {
                $("#bsrWebUIDiv").children().remove();
                var bstra = $("<object>");
                bstra.css({ "width": "100%", "height": "540px" }).attr({ "id": "bsrPlay", "type": "application/x-bsrplayercloud" });
                $("#bsrWebUIDiv").append(bstra);
            },
            //编辑通道名称
            editChannelName: function (obj) {
                var channelItemDiv = $(obj).closest(".channelItem").attr("id");
                var isEdit = $("#" + channelItemDiv + " .channelName p").is(":visible");
                if (isEdit) {
                    $("#" + channelItemDiv + " .channelName p").hide();
                    $("#" + channelItemDiv + " .channelName div").show().focus();
                } else {
                    $("#" + channelItemDiv + " .channelName p").show();
                    $("#" + channelItemDiv + " .channelName div").hide();
                }
            },
            //点击播放按钮
            playButtonClick: function (obj) {
                var channleId = $(obj).closest(".channel-item-img").attr("data-channelId");
                VideoWeb.playOpen(channleId);
            },
            //放大图片
            zoomPic: function (obj) {
                var channleName = $(obj).closest(".channel-item-img").find(".channelName p").text();
                var imgSrc = $(obj).closest(".channel-item-img").find("img").attr("data-src");
                $("#myModal").find("#channelName").text(channleName);
                $("#myModal").find("img").attr("src", imgSrc);
                $("#myModal").modal();
            },
            //切换清晰度
            changeSubStream: function (subStream) {
                var curWndIndex = DPServer.Window.getCurSelectWndIndex();
                var curWndConfig = DPServer.Window.getWndConfig(curWndIndex); //param.param.RealStreamModel.SubStream
                //设置通道码流
                BsrCloudServer.Channel.updateChannelEncoderInfo(curWndConfig.param.RealStreamModel.ChannelId, subStream, function (msg) {
                    if (msg.Code == 0) { } else { }
                });
                //修改配置参数
                curWndConfig = $.extend(true, curWndConfig, { "param": { "RealStreamModel": { "SubStream": subStream}} });
                //开始播放
                DPServer.Play.playOpen(curWndConfig.param, 1);
                jNotify("清晰度切换成功！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            },
            //开始播放
            playOpen: function (channelId) {
                VideoWeb.selectedChannelId = channelId;
                BsrCloudServer.Device.getStreamerParameter(channelId, function (msg) {
                    if (msg.Code == 0) {
                        DPServer.Play.playOpen(msg.streamerParameter, 1);
                    } else {
                        jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                });
            },
            //截图
            playCapturePic: function () {
                if (DPServer.Play.playCapturePic2) {
                    var val = DPServer.Play.playCapturePic2();
                    if (val) {
                        var imageByteBase64 = JSON.parse(val);
                        BsrCloudServer.Channel.UpLoadChannelImage(VideoWeb.selectedChannelId, imageByteBase64.bdata, imageByteBase64.format, function (msg) {
                            if (msg.Code == 0) {
                                jSuccess("设置通道封面成功", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false }) ;
                            } else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            }
                        });
                    }
                } else {
                    jError("设置通道封面异常，请稍后再试！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                }
            }
        };
        $(function () {
            VideoWeb.initChannel();
            VideoWeb.initPlayWindow();
            //播放菜单
            $("#playerMenu2,#playerMenu3").find("img").hover(
            function () {
                $(this).attr("src", $(this).attr("data-active-src")).closest("a").css("cursor", "pointer");
            },
            function () {
                $(this).attr("src", $(this).attr("data-src"));
            });
            //清晰度切换按钮事件
            $("#btnSubstream").on("shown.bs.dropdown", function (e) {
                var position = $("#subStreamMenu").position();
                var iHeight = $("#subStreamMenu").css("height");
                var iframe = $("<iframe>");
                iframe.css({ "top": position.top, "left": position.left, "width": 80 + "px", "height": parseInt(iHeight) + "px" })
                            .attr({ "src": "javascript:false", "id": "ifmCover", "frameborder": "0" });
                $("#btnSubstream").append(iframe);
            });
            //清晰度切换按钮事件
            $("#btnSubstream").on('hidden.bs.dropdown', function (e) {
                $("#ifmCover").remove();
            })
            //窗口分割切换按钮事件
            $("#btnSplitScreen").on("shown.bs.dropdown", function (e) {
                var position = $("#splitScreenMenu").position();
                var iHeight = $("#splitScreenMenu").css("height");
                var iframe = $("<iframe>");
                iframe.css({ "top": position.top, "left": position.left, "width": 80 + "px", "height": parseInt(iHeight) + "px" })
                            .attr({ "src": "javascript:false", "id": "ifmCover", "frameborder": "0" });
                $("#btnSplitScreen").append(iframe);
            });
            //窗口分割切换按钮事件
            $("#btnSplitScreen").on('hidden.bs.dropdown', function (e) {
                $("#ifmCover").remove();
            })
            //当模态框弹出时隐藏播放窗口
            $("#myModal").on('show.bs.modal', function (e) {
                if ($("#bsrWebUIDiv").length) {
                    $("#bsrWebUIDiv").css({ "width": "0" });
                }
            });
            //当模态框关闭时显示播放窗口
            $("#myModal").on('hide.bs.modal', function (e) {
                if ($("#bsrWebUIDiv").length) {
                    $("#bsrWebUIDiv").css({ "width": "100%", "height": "560px" });
                }
            })
        });

        var selectedChannelId;
    </script>
</body>
</html>
