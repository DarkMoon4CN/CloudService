<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Bsr.Cloud.WebEntry.CustomerReg.Register" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>注册</title>
</head>
<body>
    <div>
        <div class="col-xs-12">
            <form class="form-horizontal" id="registerForm">
            <div class="form-group .register-tip">
                <div class="col-xs-4 col-xs-offset-4 register-tip">
                    <span>以下为必填项</span>
                </div>
            </div>
            <div class="form-group">
                <label for="txtUsername" class="col-xs-2 col-xs-offset-2 control-label">
                    用户名</label>
                <div class="col-xs-4">
                    <input type="text" class="form-control" id="txtUsername" name="username" maxlength="33" />
                </div>
                <div class="col-xs-4">
                    <label for="txtUsername" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtPassword" class="col-xs-2 col-xs-offset-2 control-label">
                    登陆密码</label>
                <div class="col-xs-4">
                    <input type="password" class="form-control" id="txtPassword" name="password" maxlength="32" />
                </div>
                <div class="col-xs-4">
                    <label for="txtPassword" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtRePassword" class="col-xs-2 col-xs-offset-2 control-label">
                    确认密码</label>
                <div class="col-xs-4">
                    <input type="password" class="form-control" id="txtRePassword" name="rePassword"
                        maxlength="32" />
                </div>
                <div class="col-xs-4">
                    <label for="txtRePassword" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtMobile" class="col-xs-2 col-xs-offset-2 control-label">
                    手机号</label>
                <div class="col-xs-4">
                    <input type="text" class="form-control" id="txtMobilephone" name="mobilephone" maxlength="32" />
                </div>
                <div class="col-xs-4">
                    <button class="btn btn-link" id="btnSendVerifyKey" type="button">
                        发送验证码</button>
                    <label for="txtMobilephone" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtRePassword" class="col-xs-2 col-xs-offset-2 control-label">
                    验证码</label>
                <div class="col-xs-4">
                    <input type="text" id="txtRegCode" name="verfyKey" class="form-control" placeholder="请输入接收到的验证码" />
                </div>
                <div class="col-xs-4">
                    <label for="txtRegCode" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <div class="col-xs-6 col-xs-offset-3 register-tip-underline">
                    <span>以下为非必填项</span>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtEmail" class="col-xs-2 col-xs-offset-2 control-label">
                    邮箱</label>
                <div class="col-xs-4">
                    <input type="text" class="form-control" id="txtEmail" name="email" maxlength="32" />
                </div>
                <div class="col-xs-4">
                    <button class="btn  btn-primary disabled" style="display: none" id="btnSendEmail"
                        type="button">
                        发送验证码</button>
                    <label for="txtEmail" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtRealName" class="col-xs-2 col-xs-offset-2 control-label">
                    真实姓名</label>
                <div class="col-xs-4">
                    <input type="text" class="form-control" id="txtRealName" name="realName" maxlength="32" />
                </div>
            </div>
            <div class="form-group ">
                <label for="txtTelphone" class="col-xs-2 col-xs-offset-2 control-label">
                    固定电话</label>
                <div class="col-xs-4">
                    <input type="text" class="form-control" id="txtTelephone" name="telephone" maxlength="14" />
                </div>
                <div class="col-xs-4">
                    <label for="txtTelephone" class="error">
                    </label>
                </div>
            </div>
            <div class="form-group ">
                <label for="txtAddress" class="col-xs-2 col-xs-offset-2 control-label">
                    地址</label>
                <div class="col-xs-4">
                    <textarea class="form-control" name="address" rows="3" id="txtAddress"></textarea>
                </div>
            </div>
            <div class="form-group">
                <div class="col-xs-4 col-xs-offset-4">
                    <button type="submit" class="btn btn-save col-xs-12" id="btnSubmit">
                        提交</button>
                </div>
            </div>
            </form>
            <a href="../Login.aspx" target="_self" class="col-xs-offset-2 text-success" style="display: none"
                id="linkRegisterTip"><small>注册成功，<span id="secSpan">10</span>s后自动返回登陆界面，或点击此链接直接返回登陆界面</small></a>
        </div>
    </div>
    <script type="text/javascript">
        var Register = {
            pageInit: function () {
                Register.addValidate();
                $("#btnSendVerifyKey").bind("click", function () {
                    Register.sendVerifyKey();
                });
                if ($("input").placeholder) {
                    $("form input,form textarea").placeholder();
                }
            },
            /*------添加验证-------*/
            addValidate: function () {
                $("#registerForm").validate({
                    onkeyup: false,
                    rules: {
                        username: {
                            required: true,
                            string: true,
                            isExistedLogname: true,
                            alnum: true,
                            symbol:true,
                            maxlength:32
                        },
                        password: {
                            required: true,
                            password: true
                        },
                        rePassword: {
                            required: true,
                            equalTo: "#txtPassword"
                        },
                        mobilephone: { required: true, mobilephone: true, isExistedLogname: true },
                        verfyKey: { required: true, verfykey: true },
                        email: { email: true, isExistedLogname: true },
                        telephone: { telephone: true }
                    },
                    messages: {
                        username: { required: "请输入用户名", maxlength: "用户名最长32个字符" },
                        password: { required: "请输入密码" },
                        rePassword: {
                            required: "请重复输入密码",
                            equalTo: "密码不一致"
                        },
                        mobilephone: { required: "请输入手机号" },
                        verfyKey: { required: "请输入验证码" },
                        email: { email: "邮箱格式不正确" }
                    },
                    submitHandler: function () {
                        Register.addCustomer();
                    }
                });
            },
            //发送验证码
            sendVerifyKey: function () {
                var count = 60;
                var countdown;
                if ($("#registerForm").validate().element($("#txtMobilephone"))) {
                    countdown = setInterval(CountDown, 1000);
                    BsrCloudServer.Customer.sendVerifyKeyRigister($.trim($('#txtMobilephone').val()), function (msg) {
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
            },
            //提交注册信息
            addCustomer: function () {
                var user = new Object();
                user.CustomerName = $.trim($("#txtUsername").val());
                user.Password = $.md5($.trim($("#txtPassword").val()));
                user.ReceiverName = $.trim($("#txtRealName").val());
                user.ReceiverEmail = $.trim($("#txtEmail").val());
                user.ReceiverCellPhone = $.trim($("#txtMobilephone").val());
                user.AccountTelephone = $.trim($("#txtTelephone").val());
                user.AccountCompanyAddress = $.trim($("#txtAddress").val());
                user.AgentType = 1;
                user.VerifyKey = $.trim($("#txtRegCode").val());
                var msg = BsrCloudServer.Customer.addPrimaryCustomer(user);
                if (msg) {
                    if (msg.Code == 0) {
                        Register.onRegisterSuccess();
                    }
                    else {
                        jError(msg.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                }
            },
            //注册成功后事件
            onRegisterSuccess: function () {
                var count = 10;
                jSuccess("密码修改成功，<span id='jNotifyTime'>" + count + "</span>秒后自动返回登陆界面，或点击此链接返回登陆界面！", { TimeShown: 10000, autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center',
                    onClosed: function () {
                        clearInterval(countdown);
                        window.location.href = "../Login.aspx";
                    }
                });
                var countdown = setInterval(CountDown, 1000);
                function CountDown() {
                    $('#jNotifyTime').text(count);
                    if (count == 0) {
                        clearInterval(countdown);
                        window.location.href = "../Login.aspx";
                    }
                    count--;
                }
            }
        };
        $(function () {
            Register.pageInit();
        });
    </script>
</body>
</html>
