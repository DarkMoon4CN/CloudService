<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManagement_Admin.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.AccountManagement_Admin" %>

<!DOCTYPE>
<html lang="en">
<head runat="server">
    <title>前台管理员主账户管理</title>
    <meta content="text/html; charset=UTF-8" http-equiv="Content-Type" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link href="../JqueryPlugins/bootstrap-3.3.0/css/bootstrap.css" rel="stylesheet"
        type="text/css" />
        <link href="../Styles/common.css" rel="stylesheet" type="text/css" />
    <script src="../JqueryPlugins/bootstrap-3.3.0/libs/html5shiv.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/bootstrap-3.3.0/libs/respond.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <!--[if lte IE 9]>
      <script src="../JqueryPlugins/bootstrap-3.3.0/libs/html5shiv.js" type="text/javascript"></script>
      <script src="../JqueryPlugins/bootstrap-3.3.0/libs/respond.min.js" type="text/javascript"></script> 
      <script src="../Scripts/jquery.enplaceholder.js" type="text/javascript"></script>
    <![endif]-->
    <script src="../JqueryPlugins/bootstrap-3.3.0/js/bootstrap.js" type="text/javascript"></script>
    <link href="../JqueryPlugins/jNotify/jNotify.jquery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Index.css" rel="stylesheet" type="text/css" />

    <link href="../JqueryPlugins/zTree_v3.5/css/zTreeStyle/metro.css" rel="stylesheet"
        type="text/css" />
    <link href="../JqueryPlugins/jquery-easyui-1.4.1/themes/default/easyui.css" rel="stylesheet"
        type="text/css" />
    <link href="../Styles/accountManagement.css" rel="stylesheet" type="text/css" />
    <script src="../JqueryPlugins/zTree_v3.5/js/jquery.ztree.all-3.5.min.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/My97DatePicker/WdatePicker.js" type="text/javascript"></script>

    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.extent.js"
        type="text/javascript"></script>
</head>
<body>
    <div>
        <div class="head-v3">
            <div class="navigation-top container">
                <div class="navbar" role="navigation">
                    <div class="container-fluid">
                        <div class="navbar-header col-xs-4">
                            <a class="navbar-brand" href="#">
                                <img alt="Brand" src="../Images/login/Logo_Bottom.png">
                            </a>
                        </div>
                        <div class=" col-xs-4" style="text-align: center">
                            <form class="navbar-form" role="search">
                            <div class="form-group">
                                <input id="MainAccountsKeyWd" type="text" class="form-control" placeholder="搜索主账户" />
                            </div>
                            <button type="button" class="btn btn-default" onclick="SearchMainAccounts();">
                                <span class="glyphicon glyphicon-search"></span>搜索
                            </button>
                            </form>
                        </div>
                        <div class=" col-xs-4">
                            <ul class="cloud-link">
                                <li ><a id="login_username" href="#" onclick="logout();return false;"></a></li>
                                <li><a href="#">星际导航</a></li>
                                <li style="border: 0"><a href="#">关于我们</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
            <div class="navigation-up">
                <div class="navigation-inner">
                    <div class="navigation-v3" style="float:right;">
                        <ul>
                            <li _t_nav="userMangement">
                                <h2>
                                    <a href="#" >用户管理</a>
                                </h2>
                            </li>
                            
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="AccountManagementDiv" class="content row">
        </div>
    </div>
    <div style="display: none;">
        <input type="hidden" id="hidCurrentID" />
        <input type="hidden" id="hidCurrentName" />
        <input type="hidden" id="hidToken" />
        <input type="hidden" id="hidSignInType" />
    </div>
    <script src="../Scripts/common.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="../Scripts/Bsr.Cloud.Server.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jNotify/jNotify.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            BsrCloudServer.authToken();
            //获取当前登陆信息
            getCurrentLogin();
            //ShowMainAccountInfo();
            setContent($("#AccountManagementDiv"), 'AccountManagement.aspx');
        });

        function getCurrentLogin() {
            var msg = BsrCloudServer.Customer.getCustomerInfo();
            if (msg.Code == 0) {
                var customer = msg.customerReponse;
                if (customer.ReceiverName == "") {
                    $("#login_username").text(customer.CustomerName);
                }
                else {
                    $("#login_username").text(customer.ReceiverName);
                }
                $("#hidCurrentName").val(customer.ReceiverName);
                $("#currentLoginAccount").html(customer.ReceiverName);
                $("#hidCurrentID").val(customer.CustomerId);
                $("#hidSignInType").val(customer.SignInType);
            }
        }

        //显示当前登陆的账户及主账户信息
        function ShowMainAccountInfo() {
            $.ajax({
                contentType: "charset=UTF-8",
                type: "post",
                cache: false,
                async: false,
                url: "AccountManagement.aspx",
                success: function (data) {
                    if ($("#hidSignInType").val() == 1) {
                        $("#AccountManagementDiv").html(data);
                    }
                    else {
                        alert("您登陆的账户不是前台管理员账户,没有权限查看此页面!");
                    }
                },
                error: function (e) { alert("error") }
            })
        }

        //请求子页面
        function setContent(obj, url, options) {
            var settings = $.extend({
                callback: null,
                isShowTip: true
            }, options || {});
            if (settings.isShowTip) {
                jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            $.post("../PageHandler/LoginHandler.ashx", { action: "isLogin" }, function (msg) {
                obj.hide().children().remove();
                var loginInfo = $.parseJSON(msg);
                if (loginInfo.logined) {
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        async: false,
                        url: globalJS.timestamp(url),
                        success: function (data) {
                            if ($("#hidSignInType").val() == 1) {
                                obj.append(data);
                                obj.fadeIn();
                                if ($.jNotify) {
                                    $.jNotify._close();
                                }
                                if (settings.callback) {
                                    settings.callback();
                                }
                            }
                            else {
                                jError("您登陆的账户不是前台管理员账户,没有权限查看此页面!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            }
                        },
                        error: function (e) { }
                    });
                } else {
                    window.location.href = "../Login.aspx";
                }
            });
        }

        //退出
        function logout() {
            $.post("../PageHandler/LoginHandler.ashx", { action: "logout" }, function (e) {
                if (confirm("确认要退出吗?")) {
                    window.location.href = "../Login.aspx";
                }
            });

        }

    </script>
</body>
</html>
