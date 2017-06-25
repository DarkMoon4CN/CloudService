<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgetPwd.aspx.cs" Inherits="Bsr.Cloud.WebEntry.CustomerReg.ForgetPwd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div class="col-xs-12">
        <form class="form-horizontal" id="updatePwdForm">
        <div class="form-group ">
            <div class="col-xs-4 col-xs-offset-4 register-tip">
                <span>通过注册手机获取验证码,输入验证码后重置新的密码.</span>
            </div>
        </div>
        <div class="form-group ">
            <label for="txtUsername" class="col-xs-2 col-xs-offset-2 control-label">
                帐号</label>
            <div class="col-xs-4">
                <input type="text" class="form-control" id="txtUsername" name="username" maxlength="64"
                    placeholder="用户名/手机号" />
            </div>
            <div class="col-xs-4">
                <button class="btn btn-link" style="padding-left: 0px" id="btnSendVerifyKey" type="button">
                    发送验证码</button>
                <label for="txtUsername" class="error">
                </label>
            </div>
        </div>
        <div class="form-group ">
            <label for="txtRePassword" class="col-xs-2 col-xs-offset-2 control-label">
                验证码</label>
            <div class="col-xs-4">
                <input type="text" id="txtRegCode" name="verfyKey" maxlength="6" class="form-control" />
            </div>
            <div class="col-xs-4">
                <button class="btn btn-link" style="padding-left: 0px" id="btnCheckVerfyKey" type="button">
                    提交</button>
                <label for="txtRegCode" class="error">
                </label>
            </div>
        </div>
        <div class="form-group ">
            <div class="col-xs-4 col-xs-offset-4 register-tip">
                <span>验证通过输入新的密码.</span>
            </div>
        </div>
        <div class="form-group opacity20">
            <label for="txtPassword" class="col-xs-2 col-xs-offset-2 control-label">
                设置新密码</label>
            <div class="col-xs-4">
                <input type="password" class="form-control" id="txtPassword" name="password" maxlength="32" />
            </div>
            <div class="col-xs-4">
                <label for="txtPassword" class="error">
                </label>
            </div>
        </div>
        <div class="form-group opacity20">
            <label for="txtRePassword" class="col-xs-2 col-xs-offset-2 control-label">
                密码确认</label>
            <div class="col-xs-4">
                <input type="password" class="form-control" id="txtRePassword" name="rePassword"
                    maxlength="32" />
            </div>
            <div class="col-xs-4 ">
                <label for="txtRePassword" class="error">
                </label>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-4 col-xs-offset-4">
                <button type="submit" class="btn btn-save col-xs-12" style="margin-top: 40px" id="btnSubmit">
                    保存</button>
            </div>
        </div>
        </form>
    </div>
    <script type="text/javascript">
        var ForgetPwd = {
            VerfyKey: "",
            initPage: function () {
                this.addValidate();
                this.addSendCode();
                $("#btnCheckVerfyKey").bind("click", function () { ForgetPwd.checkVerfyKey(); });
                if ($("input").placeholder) {
                    $("form input,form textarea").placeholder();
                }
            },
            /*------添加验证规则和提示信息-------*/
            addValidate: function () {
                $("#updatePwdForm").validate({
                    onkeyup: false,
                    rules: {
                        username: {
                            required: true,
                            isExistedUser: true
                        },
                        verfyKey: {
                            required: true,
                            verfykey: true
                        },
                        password: {
                            required: true,
                            password: true
                        },
                        rePassword: {
                            required: true,
                            equalTo: "#txtPassword"
                        }
                    },
                    messages: {
                        username: {
                            required: "请输入要找回的帐号"
                        },
                        verfyKey: {
                            required: "请输入验证码"
                        },
                        password: {
                            required: "请输入密码"
                        },
                        rePassword: {
                            required: "请重复输入密码",
                            equalTo: "密码不一致"
                        }
                    },
                    submitHandler: function () {
                        ForgetPwd.editPwd();
                    }
                });
            },
            /*------发送验证码-------*/
            addSendCode: function () {
                $('#btnSendVerifyKey').click(function () {
                    var count = 60;
                    var countdown;
                    if ($("#updatePwdForm").validate().element($("#txtUsername"))) {
                        countdown = setInterval(CountDown, 1000);
                        BsrCloudServer.Customer.sendVerifyKeyFindPwd($.trim($('#txtUsername').val()), function (msg) {
                            if (msg.Code == 1) {
                                jSuccess(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            } else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                clearInterval(countdown);
                                $('#btnSendVerifyKey').text("重新发送").removeAttr("disabled");
                            }
                        });
                    }
                    function CountDown() {
                        $('#btnSendVerifyKey').attr("disabled", true);
                        $('#btnSendVerifyKey').text(count+"秒后重新发送");
                        if (count == 0) {
                            $('#btnSendVerifyKey').text("重新发送").removeAttr("disabled");
                            clearInterval(countdown);
                        }
                        count--;
                    }
                });
            },
            //校验安全码
            checkVerfyKey: function () {
                if ($("#updatePwdForm").validate().element($("#txtUsername")) && $("#updatePwdForm").validate().element($("#txtRegCode"))) {
                    BsrCloudServer.Customer.checkVerfyKeyFindPwd($.trim($('#txtUsername').val()), $.trim($("#txtRegCode").val()), function (msg) {
                        if (msg.Code == 0) {
                            if (msg.IsValid) {
                                ForgetPwd.VerfyKey = msg.VerifyToken;
                                jSuccess("验证码校验通过！", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                                $(".opacity20").removeClass("opacity20");
                            } else {
                                jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                            }
                        } else {
                            jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                        }
                    });
                }
            },
            //重置密码
            editPwd: function () {
                function CountDown() {
                    $('#jNotifyTime').text(count);
                    if (count == 0) {
                        clearInterval(countdown);
                        window.location.href = "../Login.aspx";
                    }
                    count--;
                }
                BsrCloudServer.Customer.updateCustomerPassWord($.md5($("#txtPassword").val()), ForgetPwd.VerfyKey, function (msg) {
                    if (msg.Code == 0) {
                        var count = 10;
                        jSuccess("密码修改成功，<span id='jNotifyTime'>"+count+"</span>秒后自动返回登陆界面，或点击此链接返回登陆界面！", { TimeShown: 10000, autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center',
                            onClosed: function () {
                                clearInterval(countdown);
                                window.location.href = "../Login.aspx";
                            }
                        });
                        var countdown = setInterval(CountDown, 1000);
                    } else {
                        jError(msg.Message, { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                });
            }
        };
        $(function () {
            ForgetPwd.initPage();
        });
        
    </script>
</body>
</html>
