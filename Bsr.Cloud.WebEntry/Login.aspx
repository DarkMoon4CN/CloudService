<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Login" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>登录星际云</title>
    <link href="JqueryPlugins/bootstrap-3.3.0/css/bootstrap.min.css" rel="stylesheet"
        type="text/css" />
    <link href="JqueryPlugins/jNotify/jNotify.jquery.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Login.css" rel="stylesheet" type="text/css" />
    <link href="JqueryPlugins/mask/mask.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="container" style="padding-top: 30px">
        <div class="row">
            <div id="loginHead" class="col-xs-12">
                <div class="col-xs-2">
                    <img src="Images/Customer/log.PNG" />
                </div>
                <div class="col-xs-3 col-xs-offset-7">
                    <ul class="nav nav-pills pull-right">
                        <li><a href="#"><strong>星际空间</strong></a></li>
                        <li><a href="#"><strong>导航</strong></a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row" id="loginDiv">
            <div class="col-xs-12">
                <div class="center signIn">
                    <form class="form-horizontal">
                    <div class="form-group">
                        <label for="username" class="col-xs-2 control-label label-user">
                        </label>
                        <div class="col-xs-8">
                            <input type="text" class="form-control" id="username" name="username" placeholder="用户名/手机号/注册邮箱" />
                        </div>
                    </div>
                    <div class="form-group" style="margin-bottom: 0px">
                        <label for="username" class="col-xs-2 control-label label-key">
                        </label>
                        <div class="col-xs-8">
                            <input type="password" class="form-control" id="password" name="password" placeholder="输入密码" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-6">
                            <div class="checkbox" style="padding-left: 25px">
                                <label>
                                    <input type="checkbox" id="ckbRememberLogin">
                                    记住密码
                                </label>
                            </div>
                        </div>
                        <div class="col-xs-4" style="padding-top: 7px">
                            <a href="Customer/Frame_Register.aspx?type=forget" target="_blank">忘记密码?</a>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-12" style="height: 55px;">
                            <a href="Customer/Frame_Register.aspx?type=register" target="_blank" style="position: absolute;
                                left: 150px; bottom: 0px;">还没有帐号，赶快来这注册一个吧>></a>
                        </div>
                    </div>
                    </form>
                </div>
                <div class="center">
                    <button type="button" id="btnLogin" onclick="btnLogin_Click();" class="btn btn-save col-xs-12">
                        登录云空间</button>
                </div>
                <div id="download">
                    <div class="col-xs-2 col-xs-offset-2">
                        <a href="#" class="thumbnail">
                            <img src="Images/login/icon_Apple.png" alt="IOS下载">
                            <span>IPhone</span> </a>
                    </div>
                    <div class="col-xs-2">
                        <a href="#" class="thumbnail">
                            <img src="Images/login/icon_Android.png" alt="安卓下载">
                            <span>Android</span> </a>
                    </div>
                    <div class="col-xs-2 ">
                        <a href="#" class="thumbnail">
                            <img src="Images/login/icon_Windows.png" alt="Windows下载">
                            <span>Windows</span> </a>
                    </div>
                    <div class="col-xs-2">
                        <a href="#" class="thumbnail">
                            <img src="Images/login/icon_MaxCard.png" alt="二维码下载">
                            <span>二维码</span> </a>
                    </div>
                </div>
            </div>
        </div>
        <div class=" footer">
            <img src="Images/login/Logo_Bottom.png" />
            <span>Copyright 2008-2013 BlueSky 版权所有 </span>
        </div>
    </div>
    <script src="Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <!--[if lte IE 9]>
      <script src="JqueryPlugins/bootstrap-3.3.0/libs/html5shiv.js" type="text/javascript"></script>
      <script src="JqueryPlugins/bootstrap-3.3.0/libs/respond.min.js" type="text/javascript"></script> 
      <script src="Scripts/jquery.enplaceholder.js" type="text/javascript"></script>
    <![endif]-->
    <script src="JqueryPlugins/bootstrap-3.3.0/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="Scripts/common.js" type="text/javascript"></script>
    <script src="Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="JqueryPlugins/jNotify/jNotify.jquery.js" type="text/javascript"></script>
    <script src="Scripts/Bsr.Cloud.Server.js" type="text/javascript"></script>
    <script src="JqueryPlugins/mask/mask.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.post("PageHandler/LoginHandler.ashx", { "action": "isLogin" }, function (msg) {
                var loginInfo = $.parseJSON(msg);
                if (loginInfo.logined && loginInfo.user.token && loginInfo.user.signInType) {
                    if (loginInfo.user.signInType == 1) {//前台管理员登录
                        window.location.href = "Pages/AccountManagement_Admin.aspx?token=" + loginInfo.user.token;
                    }
                    else {
                        window.location.href = "Pages/Index.aspx?token=" + loginInfo.user.token;
                    }
                }
            });
            if ($.cookie('username') && $.cookie('password')) {
                $("#ckbRememberLogin").prop("checked", true);
                $("#username").val($.cookie('username'));
                $("#password").val("********");
            };
            $(document).bind("keypress", function () {
                if (event.keyCode == "13") {
                    $("#btnLogin").trigger("click");
                }
            });
            if ($("input").placeholder) {
                $("form input").placeholder();
            }
        });
        /*--------登录-------*/
        function btnLogin_Click() {
            var username = $.trim($("#username").val());
            var password;
            if ($("#password").val() == "********" && $.cookie('password')) {
                password = $.cookie('password');
            } else {
                password = $.trim($.md5($("#password").val()));
            }
            if (!$.trim($("#username").val()) || $("#username").val() == $("#username").attr("placeholder")) { jError("请输入用户名!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false }); $("#username").focus(); return false; }
            if (!$.trim($("#password").val()) || $("#password").val() == $("#password").attr("placeholder")) { jError("请输入密码!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false }); $("#password").focus(); return false; }
            $("body").mask("登录中...");
            BsrCloudServer.Customer.login(username, password, function (msg) {
                if (msg) {
                    if (msg && msg.Code == 0) {
                        jSuccess("账号验证成功,后台正在处理,请稍后...", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        saveCookie();
                        authentication(msg.CustomerId, msg.CustomerName, msg.CustomerToken, msg.SignInType);
                    } else {
                        $("body").unmask();
                        jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                } else {
                    $("body").unmask();
                }
            });
        }
        //保存cookie
        function saveCookie() {
            if ($("#ckbRememberLogin").is(":checked")) {
                $.cookie('username', $("#username").val(), { expires: 7 }); // 存储一个带7天期限的 cookie
                if ($("#password").val() != "********") {
                    $.cookie('password', $.md5($("#password").val()), { expires: 7 });
                }
            } else {
                $.cookie('username', '', { expires: -1 }); // 删除 cookie
                $.cookie('password', '', { expires: -1 }); // 删除 cookie
            }
        }
        //保存票据
        function authentication(id, username, token, signInType) {
            $.post("PageHandler/LoginHandler.ashx", { "action": "login", "id": id, "current": username, "token": token, "signInType": signInType }, function (e) {
                if (signInType == 1) {//前台管理员登录
                    window.location.href = "Pages/AccountManagement_Admin.aspx?token=" + token;
                }
                else {
                    window.location.href = "Pages/Index.aspx?token=" + token;
                }
            });
        }
    </script>
</body>
</html>
