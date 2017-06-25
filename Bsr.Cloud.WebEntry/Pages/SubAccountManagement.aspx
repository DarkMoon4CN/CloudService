<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubAccountManagement.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.SubAccountManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/accountManagement.css" rel="stylesheet" type="text/css" />
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.extent.js"
        type="text/javascript"></script>
    <script src="../JqueryPlugins/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body>
    <div>
        <div>
            <div class="col-xs-10">
                <h4 style="padding-top: 10px;">
                    账户管理
                </h4>
            </div>
        </div>
        <div class="content-nav">
        </div>
        <div style="margin-top: 70px;">
            <div class="col-xs-12">
                <div class="col-xs-6 col-xs-offset-3">
                    <div class="accountBaseinfo">
                        <a id="loadCurrentSubAccountDiv_a" href="#" onclick="LoadCurrentSubAccountDiv();return false;">
                            账户基本信息</a>
                    </div>
                    <div class="accountSafeinfo">
                        <a id="loadCurrentSubAccountSafeDiv_a" href="#" onclick="LoadCurrentSubAccountSafeDiv(this);return false;">
                            账户安全设置</a>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="CurrentSubAccountInfoDiv" style="margin-top: 15px; display: none;">
                <div class="divcss">
                    <div class="col-xs-3 divimg">
                        <img class="img-circle" id="subAccountImg" src="" style="width: 140px; height: 140px;
                            margin-left: 30px; border: 1px solid #f1f1f1;" onerror="errorImg(this)" />
                    </div>
                    <div class="col-xs-9 divuserinfo">
                        <form class="form-horizontal" id="FormCurrentLoginsubAccountInfo" role="form">
                        <div class="form-group">
                            <label for="subAccontMobilePhone" class="col-xs-3 control-label">
                                手机号:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="subAccontMobilePhone"
                                    name="mobile" maxlength="16" value="" />
                            </div>
                            <div class="col-xs-4">
                                <label for="subAccontMobilePhone" class="error">
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="subAccountEmail" class="col-xs-3 control-label">
                                邮箱:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="subAccountEmail"
                                    name="emial" maxlength="64" value="" />
                            </div>
                            <div class="col-xs-4">
                                <label for="subAccountEmail" class="error">
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="subAccountRealName" class="col-xs-3 control-label">
                                真实姓名:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="subAccountRealName"
                                    name="realname" maxlength="32" value="" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="subAccountTelphone" class="col-xs-3 control-label">
                                固定电话:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="subAccountTelphone"
                                    name="telphone" maxlength="14" value="" />
                            </div>
                            <div class="col-xs-4">
                                <label for="subAccountTelphone" class="error">
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="subAccountAddress" class="col-xs-3 control-label">
                                地址:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="subAccountAddress"
                                    name="address" maxlength="512" value="" />
                            </div>
                        </div>
                        </form>
                    </div>
                    <div style="float: right; margin-right: 20px; padding-top: 15px;">
                        <a href="#" onclick="EditCurrentSubAccountInfo();return false;">
                            <img src="../Images/icons/icon_Edit_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Edit_Hover.png'"
                                title="修改" onmouseout="this.src='../Images/icons/icon_Edit_Normal.png'" />
                        </a><a href="#" onclick="ShowCurrentSubAccountHistoryInfo();return false;" style="margin: 5px;">
                            <img src="../Images/icons/icon_History_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_History_Hover.png'"
                                title="历史登录信息" onmouseout="this.src='../Images/icons/icon_History_Normal.png'" />
                        </a>
                    </div>
                </div>
            </div>
            <div class="col-md-12" id="CurrentPriamryAccountSafeInfoDiv" style="display: none;">
                <div class="divsafecss">
                    <form class="form-horizontal" role="form">
                    <div class="form-group" style="line-height: 20px;">
                        <label class="col-xs-3 control-label">
                            账户有效期</label>
                        <label id="subAccontValid" class="col-xs-3" style="margin-top: 7px; font-weight: normal;
                            margin-left: 26px;">
                        </label>
                    </div>
                    <div class="form-group" style="line-height: 25px;">
                        <label class="col-xs-3 control-label">
                            用户登陆终端</label>
                        <div class="col-xs-6">
                            <div id="currentSubAccountLoginWay" style="width: 344px;">
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="subPhoneClient" name="chkSubAccountLoginWay" value="option1" />
                                    手机客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="subBSPhoneClient" name="chkSubAccountLoginWay" value="option2" />
                                    B/S客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="subCSClient" name="chkSubAccountLoginWay" value="option3" />
                                    C/S客户端
                                </label>
                            </div>
                        </div>
                        <div class="col-xs-2 " style="padding-top: 7px; position: relative;">
                            <a href="#" id="editSubAccountSafeInfo_a" onclick="EditSubAccountSafeInfo(this);return false;"
                                style="position: absolute; left: 20px; color: #808080;">修改</a><a style="display: none;
                                    color: ;" id="cancleEditSubAccountSafeInfo" href="#" onclick="CancleEditSubAccountSafeInfo(this);return false;">
                                    取消</a>
                        </div>
                    </div>
                    <div class="form-group" style="line-height: 30px;">
                        <label class="col-md-3 control-label">
                            登陆密码</label>
                        <label class="col-md-2" style="margin-top: 7px; font-weight: normal;">
                            ******
                        </label>
                        <div class="col-xs-1 col-xs-offset-4" style="padding-top: 7px; margin-left: 263px;">
                            <a href="#" onclick="ResetSubAccountPassWd();return false;" style="color: #808080;">
                                修改</a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="divbtn">
                    <div id="saveCurrentSubAccountInfo_divId" style="display: none;">
                        <button class="btn btn-default" style="width: 120px;" onclick="javascript:$('#FormCurrentLoginsubAccountInfo').submit();return false;">
                            保存
                        </button>
                        <button class="btn btn-default" style="width: 120px;" onclick="CancelSaveCurrentSubAccountInfo();return false;">
                            取消
                        </button>
                    </div>
                    <div id="saveSubPromptMessage" style="margin-top: 10px; color: #30c5fc; display: none;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 图片上传 -->
    <div class="modal fade bs-example-modal-sm" id="modelSubUpfileWin" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        图片选择</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <input type="file" id="InputCurrentSubFile" name="InputCurrentSubFile" accept="image/*"
                            size="12" style="float: left; height: 22px; width: 275px; line-height: 20px;" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn" data-dismiss="modal" onclick="btnUpdatePrimaryOfSubImage();return false;">
                        上传</button>
                </div>
            </div>
        </div>
    </div>
    <!-- 重置密码 -->
    <div class="modal fade bs-example-modal-sm" id="editSubAccontPassWdModal" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" style="width: 560px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title">
                        重置密码</h4>
                </div>
                <div class="modal-body">
                    <form class="form-horizontal" id="FromSubAccontPassWd" role="form">
                    <div class="form-group">
                        <label for="OriginalSubAccontPassWd" class="col-md-3 control-label">
                            原始密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="OriginalSubAccontPassWd" name="OriginalSubAccontPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="OriginalSubAccontPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="NewSubAccontPassWd" class="col-md-3 control-label">
                            新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="NewSubAccontPassWd" name="NewSubAccontPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="NewSubAccontPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ConfirmSubAccontPassword" class="col-md-3 control-label">
                            确认新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="ConfirmSubAccontPassword" name="ConfirmSubAccontPassword"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="ConfirmSubAccontPassword" class="error">
                            </label>
                        </div>
                    </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn" onclick="javascript:$('#FromSubAccontPassWd').submit();return false;">
                        保存</button>
                </div>
            </div>
        </div>
    </div>
    <!-- 账户历史登陆信息页面 -->
    <div class="modal fade bs-example-modal-sm" id="subAccoutHitoryModal" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" style="width: 680px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="mySubModalLabel">
                        历史登陆信息</h4>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered" id="logSubTable">
                        <tr>
                            <th>
                                登陆时间
                            </th>
                            <th>
                                登陆终端
                            </th>
                            <th>
                                登陆终端版本
                            </th>
                        </tr>
                    </table>
                </div>
                <div class="modal-footer">
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">
                        返回</button>--%>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidSubImageBase64Code" />
        <input type="hidden" id="hideSubImgExt" />
        <input type="hidden" id="hidSubAccontPhone" />
        <input type="hidden" id="hidSubAccontEmail" />
    </div>
    <script src="../Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="../Scripts/ajaxfileupload.js" type="text/javascript"></script>
    <script type="text/javascript">
        var FormCurrentLoginsubAccountInfo_Validator = null;
        $(function () {
            //LoadCurrentSubAccountDiv();
            BsrCloudServer.Customer.CheckUpdate();
        });

        //加载当前登陆子账户的基本信息
        function LoadCurrentSubAccountDiv() {
            EnableOrDisableCurrentSubAccountBaseInfo(false);
            $("#loadCurrentSubAccountSafeDiv_a").css({
                "background-image": "url(../Images/icons/icon_UserSafe_Normal.png)",
                "background-repeat": "no-repeat",
                "color": ""
            });
            $("#loadCurrentSubAccountDiv_a").css({
                "background-image": "url(../Images/icons/icon_EnableUser_Normal.png)",
                "background-repeat": "no-repeat",
                "color": "#30c5fc"
            });
            GetSelfAccountInfo();
        }

        //获取当前登陆用户的信息
        function GetSelfAccountInfo() {
            var result = BsrCloudServer.Customer.getCustomerInfo(0);
            if (result && result.Code == 0) {
                $("#subAccontMobilePhone").val(result.customerReponse.ReceiverCellPhone);
                $("#subAccountEmail").val(result.customerReponse.ReceiverEmail);
                $("#subAccountRealName").val(result.customerReponse.ReceiverName);
                $("#subAccountTelphone").val(result.customerReponse.AccountTelephone);
                $("#subAccountAddress").val(result.customerReponse.AccountCompanyAddress);
                $("#subAccountName").text(result.customerReponse.CustomerName);
                var path = "../" + result.customerReponse.ImagePath;
                $("#subAccountImg").attr("src", path);
                if ($("#subAccountImg").height <= 0) {
                    this.src = "../customerImage/default.jpg";
                }

                $("#hidSubAccontPhone").val(result.customerReponse.ReceiverCellPhone);
                $("#hidSubAccontEmail").val(result.customerReponse.ReceiverEmail);
            }
            else {
                $("#saveSubPromptMessage").show(500, function () {
                    $("#saveSubPromptMessage").text("获取登陆信息失败");
                });
            }
        }

        //启用或者禁用当前登陆账户的基本信息
        function EnableOrDisableCurrentSubAccountBaseInfo(obj) {
            $("#CurrentSubAccountInfoDiv").show();
            $("#CurrentPriamryAccountSafeInfoDiv").hide();
            $("#saveSubPromptMessage").hide();
            if (obj) {//启用
                $("#FormCurrentLoginsubAccountInfo input").removeAttr("disabled");
                $("#subAccountImg").bind("click", ShowUpdateSubFilePage);
                $("#btnPriamryAccountGroupDiv").hide();
                $("#btnSavePriamryAccountBaseInfoDiv").show();
            }
            else {
                $("#FormCurrentLoginsubAccountInfo input").attr("disabled", "disabled");
                $("#subAccountImg").unbind("click", ShowUpdateSubFilePage);
                $("#btnPriamryAccountGroupDiv").show();
                $("#btnSavePriamryAccountBaseInfoDiv").hide();
            }
        }

        //显示上传文件界面
        function ShowUpdateSubFilePage() {
            $("#modelSubUpfileWin").modal('show');
        }

        //修改当前登陆子账户的基本信息
        function EditCurrentSubAccountInfo() {
            EnableOrDisableCurrentSubAccountBaseInfo(true);
            $("#saveCurrentSubAccountInfo_divId").show();
            easyuiValidateSub();
        }

        //撤销修改子账户的基本信息
        function CancelSaveCurrentSubAccountInfo() {
            if (FormCurrentLoginsubAccountInfo_Validator) {
                FormCurrentLoginsubAccountInfo_Validator.resetForm();
            }
            EnableOrDisableCurrentSubAccountBaseInfo(false);
            $("#saveCurrentSubAccountInfo_divId").hide();
            GetSelfAccountInfo();
        }

        //添加当前登陆子账户的基本信息 验证
        function easyuiValidateSub() {
            if (FormCurrentLoginsubAccountInfo_Validator) {
                FormCurrentLoginsubAccountInfo_Validator.resetForm();
            }
            FormCurrentLoginsubAccountInfo_Validator = $("#FormCurrentLoginsubAccountInfo").validate({
                onkeyup: false,
                rules: {
                    mobile: { required: true, mobilephone: true, isExistedLognameByEdit: [true, '0'] },
                    emial: { email: true, isExistedLognameByEdit: [true, '1'] },
                    telphone: { telephone: true }
                },
                messages: {
                    mobile: { required: "请输入手机号" },
                    emial: { email: "邮箱格式不正确" }
                },
                submitHandler: function () {
                    SaveCurrentSubAccountInfo();
                }
            });
        }

        //查看当前子账户的登陆日志
        function ShowCurrentSubAccountHistoryInfo() {
            $("#mySubModalLabel").html($("#subAccountName").text() + " 历史登陆信息");
            $("#subAccoutHitoryModal").modal('show');
            $("#logSubTable tr:gt(0)").empty();
            var result = BsrCloudServer.Customer.GetSelfLoginInfo();
            if (result && result.Code == 0) {
                if (result.operaterLogList.length > 0) {
                    for (var i = 0; i < result.operaterLogList.length; i++) {
                        var tr = "<tr><td>" + result.operaterLogList[i].OperaterTime.toString() + "</td><td>" + result.operaterLogList[i].AgentType +
                         "</td><td>" + result.operaterLogList[i].AgentVersion + "</td></tr>";
                        $("#logSubTable").append(tr);
                    }
                }
            }
        }

        //保存当前子账户基本信息
        function SaveCurrentSubAccountInfo() {
            var result = updateSubAccountBaseInfo();
            if (result && result.Code == 0) {
                EnableOrDisableCurrentSubAccountBaseInfo(false);
                $("#saveCurrentSubAccountInfo_divId").fadeOut(1000, function () {
                    $("#saveSubPromptMessage").show();
                    $("#saveSubPromptMessage").text("保存成功");
                });
            }
            else {
                $("#saveSubPromptMessage").show();
                $("#saveSubPromptMessage").text("保存失败");
            }
        }

        //更新当前主账户的基本信息
        function updateSubAccountBaseInfo() {
            var result = false;
            var user = new Object();
            user.ReceiverCellPhone = $.trim($("#subAccontMobilePhone").val());
            user.ReceiverEmail = $.trim($("#subAccountEmail").val());
            user.ReceiverName = $.trim($("#subAccountRealName").val());
            user.AccountTelephone = $.trim($("#subAccountTelphone").val());
            user.AccountCompanyAddress = $.trim($("#subAccountAddress").val());
            user.ImageByteBase64 = "";
            user.ExtName = "";
            var userJson = JSON.stringify(user);
            return BsrCloudServer.Customer.updateCustomer(userJson);
        }

        //加载当前登陆子账户的安全信息
        function LoadCurrentSubAccountSafeDiv(obj) {
            if (obj) {
                EnableOrDisableSubAccountSafeInfo(false);
                $("#loadCurrentSubAccountSafeDiv_a").css({
                    "background-image": "url(../Images/icons/icon_UserSafe_Hover.png)",
                    "background-repeat": "no-repeat",
                    "color": "#30c5fc"
                });
                $("#loadCurrentSubAccountDiv_a").css({
                    "background-image": "url(../Images/icons/icon_Account_Normal.png)",
                    "background-repeat": "no-repeat",
                    "color": ""
                });
                $("#editSubAccountSafeInfo_a").text("修改").css("color", "#808080");
                $("#cancleEditSubAccountSafeInfo").hide();
                GetSelfSafeInfo();
            }
        }

        //启用或者禁用子账户的安全信息
        function EnableOrDisableSubAccountSafeInfo(obj) {
            $("#subAccountImg").unbind("click", ShowUpdateSubFilePage);
            $("#CurrentSubAccountInfoDiv").hide();
            $("#CurrentPriamryAccountSafeInfoDiv").show();
            $("#saveSubPromptMessage").hide();
            $("#saveCurrentSubAccountInfo_divId").hide();
            if (obj) {//启用
                $("[name='chkSubAccountLoginWay']").removeAttr("disabled");
            } else {
                $("[name='chkSubAccountLoginWay']").attr("disabled", "disabled");
            }
        }

        //获取当前登陆子账户的安全信息
        function GetSelfSafeInfo() {
            var result = BsrCloudServer.Customer.getCustomerInfo(0);
            if (result && result.Code == 0) {
                if (result.customerReponse.ValidTime == null || result.customerReponse.ValidTime == "") {
                    $("#subAccontValid").text("永久有效");
                } else {
                    var time = new Date(result.customerReponse.ValidTime);
                    var dateTime = time.Format("yyyy-MM-dd hh:mm:ss");
                    $("#subAccontValid").text("截止" + dateTime);
                }
                $("[name='chkSubAccountLoginWay']").prop("checked", false);
                if (result.customerReponse.LoginTypes.indexOf("3") >= 0 || result.customerReponse.LoginTypes.indexOf("4") >= 0) {
                    $("#subPhoneClient").prop("checked", true);
                }
                if (result.customerReponse.LoginTypes.indexOf("1") >= 0) {
                    $("#subBSPhoneClient").prop("checked", true);
                }
                if (result.customerReponse.LoginTypes.indexOf("2") >= 0) {
                    $("#subCSClient").prop("checked", true);
                }
            }
            else {
                jError("获取安全信息失败", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                $("#saveSubPromptMessage").show(500, function () {
                    $("#saveSubPromptMessage").text("获取安全信息失败");
                });
            }
        }

        function CancleEditSubAccountSafeInfo(obj) {
            if (obj) {
                EnableOrDisableSubAccountSafeInfo(false);
                $(obj).hide();
                $("#editSubAccountSafeInfo_a").text("修改").css("color", "#808080");
            }
        }

        //修改当前登陆子账户的安全信息
        function EditSubAccountSafeInfo(obj) {
            if ($.trim($(obj).text()) == "修改") {
                EnableOrDisableSubAccountSafeInfo(true);
                $(obj).text("保存").css("color", "");
                $("#cancleEditSubAccountSafeInfo").show();
                $("#cancleEditSubAccountSafeInfo").css("color", "");
            }
            else {
                SaveSubAccountSafeInfo(obj);
            }
        }

        //撤销修改子账户的安全信息
        function CancelSaveSubAccountSafeInfo() {
            EnableOrDisableSubAccountSafeInfo(false);
        }

        //保存当前子账户的安全信息
        function SaveSubAccountSafeInfo(obj) {
            var loginway = "";
            if ($("#subPhoneClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "3,4";
                }
                else {
                    loginway += ",3,4";
                }
            }
            if ($("#subBSPhoneClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "1";
                }
                else {
                    loginway += ",1";
                }
            }
            if ($("#subCSClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "2";
                }
                else {
                    loginway += ",2";
                }
            }
            if ("" == loginway) {
                if (!confirm("如果您不选择登陆终端,您将无法使用该账户登陆!")) {
                    return;
                }
            }
            var result = BsrCloudServer.Customer.UpdateCustomerSafeInfo(loginway);
            if (result && result.Code == 0) {
                EnableOrDisableSubAccountSafeInfo(false);
                $("#saveSubPromptMessage").show();
                $("#cancleEditSubAccountSafeInfo").hide();
                $(obj).text("修改").css("color", "#808080");
                $("#savePromptMessage").text("保存成功");
            }
            else {
                $("#saveSubPromptMessage").show();
                $("#saveSubPromptMessage").text("保存失败");
            }
        }

        //重置当前登陆子账户的密码
        function ResetSubAccountPassWd() {
            $("#editSubAccontPassWdModal").modal('show');
            easyuiSubPassWdValidate();
            $("#OriginalSubAccontPassWd").val("");
            $("#NewSubAccontPassWd").val("");
            $("#ConfirmSubAccontPassword").val("");
        }

        //子账户修改密码验证
        function easyuiSubPassWdValidate() {
            var FromPrimaryAccontPassWd_Validator = $("#FromSubAccontPassWd").validate({
                onkeyup: false,
                rules: {
                    OriginalSubAccontPassWd: {
                        required: true,
                        password: true
                    },
                    NewSubAccontPassWd: {
                        required: true,
                        password: true
                    },
                    ConfirmSubAccontPassword: {
                        required: true,
                        equalTo: "#NewSubAccontPassWd"
                    }
                },
                messages: {
                    OriginalSubAccontPassWd: { required: "请输入密码" },
                    NewSubAccontPassWd: { required: "请输入密码" },
                    ConfirmSubAccontPassword: {
                        required: "请重复输入密码",
                        equalTo: "密码不一致"
                    }
                },
                submitHandler: function () {
                    btnSaveSubAccontPassWd();
                }
            });
            FromPrimaryAccontPassWd_Validator.resetForm();
        }

        //重置密码
        function btnSaveSubAccontPassWd() {
            var result = SaveSubAccountPassWord();
            if (result && result.Code == 0) {
                $("#editSubAccontPassWdModal").modal('hide');
                jSuccess("密码修改成功", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            } else {
                jError("密码修改失败:" + result.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        //保存密码 
        function SaveSubAccountPassWord() {
            var userPassWd = new Object();
            userPassWd.OldPassWord = $.md5($.trim($("#OriginalSubAccontPassWd").val()));
            userPassWd.NewPassWord = $.md5($.trim($("#NewSubAccontPassWd").val()));
            return BsrCloudServer.Customer.SaveAccountPassWord(userPassWd);
        }

        //图片上传
        function btnUpdatePrimaryOfSubImage() {
            readSubImageURLAndSave($("#InputCurrentSubFile")[0]);
        }

        //读取图片的URL并保存图片
        function readSubImageURLAndSave(input) {
            if ($("#InputCurrentSubFile").val() == "") {
                $("#saveSubPromptMessage").show(500, function () {
                    $("#saveSubPromptMessage").text("上传图片不能为空");
                });
                return;
            }
            var settings = $.extend({
                callback: null,
                isShowTip: true
            });
            if (settings.isShowTip) {
                jNotify("上传图片中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            if (input.files && input.files[0]) {
                var filename = input.files[0].name
                var imgexts = filename.split(".");
                var imgext = "";
                if (imgexts.length > 0) {
                    imgext = imgexts[imgexts.length - 1];
                }
                else {
                    return;
                }
                if (imgext.toUpperCase() != "GIF" && imgext.toUpperCase() != "JPG" && imgext.toUpperCase()
                         != "JPEG" && imgext.toUpperCase() != "PNG" && imgext.toUpperCase() != "ICO") {
                    $("#saveSubPromptMessage").show(500, function () {
                        $("#saveSubPromptMessage").text("上传图片格式不正确");
                    });
                    return;
                }
                $.ajaxFileUpload({
                    url: '../PageHandler/UpLoadImage.ashx',
                    type: 'post',
                    secureuri: false, //一般设置为false
                    fileElementId: 'InputCurrentSubFile', // 上传文件的id、name属性名
                    dataType: 'text', //返回值类型，一般设置为json、application/json
                    success: function (data, status) {
                        var result = BsrCloudServer.Customer.getCustomerInfo(0);
                        if (result && result.Code == 0) {
                            var user = new Object();
                            user.ReceiverCellPhone = result.customerReponse.ReceiverCellPhone;
                            user.ReceiverEmail = result.customerReponse.ReceiverEmail;
                            user.ReceiverName = result.customerReponse.ReceiverName;
                            user.AccountTelephone = result.customerReponse.AccountTelephone;
                            user.AccountCompanyAddress = result.customerReponse.AccountCompanyAddress;
                            user.ImageByteBase64 = $(data).html();
                            user.ExtName = imgext;
                            var result_upLoadImg = BsrCloudServer.Customer.updateCustomer(JSON.stringify(user));
                            if (result_upLoadImg && result_upLoadImg.Code == 0) {
                                var path_upLoadImg = "../" + result_upLoadImg.ImagePath;
                                $("#subAccountImg").attr("src", path_upLoadImg);
                                $("#saveSubPromptMessage").show(500, function () {
                                    $("#saveSubPromptMessage").text("上传图片成功");
                                });
                            }
                            else {
                                $("#saveSubPromptMessage").show(500, function () {
                                    $("#saveSubPromptMessage").text("上传图片失败");
                                });
                            }
                        }
                        else {
                            $("#saveSubPromptMessage").show(500, function () {
                                $("#saveSubPromptMessage").text("上传图片失败");
                            });
                        }

                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                });
            } else {
                //IE下，使用滤镜
                var docObj = document.getElementById('InputCurrentSubFile');
                docObj.select();
                //解决IE9下document.selection拒绝访问的错误 
                docObj.blur();
                var path = document.selection.createRange().text;
                var imgexts = path.split(".");
                var imgext = "";
                if (imgexts.length > 0) {
                    imgext = imgexts[imgexts.length - 1];
                } else {
                    return;
                }
                if (imgext.toUpperCase() != "GIF" && imgext.toUpperCase() != "JPG" && imgext.toUpperCase() != "JPEG" && imgext.toUpperCase() != "PNG"
                    && imgext.toUpperCase() != "ICO") {
                    $("#saveSubPromptMessage").show(500, function () {
                        $("#saveSubPromptMessage").text("上传图片格式不正确");
                    });
                    return;
                }
                $.ajaxFileUpload({
                    url: '../PageHandler/UpLoadImage.ashx',
                    type: 'post',
                    secureuri: false, //一般设置为false
                    fileElementId: 'InputCurrentSubFile', // 上传文件的id、name属性名
                    dataType: 'text', //返回值类型，一般设置为json、application/json
                    success: function (data, status) {
                        var result = BsrCloudServer.Customer.getCustomerInfo(0);
                        if (result && result.Code == 0) {
                            var user = new Object();
                            user.ReceiverCellPhone = result.customerReponse.ReceiverCellPhone;
                            user.ReceiverEmail = result.customerReponse.ReceiverEmail;
                            user.ReceiverName = result.customerReponse.ReceiverName;
                            user.AccountTelephone = result.customerReponse.AccountTelephone;
                            user.AccountCompanyAddress = result.customerReponse.AccountCompanyAddress;
                            user.ImageByteBase64 = $(data).html();
                            user.ExtName = imgext;
                            var result_upLoadImg = BsrCloudServer.Customer.updateCustomer(JSON.stringify(user));
                            if (result_upLoadImg && result_upLoadImg.Code == 0) {
                                var path_upLoadImg = "../" + result_upLoadImg.ImagePath;
                                $("#subAccountImg").attr("src", path_upLoadImg);
                                $("#saveSubPromptMessage").show(500, function () {
                                    $("#saveSubPromptMessage").text("上传图片成功");
                                });
                            }
                            else {
                                $("#saveSubPromptMessage").show(500, function () {
                                    $("#saveSubPromptMessage").text("上传图片失败");
                                });
                            }
                        }
                        else {
                            $("#saveSubPromptMessage").show(500, function () {
                                $("#saveSubPromptMessage").text("上传图片失败");
                            });
                        }
                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                });
            }
            if ($.jNotify) {
                $.jNotify._close();
            }
            if (settings.callback) {
                settings.callback();
            }
        }

        //图像加载出错时的处理 
        function errorImg(img) {
            img.src = "../customerImage/default.jpg";
            img.onerror = null;
        } 

    </script>
</body>
</html>
