<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Frame_Register.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Customer.Frame_Register" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>注册星际云</title>
    <link href="../JqueryPlugins/bootstrap-3.3.0/css/bootstrap.min.css" rel="stylesheet"
        type="text/css" />
    <link href="../JqueryPlugins/jNotify/jNotify.jquery.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/Register.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div>
        <div class="register-header">
            <div class="row">
                <div class="col-xs-12">
                    <div class="col-xs-2">
                        <img src="../Images/Customer/log.PNG" />
                    </div>
                    <div class="col-xs-3 col-xs-offset-7">
                        <ul class="nav nav-pills pull-right">
                            <li><a href="#"><strong>星际空间</strong></a></li>
                            <li><a href="#"><strong>导航</strong></a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div class="register-back">
            <a id="btnBackLogin" href="../Login.aspx" target="_self">
                <img src="../Images/login/icon_BackLogin.png" />
                返回登录 </a>
        </div>
        <div class="register-content">
        </div>
    </div>
    <script src="../Scripts/jquery-1.11.1.js" type="text/javascript"></script>
    <!--[if lte IE 9]>
      <script src="../JqueryPlugins/bootstrap-3.3.0/libs/html5shiv.js" type="text/javascript"></script>
      <script src="../JqueryPlugins/bootstrap-3.3.0/libs/respond.min.js" type="text/javascript"></script> 
      <script src="../Scripts/jquery.enplaceholder.js" type="text/javascript"></script>
    <![endif]-->
    <script src="../JqueryPlugins/bootstrap-3.3.0/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.cookie.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jNotify/jNotify.jquery.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.extent.js"
        type="text/javascript"></script>
    <script src="../Scripts/Bsr.Cloud.Server.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var type = globalJS.getUrlParam("type");
            if (type == "forget") {
                setContent("ForgetPwd.aspx");
            } else if (type == "register") {
                setContent("Agreement.aspx");
            }
        });
        //请求子页面
        function setContent(url, callback) {
            $(".register-content").hide();
            $(".register-content").children().remove();
            $.ajax({
                contentType: "charset=UTF-8",
                type: "post",
                cache: false,
                url: url,
                success: function (data) {
                    $(".register-content").children().remove();
                    $(".register-content").append(data);
                    $(".register-content").fadeIn();
                    if (callback) {
                        callback();
                    }
                },
                error: function (e) { }
            });
        }
    </script>
</body>
</html>
