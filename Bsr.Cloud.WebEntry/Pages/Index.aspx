<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Pages.Index" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>星际云</title>
    <meta charse="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link href="../JqueryPlugins/bootstrap-3.3.0/css/bootstrap.css" rel="stylesheet"
        type="text/css" />
    <link href="../JqueryPlugins/jNotify/jNotify.jquery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/common.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Index.css" rel="stylesheet" type="text/css" />
    <link href="../JqueryPlugins/mask/mask.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div>
        <div class="head-v3">
            <div class="navigation-top">
                <div class="navbar" role="navigation">
                    <div class="container-fluid">
                        <div class="navbar-header col-xs-4">
                            <a class="navbar-brand" href="#">
                                <img alt="Brand" src="../Images/login/Logo_Bottom.png">
                            </a>
                        </div>
                        <div class="col-xs-4" style="text-align: center">
                            <form class="navbar-form " role="search">
                            <div class="form-group" style="position: relative; width: 300px">
                                <input type="text" class="form-control " style="width: 100%" id="index_searchkey"
                                    placeholder="搜索通道">
                                <a href="#" style="position: absolute; right: 0px; top: 0px;" id="searchByCondition">
                                    <img src="../Images/icons/icon_Search_Normal.png" /></a>
                            </div>
                            </form>
                        </div>
                        <div class="col-xs-4">
                            <ul class="cloud-link">
                                <li><a href="#"><span id="currentUser">用户</span>&nbsp;<span onclick="logout();">[退出]</span></a></li>
                                <li><a href="#">星际导航</a></li>
                                <li style="border: 0"><a href="#">关于我们</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="navigation-up">
                <div class="navigation-inner">
                    <div class="navigation-v3">
                        <ul>
                            <li _t_nav="myspace">
                                <h2>
                                    <a  data-src="MySpace.aspx">我的空间</a>
                                </h2>
                            </li>
                            <li _t_nav="cloud">
                                <h2>
                                    <a  data-src="../PagesError/building.aspx">云端数据</a>
                                </h2>
                            </li>
                            <li _t_nav="message">
                                <h2>
                                    <a  data-src="../PagesError/building.aspx">消息事件</a>
                                </h2>
                            </li>
                            <li _t_nav="setting">
                                <h2>
                                    <a>系统配置</a>
                                </h2>
                            </li>
                            <li _t_nav="server">
                                <h2>
                                    <a  data-src="../PagesError/building.aspx">服务状态</a>
                                </h2>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="navigation-down">
                <div id="setting" class="nav-down-menu menu-3 menu-1" style="display: none;" _t_nav="setting">
                    <div class="navigation-down-inner">
                        <dl style="margin-left: 380px;">
                            <dd>
                                <a class="link" id="btnManageDevice"  data-src="DeviceList.aspx">设备管理</a>
                            </dd>
                        </dl>
                        <dl>
                            <dd>
                                <a class="link" id="btnManageChannel"  data-src="ChannelManage.aspx">通道管理</a>
                            </dd>
                        </dl>
                        <dl>
                            <dd>
                                <a class="link"  id="btnManageUser" data-src="PrimaryAccountManagement.aspx">
                                    用户管理</a>
                            </dd>
                        </dl>
                        <dl>
                            <dd>
                                <a class="link"  data-src="../PagesError/building.aspx">本地配置</a>
                            </dd>
                        </dl>
                    </div>
                </div>
            </div>
        </div>
        <div class="toolBar" style="display: none">
            <div>
                <a href="#" id="btnMyGroup" title="我的分组">
                    <img src="../Images/icons/icon_MyGroup_Normal.png" />
                </a>
            </div>
            <div>
                <a href="#" id="btnMyFavorite" title="我的收藏">
                    <img src="../Images/icons/icon_MyCollection_Normal.png" />
                </a>
            </div>
        </div>
        <div class="content row" id="content">
        </div>
        <div class="tools">
            <div class="tool-group tool-group-mode">
                <div>
                    <a href="#" id="btnPreview" title="视图方式">
                        <img src="../Images/icons/icon_Preview_Normal.png" data-src="../Images/icons/icon_Preview_Normal.png"
                            data-active-src="../Images/icons/icon_PreviewMode_Hover.png" active />
                    </a>
                </div>
                <div>
                    <a href="#" id="btnListMode" title="列表方式">
                        <img src="../Images/icons/icon_ListMode_Normal.png" data-src="../Images/icons/icon_ListMode_Normal.png"
                            data-active-src="../Images/icons/icon_ListMode_Selected.png" />
                    </a>
                </div>
            </div>
            <div>
                <a href="#" class="tool tool-top" id="btnTop">
                    <img src="../Images/icons/icon_BackTop_Normal.png" />
                </a>
            </div>
        </div>
        <div class="navbar navbar-default navbar-fixed-bottom" style="display: none;" role="navigation">
            <div class="container-fluid">
                <p class="text-muted">
                    <span>当前登录用户数量:</span> <span id="onlineCount">1</span>
                </p>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidCurrentID" />
        <input type="hidden" id="hidCurrentName" />
        <input type="hidden" id="hidSignInType" />
    </div>
    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <!--[if lte IE 9]>
      <script src="../JqueryPlugins/bootstrap-3.3.0/libs/html5shiv.js" type="text/javascript"></script>
      <script src="../JqueryPlugins/bootstrap-3.3.0/libs/respond.min.js" type="text/javascript"></script>
      <script src="../Scripts/jquery.enplaceholder.js" type="text/javascript"></script>
    <![endif]-->
    <script src="../JqueryPlugins/bootstrap-3.3.0/js/bootstrap.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jNotify/jNotify.jquery.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/mask/mask.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery.ellipsis/jquery.ellipsis.js" type="text/javascript"></script>
    <script src="../Scripts/Bsr.Cloud.Server.js" type="text/javascript"></script>
    <script src="../Scripts/Bsr.Cloud.DPserver.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initPage();
        });
        //初始化界面
        function initPage() {
            $(document).keypress(globalJS.rmBackspaceE);
            $(document).keydown(globalJS.rmBackspaceE);
            //一级菜单鼠标滑动
            var qcloud = {};
            $("[_t_nav]").hover(function () {
                var _nav = $(this).attr('_t_nav');
                clearTimeout(qcloud[_nav + '_timer']);
                qcloud[_nav + '_timer'] = setTimeout(function () {
                    $('[_t_nav]').each(function () {
                        $(this)[_nav == $(this).attr('_t_nav') ? 'addClass' : 'removeClass']('nav-up-selected');
                    });
                    $('#' + _nav).stop(true, true).slideDown(200);
                }, 150);
            }, function () {
                var _nav = $(this).attr('_t_nav');
                clearTimeout(qcloud[_nav + '_timer']);
                qcloud[_nav + '_timer'] = setTimeout(function () {
                    $('[_t_nav]').removeClass('nav-up-selected');
                    $('#' + _nav).stop(true, true).slideUp(200);
                }, 150);
            });
            //菜单点击
            $("[_t_nav] a").bind("click", function () {
                if ($(this).attr("data-src")) {
                    if ($(this).attr("data-permission") && $(this).attr("data-permission") == "true") {
                        setContent($("#content"), $(this).attr("data-src"));
                    } else {
                        setContent($("#content"), "../PagesError/stop.aspx");
                    }
                }
            });
            //展示方式切换
            $("#btnPreview").bind("click", function () {
                MySpace.changeShowType(0);
                $(this).closest(".tool-group-mode").find("img").each(function () { $(this).attr("src", function () { $(this).removeAttr("active"); return $(this).attr("data-src"); }) });
                $(this).find("img").attr({ "src": function () { return $(this).attr("data-active-src") }, "active": true });
            }).hover(function () {
                $(this).find("img").attr("src", "../Images/icons/icon_PreviewMode_Hover.png");
            }, function () {
                if (!$(this).find("img").attr("active")) {
                    $(this).find("img").attr("src", "../Images/icons/icon_Preview_Normal.png");
                }
            });
            $("#btnListMode").bind("click", function () {
                MySpace.changeShowType(1);
                $(this).closest(".tool-group-mode").find("img").each(function () { $(this).attr("src", function () { $(this).removeAttr("active"); return $(this).attr("data-src"); }) });
                $(this).find("img").attr({ "src": function () { return $(this).attr("data-active-src") }, "active": true });
            }).hover(function () {
                $(this).find("img").attr("src", "../Images/icons/icon_ListMode_Horver.png");
            }, function () {
                if (!$(this).find("img").attr("active")) {
                    $(this).find("img").attr("src", "../Images/icons/icon_ListMode_Normal.png");
                }
            });
            $("#btnMyGroup").hover(function () {
                $(this).find("img").attr("src", "../Images/icons/icon_MyGroup_Hover.png");
            }, function () {
                $(this).find("img").attr("src", "../Images/icons/icon_MyGroup_Normal.png");
            });
            $("#btnMyFavorite").hover(function () {
                $(this).find("img").attr("src", "../Images/icons/icon_MyCollection_Hover.png");
            }, function () {
                $(this).find("img").attr("src", "../Images/icons/icon_MyCollection_Normal.png");
            });
            //点击左侧我的分组
            $("#btnMyGroup").bind("click", function () {
                setContent($("#content"), "MySpace.aspx");
            });
            //绑定页头的搜索
            $("#searchByCondition").bind("click", function () {
                var current = $(".navigation-up a .active,.navigation-down a .active").attr("data-src");
                if (current != "MySpace.aspx") {
                    setContent($("#content"), "MySpace.aspx", { callback: function () {
                        MySpace.initChannel();
                    }
                    });
                } else {
                    MySpace.initChannel();
                }
                return false;
            });
            BsrCloudServer.authToken();
            getCurrentLogin();
            getOnlineCount();
        }
        //获取当前登陆信息
        function getCurrentLogin() {
            var msg = BsrCloudServer.Customer.getCustomerInfo();
            if (msg.Code == 0) {
                var customer = msg.customerReponse;
                $("#currentUser").text(customer.ReceiverName ? customer.ReceiverName : customer.CustomerName);
                $("#hidCurrentName").val(customer.CustomerName);
                $("#hidCurrentID").val(customer.CustomerId);
                $("#hidSignInType").val(customer.SignInType);
                if (customer.SignInType == 2) {//主账户登陆
                    $("[_t_nav] a").attr("data-permission", true);
                    $("#btnManageUser").attr("data-src", "PrimaryAccountManagement.aspx");
                }
                else if (customer.SignInType == 3) {//子账户登陆
                    $("[_t_nav] a").attr("data-permission", false);
                    $("[_t_nav='setting'] a").attr("data-permission", true);
                    BsrCloudServer.Customer.getSelfPermission(function (msg) {
                        if (msg.Code == 0) {
                            var permissionList = msg.customerPermission;
                            for (var i = 0; i < permissionList.length; i++) {
                                if (permissionList[i].PermissionName == "MySpace" && permissionList[i].IsEnable) {
                                    $("[_t_nav='myspace'] a").attr("data-permission", true);
                                }
                            }
                        }
                    });
                    $("#btnManageUser").attr("data-src", "SubAccountManagement.aspx");
                    $("#btnManageDevice,#btnManageChannel").hide().attr("data-permission", false); ;
                }
                if ($("[_t_nav] a[data-permission=true][data-src]").length != 0) {
                    $("[_t_nav] a[data-permission=true][data-src]:first").trigger("click");
                } else {
                    $("[_t_nav] a[data-src]:first").trigger("click");
                }
            } else {
                logout();
            }
        }
        //获取在线人数
        function getOnlineCount() {
            BsrCloudServer.Customer.getCustomerOnlineTotal(function (msg) {
                if (msg.Code == 0) {
                    $("#onlineCount").text(msg.tokenCachePropertyList.length);
                }
            });
        }
        //请求子页面
        function setContent(obj, url, options) {
            var settings = $.extend({
                callback: null
            }, options || {});
            //请求子页面
            $.post("../PageHandler/LoginHandler.ashx", { action: "isLogin" }, function (msg) {
                obj.hide().children().remove();
                var loginInfo = $.parseJSON(msg);
                if (loginInfo.logined) {
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: globalJS.timestamp(url), //"MySpace.aspx"
                        success: function (data) {
                            if (url == "MySpace.aspx" || url == "VideoWeb.aspx") {
                                $(".tool-group-mode").show();
                            } else {
                                $(".tool-group-mode").hide();
                            }
                            obj.append(data);
                            obj.fadeIn("normal", function () {
                                if (settings.callback) {
                                    settings.callback();
                                }
                            });
                        },
                        error: function (e) { }
                    });
                } else {
                    window.location.href = "../Login.aspx";
                }
            });
        }
        //移除子页面
        function removeContent(obj) {
            obj.fadeOut("normal", function () {
                obj.children().remove();
            });
        }

        //退出
        function logout() {
            $.post("../PageHandler/LoginHandler.ashx", { action: "logout" }, function (e) { window.location.href = "Login.aspx" });
        }
    </script>
</body>
</html>
