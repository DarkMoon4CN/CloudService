<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebFormTest.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Pages.WebFormTest" %>

<!DOCTYPE>
<html lang="en">
<head>
    <title>前台管理员主账户管理 </title>
    <meta charse="utf-8" />
    <meta content="IE=edge" http-equiv="X-UA-Compatible" />
    <meta content="width=device-width,initial-scale=1" name="viewport" />
    <link type="text/css" rel="stylesheet" href="../JqueryPlugins/bootstrap-3.3.0/css/bootstrap.css" />
    <script type="text/javascript" src="../JqueryPlugins/bootstrap-3.3.0/libs/html5shiv.js"></script>
    <script type="text/javascript" src="../JqueryPlugins/bootstrap-3.3.0/libs/respond.min.js"></script>
    <script type="text/javascript" src="../Scripts/jquery-1.11.1.js"></script>
    <script type="text/javascript" src="../JqueryPlugins/bootstrap-3.3.0/js/bootstrap.js"></script>
    <script src="../JqueryPlugins/jQuery.validVal-master/src/js/jquery.validVal.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jQuery.validVal-master/src/js/jquery.validVal-customValidations.js"
        type="text/javascript"></script>
    <link type="text/css" rel="stylesheet" href="../JqueryPlugins/jNotify/jNotify.jquery.css" />
    <style type="text/css">
        
    </style>
</head>
<body>
    <div>
        <form class="form-horizontal form-wrapper" id="FormCurrentLoginCostomerInfo" role="form">
        <div class="form-group">
            <label for="primaryAccontMobilePhone" class="col-xs-3 control-label">
                手机号:</label>
            <div class="col-xs-7">
                <input type="text" readonly="readonly" class="form-control" id="primaryAccontMobilePhone"
                    name="mobile" maxlength="32" value="" />
            </div>
        </div>
        </form>
    </div>
    <script type="text/javascript">

        $(function () {
            $("#FormCurrentLoginCostomerInfo").validVal();
        });

      
    </script>
</body>
</html>
