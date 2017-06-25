<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManagement_Child.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.AccountManagement_Child" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>主账户基本信息及操作</title>
</head>
<body>
    <div>
        <div class="col-xs-12">
            <div class="col-xs-12">
                <form class="form-horizontal" id="currentChildLoginCostomerInfo" role="form">
                <div id="currentChildCustomerInfoDiv" class="ChildCustomerInfoDiv">
                    <div class="form-group">
                        <label for="childAccontMobilePhone" class="col-xs-2 control-label">
                            手机号:</label>
                        <div class="col-xs-6">
                            <input type="text" disabled="disabled" class="form-control" id="childAccontMobilePhone"
                                name="childAccontMobilePhone" maxlength="16" value="" />
                        </div>
                        <div class="col-xs-4">
                            <label for="childAccontMobilePhone" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="childAccountEmail" class="col-xs-2 control-label">
                            邮箱:</label>
                        <div class="col-xs-6">
                            <input type="text" disabled="disabled" class="form-control" id="childAccountEmail"
                                name="childAccountEmail" maxlength="64" value="" />
                        </div>
                        <div class="col-xs-4">
                            <label for="childAccountEmail" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="childAccountRealName" class="col-xs-2 control-label">
                            真实姓名:</label>
                        <div class="col-xs-6">
                            <input type="text" disabled="disabled" class="form-control" id="childAccountRealName"
                                name="realname" maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="childAccountTelphone" class="col-xs-2 control-label">
                            固定电话:</label>
                        <div class="col-xs-6">
                            <input type="text" disabled="disabled" class="form-control" id="childAccountTelphone"
                                name="childAccountTelphone" maxlength="14" value="" />
                        </div>
                        <div class="col-xs-4">
                            <label for="childAccountTelphone" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="childAccountAddress" class="col-xs-2 control-label">
                            地址:</label>
                        <div class="col-xs-6">
                            <input type="text" disabled="disabled" class="form-control" id="childAccountAddress"
                                name="address" maxlength="512" value="" />
                        </div>
                    </div>
                </div>
                <div id="currentChildCustomerSafeInfoDiv" class="ChildCustomerInfoDiv">
                    <div class="form-group">
                        <label class="col-xs-3 control-label">
                            账户有效期</label>
                        <div class="radio col-xs-3 ChildValidRadio">
                            <label class="radio-inline">
                                <input type="radio" onclick="ChangeToChildEver();" name="inlineChildRadioOptions"
                                    id="inlineRadio_ChildEvertime" value="option1" />
                                永久有效
                            </label>
                            <label class="radio-inline">
                                <input type="radio" onclick="ChangeToChildEnd();" name="inlineChildRadioOptions"
                                    id="inlineRadio_ChildEndtime" value="option2" />
                                截止
                            </label>
                        </div>
                        <div class="ChildValidDate">
                            <label>
                                <input class="Wdate" readonly="readonly" type="text" id="childValidEndtime" style="width: 160px;"
                                    onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',isShowClear:false,readOnly:true,minDate:'%y-%M-#{%d}',startDate:'%y-%M-#{%d}'});return false;" />
                                有效
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="col-xs-3 control-label">
                            用户登陆终端</label>
                        <div class="col-xs-6">
                            <div id="EditCurrentChildCustomerLoginWay" class="ChildValidCheckBox">
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="childPhoneClient" name="chkChildLoginWay" value="option1" />
                                    手机客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="childBSPhoneClient" name="chkChildLoginWay" value="option2" />
                                    B/S客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="childCSClient" name="chkChildLoginWay" value="option3" />
                                    C/S客户端
                                </label>
                                <div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-2" style="padding-top: 7px; position: relative;">
                            <a href="#" id="editPrimaryAccountSafeInfo_a" onclick="EditChildAccountSafeInfo(this);return false;"
                                style="position: absolute; left: 25px; color: #808080;">修改</a><a style="display: none;"
                                    id="cancleEditPrimaryAccountSafeInfo" href="#" onclick="CancelEditChildAccountSafeInfo(this);return false;">
                                    取消</a>
                        </div>
                    </div>
                    <div class="form-group" style="line-height: 30px;">
                        <label class="col-xs-3 control-label">
                            登陆密码</label>
                        <label class="col-xs-5" style="margin-top: 7px; font-weight: normal;">
                            ******
                        </label>
                        <div class="col-xs-1 col-xs-offset-1" style="padding-top: 7px;">
                            <a style="color: #808080;" href="#" onclick="ResetChildAccountPassWd();return false;">
                                修改</a>
                        </div>
                    </div>
                </div>
                </form>
            </div>
        </div>
        <div class="col-xs-12" style="margin-top: 12px;">
            <div id="ChildCustomerDivID" class="ChildCustomerDivBtn">
                <button class="btn btn-default" onclick="javascript:$('#currentChildLoginCostomerInfo').submit();return false;">
                    保存
                </button>
                <button class="btn btn-default" onclick="CancelEditInfo();return false;">
                    取消
                </button>
            </div>
            <div id="saveChildPromptMessage" class="ChildCustomerDivBtn" style="color: #30c5fc;">
            </div>
        </div>
    </div>
    <!-- 账户历史登陆信息页面 -->
    <div class="modal fade bs-example-modal-sm" id="myChildModal" tabindex="-1" role="dialog"
        aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" style="width: 680px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 id="myChildModalLabel" class="modal-title">
                        历史登陆信息</h4>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered" id="childLogtable">
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
                    <%-- <button type="button" class="btn btn-default" data-dismiss="modal">
                        返回</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- 重置密码 -->
    <div class="modal fade bs-example-modal-sm" id="EditChildAccontPassWdModal" tabindex="-1"
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
                    <form class="form-horizontal" id="fromChildAccountPassWd" role="form">
                    <div class="form-group">
                        <label for="childOriginalPassWd" class="col-md-3 control-label">
                            原始密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="childOriginalPassWd" name="childOriginalPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="childOriginalPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="childNewPassWd" class="col-md-3 control-label">
                            新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="childNewPassWd" name="childNewPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="childNewPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="childConfirmPassword" class="col-md-3 control-label">
                            确认新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="childConfirmPassword" name="childConfirmPassword"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="childConfirmPassword" class="error">
                            </label>
                        </div>
                    </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn" onclick="javascript:$('#fromChildAccountPassWd').submit();return false;">
                        保存</button>
                </div>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hideChildCustomerPhone" />
        <input type="hidden" id="hideChildCustomerEmail" />
        <input type="hidden" id="hideChildImageBase64Code" />
        <input type="hidden" id="hideChildImgExt" />
    </div>
    <script type="text/javascript">
        var strChildLoginTerminal = "";
        var strChildValidDate = "";
        var currentChildLoginCostomerInfo_Validator = null;
        $(function () {
            if (IsBaseInfoOrSafeInfo) {
                LoadCurrentChildAccountDiv();
            }
            else {
                LoadCurrentChikdAccountSafeDiv();
            }
        });

        //当前选中的主账户基本信息的验证
        function easyuiSelectMainAccountValidate() {
            currentChildLoginCostomerInfo_Validator = $("#currentChildLoginCostomerInfo").validate({
                onkeyup: false,
                rules: {
                    childAccontMobilePhone: { required: true, mobilephone: true, isExistedMainLognameByEdit: [true, '0', $("#hideSelectMainAccountID").val()] },
                    childAccountEmail: { email: true, isExistedMainLognameByEdit: [true, '1', $("#hideSelectMainAccountID").val()] },
                    childAccountTelphone: { telephone: true }
                },
                messages: {
                    childAccontMobilePhone: { required: "请输入手机号" },
                    childAccountEmail: { email: "邮箱格式不正确" }
                },
                submitHandler: function () {
                    SaveCurrentChildAccountInfo();
                }
            });
        }

        //获取当前选择的主账户的信息
        function getChildCurrentCustomerInfo() {
            $("#currentChildCustomerInfoDiv").show();
            $("#currentChildCustomerSafeInfoDiv").hide();
            ChangeControlToDisble();
            var result = BsrCloudServer.Customer.GetSinglePrimaryCustomer($("#hideSelectMainAccountID").val());
            if (result && result.Code == 0) {
                $("#childAccontMobilePhone").val(result.customerReponse.ReceiverCellPhone);
                $("#childAccountEmail").val(result.customerReponse.ReceiverEmail);
                $("#childAccountRealName").val(result.customerReponse.ReceiverName);
                $("#childAccountTelphone").val(result.customerReponse.AccountTelephone);
                $("#childAccountAddress").val(result.customerReponse.AccountCompanyAddress);
                $("#hideChildCustomerPhone").val(result.customerReponse.ReceiverCellPhone);
                $("#hideChildCustomerEmail").val(result.customerReponse.ReceiverEmail);
            }
            else {
                alert("获取当前选择账户信息失败！");
            }
        }

        //禁用控件
        function ChangeControlToDisble() {
            $("#childAccontMobilePhone").attr("disabled", "disabled");
            $("#childAccountEmail").attr("disabled", "disabled");
            $("#childAccountRealName").attr("disabled", "disabled");
            $("#childAccountTelphone").attr("disabled", "disabled");
            $("#childAccountAddress").attr("disabled", "disabled");
        }

        //启用控件
        function ChangeControlToAble() {
            $("#childAccontMobilePhone").removeAttr("disabled");
            $("#childAccountEmail").removeAttr("disabled");
            $("#childAccountRealName").removeAttr("disabled");
            $("#childAccountTelphone").removeAttr("disabled");
            $("#childAccountAddress").removeAttr("disabled");
            $("#hideChildImageBase64Code").val("");
        }

        //显示当前选中的主账户的基本信息
        function LoadCurrentChildAccountDiv() {
            getChildCurrentCustomerInfo();
        }

        //当前选中主账户的安全设置
        function LoadCurrentChikdAccountSafeDiv() {
            $("#currentChildCustomerInfoDiv").hide();
            $("#currentChildCustomerSafeInfoDiv").show();
            EnableOrBanPrimaryAccountSafeInfo(false);
            var result = BsrCloudServer.Customer.GetSinglePrimaryCustomer($("#hideSelectMainAccountID").val());
            if (result && result.Code == 0) {
                var strChildDesLoginTerminal = result.customerReponse.LoginTypes;
                $("[name='chkChildLoginWay']").prop("checked", false);
                if (strChildDesLoginTerminal != null && strChildDesLoginTerminal != "") {
                    if (strChildDesLoginTerminal.indexOf("3") >= 0 || strChildDesLoginTerminal.indexOf("4") >= 0) {
                        $("#childPhoneClient").prop("checked", true);
                    }
                    if (strChildDesLoginTerminal.indexOf("1") >= 0) {
                        $("#childBSPhoneClient").prop("checked", true);
                    }
                    if (strChildDesLoginTerminal.indexOf("2") >= 0) {
                        $("#childCSClient").prop("checked", true);
                    }
                }

                if (result.customerReponse.ValidTime == "" || result.customerReponse.ValidTime == "永久有效") {
                    $("#inlineRadio_ChildEvertime").prop("checked", "true");
                }
                else {
                    $("#inlineRadio_ChildEndtime").prop("checked", "true");
                    var time = new Date(result.customerReponse.ValidTime);
                    var dateTime = time.Format("yyyy-MM-dd hh:mm:ss");
                    $("#childValidEndtime").val(dateTime);
                }
            }
            else {
                alert("获取当前选择账户安全信息失败！");
            }
        }

        function EditCurrentChildAccountInfo() {
            easyuiSelectMainAccountValidate();
            ChangeControlToAble();
            $("#ChildCustomerDivID").show();
        }

        //启用或者禁用主账户的安全信息
        function EnableOrBanPrimaryAccountSafeInfo(obj) {
            $("#ChildCustomerDivID").hide();
            $("#saveChildPromptMessage").hide();
            if (obj) {
                $("[name='chkChildLoginWay']").removeAttr("disabled");
                $("[name='inlineChildRadioOptions']").removeAttr("disabled");
                $("#childValidEndtime").removeAttr("disabled");
            }
            else {
                $("[name='chkChildLoginWay']").attr("disabled", "disabled");
                $("[name='inlineChildRadioOptions']").attr("disabled", "disabled");
                $("#childValidEndtime").attr("disabled", "disabled");
            }
        }

        //修改当前选中用户的安全信息
        function EditChildAccountSafeInfo(obj) {
            if (obj) {
                if ($.trim($(obj).text()) == "修改") {
                    EnableOrBanPrimaryAccountSafeInfo(true);
                    $(obj).text("保存").css("color", "");
                    $("#cancleEditPrimaryAccountSafeInfo").show();
                    $("#cancleEditPrimaryAccountSafeInfo").css("color", "");
                }
                else {
                    SaveChildAccountSafeInfo();
                }
            }
        }

        //保存当前主账户的基本信息
        function SaveCurrentChildAccountInfo() {
            var result = updateChildCustomer();
            if (result && result.Code == 0) {
                //当前登陆账户信息禁用
                ChangeControlToDisble();
                $("#ChildCustomerDivID").hide();
                $("#saveChildPromptMessage").show(500, function () {
                    $("#saveChildPromptMessage").text("保存成功");
                });
            }
            else {
                $("#saveChildPromptMessage").show(500, function () {
                    $("#saveChildPromptMessage").text("保存失败");
                });
            }
        }

        //取消保存当前主账户的基本信息
        function CancelEditInfo() {
            if (currentChildLoginCostomerInfo_Validator) {
                currentChildLoginCostomerInfo_Validator.resetForm();
            }
            getChildCurrentCustomerInfo();
            $("#ChildCustomerDivID").hide();
            $("#saveChildPromptMessage").hide();
        }

        //更新当前选择的主账户的基本信息
        function updateChildCustomer() {
            var result = false;
            var user = new Object();
            user.PrimaryCustomerId = $("#hideSelectMainAccountID").val();
            user.ReceiverCellPhone = $.trim($("#childAccontMobilePhone").val());
            user.ReceiverEmail = $.trim($("#childAccountEmail").val());
            user.ReceiverName = $.trim($("#childAccountRealName").val());
            user.AccountTelephone = $.trim($("#childAccountTelphone").val());
            user.AccountCompanyAddress = $.trim($("#childAccountAddress").val());
            user.ImageByteBase64 = "";
            user.ExtName = "";
            return BsrCloudServer.Customer.UpdatePrimaryCustomer(user);
        }

        //保存选中的主账户信息 登陆方式 3,4-手机客户端 1-BS端登陆(WEB) 2-CS端登陆
        function SaveChildAccountSafeInfo() {
            var loginway = "";
            if ($("#childPhoneClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "3,4";
                }
                else {
                    loginway += ",3,4";
                }
            }
            if ($("#childBSPhoneClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "1";
                }
                else {
                    loginway += ",1";
                }
            }
            if ($("#childCSClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "2";
                }
                else {
                    loginway += ",2";
                }
            }
            if ("" == loginway) {
                if (!confirm("如果您不选择登陆终端,您将无法使用该主账户登陆!")) {
                    return;
                }
            }
            var userSafeInfo = new Object();
            userSafeInfo.PrimaryCustomerId = $("#hideSelectMainAccountID").val();
            userSafeInfo.IsModify = 1;
            if ($("input:radio[name='inlineChildRadioOptions']:checked")[0].id == "inlineRadio_ChildEvertime") {
                userSafeInfo.ValidTime = "null";
            }
            else {
                userSafeInfo.ValidTime = $("#childValidEndtime").val();
            }
            userSafeInfo.LoginTypes = loginway;
            var result = BsrCloudServer.Customer.UpdatePrimarySafeByManagerAccount(userSafeInfo);
            if (result && result.Code == 0) {
                EnableOrBanPrimaryAccountSafeInfo(false);
                $("#cancleEditPrimaryAccountSafeInfo").hide();
                $("#editPrimaryAccountSafeInfo_a").text("修改").css("color", "#808080");
                $("#saveChildPromptMessage").show();
                $("#saveChildPromptMessage").text("保存成功");
            }
            else {
                $("#saveChildPromptMessage").show();
                $("#saveChildPromptMessage").text("保存失败");
            }
        }

        //取消修改主账户安全信息修改操作
        function CancelEditChildAccountSafeInfo() {
            EnableOrBanPrimaryAccountSafeInfo(false);
            $("#cancleEditPrimaryAccountSafeInfo").hide();
            $("#editPrimaryAccountSafeInfo_a").text("修改").css("color", "#808080");
            LoadCurrentChikdAccountSafeDiv();
        }

        function ChangeToChildEver() {
            $("#childValidEndtime").attr("disabled", "disabled");
            $("#childValidEndtime").val("");
        }

        function ChangeToChildEnd() {
            $("#childValidEndtime").removeAttr("disabled");
            var endtime = new Date();
            endtime.setMonth(endtime.getMonth() + 3);
            var dateTime = endtime.Format("yyyy-MM-dd hh:mm:ss");
            $("#childValidEndtime").val(dateTime);
        }

        function ResetChildAccountPassWd() {
            $("#EditChildAccontPassWdModal").modal('show');
            $("#childOriginalPassWd").val("");
            $("#childNewPassWd").val("");
            $("#childConfirmPassword").val("");
            easyuiChildCustomerPassWdValidate();
        }

        //当前选中的主账户密码验证
        function easyuiChildCustomerPassWdValidate() {
            var fromChildAccountPassWd_Validator = $("#fromChildAccountPassWd").validate({
                onkeyup: false,
                rules: {
                    childOriginalPassWd: {
                        required: true,
                        password: true
                    },
                    childNewPassWd: {
                        required: true,
                        password: true
                    },
                    childConfirmPassword: {
                        required: true,
                        equalTo: "#childNewPassWd"
                    }
                },
                messages: {
                    childOriginalPassWd: { required: "请输入密码" },
                    childNewPassWd: { required: "请输入密码" },
                    childConfirmPassword: {
                        required: "请重复输入密码",
                        equalTo: "密码不一致"
                    }
                },
                submitHandler: function () {
                    btnSaveChildAccountPassWd();
                }
            });
            fromChildAccountPassWd_Validator.resetForm();
        }
        //获取选中主账户的登陆信息
        function ShowChildHistoryInfo() {
            $("#myChildModalLabel").html(" 历史登陆信息");
            $('#myChildModal').modal('show');
            $("#childLogtable tr:gt(0)").empty();
            var result = BsrCloudServer.Customer.GetPrimaryCustomerLoginInfo($("#hideSelectMainAccountID").val());
            if (result && result.Code == 0) {
                if (result.operaterLogList.length > 0) {
                    for (var i = 0; i < result.operaterLogList.length; i++) {
                        var tr = "<tr><td>" + result.operaterLogList[i].OperaterTime.toString() + "</td><td>" + result.operaterLogList[i].AgentType +
                            "</td><td>" + result.operaterLogList[i].AgentVersion + "</td></tr>";
                        $("#childLogtable").append(tr);
                    }
                }
            }
        }

        //保存选中主账户的密码信息
        function btnSaveChildAccountPassWd() {
            var result = SaveSelectedMainAccountPassWord();
            if (result && result.Code == 0) {
                $("#EditChildAccontPassWdModal").modal('hide');
                jSuccess("密码修改成功", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            } else {
                jError("密码修改失败:" + result.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                $("#childOriginalPassWd").val("").focus();
                $("#childNewPassWd").val("");
                $("#childConfirmPassword").val("");
            }
        }

        //保存选中主账户的密码
        function SaveSelectedMainAccountPassWord() {
            var userPassWdInfo = new Object();
            userPassWdInfo.PrimaryCustomerId = $("#hideSelectMainAccountID").val();
            userPassWdInfo.OldPassWord = $.md5($.trim($("#childOriginalPassWd").val()));
            userPassWdInfo.NewPassWord = $.md5($.trim($("#childNewPassWd").val()));
            return BsrCloudServer.Customer.UpdatePrimaryPassWordByManagerAccount(userPassWdInfo);
        }

        
    </script>
</body>
</html>
