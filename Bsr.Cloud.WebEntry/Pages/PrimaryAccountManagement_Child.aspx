<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrimaryAccountManagement_Child.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.PrimaryAccountManagement_Child" %>

<!DOCTYPE>
<html lang="en">
<head runat="server">
    <title>子账户基本信息及操作</title>
</head>
<body>
    <div>
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
                        账户有效期:</label>
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
                        用户登陆终端:</label>
                    <div class="col-md-6">
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
                    <%--<div class="col-xs-1 " style="padding-top: 7px;">
                            <a href="#" onclick="EditChildAccountSafeInfo(this);return false;">修改</a>
                        </div>--%>
                    <div class="col-xs-2 " style="padding-top: 7px; position: relative;">
                        <a href="#" id="editChildAccountSafeInfo_a" onclick="EditChildAccountSafeInfo(this);return false;"
                            style="position: absolute; left: 28px; color: #808080;">修改</a><a style="display: none;"
                                id="cancleEditChildAccountSafeInfo_a" href="#" onclick="CancelEditChildAccountSafeInfo(this);return false;">
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
        <div class="col-xs-12" style="margin-top: 12px;">
            <div id="ChildCustomerDivID" class="ChildCustomerDivBtn">
                <button class="btn btn-default" onclick="javascript:$('#currentChildLoginCostomerInfo').submit();return false;">
                    保存
                </button>
                <button class="btn btn-default" onclick="CancelEditInfo();return false;">
                    取消
                </button>
            </div>
            <div id="saveSubPromptMessage" class="ChildCustomerDivBtn" style="color: #30c5fc;">
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
                    <%--<button type="button" class="btn btn-default" data-dismiss="modal">
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
                        <div class="col-xs-8">
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
                        <div class="col-xs-8">
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
                        <div class="col-xs-8">
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
        <input type="hidden" id="childMainAccountImgName" />
    </div>
    <script type="text/javascript">
        var strChildValidDate = "";
        var currentChildLoginCostomerInfo_Validator = null;
        $(function () {
            if (IsBaseInfoOrSafeInfo) {
                getChildCurrentCustomerInfo();
            }
            else {
                LoadCurrentChildAccountSafeDiv();
            }
        });

        //当前选中的主账户基本信息的验证
        function easyuiSelectMainAccountValidate() {
            currentChildLoginCostomerInfo_Validator = $("#currentChildLoginCostomerInfo").validate({
                onkeyup: false,
                rules: {
                    childAccontMobilePhone: { required: true, mobilephone: true, isExistedSubLognameByEdit: [true, '0', $("#hideSelectSubAccountID").val()] },
                    childAccountEmail: { email: true, isExistedSubLognameByEdit: [true, '1', $("#hideSelectSubAccountID").val()] },
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

        //获取当前选择的子账户的信息
        function getChildCurrentCustomerInfo() {
            $("#currentChildCustomerInfoDiv").show();
            $("#currentChildCustomerSafeInfoDiv").hide();
            ChangeControlToAbleOrDisble(false);
            var result = BsrCloudServer.Customer.GetSingleSubCustomer($("#hideSelectSubAccountID").val());
            if (result && result.Code == 0) {
                $("#childMainAccountImgName").val(result.customerReponse.CustomerName);
                $("#childAccontMobilePhone").val(result.customerReponse.ReceiverCellPhone);
                $("#childAccountEmail").val(result.customerReponse.ReceiverEmail);
                $("#childAccountRealName").val(result.customerReponse.ReceiverName);
                $("#childAccountTelphone").val(result.customerReponse.AccountTelephone);
                $("#childAccountAddress").val(result.customerReponse.AccountCompanyAddress);
                $("#hideChildCustomerEmail").val(result.customerReponse.ReceiverEmail);
                $("#hideChildCustomerPhone").val(result.customerReponse.ReceiverCellPhone);
            }
            else {
                alert("获取当前选择账户信息失败！");
            }
        }

        //禁用控件
        function ChangeControlToAbleOrDisble(obj) {
            if (obj) {
                $("#currentChildLoginCostomerInfo input").removeAttr("disabled");
            }
            else {
                $("#currentChildLoginCostomerInfo input").attr("disabled", "disabled");
            }
        }

        //显示当前选中的主账户的基本信息
        function LoadCurrentChildAccountDiv() {
            getChildCurrentCustomerInfo();
        }

        //启用或者禁用账户安全信息
        function AbleOrDisableAccountInfo(obj) {
            if (obj) {
                $("[name='inlineChildRadioOptions']").removeAttr("disabled");
                $("[name='chkChildLoginWay']").removeAttr("disabled");
                $("#childValidEndtime").removeAttr("disabled");
            } else {
                $("[name='inlineChildRadioOptions']").attr("disabled", "disabled");
                $("[name='chkChildLoginWay']").attr("disabled", "disabled");
                $("#childValidEndtime").attr("disabled", "disabled");
                $("#editChildAccountSafeInfo_a").text("修改").css("color", "#808080");
                $("#cancleEditChildAccountSafeInfo_a").hide();
            }
        }

        //当前选中主账户的安全设置
        function LoadCurrentChildAccountSafeDiv() {
            AbleOrDisableAccountInfo(false);
            $("#currentChildCustomerInfoDiv").hide();
            $("#currentChildCustomerSafeInfoDiv").show();

            var result = BsrCloudServer.Customer.GetSingleSubCustomer($("#hideSelectSubAccountID").val());
            if (result && result.Code == 0) {
                $("#childAccontValid").val(result.customerReponse.ValidTime);
                var strChildLoginTerminal = result.customerReponse.LoginTypes;
                $("[name='chkChildLoginWay']").prop("checked", false);
                if (strChildLoginTerminal != null && strChildLoginTerminal != "") {
                    if (strChildLoginTerminal.indexOf("3") >= 0 || strChildLoginTerminal.indexOf("4") >= 0) {
                        $("#childPhoneClient").prop("checked", true);
                    }
                    if (strChildLoginTerminal.indexOf("1") >= 0) {
                        $("#childBSPhoneClient").prop("checked", true);
                    }
                    if (strChildLoginTerminal.indexOf("2") >= 0) {
                        $("#childCSClient").prop("checked", true);
                    }
                }
                strChildValidDate = result.customerReponse.ValidTime;
                if (result.customerReponse.ValidTime == "" || result.customerReponse.ValidTime == "永久有效") {
                    $("#inlineRadio_ChildEvertime").prop("checked", true);
                }
                else {
                    $("#inlineRadio_ChildEndtime").prop("checked", true);
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
            ChangeControlToAbleOrDisble(true);
            easyuiSelectMainAccountValidate();
            $("#ChildCustomerDivID").show();
            $("#saveSubPromptMessage").hide();
        }

        //修改当前选中用户的安全信息
        function EditChildAccountSafeInfo(obj) {
            if ($.trim($(obj).text()) == "修改") {
                AbleOrDisableAccountInfo(true);
                $(obj).text("保存").css("color", "");
                $("#cancleEditChildAccountSafeInfo_a").show();
                $("#cancleEditChildAccountSafeInfo_a").css("color", "");
            }
            else {
                SaveChildAccountSafeInfo(obj);
            }
        }

        //显示图片上传界面
        function ShowChildUpdateFilePage() {
            $('#modelChildUpfileWin').modal('show');
        }

        function btnChildUpdateImage() {
            //readChildURL($("#childInputFile")[0]);
        }

        function readChildURL(input) {
            $("#hideChildImageBase64Code").val("");
            $("#hideChildImgExt").val("");
            if ($("#childInputFile").val() == "") {
                alert('上传图片不能为空！');
                return;
            }
            var imgSrc;
            if (input.files && input.files[0]) {
                imgSrc = input.files[0];
                imgSrc = window.URL.createObjectURL(input.files[0]);
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
                    alert('上传图片格式不正确！');
                    return;
                }
                imgSrc = imgSrc.replace(/\\/g, "\\\\");
                ajaxFileUpload(imgext);
                document.getElementById("selectedPrimaryCustomerImg").innerHTML = "<img id='img3' width=100% height=100% src='" + imgSrc + "'/>";
            } else {
                //IE下，使用滤镜
                var docObj = document.getElementById('childInputFile');
                docObj.select();
                //解决IE9下document.selection拒绝访问的错误 
                docObj.blur();
                imgSrc = document.selection.createRange().text;
                var imgexts = imgSrc.split(".");
                var imgext = "";
                if (imgexts.length > 0) {
                    imgext = imgexts[imgexts.length - 1];
                } else {
                    return;
                }
                if (imgext.toUpperCase() != "GIF" && imgext.toUpperCase() != "JPG" && imgext.toUpperCase() != "JPEG" && imgext.toUpperCase() != "PNG"
                    && imgext.toUpperCase() != "ICO") {
                    alert('上传图片格式不正确！');
                    return;
                }
                imgSrc = imgSrc.replace(/\\/g, "\\\\");
                ajaxFileUpload(imgext);
                document.getElementById("selectedPrimaryCustomerImg").style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(enabled='true',sizingMethod='scale',src=\"" + imgSrc + "\")"; //使用滤镜效果   
            }
        }

        function ajaxFileUpload(obj) {
            $.ajaxFileUpload({
                url: '../PageHandler/UpLoadImage.ashx',
                type: 'post',
                secureuri: false, //一般设置为false
                fileElementId: 'childInputFile', // 上传文件的id、name属性名
                dataType: 'text', //返回值类型，一般设置为json、application/json
                success: function (data, status) {
                    $("#hideChildImageBase64Code").val($(data).html());
                    $("#hideChildImgExt").val(obj);
                    //$("#imgChildMainAccount").hide();
                    $("#selectedPrimaryCustomerImg").show();
                },
                error: function (data, status, e) {
                    alert(e);
                }
            });
        }

        //保存当前子账户的基本信息
        function SaveCurrentChildAccountInfo() {
            var result = updateChildCustomer();
            if (result && result.Code == 0) {
                //当前登陆账户信息禁用
                ChangeControlToAbleOrDisble(false);
                $("#ChildCustomerDivID").hide(500, function () {
                    $("#saveSubPromptMessage").show();
                    $("#saveSubPromptMessage").text("保存成功");
                });
            }
            else {
                $("#saveSubPromptMessage").show();
                $("#saveSubPromptMessage").text("保存失败");
            }
        }

        //取消保存当前主账户的基本信息
        function CancelEditInfo() {
            if (currentChildLoginCostomerInfo_Validator) {
                currentChildLoginCostomerInfo_Validator.resetForm();
            }
            getChildCurrentCustomerInfo();
            ChangeControlToAbleOrDisble(false);
            $("#ChildCustomerDivID").hide();
        }

        //更新当前选择的子账户的基本信息
        function updateChildCustomer() {
            var result = false;
            var user = new Object();
            user.SubCustomerId = $("#hideSelectSubAccountID").val();
            user.ReceiverCellPhone = $.trim($("#childAccontMobilePhone").val());
            user.ReceiverEmail = $.trim($("#childAccountEmail").val());
            user.ReceiverName = $.trim($("#childAccountRealName").val());
            user.AccountTelephone = $.trim($("#childAccountTelphone").val());
            user.AccountCompanyAddress = $.trim($("#childAccountAddress").val());
            user.ImageByteBase64 = "";
            user.ExtName = "";
            return BsrCloudServer.Customer.UpdateSubCustomer(user);
        }

        //保存选中的主账户信息 登陆方式 3,4-手机客户端 1-BS端登陆(WEB) 2-CS端登陆
        function SaveChildAccountSafeInfo(obj) {
            var loginway = "";
            $("[name='chkChildLoginWay']").each(function () {
                if ($(this)[0].checked) {
                    if ("" == loginway) {
                        if ($(this).attr("id") == "childPhoneClient") {
                            loginway = "3,4";
                        }
                        else if ($(this).attr("id") == "childBSPhoneClient") {
                            loginway = "1";
                        }
                        else {
                            loginway = "2";
                        }
                    }
                    else {
                        if ($(this).attr("id") == "childPhoneClient") {
                            loginway += ",3,4";
                        }
                        else if ($(this).attr("id") == "childBSPhoneClient") {
                            loginway += ",1";
                        }
                        else {
                            loginway += ",2";
                        }
                    }
                }
            });
            if ("" == loginway) {
                if (!confirm("如果您不选择登陆终端,您将无法使用该主账户登陆!")) {
                    return;
                }
            }
            var userSafeInfo = new Object();
            userSafeInfo.SubCustomerId = $("#hideSelectSubAccountID").val();
            userSafeInfo.IsModify = 1;
            if ($("input:radio[name='inlineChildRadioOptions']:checked")[0].id == "inlineRadio_ChildEvertime") {
                userSafeInfo.ValidTime = "null";
            }
            else {
                userSafeInfo.ValidTime = $("#childValidEndtime").val();
            }
            userSafeInfo.LoginTypes = loginway;
            var result = BsrCloudServer.Customer.UpdateSubSafeByPrimary(userSafeInfo);
            if (result && result.Code == 0) {
                $(obj).text("修改").css("color", "#808080");
                $("#cancleEditChildAccountSafeInfo_a").hide();
                $("#saveSubPromptMessage").text("保存成功");
                $("#saveSubPromptMessage").show();
                AbleOrDisableAccountInfo(false);
            }
            else {
                $("#saveSubPromptMessage").text("保存失败");
                $("#saveSubPromptMessage").show();
            }
        }

        //取消修改主账户安全信息修改操作
        function CancelEditChildAccountSafeInfo() {
            LoadCurrentChildAccountSafeDiv();
        }

        function ChangeToChildEver() {
            $("#childValidEndtime").val("");
        }

        function ChangeToChildEnd() {
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
            $("#myChildModalLabel").html($("#childMainAccountImgName").val() + " 历史登陆信息");
            $('#myChildModal').modal('show');
            $("#childLogtable tr:gt(0)").empty();
            var result = BsrCloudServer.Customer.GetSubCustomerLoginInfo($("#hideSelectSubAccountID").val());
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

        //保存选中主账户下的子账户的密码信息
        function btnSaveChildAccountPassWd() {
            var result = SaveAccountPassWord();
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

        //子账户的密码修改
        function SaveAccountPassWord() {
            var userPassWd = new Object();
            userPassWd.SubCustomerId = $("#hideSelectSubAccountID").val();
            userPassWd.OldPassWord = $.md5($.trim($("#childOriginalPassWd").val()));
            userPassWd.NewPassWord = $.md5($.trim($("#childNewPassWd").val()));
            return BsrCloudServer.Customer.UpdateSubPassWordByPrimary(userPassWd);
        }

        
    </script>
</body>
</html>
