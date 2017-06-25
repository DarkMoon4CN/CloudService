<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agreement.aspx.cs" Inherits="Bsr.Cloud.WebEntry.Customer.Agreement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="col-xs-8 col-xs-offset-2">
        <p class="lead">
            “星际云”服务协议
        </p>
        <p style="text-indent: 2em">
            <strong>重要须知：</strong> 北京蓝色星际软件技术发展有限公司(下称“蓝色星际”)在此特别提醒您认真阅读、充分理解本《“星际云”服务协议》（下称《协议》）。
        </p>
        <p style="text-indent: 2em">
            您应认真阅读、充分理解本《协议》各条款，特别涉及免除或者限制海康威视责任的免责条款及对用户的权利限制条款。请您审慎阅读并判断接受或不接受本《协议》。 如您不能接受本《协议》所有条款，请不要注册、使用本服务。您的注册、登录、使用等行为将视为对本《协议》的接受，并同意接受本《协议》各项条款的约束。
        </p>
        <p style="text-indent: 2em">
            本《协议》是您（下称“用户”）与蓝色星际之间关于注册、使用“星际云”服务（下称“本服务”）所订立的协议。本《协议》描述蓝色星际与用户之间关于本服务相关方面的权利义务。
            “用户”是指注册使用本服务的个人或单位。
        </p>
        <p>
            <strong>1.“星际云”服务内容</strong></p>
        <p style="text-indent: 2em">
            “星际云”是一个为用户提供视频查看服务的平台，本身不直接产生任何内容。用户注册后，可将其监控设备接入本平台， 即可查看已接入监控设备相应场所的实时视频或历史录像。用户可根据自身需要，自行添加或删除所接入的监控设备。</p>
        <p>
            <strong>2.用户注册</strong></p>
        <p style="text-indent: 2em">
            2.1 用户可以通过网站注册使用蓝色星际提供的“星际云”服务。用户在申请使用本服务时，必须向海康威视提供准确的个人或单位资料 （包括但不限于用户名、密码、联系人、手机号码等）。用户名与密码由用户自行设定。用户申请成功后，即可登录使用。
        </p>
        <p style="text-indent: 2em">
            2.2 用户填写的信息应真实准确，如因用户信息填写错误等原因导致用户无法使用本服务，蓝色星际不承担任何责任； 用户须根据需要维护并及时更新注册信息，使之始终保持最新、完整且准确。
        </p>
        <p style="text-indent: 2em">
            2.3 用户帐号和密码由用户负责保管；用户应当对以其用户帐号进行的所有活动和事件负法律责任。若单位员工或任何第三方使用用户帐号和密码登录网站， 在确认其提供账户、密码信息准确的情况下，用户同意且蓝色星际有权视为该行为获得了用户的充分授权，该行为所产生结果直接归属于用户本身。
        </p>
    </div>
    <div class="col-xs-8 col-xs-offset-2">
        <hr />
        <label class="col-xs-4">
            <input type="checkbox" checked="checked" id="ckbAgree" />
            我已经阅读并接受协议内容
        </label>
        <button type="button" id="btnAgree" class="col-xs-2 col-xs-offset-6 btn-link" onclick="setContent('Register.aspx');return false;">
            下一步
        </button>
    </div>
    <script type="text/javascript">
        $(function () {
            $("#ckbAgree").click(function () {
                if ($("#ckbAgree").is(":checked")) {
                    $("#btnAgree").attr("disabled", false).css("col-md-1 col-md-offset-6 btn-danger");
                } else {
                    $("#btnAgree").attr("disabled", true).css("col-md-1 col-md-offset-6 btn-danger disabled");
                }
            });
        });
    </script>
</body>
</html>
