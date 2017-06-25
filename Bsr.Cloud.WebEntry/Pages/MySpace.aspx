<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MySpace.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Pages.MySpace" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我的空间</title>
    <link href="../JqueryPlugins/zTree_v3.5/css/zTreeStyle/metro.css" rel="stylesheet"
        type="text/css" />
    <link href="../JqueryPlugins/popModal/popModal.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/MySpace.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="container" style="padding-top: 50px; display: none" id="emptyDiv">
        <div class="jumbotron">
            <h1>
                欢迎登陆星际云视频!</h1>
            <p>
                当前还没有添加任何设备,是否要进行添加...</p>
            <p>
                <a class="btn btn-primary btn-lg" href="#" onclick="setContent($('#content'),'ManageEquipment.aspx');"
                    role="button">进入设备管理</a></p>
        </div>
    </div>
    <div id="dataDiv" style="display: none">
        <div class="content-nav">
            <div>
                <span><a class="btn btn-link" onclick="setContent($('#content'), 'MySpace.aspx');">我的视频
                </a></span><span><a class="btn btn-link" style="border-bottom: 0px" id="btnPopModal_myGroup">
                    <span class="caret"></span></a></span><span><a class="btn btn-link" style="display: none"
                        id="txtCurrentGroup" data-groupid="0"></a></span><span style="display: none"><a class="btn btn-link">
                            现场视频</a></span>
            </div>
        </div>
        <div style="margin: 5px 5px 5px 5px">
            <div id="channelListDiv" style="min-height: 450px">
            </div>
            <div style="text-align: center; display: none;" class="col-xs-12">
                <div class="nav">
                    <ul class="pagination" id="pager">
                    </ul>
                </div>
            </div>
        </div>
        <div class="container">
        </div>
        <!--设备和通道展示模板-->
        <div id="template" style="display: none">
            <div id="template-img-type">
                <div class="col-sm-4 col-md-3">
                    <div class="channel-item-img  channelItem " data-channleid="0">
                        <div>
                            <img data-src="../channelImage/small/default.jpg" src="../channelImage/small/default.jpg" alt="通道截图" onerror="javascript:this.src='../channelImage/small/default.jpg';"   />
                            <a class="play play72" href="#" onclick="MySpace.redirectPlay(this);"></a>
                        </div>
                        <div class="channel-footer">
                            <div class="channelName col-xs-8" style="padding-left: 5px">
                                <p  style="padding-top: 5px; float:left; width:100%">
                                    通道名称</p>
                                <div class="input-group input-group-sm" style="display: none">
                                    <input type="text" class="form-control" placeholder="通道名称">
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" type="button" onclick="MySpace.saveChannelName(this);">
                                            <span class="glyphicon glyphicon-ok"></span>
                                        </button>
                                        <button class="btn btn-default" type="button" onclick="MySpace.editChannelName(this);">
                                            <span class="glyphicon glyphicon-remove"></span>
                                        </button>
                                    </span>
                                </div>
                            </div>
                            <div class="col-xs-4" style="text-align: right; padding-top: 5px">
                                <a href="#" class="icon-btn icon-btn-edit" title="修改通道名称" onclick="MySpace.editChannelName(this);return false;">
                                </a><a href="#" class="icon-btn icon-btn-zoom" title="放大通道截图" onclick="MySpace.zoomPic(this);return false;">
                                </a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="template-list-type">
                <div class="col-sm-6 col-md-6">
                    <div class="channel-item-list channelItem" data-channleid="0" style="vertical-align: bottom">
                        <div class="col-xs-8">
                            <a class="play play36 col-xs-2" href="#" onclick="MySpace.redirectPlay(this);"></a>
                            <div class="channelName col-xs-10">
                                <p style="vertical-align: middle; padding-top: 7px;float:left; width:100%">
                                    通道名称</p>
                                <div class="input-group input-group-sm " style="display: none; padding-top: 5px">
                                    <input type="text" class="form-control" placeholder="通道名称">
                                    <span class="input-group-btn">
                                        <button class="btn btn-default" type="button" onclick="MySpace.saveChannelName(this);">
                                            <span class="glyphicon glyphicon-ok"></span>
                                        </button>
                                        <button class="btn btn-default" type="button" onclick="MySpace.editChannelName(this);">
                                            <span class="glyphicon glyphicon-remove"></span>
                                        </button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-4" style="text-align: right; padding-top: 10px">
                            <a href="#" class="icon-btn icon-btn-edit" title="修改通道名称" onclick="MySpace.editChannelName(this);return false;">
                            </a><a href="#" class="icon-btn icon-btn-zoom" title="放大通道截图" onclick="MySpace.zoomPic(this);return false;">
                            </a>
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
                        <img src="" id="channelImg" style="width: 100%; height: 550px" />
                    </div>
                </div>
            </div>
        </div>
        <div style="display: none">
            <div id="myGroup">
                <li>
                    <ul id="groupTree" class="ztree">
                    </ul>
                </li>
            </div>
        </div>
    </div>
    <script src="../JqueryPlugins/zTree_v3.5/js/jquery.ztree.all-3.5.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/popModal/popModal.js" type="text/javascript"></script>
    <script type="text/javascript">
        var MySpace = {
            channelShowType: 0, //通道展示方式 0图片,1列表
            pageShowType: 0, //我的空间下当前展示界面 0通道界面展示,1播放界面展示
            isGroup: 0,
            channelTotal: 0,
            currentPage: 1,
            pageSize: 999,
            //初始化页面
            initPage: function () {
                MySpace.initGroup();
                MySpace.initChannel();
                //通过cookie获取通道展示方式
                if ($.cookie('bsrChannelShowType')) {
                    this.channelShowType = $.cookie('bsrChannelShowType');
                    if (this.channelShowType == 0) {
                        $("#btnPreview img").attr({ "src": function () { return $(this).attr("data-active-src") }, "active": true });
                    } else {
                        $("#btnListMode img").attr({ "src": function () { return $(this).attr("data-active-src") }, "active": true });
                    }
                } else {
                    $("#btnPreview img").attr({ "src": function () { return $(this).attr("data-active-src") }, "active": true });
                }
                //绑定弹出我的分组
                $('#btnPopModal_myGroup').click(function () {
                    $('#btnPopModal_myGroup').popModal({
                        html: $('#myGroup'),
                        placement: 'bottomLeft',
                        showCloseBut: true,
                        onDocumentClickClose: true,
                        onLoad: function () {
                            setTimeout(function () {
                                var position = $(".popModal").position();
                                var iWidth = $(".popModal").css("width");
                                var iHeight = $(".popModal").css("height");
                                var iframe = $("<iframe>");
                                iframe.css({ "top": position.top, "left": position.left, "width": iWidth + "px", "height": parseInt(iHeight) + "px" })
                            .attr({ "src": "javascript:false", "id": "ifmCover", "frameborder": "0" });
                                $("#btnPopModal_myGroup").parent().append(iframe);
                            }, 300);
                        },
                        onClose: function () {
                            $("#ifmCover").remove();
                        }
                    });
                });
                //绑定通道修改
                $(document).on({
                    "keypress": function (event) {
                        if (event.keyCode == "13") {//回车
                            $(this).siblings(".input-group-btn").find("button:first").trigger("click");
                        } else if (event.keyCode == "27") {//esc
                            $(this).siblings(".input-group-btn").find("button:last").trigger("click");
                        }
                    }
                }, ".channelName input[type='text']");
            },
            //初始化分组
            initGroup: function () {
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
                            switchObj.remove();
                            aObj.prepend(switchObj);
                            if (treeNode.level <= 1) {
                                var spaceStr = "<span style='display: inline-block;width:10px;'></span>";
                                aObj.prepend(spaceStr);
                            }
                            if (treeNode.level > 1) {
                                var spaceStr = "<span style='display: inline-block;width:" + (10 + spaceWidth * treeNode.level) + "px;'></span>";
                                aObj.prepend(spaceStr);
                            }
                        }
                    },
                    callback: {
                        onClick: function (event, treeId, treeNode, clickFlag) {
                            MySpace.isGroup = 1;
                            $("#txtCurrentGroup").attr("data-groupid", treeNode.id).text(treeNode.name);
                            if (MySpace.pageShowType == 0) {
                                MySpace.initChannel();
                            } else {
                                VideoWeb.initChannel();
                            }
                        }
                    }
                };
                var zNodes = [];
                $("#groupTree").children().remove();
                $.fn.zTree.init($("#groupTree"), setting, zNodes);
                var treeObj = $.fn.zTree.getZTreeObj("groupTree");
                BsrCloudServer.ResourceGroup.getChannelGroup(function (msg) {
                    if (msg.Code == 0) {
                        var resourceGroupList = msg.resourceGroupList
                        var newNodes = [];
                        for (var i = 0; i < resourceGroupList.length; i++) {
                            newNodes.push({ id: resourceGroupList[i].ResourceGroupId, pId: resourceGroupList[i].ParentResourceGroupId, name: resourceGroupList[i].ResourceGroupName, isParent: true, open: true });
                        }
                        treeObj.addNodes(null, newNodes);
                        treeObj.addNodes(null, { name: "未分组通道", id: 0, isParent: true });
                    } else {
                        jError(msg.Message + "，获取分组失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                });
            },
            //获取通道数据
            initChannel: function () {
                var selectedGroupId = parseInt($("#txtCurrentGroup").attr("data-groupid"));
                $("#pager").show();
                //获取分页通道
                var paras = { keyWord: $.trim($("#index_searchkey").val()), isGroup: MySpace.isGroup, resouceGroupId: selectedGroupId };
                var startCount = (MySpace.channelTotal == 0 ? 1 : (MySpace.pageSize * (MySpace.currentPage - 1)));
                BsrCloudServer.Channel.getChannelByPage(MySpace.pageSize, startCount, function (msg) {
                    if (msg.Code == 0) {
                        var channelList = msg.groupChannelResponseList;
                        channelList = $.grep(channelList, function (n) { return n.IsEnable == 1 });
                        MySpace.creatChannelShow(channelList);
                        if (MySpace.channelTotal == 0 || MySpace.channelTotal != msg.Total) {
                            MySpace.channelTotal = msg.Total;
                            MySpace.creatPage();
                        }
                    } else {
                        jError(msg.Message + "，获取通道失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                }, paras);
            },
            //创建通道显示
            creatChannelShow: function (channelList) {
                $("#dataDiv").show();
                $("#channelListDiv").children().remove();
                if (!channelList.length) {
                    $("#channelListDiv").append(" <div class=\"col-xs-12 col-xs-12\"><h2 class=\"text-warning\">当前组下还没有通道</h2></div>");
                } else {
                    for (var i = 0; i < channelList.length; i++) {
                        //授权设备屏蔽修改
                        if (channelList[i].IsAuthorize == 1) {
                            $("#template .icon-btn-edit").hide();
                        } else {
                            $("#template .icon-btn-edit").show();
                        }
                        if (MySpace.channelShowType == 0) {
                            $("#template-img-type img:first").attr({ "src": "../channelImage/small/" + channelList[i].ImagePath, "data-src": "../channelImage/" + channelList[i].ImagePath });
                            $("#template-img-type .channelName p").text(channelList[i].ChannelName).attr("title", channelList[i].ChannelName);
                            $("#template-img-type .channelName input[type='text']").val(channelList[i].ChannelName);
                            $("#template-img-type .channelItem").attr({ "data-channelId": channelList[i].ChannelId, "id": "channelItem" + channelList[i].ChannelId });
                            if (i < 3) {
                                $("#template-img-type img:first").css({ "width": "100%", "height": "240px" });
                                $("#template-img-type>div").attr("class", "col-sm-5 col-md-4");
                                $("#template-img-type .play").addClass("play72");
                            } else {
                                $("#template-img-type img:first").css({ "width": "100%", "height": "165px" });
                                $("#template-img-type>div").attr("class", "col-sm-4 col-md-3");
                                $("#template-img-type .play").removeClass("play72").addClass("play64");
                            }
                            $("#template-img-type").children().clone().appendTo($("#channelListDiv"));
                            $("#template-img-type .channelItem").removeAttr("id");
                            //清除浮动
                            if (i == 2) {
                                $("#channelListDiv").append("<div class=\"clearfix\"></div>");
                            }
                        } else {
                            $("#template-list-type .channelName p").text(channelList[i].ChannelName).attr("title", channelList[i].ChannelName);
                            $("#template-list-type .channelItem").attr({ "data-channelId": channelList[i].ChannelId, "id": "channelItem" + channelList[i].ChannelId, "data-src": "../channelImage/" + channelList[i].ImagePath });
                            $("#template-list-type .channelName input[type='text']").val(channelList[i].ChannelName);
                            $("#template-list-type").children().clone().appendTo($("#channelListDiv"));
                            $("#template-list-type .channelItem").removeAttr("id");
                        }
                    }
                }
                $(".channelName p").ellipsis({ maxLine: 1 });
            },
            //创建分页
            creatPage: function () {
                $("#pager").children().remove();
                var pageCount = Math.ceil(MySpace.channelTotal / MySpace.pageSize);
                MySpace.currentPage = 1;
                var prePage = $('<li><a href="#">&laquo;</a></li>').attr("data-page", "pre");
                var firstPage = $('<li class="active"><a href="#">1</a></li>').attr("data-page", 1);
                var nextPage = $('<li><a href="#">&raquo;</a></li>').attr("data-page", "next");
                $("#pager").append(prePage).append(firstPage);
                for (var i = 1; i < pageCount; i++) {
                    var page = $('<li><a href="#">' + (i + 1) + '</a></li>').attr("data-page", (i + 1));
                    $("#pager").append(page);
                }
                $("#pager").append(nextPage);
                $("#pager li:first").addClass("disabled");
                if (pageCount == 1) {
                    $("#pager li:last").addClass("disabled");
                }
                //绑定点击事件
                $("#pager li a").bind("click", function () {
                    var pageVal = $(this).closest("li").attr("data-page");
                    if (pageVal == "pre" && MySpace.currentPage != 1) {
                        MySpace.currentPage = MySpace.currentPage - 1;
                        setPageStyle();
                        MySpace.initChannel();
                    } else if (pageVal == "next" && MySpace.currentPage != pageCount) {
                        MySpace.currentPage = MySpace.currentPage + 1;
                        setPageStyle();
                        MySpace.initChannel();
                    } else if (pageVal != "pre" && pageVal != "next") {
                        MySpace.currentPage = parseInt(pageVal);
                        setPageStyle();
                        MySpace.initChannel();
                    }
                    //设置页码样式
                    function setPageStyle() {
                        $("#pager li").removeClass("active").removeClass("disabled").attr("disabled", false);
                        $("#pager li[data-page='" + MySpace.currentPage + "']").addClass("active");
                        if (pageCount == 1) {
                            $("#pager li:first,#pager li:last").attr("disabled", true).addClass("disabled");
                        } else if (MySpace.currentPage == 1) {
                            $("#pager li:first").attr("disabled", true).addClass("disabled");
                        } else if (MySpace.currentPage == pageCount) {
                            $("#pager li:last").attr("disabled", true).addClass("disabled");
                        }
                    }
                    return false;
                });
            },
            //改变显示方式
            changeShowType: function (showType) {
                MySpace.channelShowType = showType;
                $.cookie('bsrChannelShowType', showType, { expires: 7 });
                if (MySpace.pageShowType == 0) {
                    MySpace.initChannel();
                } else {
                    VideoWeb.initChannel();
                }
            },
            //跳转播放界面
            redirectPlay: function (obj) {
                MySpace.pageShowType = 1;
                $("#pager").hide();
                var channelId = $(obj).closest(".channelItem").attr("data-channelId");
                $(".content-nav div:first span:last").show();
                setContent($("#channelListDiv"), "VideoWeb.aspx", { callback: function () {
                    DPServer.Window.setWndMode(1);
                    VideoWeb.playOpen(channelId);
                }
                });
            },
            //放大图片
            zoomPic: function (obj) {
                if (MySpace.channelShowType == 0) {
                    var channleName = $(obj).closest(".channelItem").find(".channelName p").text();
                    var imgSrc = $(obj).closest(".channelItem").find("img:first").attr("data-src");
                    $("#myModal").find("#channelName").text(channleName);
                    $("#myModal").find("img").attr("src", imgSrc);
                } else {
                    var channleName = $(obj).closest(".channelItem").find(".channelName p").text();
                    var imgSrc = $(obj).closest(".channelItem").attr("data-src");
                    $("#myModal").find("#channelName").text(channleName);
                    $("#myModal").find("img").attr("src", imgSrc);
                }
                $("#myModal").modal();
            },
            //修改/取消编辑通道名称
            editChannelName: function (obj) {
                var channelItemDiv = $(obj).closest(".channelItem").attr("id");
                var isEdit = $("#" + channelItemDiv + " .channelName p").is(":visible");
                if (isEdit) {
                    $("#" + channelItemDiv + " .channelName p").hide();
                    $("#" + channelItemDiv + " .channelName div").show().find("[type='text']").focus();
                } else {
                    $("#" + channelItemDiv + " .channelName p").show();
                    $("#" + channelItemDiv + " .channelName div").hide();
                    $("#" + channelItemDiv + " .channelName div").find("[type='text']").val($("#" + channelItemDiv + " .channelName p").text());
                }
            },
            //保存通道名字
            saveChannelName: function (obj) {
                var channelItemDiv = $(obj).closest(".channelItem").attr("id");
                var channelId = $(obj).closest(".channelItem").attr("data-channelId");
                var newChannelName = $("#" + channelItemDiv + " input[type='text']").val();
                var oldChanelName = $("#" + channelItemDiv + " .channelName p").text();
                if (!newChannelName) { $("#" + channelItemDiv + " input[type='text']").focus(); return false; }
                $("#" + channelItemDiv).mask("请稍候...");
                BsrCloudServer.Channel.updateChannelName(channelId, newChannelName, function (msg) {
                    if (msg.Code == 0) {
                        $("#" + channelItemDiv + " .channelName p").text(newChannelName).attr("title", newChannelName).show();
                        $("#" + channelItemDiv + " .channelName p").ellipsis({ maxLine: 1 });
                        jSuccess("数据保存成功！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    } else {
                        $("#" + channelItemDiv + " .channelName p").show();
                        $("#" + channelItemDiv + " .channelName input[type='text']").val(oldChanelName);
                        jError(msg.Message + "，数据保存失败!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                    $("#" + channelItemDiv + " .channelName div").hide();
                    $("#" + channelItemDiv).unmask();
                });
            }
        };

        $(function () {
            MySpace.initPage();
        });
    </script>
</body>
</html>
