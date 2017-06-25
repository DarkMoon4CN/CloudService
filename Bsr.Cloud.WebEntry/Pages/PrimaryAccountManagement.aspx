<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrimaryAccountManagement.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.PrimaryAccountManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Styles/accountManagement.css" rel="stylesheet" type="text/css" />
    <script src="../JqueryPlugins/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
    <style type="text/css">
        .marginleft
        {
            margin-left: 5px;
        }
    </style>
</head>
<body>
    <div>
        <div>
            <div class="col-xs-12">
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
                        <a id="accountBaseinfo_a" href="#" onclick="LoadCurrentPrimaryAccountDiv();return false;">
                            账户基本信息</a>
                    </div>
                    <div class="accountSafeinfo">
                        <a id="accountSafeinfo_a" href="#" onclick="LoadCurrentPrimaryAccountSafeDiv();return false;">
                            账户安全设置</a>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="CurrentPrimaryAccountInfoDiv" style="margin-top: 15px;
                display: none;">
                <div class="divcss">
                    <div class="col-xs-3 divimg">
                        <img class="img-circle" id="primaryAccountImg" src="" style="width: 140px; height: 140px;
                            margin-left: 30px; cursor: pointer;" onerror="errorImg(this)" />
                    </div>
                    <div class="col-xs-9 divuserinfo">
                        <form class="form-horizontal" id="FormCurrentLoginCostomerInfo" role="form">
                        <div>
                            <div class="form-group">
                                <label for="primaryAccontMobilePhone" class="col-xs-3 control-label">
                                    手机号:</label>
                                <div class="col-xs-5">
                                    <input type="text" disabled="disabled" class="form-control" id="primaryAccontMobilePhone"
                                        name="mobile" maxlength="16" value="" />
                                </div>
                                <div class="col-xs-4">
                                    <label for="primaryAccontMobilePhone" class="error">
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="primaryAccountEmail" class="col-xs-3 control-label">
                                    邮箱:</label>
                                <div class="col-xs-5">
                                    <input type="text" disabled="disabled" class="form-control" id="primaryAccountEmail"
                                        name="emial" maxlength="64" value="" />
                                </div>
                                <div class="col-xs-4">
                                    <label for="primaryAccountEmail" class="error">
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="primaryAccountRealName" class="col-xs-3 control-label">
                                    真实姓名:</label>
                                <div class="col-xs-5">
                                    <input type="text" disabled="disabled" class="form-control" id="primaryAccountRealName"
                                        name="realname" maxlength="32" value="" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="primaryAccountTelphone" class="col-xs-3 control-label">
                                    固定电话:</label>
                                <div class="col-xs-5">
                                    <input type="text" disabled="disabled" class="form-control" id="primaryAccountTelphone"
                                        name="telphone" maxlength="14" value="" />
                                </div>
                                <div class="col-xs-4">
                                    <label for="primaryAccountTelphone" class="error">
                                    </label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="primaryAccountAddress" class="col-xs-3 control-label">
                                    地址:</label>
                                <div class="col-xs-5">
                                    <input type="text" disabled="disabled" class="form-control" id="primaryAccountAddress"
                                        name="address" maxlength="512" value="" />
                                </div>
                            </div>
                        </div>
                        </form>
                    </div>
                    <div style="float: right; margin-right: 20px; padding-top: 15px;">
                        <a href="#" onclick="EditCurrentPriamryAccountInfo();return false;">
                            <img src="../Images/icons/icon_Edit_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Edit_Hover.png'"
                                title="修改" onmouseout="this.src='../Images/icons/icon_Edit_Normal.png'" />
                        </a><a href="#" onclick="ShowCurrentPriamryAccountHistoryInfo();return false;" style="margin-left: 5px;">
                            <img src="../Images/icons/icon_History_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_History_Hover.png'"
                                title="历史登录信息" onmouseout="this.src='../Images/icons/icon_History_Normal.png'" />
                        </a>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="CurrentPriamryAccountSafeInfoDiv" style="display: none;">
                <div class="divsafecss">
                    <form class="form-horizontal" role="form">
                    <div class="form-group" style="line-height: 20px;">
                        <label for="accontValid" class="col-md-3 control-label">
                            账户有效期</label>
                        <label id="priamryAccontValid" class="col-md-3" style="margin-top: 7px; margin-left: 26px;
                            font-weight: normal;">
                        </label>
                    </div>
                    <div class="form-group" style="line-height: 25px;">
                        <label class="col-xs-3 control-label">
                            用户登陆终端</label>
                        <div class="col-xs-6">
                            <div id="currentPrimaryAccountLoginWay" style="width: 344px;">
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="primaryPhoneClient" name="chkPrimaryAccountLoginWay" value="option1" />
                                    手机客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="primaryBSPhoneClient" name="chkPrimaryAccountLoginWay"
                                        value="option2" />
                                    B/S客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="primaryCSClient" name="chkPrimaryAccountLoginWay" value="option3" />
                                    C/S客户端
                                </label>
                            </div>
                        </div>
                        <div class="col-xs-2 " style="padding-top: 7px; position: relative;">
                            <a href="#" id="editPrimaryAccountSafeInfo_a" onclick="EditOrSavePrimaryAccountSafeInfo(this);return false;"
                                style="position: absolute; left: 20px; color: #808080;">修改</a><a style="display: none;
                                    margin-left: 5px; color: ;" id="cancleEditPrimaryAccountSafeInfo_a" href="#"
                                    onclick="CancelSavePrimaryAccountSafeInfo(this);return false;">取消</a>
                        </div>
                    </div>
                    <div class="form-group" style="line-height: 30px;">
                        <label class="col-md-3 control-label">
                            登陆密码</label>
                        <label class="col-md-2" style="margin-top: 7px; font-weight: normal;">
                            ******
                        </label>
                        <div class="col-xs-1 col-xs-offset-4" style="padding-top: 7px; margin-left: 263px;">
                            <a style="color: #808080;" href="#" onclick="ResetPrimaryAccountPassWd();return false;">
                                修改</a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="divbtn">
                    <div id="saveCurrentPriamryAccountInfo_divId" style="display: none;">
                        <button class="btn btn-default" style="width: 120px;" onclick="javascript:$('#FormCurrentLoginCostomerInfo').submit();return false;">
                            保存
                        </button>
                        <button class="btn btn-default" style="margin-left: 16px; width: 120px;" onclick="CancelSaveCurrentPriamryAccountInfo();return false;">
                            取消
                        </button>
                    </div>
                    <div id="savePromptMessage" style="margin-top: 10px; color: #30c5fc; display: none;">
                        保存成功
                    </div>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="panel-group" id="subAccordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default" id="subPanelOne" style="margin-left: -15px; margin-right: -15px;
                        box-shadow: 0 1px 1px rgba(0, 0, 0, 0);">
                        <div class="panel-heading" role="tab" id="headingOne">
                            <label class="control-label">
                                我的子账户</label>
                        </div>
                        <div class="createChildAccoutDiv">
                            <a href="#" onclick="ShowCollapsedSubAccoutPanelBody();return false;">创建子账户</a>
                        </div>
                        <div id="primaryOfSubCollapsePanel" class="panel-collapse collapse" role="tabpanel"
                            aria-labelledby="headingOne">
                            <div class="panel-body" style="border-style: none;">
                                <div style="height: 478px;">
                                    <div class="col-xs-12">
                                        <div class="col-xs-5">
                                            <form id="createSubAccountFrom" class="form-horizontal" role="form">
                                            <div class="form-group">
                                                <label class="col-xs-3 col-xs-offset-4 control-label">
                                                    基本信息:</label>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountUserName" class="col-xs-4 control-label">
                                                    用户名:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="subAccountUserName" name="subAccountUserName"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="subAccountUserName" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountPassword" class="col-xs-4 control-label">
                                                    密码:</label>
                                                <div class="col-xs-8">
                                                    <input type="password" class="form-control" id="subAccountPassword" name="subAccountPassword"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="subAccountPassword" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountPasswordAgain" class="col-xs-4 control-label">
                                                    确认密码:</label>
                                                <div class="col-xs-8">
                                                    <input type="password" class="form-control" id="subAccountPasswordAgain" name="subAccountPasswordAgain"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="subAccountPasswordAgain" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountMobilePhone" class="col-xs-4 control-label">
                                                    手机号:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="subAccountMobilePhone" name="subAccountMobilePhone"
                                                        maxlength="16" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="subAccountMobilePhone" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountEmial" class="col-md-4 control-label">
                                                    邮箱:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="subAccountEmial" name="subAccountEmial"
                                                        maxlength="64" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="subAccountEmial" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountRealName" class="col-xs-4 control-label">
                                                    真实姓名:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="subAccountRealName" name="subAccountRealName"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountPhone" class="col-xs-4 control-label">
                                                    固定电话:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="subAccountPhone" name="subAccountPhone"
                                                        maxlength="14" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="subAccountPhone" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="subAccountAddr" class="col-xs-4 control-label">
                                                    地址:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="subAccountAddr" name="subAccountAddr"
                                                        maxlength="512" />
                                                </div>
                                            </div>
                                            </form>
                                        </div>
                                        <div class="col-xs-7">
                                            <form class="form-horizontal" role="form">
                                            <div class="form-group">
                                                <label class="col-md-3 col-md-offset-4 control-label">
                                                    安全信息:</label>
                                            </div>
                                            <div class="form-group">
                                                <div class="radio col-xs-5 col-xs-offset-1">
                                                    <label class="radio-inline">
                                                        <input type="radio" onclick="ChangeToSubEver()" name="inlineRadioSubValid" id="inlineRadioSubValid_Evertime"
                                                            value="option1" />
                                                        永久有效
                                                    </label>
                                                    <label class="radio-inline" style="margin-left: 40px;">
                                                        <input type="radio" onclick="ChangeToSubEnd()" name="inlineRadioSubValid" id="inlineRadioSubValid_Endtime"
                                                            value="option2" />
                                                        截止
                                                    </label>
                                                </div>
                                                <div class="col-xs-6" style="padding-top: 14px; margin-left: -48px;">
                                                    <label>
                                                        <input class="Wdate" readonly="readonly" type="text" id="subAccoutValidDate" style="width: 160px;"
                                                            onclick="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss',isShowClear:false,readOnly:true,minDate:'%y-%M-#{%d}',startDate:'%y-%M-#{%d}'});return false;" />
                                                        有效
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-xs-3 control-label" style="margin-left: 8px;">
                                                    账户登录终端:</label>
                                                <div class="col-xs-8">
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" id="inlineChkSubPhone" value="option1" name="chksubAccount" />
                                                        手机客户端
                                                    </label>
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" id="inlineChkSubWeb" value="option2" name="chksubAccount" />
                                                        B/S客户端
                                                    </label>
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" id="inlineChkSubCS" value="option3" name="chksubAccount" />
                                                        C/S客户端
                                                    </label>
                                                </div>
                                            </div>
                                            </form>
                                        </div>
                                    </div>
                                    <div class="col-xs-12" style="text-align: center;">
                                        <button id="SaveSubAccoutInfo_Btn" class="btn btn-default" style="width: 120px;"
                                            onclick="javascript:$('#createSubAccountFrom').submit();return false;">
                                            保存
                                        </button>
                                        <p id="SaveSubAccoutInfo_Text" style="color: #30c5fc; display: none;">
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="accountImglist" style="display: none; position: relative;
                margin-bottom: 30px;">
            </div>
        </div>
    </div>
    <!-- 账户图片列表项 -->
    <div style="display: none">
        <div id="template-account-img-list-item">
            <div style="display: inline-block; margin-left: 128px; position: relative;">
                <img onclick="btnShowImgToInfo(this)" class="img-circle" src="" style="width: 75px;
                    height: 75px; cursor: pointer;" onerror="errorImg(this)" />
                <i></i>
                <div style="text-align: center;">
                    <center style="-o-text-overflow: ellipsis; text-overflow: ellipsis; width: 75px;
                        overflow: hidden; white-space: nowrap;">
                    </center>
                </div>
            </div>
        </div>
    </div>
    <!-- 账户图片列表 -->
    <div id="template-account-img-list" style="display: none;">
        <div style="width: 980px; margin-top: 60px;">
        </div>
    </div>
    <!-- 账户图片列表项对应的详细信息 -->
    <div id="template-account-img-info" style="display: none">
        <div style="background: url(../Images/icons/Bg_ProfileOpen.png) no-repeat; height: 16px;
            width: 110px; text-align: center;">
        </div>
        <div style="height: 478px; background-color: #f1f1f1; text-align: center; position: relative;
            top: 5px; margin-left: -15px; margin-right: -15px;">
            <div style="float: right; margin-right: 20px; padding-top: 3px;">
                <a id="editCurrentChildAccountInfo_a" href="#" onclick="EditCurrentChildAccountInfo(this);return false;">
                    <img src="../Images/icons/icon_Edit_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Edit_Hover.png'"
                        title="修改" onmouseout="this.src='../Images/icons/icon_Edit_Normal.png'" /></a>
                <a class="marginleft" href="#" onclick="FreezeSubAccount(this);return false;">
                    <img src="../Images/icons/icon_DisableUser_Normal.png" width="24px" height="24px"
                        onmouseover="this.src='../Images/icons/icon_DisableUser_Hover.png'" title="冻结"
                        onmouseout="this.src='../Images/icons/icon_DisableUser_Normal.png'" /></a>
                <a class="marginleft" id="showChildHistoryInfo_a" href="#" onclick="ShowChildHistoryInfo();return false;">
                    <img src="../Images/icons/icon_History_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_History_Hover.png'"
                        title="历史登录信息" onmouseout="this.src='../Images/icons/icon_History_Normal.png'" />
                </a><a class="marginleft" href="#" onclick="DeleteSubAccoutByModelOne();return false;">
                    <img src="../Images/icons/icon_Delete_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Delete_Hover.png'"
                        title="删除" onmouseout="this.src='../Images/icons/icon_Delete_Normal.png'" />
                </a>
            </div>
            <div id="img-info-title" style="top: 39px; width: 900px; text-align: center; position: absolute;">
                <a href="#" target="_blank" onclick="OnShowBanner(this);return false;">基本信息</a>
                <a href="#" target="_blank" onclick="OnShowBanner(this);return false;">账户安全</a>
                <a href="#" target="_blank" onclick="OnShowBanner(this);return false;">权限设置</a>
            </div>
            <div class="imageSpanBanner">
            </div>
            <div style="clear: both">
            </div>
            <div>
            </div>
        </div>
    </div>
    <!-- 图片上传 -->
    <div class="modal fade bs-example-modal-sm" id="modelPrimaryOfSubUpfileWin" tabindex="-1"
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
                        <input type="file" id="InputPrimaryOfSubFile" name="InputPrimaryOfSubFile" accept="image/*"
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
    <div class="modal fade bs-example-modal-sm" id="editPrimaryAccontPassWdModal" tabindex="-1"
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
                    <form class="form-horizontal" id="FromPrimaryAccontPassWd" role="form">
                    <div class="form-group">
                        <label for="OriginalPrimaryAccontPassWd" class="col-md-3 control-label">
                            原始密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="OriginalPrimaryAccontPassWd" name="OriginalPrimaryAccontPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="OriginalPrimaryAccontPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="NewPrimaryAccontPassWd" class="col-md-3 control-label">
                            新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="NewPrimaryAccontPassWd" name="NewPrimaryAccontPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="NewPrimaryAccontPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ConfirmPrimaryAccontPassword" class="col-md-3 control-label">
                            确认新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="ConfirmPrimaryAccontPassword" name="ConfirmPrimaryAccontPassword"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="ConfirmPrimaryAccontPassword" class="error">
                            </label>
                        </div>
                    </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn" onclick="javascript:$('#FromPrimaryAccontPassWd').submit();return false;">
                        保存</button>
                </div>
            </div>
        </div>
    </div>
    <!-- 账户历史登陆信息页面 -->
    <div class="modal fade bs-example-modal-sm" id="primaryAccoutHitoryModal" tabindex="-1"
        role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" style="width: 680px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        历史登陆信息</h4>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered" id="logPrimaryTable">
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
    <!--主账户列表展示模板 -->
    <div style="display: none">
        <div id="primary-template-panel-type">
            <div class="panel panel-default" style="margin-left: 60px; margin-right: 60px;">
                <div name="panelHeadingSelected" style="height: 40px; width: 4px; background-color: #30c5fc;
                    float: left; display: none;">
                </div>
                <div onclick="OnShowSubAccoutPanelBody(this);" class="panel-heading" role="tab" style="height: 40px;
                    background-color: #f1f1f1;">
                    <div class="col-md-8">
                        <a href="#" onclick="return false;"></a>
                        <label class="control-label" style="margin-left: 40px;">
                        </label>
                    </div>
                    <div style="float: right; margin-top: -5px; display: none;">
                        <img src="../Images/icons/icon_DisableUser_Normal.png" alt=""></img>
                    </div>
                </div>
                <div class="panel-collapse collapse" role="tabpanel" aria-labelledby="panelheadingtemp">
                    <div class="panel-body" style="border-style: none;">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 账户列表项对应的详细信息 -->
    <div id="primary-template-panelbody-div" style="display: none">
        <div style="height: 450px; background-color: #fbfbfb; text-align: center; position: relative;">
            <div style="float: right; margin-right: 20px; padding-top: 3px;">
                <a id="A1" href="#" onclick="EditCurrentChildAccountInfo(this);return false;">
                    <img src="../Images/icons/icon_Edit_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Edit_Hover.png'"
                        title="修改" onmouseout="this.src='../Images/icons/icon_Edit_Normal.png'" /></a>
                <a href="#" onclick="FreezeSubPanelAccount(this);return false;">
                    <img src="../Images/icons/icon_DisableUser_Normal.png" width="24px" height="24px"
                        onmouseover="this.src='../Images/icons/icon_DisableUser_Hover.png'" title="冻结"
                        onmouseout="this.src='../Images/icons/icon_DisableUser_Normal.png'" /></a>
                <a id="A2" href="#" onclick="ShowChildHistoryInfo();return false;">
                    <img src="../Images/icons/icon_History_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_History_Hover.png'"
                        title="历史登录信息" onmouseout="this.src='../Images/icons/icon_History_Normal.png'" />
                </a><a href="#" onclick="DeleteSubAccoutByModelTwo();return false;">
                    <img src="../Images/icons/icon_Delete_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Delete_Hover.png'"
                        title="删除" onmouseout="this.src='../Images/icons/icon_Delete_Normal.png'" />
                </a>
            </div>
            <div id="panelbody-div-img-info" style="padding-top: 5px; float: left; width: 900px;
                text-align: center;">
                <a style="margin-left: 12px;" href="#" onclick="OnShowPanelBanner(this);return false;">
                    基本信息</a> <a style="margin-left: 12px;" href="#" target="_blank" onclick="OnShowPanelBanner(this);return false;">
                        账户安全</a> <a style="margin-left: 12px;" href="#" target="_blank" onclick="OnShowPanelBanner(this);return false;">
                            权限设置</a>
            </div>
            <div class="panelImageSpanBanner">
            </div>
            <div style="clear: both">
            </div>
            <div>
            </div>
        </div>
    </div>
    <!-- 权限配置模板 -->
    <div id="subAuthorizeConfigMode" style="display: none">
        <div class="subAuthorizeDiv">
            <div class="col-md-12" style="text-align: left; margin-left: 25px; margin-top: 10px;">
                <label class="checkbox-inline">
                    <input type="checkbox" id="showMySpace" name="inlineChkCustomerToSee" value="option1">
                    显示我的空间
                </label>
                <label class="checkbox-inline">
                    <input type="checkbox" id="showEventMessage" name="inlineChkCustomerToSee" value="option2">
                    显示消息事件
                </label>
                <label class="checkbox-inline">
                    <input type="checkbox" id="showCloudVideo" name="inlineChkCustomerToSee" value="option3">
                    显示云视频
                </label>
            </div>
            <div class="col-xs-12 subAuthorizeTableHead">
                <div class="subAuthorizeTableHead videoDiv">
                    <a href="#" onclick="ShowSubAuthorizeVideo(this);return false;" style="color: #30c5fc;">
                        现场视频</a>
                </div>
                <div class="subAuthorizeTableHead backDiv">
                    <a href="#" onclick="ShowSubAuthorizePlay(this);return false;">回放</a>
                </div>
                <div class="subAuthorizeTableHead configDiv">
                    <a href="#" onclick="ShowEditSubAuthorize();return false;" style="color: #0a90e2;">编辑权限</a>
                </div>
            </div>
            <div class="col-xs-12 subAuthorizeTableDiv">
                <table class="subAuthorizeTable">
                </table>
                <table class="subAuthorizeTableNext">
                </table>
            </div>
            <div id="anthorizeHandleDiv" class="col-xs-4 col-xs-offset-4" style="margin-top: 8px;">
                <button type="button" onclick="btnSaveSubAuthorizeInfo();return false;" class="btn btn-default">
                    保存
                </button>
                <button type="button" onclick="CancleSaveSubAuthorizeInfo();return false;" class="btn btn-default">
                    取消
                </button>
            </div>
            <div id="anthorizeHandleMessageDiv" class="col-xs-4 col-xs-offset-4" style="margin-top: 8px;">
                <p style="color: #30c5fc;">
                    保存成功</p>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidPrimaryImageBase64Code" />
        <input type="hidden" id="hidePrimaryImgExt" />
        <input type="hidden" id="hidPrimaryOfSubImgBase64Code" />
        <input type="hidden" id="hidPrimaryOfSubImgExt" />
        <input type="hidden" id="hideSelectSubAccountID" />
        <input type="hidden" id="hidPrimaryAccontPhone" />
        <input type="hidden" id="hidPrimaryAccontEmail" />
        <input type="hidden" id="hidPrimaryOfSubImageBase64Code" />
        <input type="hidden" id="hidePrimaryOfSubImgExt" />
    </div>
    <script src="../Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="../Scripts/ajaxfileupload.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.js" type="text/javascript"></script>
    <script src="../JqueryPlugins/jquery-validation-1.13.1/dist/jquery.validate.extent.js" type="text/javascript"></script>
    <script type="text/javascript">
        var IsBaseInfoOrSafeInfo = true; //true-代表默认加载账户基本信息
        var AccountListStyle = 1; // 1-代表图片模式，2-代表列表模式
        var FormCurrentLoginCostomerInfo_Validator = null;
        $(function () {
            LoadCurrentPrimaryAccountDiv();

            //图片列表形式显示所有子账户
            LoadChildAccountImageListInfo();
        })

        //index.aspx主界面的查询功能
        function LoadChildAccountPanelListInfo() {
            $("#subAccordion").children().not("#subPanelOne").remove();
            //$("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-heading").next().collapse('show');
            var settings = $.extend({
                callback: null,
                isShowTip: true
            }, {});
            if (settings.isShowTip) {
                jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            var result = BsrCloudServer.Customer.SearchSubCustomers("");
            if (result && result.Code == 0) {
                if (result.customerReponseList.length > 0) {
                    for (var i = 0; i < result.customerReponseList.length; i++) {
                        if (0 == i) {
                            $("#hideSelectSubAccountID").val(result.customerReponseList[i].CustomerId);
                            $("#primary-template-panel-type .panel .panel-heading").prev().show();
                        } else {
                            $("#primary-template-panel-type .panel .panel-heading").prev().hide();
                        }
                        $("#primary-template-panel-type .panel .panel-heading div:eq(0) a").text(result.customerReponseList[i].CustomerName);
                        $("#primary-template-panel-type .panel .panel-heading").attr("id", result.customerReponseList[i].CustomerId);
                        $("#primary-template-panel-type .panel .panel-heading div:eq(0) label:eq(0)").text("");
                        if (result.customerReponseList[i].IsEnable == 0) {
                            if (result.customerReponseList[i].ForbiddenTime != null && result.customerReponseList[i].ForbiddenTime != "") {
                                $("#primary-template-panel-type .panel .panel-heading div:eq(0) label:eq(0)").text("冻结日期:" + result.customerReponseList[i].ForbiddenTime).css({
                                    "color": "red", "font-family": "Arial"
                                });
                            }
                            $("#primary-template-panel-type").find(".panel-heading img").parent().show();
                        }
                        $("#primary-template-panel-type").children().clone().appendTo($("#subAccordion"));
                    }
                    $("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-heading").next().collapse('show');

                    var cloneImgInfo = $("#primary-template-panelbody-div").children().clone();
                    cloneImgInfo.attr("name", "ListInfo");
                    //cloneImgInfo.eq(1).children().first().find("img").eq(1).attr("title", $(obj).attr("isenable"));
                    cloneImgInfo.children().last().attr("class", "panelBodyInfo");
                    $("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-body").html(cloneImgInfo);
                    IsBaseInfoOrSafeInfo = true;
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "PrimaryAccountManagement_Child.aspx",
                        success: function (data) {
                            $(".panelBodyInfo").html(data);

                        },
                        error: function (e) { alert("error") }
                    })
                }
                if ($.jNotify) {
                    $.jNotify._close();
                }
                if (settings.callback) {
                    settings.callback();
                }
            } else {
                alert("查询子用户失败!");
            }
        }

        //加载当前登陆主账户的基本信息 
        function LoadCurrentPrimaryAccountDiv() {
            $("#CurrentPrimaryAccountInfoDiv").show();
            $("#CurrentPriamryAccountSafeInfoDiv").hide();
            $("#accountSafeinfo_a").css({
                "background-image": "url(../Images/icons/icon_UserSafe_Normal.png)",
                "background-repeat": "no-repeat",
                "color": ""
            });
            $("#accountBaseinfo_a").css({
                "background-image": "url(../Images/icons/icon_EnableUser_Normal.png)",
                "background-repeat": "no-repeat",
                "color": "#30c5fc"
            });
            $("#saveCurrentPriamryAccountInfo_divId").hide();
            EnableOrDisPrimaryBaseInfo(false);
            GetSelfCustomer();
        }

        //添加当前登陆主账户的基本信息 验证
        function easyuiValidatePrimary() {
            FormCurrentLoginCostomerInfo_Validator = $("#FormCurrentLoginCostomerInfo").validate({
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
                    SaveCurrentPriamryAccountInfo();
                }
            });
        }

        //初始化创建子账户列表
        function InitCreateSubAccountList() {
            $('#subAccountUserName').val("");
            $('#subAccountPassword').val("");
            $('#subAccountPasswordAgain').val("");
            $('#subAccountMobilePhone').val("");
            $('#subAccountEmial').val("");
            $('#subAccountRealName').val("");
            $('#subAccountPhone').val("");
            $('#subAccountAddr').val("");
            $("#hidPrimaryOfSubImageBase64Code").val("");
            $("#hidePrimaryOfSubImgExt").val("");

            $("#inlineRadioSubValid_Endtime").attr("checked", "checked");
            var endtime = new Date();
            endtime.setMonth(endtime.getMonth() + 3);
            var dateTime = endtime.Format("yyyy-MM-dd hh:mm:ss");
            $("#subAccoutValidDate").val(dateTime);
            $("#subAccoutValidDate").removeAttr("disabled");
            $("[name='chksubAccount']").attr("checked", "checked");
        }

        /*--创建子账户基本信息验证--*/
        function easyuiSubAccountValidate() {
            var createSubAccountFrom_Validator = $("#createSubAccountFrom").validate({
                onkeyup: false,
                rules: {
                    subAccountUserName: {
                        required: true,
                        string: true,
                        alnum: true,
                        isExistedLogname: true
                    },
                    subAccountPassword: {
                        required: true,
                        password: true
                    },
                    subAccountPasswordAgain: {
                        required: true,
                        equalTo: "#subAccountPassword"
                    },
                    subAccountMobilePhone: { required: true, mobilephone: true, isExistedLogname: true },
                    subAccountEmial: { email: true, isExistedLogname: true },
                    subAccountPhone: { telephone: true }
                },
                messages: {
                    subAccountUserName: { required: "请输入用户名" },
                    subAccountPassword: { required: "请输入密码" },
                    subAccountPasswordAgain: {
                        required: "请重复输入密码",
                        equalTo: "密码不一致"
                    },
                    subAccountMobilePhone: { required: "请输入手机号" },
                    subAccountEmial: { email: "邮箱格式不正确" }
                },
                submitHandler: function () {
                    SaveSubAccoutInfo();
                }
            });
            createSubAccountFrom_Validator.resetForm();
        }

        //主账户修改密码验证
        function easyuiPrimaryPassWdValidate() {
            var FromPrimaryAccontPassWd_Validator = $("#FromPrimaryAccontPassWd").validate({
                onkeyup: false,
                rules: {
                    OriginalPrimaryAccontPassWd: {
                        required: true,
                        password: true
                    },
                    NewPrimaryAccontPassWd: {
                        required: true,
                        password: true
                    },
                    ConfirmPrimaryAccontPassword: {
                        required: true,
                        equalTo: "#NewPrimaryAccontPassWd"
                    }
                },
                messages: {
                    OriginalPrimaryAccontPassWd: { required: "请输入密码" },
                    NewPrimaryAccontPassWd: { required: "请输入密码" },
                    ConfirmPrimaryAccontPassword: {
                        required: "请重复输入密码",
                        equalTo: "密码不一致"
                    }
                },
                submitHandler: function () {
                    btnSavePrimaryAccontPassWd();
                }
            });
            FromPrimaryAccontPassWd_Validator.resetForm();
        }

        //获取当前登陆用户的信息
        function GetSelfCustomer() {
            var result = BsrCloudServer.Customer.getCustomerInfo(0);
            if (result && result.Code == 0) {
                $("#primaryAccontMobilePhone").val(result.customerReponse.ReceiverCellPhone);
                $("#primaryAccountEmail").val(result.customerReponse.ReceiverEmail);
                $("#primaryAccountRealName").val(result.customerReponse.ReceiverName);
                $("#primaryAccountTelphone").val(result.customerReponse.AccountTelephone);
                $("#primaryAccountAddress").val(result.customerReponse.AccountCompanyAddress);
                var path = "../" + result.customerReponse.ImagePath;
                $("#primaryAccountImg").attr("src", path);
                $("#hidPrimaryAccontPhone").val(result.customerReponse.ReceiverCellPhone);
                $("#hidPrimaryAccontEmail").val(result.customerReponse.ReceiverEmail);
            }
            else {
                $("#savePromptMessage").text("获取登陆信息失败");
                $("#savePromptMessage").show();
            }
        }

        //获取当前登陆用户的安全信息
        function GetSelfSafeInfo() {
            var result = BsrCloudServer.Customer.getCustomerInfo(0);
            if (result && result.Code == 0) {
                if (null == result.customerReponse.ValidTime || "" == result.customerReponse.ValidTime) {
                    $("#priamryAccontValid").text("永久有效");
                } else {
                    var endtime = new Date(result.customerReponse.ValidTime);
                    var dateTime = endtime.Format("yyyy-MM-dd hh:mm:ss");
                    $("#priamryAccontValid").text("截止" + dateTime);
                }
                strOrgLoginTerminal = result.customerReponse.LoginTypes;
                $("[name ='chkPrimaryAccountLoginWay']").prop("checked", false);
                if (result.customerReponse.LoginTypes.indexOf("3") >= 0 || result.customerReponse.LoginTypes.indexOf("4") >= 0) {
                    $("#primaryPhoneClient").prop("checked", true);
                }
                if (result.customerReponse.LoginTypes.indexOf("1") >= 0) {
                    $("#primaryBSPhoneClient").prop("checked", true);
                }
                if (result.customerReponse.LoginTypes.indexOf("2") >= 0) {
                    $("#primaryCSClient").prop("checked", true);
                }
            }
            else {
                jError("获取安全信息失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        //加载当前登陆的主账户的安全信息
        function LoadCurrentPrimaryAccountSafeDiv() {
            $("#CurrentPrimaryAccountInfoDiv").hide();
            $("#CurrentPriamryAccountSafeInfoDiv").show();
            $("#accountSafeinfo_a").css({
                "background-image": "url(../Images/icons/icon_UserSafe_Hover.png)",
                "background-repeat": "no-repeat",
                "color": "#30c5fc"
            });
            $("#accountBaseinfo_a").css({
                "background-image": "url(../Images/icons/icon_Account_Normal.png)",
                "background-repeat": "no-repeat",
                "color": ""
            });
            EnableOrDisPrimarySafeInfo(false);
            GetSelfSafeInfo();
        }

        //修改当前登陆主账户的基本信息
        function EditCurrentPriamryAccountInfo() {
            EnableOrDisPrimaryBaseInfo(true);
            easyuiValidatePrimary();
        }

        //显示当前登陆的主账户的日志信息
        function ShowCurrentPriamryAccountHistoryInfo() {
            $("#myModalLabel").html($("#primaryAccountName").text() + " 历史登陆信息");
            $("#primaryAccoutHitoryModal").modal('show');
            $("#logPrimaryTable tr:gt(0)").empty();
            var result = BsrCloudServer.Customer.GetSelfLoginInfo();
            if (result && result.Code == 0) {
                if (result.operaterLogList.length > 0) {
                    for (var i = 0; i < result.operaterLogList.length; i++) {
                        var tr = "<tr><td>" + result.operaterLogList[i].OperaterTime.toString() + "</td><td>" + result.operaterLogList[i].AgentType +
                         "</td><td>" + result.operaterLogList[i].AgentVersion + "</td></tr>";
                        $("#logPrimaryTable").append(tr);
                    }
                }
            }
        }

        //保存当前登陆主账户的基本信息
        function SaveCurrentPriamryAccountInfo() {
            var result = updatePrimaryAccountBaseInfo();
            if (result && result.Code == 0) {
                EnableOrDisPrimaryBaseInfo(false);
                $("#primaryAccountImg").unbind("click", ShowUpdatePrimaryOfSubFilePage);
                $("#saveCurrentPriamryAccountInfo_divId").hide(500, function () {
                    $("#savePromptMessage").text("保存成功");
                    $("#savePromptMessage").show();
                });
            }
            else {
                $("#savePromptMessage").text("保存失败");
                $("#savePromptMessage").show();
            }
        }

        //更新当前主账户的基本信息
        function updatePrimaryAccountBaseInfo() {
            var result = false;
            var user = new Object();
            user.ReceiverCellPhone = $.trim($("#primaryAccontMobilePhone").val());
            user.ReceiverEmail = $.trim($.trim($("#primaryAccountEmail").val()));
            user.ReceiverName = $.trim($("#primaryAccountRealName").val());
            user.AccountTelephone = $.trim($("#primaryAccountTelphone").val());
            user.AccountCompanyAddress = $.trim($("#primaryAccountAddress").val());
            user.ImageByteBase64 = "";
            user.ExtName = "";
            var userJson = JSON.stringify(user);
            return BsrCloudServer.Customer.updateCustomer(userJson);
        }

        //取消对当前登陆的主账户信息的修改
        function CancelSaveCurrentPriamryAccountInfo() {
            if (FormCurrentLoginCostomerInfo_Validator) {
                FormCurrentLoginCostomerInfo_Validator.resetForm();
            }
            EnableOrDisPrimaryBaseInfo(false);
            $("#saveCurrentPriamryAccountInfo_divId").hide();
            GetSelfCustomer();
        }

        //重置当前登陆的主账户的密码
        function ResetPrimaryAccountPassWd() {
            $("#FromPrimaryAccontPassWd input").val("");
            easyuiPrimaryPassWdValidate();
            $("#editPrimaryAccontPassWdModal").modal('show');
        }

        //保存当前登陆的主账户密码
        function btnSavePrimaryAccontPassWd() {
            var result = SavePrimaryAccountPassWord();
            if (result && result.Code == 0) {
                $("#editPrimaryAccontPassWdModal").modal('hide');
                jSuccess("密码修改成功", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            } else {
                jError("密码修改失败:" + result.Message, { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        //保存密码 
        function SavePrimaryAccountPassWord() {
            var userPassWd = new Object();
            userPassWd.OldPassWord = $.md5($.trim($("#OriginalPrimaryAccontPassWd").val()));
            userPassWd.NewPassWord = $.md5($.trim($("#NewPrimaryAccontPassWd").val()));
            return BsrCloudServer.Customer.SaveAccountPassWord(userPassWd);
        }

        //修改当前登陆的主账户的安全信息  登陆方式 3,4-手机客户端 1-BS端登陆(WEB) 2-CS端登陆
        function EditOrSavePrimaryAccountSafeInfo(obj) {
            if ($.trim($(obj).text()) == "修改") {
                EnableOrDisPrimarySafeInfo(true);
                $(obj).text("保存").css("color", "");
                $("#cancleEditPrimaryAccountSafeInfo_a").show();
            }
            else {
                SavePrimaryAccountSafeInfo(obj);
            }
        }

        //保存当前登陆的主账户的安全信息
        function SavePrimaryAccountSafeInfo(obj) {
            var loginway = "";
            $("[name='chkPrimaryAccountLoginWay']").each(function () {
                if ($(this)[0].checked) {
                    if ("" == loginway) {
                        if ($(this).attr("id") == "primaryPhoneClient") {
                            loginway = "3,4";
                        }
                        else if ($(this).attr("id") == "primaryBSPhoneClient") {
                            loginway = "1";
                        }
                        else {
                            loginway = "2";
                        }
                    }
                    else {
                        if ($(this).attr("id") == "primaryPhoneClient") {
                            loginway += ",3,4";
                        }
                        else if ($(this).attr("id") == "primaryBSPhoneClient") {
                            loginway += ",1";
                        }
                        else {
                            loginway += ",2";
                        }
                    }
                }
            });
            if ("" == loginway) {
                if (!confirm("如果您不选择登陆终端,您将无法使用该账户登陆!")) {
                    return;
                }
            }
            var result = BsrCloudServer.Customer.UpdateCustomerSafeInfo(loginway);
            if (result && result.Code == 0) {
                EnableOrDisPrimarySafeInfo(false);
                $("#savePromptMessage").show();
                $(obj).text("修改").css("color", "#808080");
                $("#cancleEditPrimaryAccountSafeInfo_a").hide();
                $("#savePromptMessage").text("保存成功");
            }
            else {
                $("#savePromptMessage").show();
                $("#savePromptMessage").text("保存失败");
            }
        }

        //取消当前登陆的主账户的安全信息的修改操作
        function CancelSavePrimaryAccountSafeInfo() {
            EnableOrDisPrimarySafeInfo(false);
            $("#editPrimaryAccountSafeInfo_a").text("修改").css("color", "#808080");
            $("#cancleEditPrimaryAccountSafeInfo_a").hide();
            GetSelfSafeInfo();
        }

        //显示创建子账户界面
        function ShowCollapsedSubAccoutPanelBody() {
            if ($("#primaryOfSubCollapsePanel").hasClass('in')) {
                $("#primaryOfSubCollapsePanel").collapse('hide');
            }
            else {
                $("#primaryOfSubCollapsePanel").collapse('show');
                $("#primaryOfSubAccountImg").hide();
                $("#finalPrimaryOfSubAccountImg").show();
                $("#SaveSubAccoutInfo_Text").hide();
                InitCreateSubAccountList();
                easyuiSubAccountValidate();
            }
        }

        //显示图片上传界面
        function ShowSubFilePage() {
            $("#modelPrimaryOfSubUpfileWin").modal('show');
        }

        //显示当前登陆的主账户图片上传界面
        function ShowPrimaryFilePage() {
            $("#modelPrimaryOfSubUpfileWin").modal('show');
        }

        //切换到永久有效
        function ChangeToSubEver() {
            $("#subAccoutValidDate").val("");
            $("#subAccoutValidDate").attr("disabled", "disabled")
        }

        //切换到截止
        function ChangeToSubEnd() {
            var endtime = new Date();
            endtime.setMonth(endtime.getMonth() + 3);
            $("#subAccoutValidDate").removeAttr("disabled");
            $("#subAccoutValidDate").val(endtime.Format("yyyy-MM-dd hh:mm:ss"));
        }

        //保存新创建的子账户的信息
        function SaveSubAccoutInfo() {
            var flag = true;
            var loginway = "";
            $("[name='chksubAccount']").each(function () {
                if ($(this)[0].checked) {
                    flag = false;
                    if ($(this)[0].id == "inlineChkSubPhone") {
                        if ("" == loginway) {
                            loginway = "3,4";
                        }
                        else {
                            loginway += ",3,4";
                        }
                    }
                    else if ($(this)[0].id == "inlineChkSubWeb") {
                        if ("" == loginway) {
                            loginway = "1";
                        }
                        else {
                            loginway += ",1";
                        }
                    }
                    else if ($(this)[0].id == "inlineChkSubCS") {
                        if ("" == loginway) {
                            loginway = "2";
                        }
                        else {
                            loginway += ",2";
                        }
                    }
                }
            })
            if (flag) {
                if (!confirm("如果您不选择登陆终端,您将无法使用该账户登陆!")) {
                    return;
                }
            }
            var userinfo = new Object();
            userinfo.CustomerName = $('#subAccountUserName').val();
            userinfo.Password = $.md5($.trim($("#subAccountPassword").val()));
            userinfo.ReceiverName = $.trim($("#subAccountRealName").val());
            userinfo.ReceiverEmail = $.trim($("#subAccountEmial").val());
            userinfo.ReceiverCellPhone = $.trim($("#subAccountMobilePhone").val());
            userinfo.AccountTelephone = $.trim($("#subAccountPhone").val());
            userinfo.AccountCompanyAddress = $.trim($("#subAccountAddr").val());
            userinfo.ImageByteBase64 = "";
            userinfo.ExtName = "";
            if ($("#inlineRadioSubValid_Evertime").is(":checked")) {
                userinfo.ValidTime = null;
            }
            else {
                userinfo.ValidTime = $('#subAccoutValidDate').val();
            }
            userinfo.LoginTypes = loginway;
            var result = BsrCloudServer.Customer.AddSubCustomer(userinfo);
            if (result && result.Code == 0) {
                if (1 == AccountListStyle) {
                    AfterSuccessSaveSubAccoutInfo_ImglistStyle();
                } else {
                    AfterSuccessSaveSubAccoutInfo_PanleListStyle(result);
                }
            }
            else {
                $("#SaveSubAccoutInfo_Text").show(function () {
                    $("#SaveSubAccoutInfo_Text").text("保存失败");
                })
            }
        }

        function AfterSuccessSaveSubAccoutInfo_ImglistStyle() {
            $("#SaveSubAccoutInfo_Text").show(1000, function () {
                $("#SaveSubAccoutInfo_Text").text("保存成功");
                LoadChildAccountImageListInfo();
                InitCreateSubAccountList();
            });
        }

        function AfterSuccessSaveSubAccoutInfo_PanleListStyle(result) {
            $("#hideSelectSubAccountID").val(result.CustomerId);
            $("#primary-template-panel-type .panel .panel-heading div:eq(0) a").text($('#subAccountUserName').val());
            $("#primary-template-panel-type .panel .panel-heading").attr("id", result.CustomerId);
            $("#subPanelOne").after($("#primary-template-panel-type").children().clone());
            $("#subAccordion").children().not("#subPanelOne").each(function () {
                if ($(this).find(".panel-heading").next().hasClass("in")) {
                    $(this).find(".panel-body").children().remove();
                    $(this).find(".panel-heading").next().collapse('hide');
                }
            })
            $("#subAccordion").children().eq(0).find(".panel-heading").next().collapse('hide');
            $("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-heading").next().collapse('show');
            $.ajax({
                contentType: "charset=UTF-8",
                type: "post",
                cache: false,
                url: "PrimaryAccountManagement_Child.aspx",
                success: function (data) {
                    $("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-body").html(data);
                },
                error: function (e) { alert("error") }
            })
        }

        //图片上传
        function btnUpdatePrimaryOfSubImage() {
            readPrimaryOfSubImageURLAndSave($("#InputPrimaryOfSubFile")[0]);
        }

        //读取图片的URL并保存图片
        function readPrimaryOfSubImageURLAndSave(input) {
            if ($("#InputPrimaryOfSubFile").val() == "") {
                $("#savePromptMessage").show(500, function () {
                    $("#savePromptMessage").text("上传图片不能为空!");
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
                    $("#savePromptMessage").show(500, function () {
                        $("#savePromptMessage").text("上传图片格式不正确!");
                    });
                    return;
                }
                $.ajaxFileUpload({
                    url: '../PageHandler/UpLoadImage.ashx',
                    type: 'post',
                    secureuri: false, //一般设置为false
                    fileElementId: 'InputPrimaryOfSubFile', // 上传文件的id、name属性名
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
                                $("#primaryAccountImg").attr("src", path_upLoadImg);
                            }
                            else {
                                $("#savePromptMessage").show(500, function () {
                                    $("#savePromptMessage").text("上传图片失败!");
                                });
                            }
                        }
                        else {
                            $("#savePromptMessage").show(500, function () {
                                $("#savePromptMessage").text("上传图片失败!");
                            });
                        }
                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                });
            } else {
                //IE下，使用滤镜
                var docObj = document.getElementById('InputPrimaryOfSubFile');
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
                    $("#savePromptMessage").show(500, function () {
                        $("#savePromptMessage").text("上传图片格式不正确!");
                    });
                    return;
                }
                $.ajaxFileUpload({
                    url: '../PageHandler/UpLoadImage.ashx',
                    type: 'post',
                    secureuri: false, //一般设置为false
                    fileElementId: 'InputPrimaryOfSubFile', // 上传文件的id、name属性名
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
                                $("#primaryAccountImg").attr("src", path_upLoadImg);
                            }
                            else {
                                $("#savePromptMessage").show(500, function () {
                                    $("#savePromptMessage").text("上传图片失败!");
                                });
                            }
                        }
                        else {
                            $("#savePromptMessage").show(500, function () {
                                $("#savePromptMessage").text("上传图片失败!");
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

        //绑定显示图片上传界面
        function ShowUpdatePrimaryOfSubFilePage() {
            $("#modelPrimaryOfSubUpfileWin").modal('show');
        }

        //启用或者禁用用户的基本信息
        function EnableOrDisPrimaryBaseInfo(obj) {
            $("#savePromptMessage").hide();
            if (obj) {//启用
                $("#FormCurrentLoginCostomerInfo input").removeAttr("disabled");
                $("#saveCurrentPriamryAccountInfo_divId").show();
                $("#btnPriamryAccountGroupDiv").hide();
                $("#btnSavePriamryAccountBaseInfoDiv").show();
                $("#primaryAccountImg").bind("click", ShowUpdatePrimaryOfSubFilePage);
            } else {
                $("#FormCurrentLoginCostomerInfo input").attr("disabled", "disabled");
                $("#btnSavePriamryAccountBaseInfoDiv").hide();
                $("#primaryAccountImg").unbind("click", ShowUpdatePrimaryOfSubFilePage);
            }
        }

        //启用或者禁用用户的安全信息
        function EnableOrDisPrimarySafeInfo(obj) {
            $("#savePromptMessage").hide();
            $("#saveCurrentPriamryAccountInfo_divId").hide();
            if (obj) {//启用
                $("[name='chkPrimaryAccountLoginWay']").removeAttr("disabled");
                $("#CurrentPrimaryAccountSafeHandler").hide();
            } else {
                $("[name='chkPrimaryAccountLoginWay']").attr("disabled", "disabled");
                $("#CurrentPrimaryAccountSafeHandler").show();
                $("#editPrimaryAccountSafeInfo_a").text("修改").css("color", "#808080");
                $("#cancleEditPrimaryAccountSafeInfo_a").hide();
            }
        }

        //显示选中的子账户信息
        function OnShowSubAccoutPanelBody(obj) {
            ShowOneSubPanel(obj);
            //            $.ajax({
            //                contentType: "charset=UTF-8",
            //                type: "post",
            //                cache: false,
            //                url: "Pages/PrimaryAccountManagement_Child.aspx",
            //                success: function (data) {
            //                    $(obj).closest(".panel-heading").next().children(".panel-body").html(data);
            //                },
            //                error: function (e) { alert("error") }
            //            })

        }

        //删除子账户
        function DeleteSubAccout(obj) {
            var SubCustomerIds = new Object();
            var mySubCustomerId = [];
            mySubCustomerId.push($(obj).closest(".panel-heading").attr("id"));
            SubCustomerIds.SubCustomer = mySubCustomerId;
            var result = BsrCloudServer.Customer.DeleteSubCustomer(SubCustomerIds);
            if (result && result.Code == 0) {
                if ($(obj).closest(".panel-heading").next().hasClass("in")) {
                    $(obj).closest(".panel").remove();
                    if ($("#subAccordion").children().not("#subPanelOne").length > 0) {
                        $("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-heading").next().collapse('show');
                        $("#hideSelectSubAccountID").val($("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-heading").attr("id"));
                        $.ajax({
                            contentType: "charset=UTF-8",
                            type: "post",
                            cache: false,
                            url: "PrimaryAccountManagement_Child.aspx",
                            success: function (data) {
                                $("#subAccordion").children().not("#subPanelOne").eq(0).find(".panel-body").html(data);
                            },
                            error: function (e) { alert("error") }
                        })
                    }
                }
                else {
                    $(obj).closest(".panel").remove();
                }

                alert("删除成功!");
            }
            else {
                alert("删除失败!");
            }
        }

        //冻结或者解冻子账户
        function FreezeOrThawSubAccount(obj) {
            var userinfo = new Object();
            userinfo.SubCustomerId = $(obj).closest(".panel-heading").attr("id");
            if ($(obj).text() == "冻结") {
                userinfo.IsEnable = 0;
            }
            else {
                userinfo.IsEnable = 1;
            }
            var result = BsrCloudServer.Customer.EnableSubCustomer(userinfo);
            if (result && result.Code == 0) {
                if ($(obj).text() == "冻结") {
                    var freezeDate = new Date();
                    freezeDate = freezeDate.toLocaleDateString() + freezeDate.toLocaleTimeString();
                    $(obj).closest(".panel-heading").find("div:eq(0) label:eq(0)").text("冻结日期:" + freezeDate).css({
                        "color": "red", "font-family": "Arial"
                    });
                    $(obj).text("解冻");
                }
                else {
                    $(obj).closest(".panel-heading").find("div:eq(0) label:eq(0)").text("");
                    $(obj).text("冻结");
                }
            }
            else {
                if ($(obj).text() == "冻结") {
                    jError("子账户冻结失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                }
                else {
                    jError("子账户解冻失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                }
            }
        }

        function FreezeSubPanelAccount(obj) {
            if (obj) {
                var flag = false;
                var userinfo = new Object();
                userinfo.SubCustomerId = $("#hideSelectSubAccountID").val();
                if ($.trim($(obj).find("img").attr("title")) == "冻结") {
                    userinfo.IsEnable = 0;
                    flag = true;
                }
                else {
                    userinfo.IsEnable = 1;
                    flag = false;
                }
                var result = BsrCloudServer.Customer.EnableSubCustomer(userinfo);
                if (result && result.Code == 0) {
                    if (flag) {
                        $(obj).find("img").attr("title", "解冻");
                        $("#" + $("#hideSelectSubAccountID").val()).find("img").parent().show();
                        var freezeDate = new Date();
                        freezeDate = freezeDate.toLocaleDateString() + freezeDate.toLocaleTimeString();
                        $("#" + $("#hideSelectSubAccountID").val()).find("label").text("冻结日期:" + freezeDate).css({
                            "color": "red", "font-family": "Arial"
                        });
                    }
                    else {
                        $(obj).find("img").attr("title", "冻结");
                        $("#" + $("#hideSelectSubAccountID").val()).find("img").parent().hide();
                        $("#" + $("#hideSelectSubAccountID").val()).find("label").text("");
                    }
                }
                else {

                }
            }
        }

        //冻结子账户
        function FreezeSubAccount(obj) {
            if (obj) {
                var flag = false;
                var userinfo = new Object();
                userinfo.SubCustomerId = $("#hideSelectSubAccountID").val();
                if ($.trim($(obj).find("img").attr("title")) == "冻结") {
                    userinfo.IsEnable = 0;
                    flag = true;
                }
                else {
                    userinfo.IsEnable = 1;
                    flag = false;
                }
                var result = BsrCloudServer.Customer.EnableSubCustomer(userinfo);
                if (result && result.Code == 0) {
                    if (flag) {
                        $(obj).find("img").attr("title", "解冻");
                        $("#" + $("#hideSelectSubAccountID").val()).next().addClass("ImgOn");
                    }
                    else {
                        $(obj).find("img").attr("title", "冻结");
                        $("#" + $("#hideSelectSubAccountID").val()).next().removeClass("ImgOn");
                    }
                }
                else {

                }
            }
        }

        //删除子账户
        function DeleteSubAccoutByModelTwo() {
            var SubCustomerIds = new Object();
            var mySubCustomerId = [];
            mySubCustomerId.push($("#hideSelectSubAccountID").val());
            SubCustomerIds.SubCustomer = mySubCustomerId;
            var result = BsrCloudServer.Customer.DeleteSubCustomer(SubCustomerIds);
            if (result && result.Code == 0) {
                LoadChildAccountPanelListInfo();
            }
            else {
                jError("删除失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        //删除子账户
        function DeleteSubAccoutByModelOne() {
            var SubCustomerIds = new Object();
            var mySubCustomerId = [];
            mySubCustomerId.push($("#hideSelectSubAccountID").val());
            SubCustomerIds.SubCustomer = mySubCustomerId;
            var result = BsrCloudServer.Customer.DeleteSubCustomer(SubCustomerIds);
            if (result && result.Code == 0) {
                LoadChildAccountImageListInfo();
                jSuccess("删除成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            else {
                jError("删除失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        var treeObj;
        //授权或取消授权子账户
        function AuthorizeSubAccount(obj) {
            ShowOneSubPanel(obj);
            $("#hideSelectSubAccountID").val($(obj).closest(".panel-heading").attr("id"));
            $.ajax({
                contentType: "charset=UTF-8",
                type: "post",
                cache: false,
                url: "PrimaryAccountManagement_ChildAuthorize.aspx",
                success: function (data) {
                    $(obj).closest(".panel-heading").next().children(".panel-body").html(data);
                    if ($(obj).text() == "授权") {
                        $(obj).text("取消授权");
                    }
                    else {
                        $(obj).text("授权");
                    }
                },
                error: function (e) { alert("error") }
            })
        }

        //保存当前选中子账户的权限
        function btnSaveAuthorizeSub() {
            var SubAuthorize = new Object();
            var AuthorizeSubResponse = new Object();
            //var AuthorizeSubResponseArray = [];
            AuthorizeSubResponse.CustomerId = $("#hideSelectSubAccountID").val();
            if (treeObj.getCheckedNodes(true).length > 0) {
                var flag = false;
                var AuthorizeSubResponseArray = [];
                for (var i = 0; i < treeObj.getCheckedNodes(true).length; i++) {
                    if (treeObj.getCheckedNodes(true)[i].isParent && treeObj.getCheckedNodes(true)[i].children.length > 0) {
                        flag = true;
                        var AuthorizeDeviceResponse = new Object();
                        AuthorizeDeviceResponse.DeviceId = treeObj.getCheckedNodes(true)[i].id;
                        AuthorizeDeviceResponse.IsEnable = 1;
                        var AuthorizeDeviceResponseArray = [];
                        for (var j = 0; j < treeObj.getCheckedNodes(true)[i].children.length; j++) {
                            if (treeObj.getCheckedNodes(true)[i].children[j].checked) {
                                var AuthorizeChannelResponse = new Object();
                                AuthorizeChannelResponse.ChannelId = treeObj.getCheckedNodes(true)[i].children[j].id;
                                AuthorizeChannelResponse.IsEnable = 1;
                                var AuthorizePermissionResponseArray = [];
                                if (treeObj.getCheckedNodes(true)[i].children[j].Video == 1) {
                                    var AuthorizePermissionResponse = new Object();
                                    AuthorizePermissionResponse.PermissionName = 'Video';
                                    AuthorizePermissionResponse.IsEnable = 1;
                                    AuthorizePermissionResponseArray.push(AuthorizePermissionResponse);
                                }
                                if (treeObj.getCheckedNodes(true)[i].children[j].Playback == 1) {
                                    var AuthorizePermissionResponse = new Object();
                                    AuthorizePermissionResponse.PermissionName = 'Playback';
                                    AuthorizePermissionResponse.IsEnable = 1;
                                    AuthorizePermissionResponseArray.push(AuthorizePermissionResponse);
                                }
                                AuthorizeChannelResponse.authorizePermissionResponse = AuthorizePermissionResponseArray;
                                AuthorizeDeviceResponseArray.push(AuthorizeChannelResponse);
                            }
                        }
                        AuthorizeDeviceResponse.authorizeChannelResponse = AuthorizeDeviceResponseArray;
                        AuthorizeSubResponseArray.push(AuthorizeDeviceResponse);
                    }
                }
                AuthorizeSubResponse.authorizeDeviceResponseList = AuthorizeSubResponseArray;
                //概况权限
                var AuthorizePermissionResponseSelfArray = [];
                $("[name='inlineChkCustomerToSee']").each(function () {
                    if ($(this)[0].checked) {
                        if ($(this)[0].id == "showMySpace") {
                            var AuthorizePermissionResponseSelf = new Object();
                            AuthorizePermissionResponseSelf.PermissionName = 'MySpace';
                            AuthorizePermissionResponseSelf.IsEnable = 1;
                            AuthorizePermissionResponseSelfArray.push(AuthorizePermissionResponseSelf);
                        }
                        else if ($(this)[0].id == "showEventMessage") {
                            var AuthorizePermissionResponseSelf = new Object();
                            AuthorizePermissionResponseSelf.PermissionName = 'EventMessage';
                            AuthorizePermissionResponseSelf.IsEnable = 1;
                            AuthorizePermissionResponseSelfArray.push(AuthorizePermissionResponseSelf);
                        }
                        else if ($(this)[0].id == "showCloudVideo") {
                            var AuthorizePermissionResponseSelf = new Object();
                            AuthorizePermissionResponseSelf.PermissionName = 'CloudVideo';
                            AuthorizePermissionResponseSelf.IsEnable = 1;
                            AuthorizePermissionResponseSelfArray.push(AuthorizePermissionResponseSelf);
                        }
                    }
                });
                AuthorizeSubResponse.subCustomerPermissionList = AuthorizePermissionResponseSelfArray;
                SubAuthorize.authorizeSubResponse = AuthorizeSubResponse;
                if (flag) {//保存
                    var result = BsrCloudServer.Customer.AuthorizeSubByPrimary(SubAuthorize);
                    if (result && result.Code == 0) {
                        alert("授权成功!");
                    } else {
                        alert("授权失败!");
                    }
                }
            }
        }

        //同一时间只显示一个TAB页
        function ShowOneSubPanel(obj) {
            $("[name='panelHeadingSelected']").hide();
            if (!$(obj).next().hasClass("in")) {
                $("#hideSelectSubAccountID").val($(obj).attr("id"));
                $("#subAccordion").children().not("#subPanelOne").each(function () {
                    if ($(this).find(".panel-heading").next().hasClass("in")) {
                        $(this).find(".panel-body").children().remove();
                        $(this).find(".panel-heading").next().collapse('hide');
                    }
                })
                $(obj).prev().show();
                $(obj).next().collapse('show');

                var cloneImgInfo = $("#primary-template-panelbody-div").children().clone();
                cloneImgInfo.children().last().attr("class", "panelBodyInfo");
                $(obj).next().find(".panel-body").html(cloneImgInfo);
                IsBaseInfoOrSafeInfo = true;
                $.ajax({
                    contentType: "charset=UTF-8",
                    type: "post",
                    cache: false,
                    url: "PrimaryAccountManagement_Child.aspx",
                    success: function (data) {
                        $(".panelBodyInfo").html(data);

                    },
                    error: function (e) { alert("error") }
                })
            }
            else {
                $(obj).next().find(".panel-body").children().remove();
                $(obj).next().collapse('hide');
            }
        }

        //向子账户列表中添加子账户
        function AddSubAccountToImageList(id, name) {
            if (id && id > 0) {
                var listitem = $("#template-account-img-list-item").children().clone();
                listitem.find("img").attr("id", id);
                listitem.find("img").attr("isenable", "冻结");
                listitem.find("img").attr("src", "../customerImage/default.jpg");
                listitem.find("center").attr("title", name).css("cursor", "pointer");
                listitem.find("center").text(name);
                listitem.prependTo($("#accountImglist").children().eq(0));
            }
        }

        //初始化加载主账户下子账户的图片列表
        function LoadChildAccountImageListInfo() {
            AccountListStyle = 1;
            if ($("#accountImglist").children().length > 0) {
                $("#accountImglist").children().remove();
            }
            $("#accountImglist").show();
            var settings = $.extend({
                callback: null,
                isShowTip: true
            }, {});
            if (settings.isShowTip) {
                jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            var result = BsrCloudServer.Customer.SearchSubCustomers("");
            if (result && result.Code == 0) {
                if (result.customerReponseList.length > 0) {
                    for (var i = 0; i < result.customerReponseList.length; i++) {
                        var listitem = $("#template-account-img-list-item").children().clone();
                        var status = "冻结";
                        if (result.customerReponseList[i].IsEnable == 0) {
                            listitem.find("i").addClass("ImgOn");
                            status = "解冻";
                        }
                        listitem.find("img").attr("id", result.customerReponseList[i].CustomerId);
                        listitem.find("img").attr("isenable", status);
                        listitem.find("img").attr("src", "../" + result.customerReponseList[i].ImagePath);

                        var CustomerName = result.customerReponseList[i].CustomerName;
                        //                        if (CustomerName.length > 6) {
                        //                            CustomerName = CustomerName.substring(0, 6) + "...";
                        //                        }
                        listitem.find("center").attr("title", result.customerReponseList[i].CustomerName).css("cursor", "pointer");
                        listitem.find("center").text(CustomerName);
                        listitem.appendTo($("#template-account-img-list").children());
                        if (Math.floor(i / 4) < Math.floor(result.customerReponseList.length / 4) && 0 == (i + 1) % 4) {
                            $("#template-account-img-list").children().clone().css("height", "106px").appendTo($("#accountImglist"));
                            $("#template-account-img-list").children().children().remove();

                        }
                        else if (Math.floor(i / 4) == Math.floor(result.customerReponseList.length / 4) && i == (result.customerReponseList.length - 1)) {
                            $("#template-account-img-list").children().clone().css("height", "106px").appendTo($("#accountImglist"));
                            $("#template-account-img-list").children().children().remove();
                        }
                    }
                }
            } else {
                jError("加载子用户信息失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            if ($.jNotify) {
                $.jNotify._close();
            }
            if (settings.callback) {
                settings.callback();
            }
        }

        //点击图片显示账户图片对应的详细信息
        function btnShowImgToInfo(obj) {
            if (obj) {
                if ($(obj).parents().eq(1).next("[name='ImgListInfo']").length > 0 &&
            ($(obj).parent().position().left + 105).toFixed(0) + "px" == $(obj).parents().eq(1).next().css("left")) {
                    $("[name='ImgListInfo']").remove();
                }
                else {
                    $("[name='ImgListInfo']").remove();
                    var cloneImgInfo = $("#template-account-img-info").children().clone();
                    cloneImgInfo.attr("name", "ImgListInfo");
                    cloneImgInfo.eq(1).children().first().find("img").eq(1).attr("title", $(obj).attr("isenable"));
                    cloneImgInfo.eq(1).children().last().attr("class", "imageInfo row");
                    $(obj).parents().eq(1).after(cloneImgInfo);
                    var left = ($(obj).parent().position().left + 105).toFixed(0);
                    $(obj).parents().eq(1).next().css({ "position": "absolute", "left": left + "px", "top": (96 + $(obj).parent().position().top) + "px" });
                }
                if ($(obj).attr("id")) {
                    $("#hideSelectSubAccountID").val($(obj).attr("id"));
                    IsBaseInfoOrSafeInfo = true;
                    $("#editCurrentChildAccountInfo_a").show();
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "PrimaryAccountManagement_Child.aspx",
                        success: function (data) {
                            $(".imageInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
            }
        }

        var flag_VideoOrPlayback = true;
        var flag_ReadOnlyOrConfig = true;
        //加载子账户的权限信息 
        //VideoOrPlayback--判断是现场视频(true)还是回放(false)的权限;ReadOnlyOrConfig--判断是只读(true)还是进行配置(false)
        function LoadSubAccountAuthorizeInfo(VideoOrPlayback, ReadOnlyOrConfig) {
            var result = BsrCloudServer.Customer.GetAuthorizeSubCustomer($("#hideSelectSubAccountID").val());
            if (result && result.Code == 0) {
                if (result.authorizeSubResponse != null) {
                    $("[name='inlineChkCustomerToSee']").prop("checked", false);
                    if (result.authorizeSubResponse.subCustomerPermissionList == null || result.authorizeSubResponse.subCustomerPermissionList.length == 0) {
                        $("[name='inlineChkCustomerToSee']").prop("checked", false);
                    }
                    else {
                        for (var i = 0; i < result.authorizeSubResponse.subCustomerPermissionList.length; i++) {
                            var key = result.authorizeSubResponse.subCustomerPermissionList[i].PermissionName;
                            var name = result.authorizeSubResponse.subCustomerPermissionList[i].IsEnable;
                            switch (key) {
                                case "MySpace":
                                    if (name.toString() == "1") {
                                        $("#showMySpace").prop("checked", true);
                                    }
                                    else {
                                        $("#showMySpace").prop("checked", false);
                                    }
                                    break;
                                case "EventMessage":
                                    if (name.toString() == "1") {
                                        $("#showEventMessage").prop("checked", true);
                                    }
                                    else {
                                        $("#showEventMessage").prop("checked", false);
                                    }
                                    break;
                                case "CloudVideo":
                                    if (name.toString() == "1") {
                                        $("#showCloudVideo").prop("checked", true);
                                    }
                                    else {
                                        $("#showCloudVideo").prop("checked", false);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (result.authorizeSubResponse.authorizeDeviceResponseList != null && result.authorizeSubResponse.authorizeDeviceResponseList.length > 0) {
                        var subAuthorizeTable = null; //subAuthorizeTableNext
                        var classTb = "";
                        if (VideoOrPlayback) {
                            classTb = "subAuthorizeTable";
                            subAuthorizeTable = $(".subAuthorizeTable");
                            $(".subAuthorizeTable").html("");
                            $(".subAuthorizeTable").show();
                            $(".subAuthorizeTableNext").hide();
                        }
                        else {
                            classTb = "subAuthorizeTableNext";
                            $(".subAuthorizeTableNext").html("");
                            subAuthorizeTable = $(".subAuthorizeTableNext");
                            $(".subAuthorizeTable").hide();
                            $(".subAuthorizeTableNext").show();
                        }
                        var maxChannelCount = 0;
                        for (var i = 0; i < result.authorizeSubResponse.authorizeDeviceResponseList.length; i++) {
                            if (result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse.length > maxChannelCount) {
                                maxChannelCount = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse.length;
                            }
                        }
                        for (var i = 0; i < result.authorizeSubResponse.authorizeDeviceResponseList.length; i++) {
                            var flag = true;
                            var tr = $("<tr id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "'></tr>");
                            var td_Device = $("<td style='color:#000000;' DeviceId='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "'></td>");
                            for (var j = 0; j < result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse.length; j++) {
                                var td_Channel = $("<td class='" + classTb + " SecondTd'></td>");
                                var td_Channel_Div = $("<div style='line-height: 19px; text-align: left;word-break:break-all;'></div>");
                                var td_Channel_Div_Lable = null;
                                var td_Channel_Div_Lable_Img = null;
                                var td_Channel_Div_Lable_Input = null;
                                var ChannelPermission = "";
                                for (var k = 0; k < result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse.length; k++) {
                                    if ("" == ChannelPermission) {
                                        ChannelPermission = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse[k].PermissionName;
                                    }
                                    else {
                                        ChannelPermission += result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].authorizePermissionResponse[k].PermissionName;
                                    }
                                }
                                var ChannelName = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName;
                                //                                if (ChannelName.length > 7) {
                                //                                    ChannelName = ChannelName.substring(0, 7) + "...";
                                //                                }
                                if (VideoOrPlayback) {//现场视频
                                    if (ChannelPermission.indexOf("Video") >= 0) {
                                        if (ReadOnlyOrConfig) {//只读
                                            td_Channel_Div_Lable = $("<a class='text-overflow-ellipsis' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'>" + ChannelName + "</a>");
                                            td_Channel_Div_Lable_Img = $("<img src='../Images/icons/icon_Selected.png' width='16px' height='16px' style='vertical-align: middle;margin-right:6px;' />");
                                            td_Channel_Div_Lable.prepend(td_Channel_Div_Lable_Img);
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        } else {
                                            td_Channel_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'></label>");
                                            td_Channel_Div_Lable_Input = $("<input onclick='OnChangeVideoStatus(this)' id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelId + "' name='ChannelVideo-" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "' type='checkbox' style='vertical-align: -1px;' checked='checked'>" + ChannelName + "</input>");
                                            td_Channel_Div_Lable.append(td_Channel_Div_Lable_Input);
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        }
                                    }
                                    else {
                                        flag = false;
                                        if (!ReadOnlyOrConfig) {
                                            td_Channel_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'></label>");
                                            td_Channel_Div_Lable_Input = $("<input onclick='OnChangeVideoStatus(this)' id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelId + "' name='ChannelVideo-" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "' type='checkbox' style='vertical-align: -1px;'>" + ChannelName + "</input>");
                                            td_Channel_Div_Lable.append(td_Channel_Div_Lable_Input);
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        }
                                        else {
                                            td_Channel_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'>" + ChannelName + "</label>");
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        }
                                    }
                                }
                                else {//回放
                                    if (ChannelPermission.indexOf("Playback") >= 0) {
                                        if (ReadOnlyOrConfig) {//只读
                                            td_Channel_Div_Lable = $("<a class='text-overflow-ellipsis' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'>" + ChannelName + "</a>");
                                            td_Channel_Div_Lable_Img = $("<img src='../Images/icons/icon_Selected.png' width='16px' height='16px' style='vertical-align: middle;margin-right:6px;' />");
                                            td_Channel_Div_Lable.prepend(td_Channel_Div_Lable_Img);
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        } else {
                                            td_Channel_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'></label>");
                                            td_Channel_Div_Lable_Input = $("<input onclick='OnChangeVideoStatus(this)' id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelId + "' name='ChannelPlayback-" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "' type='checkbox' style='vertical-align: -1px;' checked='checked'>" + ChannelName + "</input>");
                                            td_Channel_Div_Lable.append(td_Channel_Div_Lable_Input);
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        }
                                    }
                                    else {
                                        flag = false;
                                        if (!ReadOnlyOrConfig) {
                                            td_Channel_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'></label>");
                                            td_Channel_Div_Lable_Input = $("<input onclick='OnChangeVideoStatus(this)' id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelId + "' name='ChannelPlayback-" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "' type='checkbox' style='vertical-align: -1px;'>" + ChannelName + " </input>");
                                            td_Channel_Div_Lable.append(td_Channel_Div_Lable_Input);
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        }
                                        else {
                                            td_Channel_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse[j].ChannelName + "'>" + ChannelName + "</label>");
                                            td_Channel_Div.append(td_Channel_Div_Lable);
                                        }
                                    }
                                }
                                td_Channel.append(td_Channel_Div);
                                tr.append(td_Channel);
                            }
                            if (maxChannelCount <= 8) {
                                for (var k = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse.length; k < 8; k++) {
                                    var td_Channel_Add = $("<td></td>");
                                    tr.append(td_Channel_Add);
                                }
                            }
                            else {
                                for (var k = result.authorizeSubResponse.authorizeDeviceResponseList[i].authorizeChannelResponse.length; k < maxChannelCount; k++) {
                                    var td_Channel_Add = $("<td></td>");
                                    tr.append(td_Channel_Add);
                                }
                            }
                            var td_Device_Div = $("<div style='line-height: 19px; text-align: left;word-break:break-all;'></div>");
                            var td_Device_Div_Lable = null;
                            var td_Device_Div_Lable_Img = null;
                            var td_Device_Div_Lable_Input = null;
                            var DeviceName = result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceName;
                            //                            if (DeviceName.length > 7) {
                            //                                DeviceName = DeviceName.substring(0, 7) + "...";
                            //                            }
                            if (ReadOnlyOrConfig) {//只读
                                if (flag) {
                                    td_Device_Div_Lable = $("<a class='text-overflow-ellipsis' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceName + "'>" + DeviceName + "</a>");
                                    td_Device_Div_Lable_Img = $("<img src='../Images/icons/icon_Selected.png' width='16px' height='16px' style='vertical-align: middle;margin-right:6px;' />");
                                    // td_Device_Div.append(td_Device_Div_Lable_Img);
                                    td_Device_Div_Lable.prepend(td_Device_Div_Lable_Img);
                                    td_Device_Div.append(td_Device_Div_Lable);
                                }
                                else {
                                    td_Device_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceName + "'>" + DeviceName + "</label>");
                                    td_Device_Div.append(td_Device_Div_Lable);
                                }
                                td_Device.append(td_Device_Div);
                            }
                            else {
                                if (flag) {
                                    td_Device_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceName + "'></label>");
                                    td_Device_Div_Lable_Input = $("<input onclick='OnChangeChannelStatus(this);' id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "' type='checkbox' style='vertical-align: -1px;' checked='checked'>" + DeviceName + "</input>");
                                    td_Device_Div_Lable.append(td_Device_Div_Lable_Input);
                                    td_Device_Div.append(td_Device_Div_Lable);

                                }
                                else {
                                    td_Device_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceName + "'></label>");
                                    td_Device_Div_Lable_Input = $("<input onclick='OnChangeChannelStatus(this)' id='" + result.authorizeSubResponse.authorizeDeviceResponseList[i].DeviceId + "' type='checkbox' style='vertical-align: -1px;'>" + DeviceName + "</input>");
                                    td_Device_Div_Lable.append(td_Device_Div_Lable_Input);
                                    td_Device_Div.append(td_Device_Div_Lable);
                                }
                                td_Device.append(td_Device_Div);
                            }
                            tr.prepend(td_Device);
                            subAuthorizeTable.append(tr);
                        }
                    }
                }
            }
            else {

            }
        }

        var selectTab;
        function OnChangeChannelStatus(obj) {
            if (obj) {//VideoOrPlayback
                if ($(obj).is(":checked")) {
                    $(obj).closest("tr").find("[name='Channel" + selectTab + "-" + $(obj).attr("id") + "']").prop("checked", true);
                }
                else {
                    $(obj).closest("tr").find("[name='Channel" + selectTab + "-" + $(obj).attr("id") + "']").prop("checked", false);
                }
            }
        }

        function OnChangeVideoStatus(obj) {
            if (obj) {
                if ($(obj).is(":checked")) {
                    var flag = true;
                    $(obj).closest("tr").find("[name='" + $(obj).attr("name") + "']").each(function () {
                        if (!$(this).is(":checked")) {
                            flag = false;
                            return false;
                        }
                    })
                    if (flag) {
                        $(obj).closest("tr").find("td:first").find("input").prop("checked", true);
                    }
                }
                else {
                    $(obj).closest("tr").find("td:first").find("input").prop("checked", false);
                }
            }
        }

        //保存当前选中子账户的权限
        function btnSaveSubAuthorizeInfo() {
            var SubAuthorize = new Object();
            var AuthorizeSubResponse = new Object();
            AuthorizeSubResponse.CustomerId = $("#hideSelectSubAccountID").val();
            var AuthorizeSubResponseArray = [];
            var tableCount = $(".imageInfo").find("tr").length / 2;
            for (var i = 0; i < $(".imageInfo").find("tr").length / 2; i++) {
                var AuthorizeDeviceResponse = new Object();
                AuthorizeDeviceResponse.DeviceId = $(".imageInfo").find("tr").eq(i).attr("id");
                AuthorizeDeviceResponse.IsEnable = 1;
                var AuthorizeDeviceResponseArray = [];
                for (var j = 0; j < $(".imageInfo").find("tr").eq(i).find("td").length; j++) {
                    if (0 != j) {
                        if ($(".imageInfo").find("tr").eq(i).find("td").eq(j).find("input").is(":checked") ||
                        $(".imageInfo").find("tr").eq(i + tableCount).find("td").eq(j).find("input").is(":checked")) {
                            var AuthorizeChannelResponse = new Object();
                            AuthorizeChannelResponse.ChannelId = $(".imageInfo").find("tr").eq(i).find("td").eq(j).find("input").attr("id");
                            AuthorizeChannelResponse.IsEnable = 1;
                            var AuthorizePermissionResponseArray = [];
                            if ($(".imageInfo").find("tr").eq(i).find("td").eq(j).find("input").is(":checked")) {
                                var AuthorizePermissionResponse = new Object();
                                AuthorizePermissionResponse.PermissionName = 'Video';
                                AuthorizePermissionResponse.IsEnable = 1;
                                AuthorizePermissionResponseArray.push(AuthorizePermissionResponse);
                            }
                            if ($(".imageInfo").find("tr").eq(i + tableCount).find("td").eq(j).find("input").is(":checked")) {
                                var AuthorizePermissionResponse = new Object();
                                AuthorizePermissionResponse.PermissionName = 'Playback';
                                AuthorizePermissionResponse.IsEnable = 1;
                                AuthorizePermissionResponseArray.push(AuthorizePermissionResponse);
                            }
                            AuthorizeChannelResponse.authorizePermissionResponse = AuthorizePermissionResponseArray;
                            AuthorizeDeviceResponseArray.push(AuthorizeChannelResponse);
                        }
                    }
                }
                AuthorizeDeviceResponse.authorizeChannelResponse = AuthorizeDeviceResponseArray;
                AuthorizeSubResponseArray.push(AuthorizeDeviceResponse);
            }
            AuthorizeSubResponse.authorizeDeviceResponseList = AuthorizeSubResponseArray;
            //概况权限
            var AuthorizePermissionResponseSelfArray = [];
            $("[name='inlineChkCustomerToSee']").each(function () {
                if ($(this)[0].checked) {
                    if ($(this)[0].id == "showMySpace") {
                        var AuthorizePermissionResponseSelf = new Object();
                        AuthorizePermissionResponseSelf.PermissionName = 'MySpace';
                        AuthorizePermissionResponseSelf.IsEnable = 1;
                        AuthorizePermissionResponseSelfArray.push(AuthorizePermissionResponseSelf);
                    }
                    else if ($(this)[0].id == "showEventMessage") {
                        var AuthorizePermissionResponseSelf = new Object();
                        AuthorizePermissionResponseSelf.PermissionName = 'EventMessage';
                        AuthorizePermissionResponseSelf.IsEnable = 1;
                        AuthorizePermissionResponseSelfArray.push(AuthorizePermissionResponseSelf);
                    }
                    else if ($(this)[0].id == "showCloudVideo") {
                        var AuthorizePermissionResponseSelf = new Object();
                        AuthorizePermissionResponseSelf.PermissionName = 'CloudVideo';
                        AuthorizePermissionResponseSelf.IsEnable = 1;
                        AuthorizePermissionResponseSelfArray.push(AuthorizePermissionResponseSelf);
                    }
                }
            });
            AuthorizeSubResponse.subCustomerPermissionList = AuthorizePermissionResponseSelfArray;
            SubAuthorize.authorizeSubResponse = AuthorizeSubResponse;
            var result = BsrCloudServer.Customer.AuthorizeSubByPrimary(SubAuthorize);
            if (result && result.Code == 0) {
                $("#anthorizeHandleDiv").hide();
                $("#anthorizeHandleMessageDiv").show(500, function () {
                    $("#anthorizeHandleMessageDiv").find("p").text("保存成功");
                });
                flag_VideoOrPlayback = true;
                flag_ReadOnlyOrConfig = true;
                LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
            }
            else {
                $("#anthorizeHandleMessageDiv").show(500, function () {
                    $("#anthorizeHandleMessageDiv").find("p").text("保存失败");
                });
            }
        }

        function CancleSaveSubAuthorizeInfo() {
            $("[name='inlineChkCustomerToSee']").attr("disabled", "disabled");
            $("#anthorizeHandleDiv").hide();
            $("#anthorizeHandleMessageDiv").hide();
            selectTab = "Video";
            flag_ReadOnlyOrConfig = true;
            flag_VideoOrPlayback = true;
            LoadSubAccountAuthorizeInfo(!flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
            LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
        }

        function OnShowPanelBanner(obj) {
            if (obj) {
                $(obj).parent().next().css({ "left": ($(obj).position().left + 0).toFixed(0) + "px", "top": ($(obj).position().top + 41).toFixed(0) + "px" });
                if ($.trim($(obj).text()) == "基本信息") {
                    IsBaseInfoOrSafeInfo = true;
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "PrimaryAccountManagement_Child.aspx",
                        success: function (data) {
                            $(".panelBodyInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
                else if ($.trim($(obj).text()) == "账户安全") {
                    //LoadCurrentChikdAccountSafeDiv();
                    IsBaseInfoOrSafeInfo = false;
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "PrimaryAccountManagement_Child.aspx",
                        success: function (data) {
                            $(".panelBodyInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
                else {
                    $("[name='inlineChkCustomerToSee']").attr("disabled", "disabled");
                    $("#anthorizeHandleDiv").hide();
                    $("#anthorizeHandleMessageDiv").hide();
                    selectTab = "Video";
                    flag_ReadOnlyOrConfig = true;
                    flag_VideoOrPlayback = true;
                    LoadSubAccountAuthorizeInfo(!flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                    LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                    var clone = $("#subAuthorizeConfigMode").children().clone();
                    $(".panelBodyInfo").html(clone);
                }
            }
        }

        //在相应操作下显示下划线图标并显示相应信息
        function OnShowBanner(obj) {
            if (obj) {
                $(obj).parent().next().css({ "left": ($(obj).position().left + 0).toFixed(0) + "px", "top": ($(obj).position().top + 62).toFixed(0) + "px" });
                if ($.trim($(obj).text()) == "基本信息") {
                    IsBaseInfoOrSafeInfo = true;
                    $("#editCurrentChildAccountInfo_a").show();
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "PrimaryAccountManagement_Child.aspx",
                        success: function (data) {
                            $(".imageInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
                else if ($.trim($(obj).text()) == "账户安全") {
                    //LoadCurrentChikdAccountSafeDiv();
                    $("#editCurrentChildAccountInfo_a").hide();
                    IsBaseInfoOrSafeInfo = false;
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "PrimaryAccountManagement_Child.aspx",
                        success: function (data) {
                            $(".imageInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
                else {
                    $(".imageInfo").html("");
                    var settings = $.extend({
                        callback: null,
                        isShowTip: true
                    });
                    if (settings.isShowTip) {
                        jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                    }
                    $("[name='inlineChkCustomerToSee']").attr("disabled", "disabled");
                    $("#anthorizeHandleDiv").hide();
                    $("#anthorizeHandleMessageDiv").hide();
                    selectTab = "Video";
                    flag_ReadOnlyOrConfig = true;
                    flag_VideoOrPlayback = true;
                    LoadSubAccountAuthorizeInfo(!flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                    LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                    var clone = $("#subAuthorizeConfigMode").children().clone();
                    $(".imageInfo").html(clone);
                    if ($.jNotify) {
                        $.jNotify._close();
                    }
                    if (settings.callback) {
                        settings.callback();
                    }
                }
            }
        }

        //查看子账户现场视频的权限
        function ShowSubAuthorizeVideo(obj) {
            if (obj) {
                $(obj).css({
                    "background-image": "url(../Images/icons/icon_Live_active.png)",
                    "background-repeat": "no-repeat",
                    "color": "#30c5fc"
                });
                $(obj).parent().next().find("a").css({
                    "background-image": "url(../Images/icons/icon_Play.png)",
                    "background-repeat": "no-repeat",
                    "color": ""
                });

                selectTab = "Video";
                if (flag_ReadOnlyOrConfig) {
                    flag_VideoOrPlayback = true;
                    LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                }
                else {
                    $(".subAuthorizeTableNext").fadeOut(350, function () {
                        $(".subAuthorizeTable").show();
                    });
                }
            }
        }

        //查看子账户回放的权限
        function ShowSubAuthorizePlay(obj) {
            if (obj) {
                $(obj).css({
                    "background-image": "url(../Images/icons/icon_Play_active.png)",
                    "background-repeat": "no-repeat",
                    "color": "#30c5fc"
                });
                $(obj).parent().prev().find("a").css({
                    "background-image": "url(../Images/icons/icon_Live.png)",
                    "background-repeat": "no-repeat",
                    "color": ""
                });

                selectTab = "Playback";
                if (flag_ReadOnlyOrConfig) {
                    flag_VideoOrPlayback = false;
                    LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                }
                else {
                    $(".subAuthorizeTable").fadeOut(350, function () {
                        $(".subAuthorizeTableNext").show();
                    });
                }
            }
        }

        //编辑子账户的权限
        function ShowEditSubAuthorize() {
            $("#anthorizeHandleDiv").show();
            $("[name='inlineChkCustomerToSee']").removeAttr("disabled");
            if (flag_ReadOnlyOrConfig) {
                flag_ReadOnlyOrConfig = false;
                LoadSubAccountAuthorizeInfo(!flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
                LoadSubAccountAuthorizeInfo(flag_VideoOrPlayback, flag_ReadOnlyOrConfig);
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
