<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManagement.aspx.cs"
    Inherits="Bsr.Cloud.WebEntry.Pages.AccountManagement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>账户管理</title>
    <style type="text/css">
        .modal-body input
        {
            background-color: White;
        }
    </style>
</head>
<body>
    <div>
        <div class="col-xs-12">
            <h4 style="padding-top: 10px;">
                账户管理
            </h4>
        </div>
        <div class="content-nav">
        </div>
        <div style="margin-top: 80px;">
            <div class="col-xs-12">
                <div class="col-xs-6 col-xs-offset-3">
                    <div class="accountBaseinfo">
                        <a id="accountBaseinfo_a" href="#" onclick="LoadCurrentAccountDiv();return false;">账户基本信息</a>
                    </div>
                    <div class="accountSafeinfo">
                        <a id="accountSafeinfo_a" href="#" onclick="LoadCurrentAccountSafeDiv();return false;">
                            账户安全设置</a>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="CurrentCustomerInfoDiv" style="margin-top: 15px; display: none;">
                <div class="divcss">
                    <div class="col-xs-3 divimg">
                        <img class="img-circle" id="userImg" src="" style="width: 140px; height: 140px; margin-left: 30px;"
                            alt="加载图片失败" onerror="errorImg(this)" />
                    </div>
                    <div class="col-xs-9 divuserinfo">
                        <form class="form-horizontal" id="CurrentLoginCostomerInfo" role="form">
                        <div class="form-group">
                            <label for="accontmobilePhone" class="col-xs-3 control-label">
                                手机号:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="accontmobilePhone"
                                    name="mobile" maxlength="16" value="" />
                            </div>
                            <div class="col-xs-4">
                                <label for="accontmobilePhone" class="error">
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="accountEmail" class="col-xs-3 control-label">
                                邮箱:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="accountEmail" name="emial"
                                    maxlength="64" value="" />
                            </div>
                            <div class="col-xs-4">
                                <label for="accountEmail" class="error">
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="accountRealName" class="col-xs-3 control-label">
                                真实姓名:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="accountRealName"
                                    name="realname" maxlength="32" value="" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="accountTelphone" class="col-xs-3 control-label">
                                固定电话:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="accountTelphone"
                                    name="telphone" maxlength="14" value="" />
                            </div>
                            <div class="col-xs-4">
                                <label for="accountTelphone" class="error">
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="accountAddress" class="col-xs-3 control-label">
                                地址:</label>
                            <div class="col-xs-5">
                                <input type="text" disabled="disabled" class="form-control" id="accountAddress" name="address"
                                    maxlength="512" value="" />
                            </div>
                        </div>
                        </form>
                    </div>
                    <div style="float: right; margin-right: 20px; padding-top: 15px;">
                        <a href="#" onclick="EditCurrentAccountInfo();return false;">
                            <img src="../Images/icons/icon_Edit_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Edit_Hover.png'"
                                title="修改" onmouseout="this.src='../Images/icons/icon_Edit_Normal.png'" />
                        </a><a href="#" onclick="ShowHistoryInfo();return false;" style="margin-left: 5px;">
                            <img src="../Images/icons/icon_History_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_History_Hover.png'"
                                title="历史登录信息" onmouseout="this.src='../Images/icons/icon_History_Normal.png'" />
                        </a>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="CurrentCustomerSafeInfoDiv" style="display: none">
                <div class="divsafecss">
                    <form class="form-horizontal" role="form">
                    <div class="form-group" style="line-height: 20px;">
                        <label class="col-xs-3 control-label">
                            账户有效期</label>
                        <label id="accontValid" class="col-xs-3" style="margin-top: 7px; font-weight: normal;
                            margin-left: 26px;">
                        </label>
                    </div>
                    <div class="form-group" style="line-height: 25px;">
                        <label class="col-xs-3 control-label">
                            用户登陆终端</label>
                        <div class="col-xs-6">
                            <div id="EditCurrentCustomerLoginWay" style="width: 344px;">
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="PhoneClient" name="chkLoginWay" value="option1" />
                                    手机客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="BSPhoneClient" name="chkLoginWay" value="option2" />
                                    B/S客户端
                                </label>
                                <label class="checkbox-inline">
                                    <input type="checkbox" id="CSClient" name="chkLoginWay" value="option3" />
                                    C/S客户端
                                </label>
                            </div>
                        </div>
                        <div class="col-xs-2 " style="padding-top: 7px; position: relative;">
                            <a href="#" id="editAccountSafeInfo_a" onclick="EditAccountSafeInfo(this);return false;"
                                style="position: absolute; left: 20px; color: #808080;">修改</a><a style="display: none;
                                    margin-left: 5px;" id="cancleEditAccountSafeInfo" href="#" onclick="CancelSaveAccountSafeInfo(this);return false;">
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
                            <a href="#" style="color: #808080;" onclick="ResetAccountPassWd();return false;">修改</a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="divbtn">
                    <div id="saveCurrentAccountInfo_divId" style="display: none;">
                        <button class="btn btn-default" style="width: 120px;" onclick="$('#CurrentLoginCostomerInfo').submit();return false;">
                            保存
                        </button>
                        <button class="btn btn-default" style="width: 120px;" onclick="CancelSaveCurrentAccountInfo();return false;">
                            取消
                        </button>
                    </div>
                    <div id="savePromptMessage" style="margin-top: 10px; color: #30c5fc; display: none;">
                        保存成功
                    </div>
                </div>
            </div>
            <div class="col-xs-12">
                <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                    <div class="panel panel-default" id="panelOne" style="margin-left: -15px; margin-right: -15px;
                        box-shadow: 0 1px 1px rgba(0, 0, 0, 0);">
                        <div class="panel-heading" role="tab" id="headingOne">
                            <label class="control-label">
                                我的主账户</label>
                        </div>
                        <div class="createChildAccoutDiv">
                            <a href="#" onclick="ShowCollapseNewAccoutPanelBody();return false;">创建主账户</a>
                        </div>
                        <div id="MianAccountCollapsePanel" class="panel-collapse collapse" role="tabpanel"
                            aria-labelledby="headingOne">
                            <div class="panel-body" style="border-style: none;">
                                <div style="height: 478px;">
                                    <div class="col-xs-12">
                                        <div class="col-xs-5">
                                            <form id="mianAccountFrom" class="form-horizontal" role="form">
                                            <div class="form-group">
                                                <label class="col-xs-3 col-xs-offset-4 control-label">
                                                    基本信息:</label>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountUserName" class="col-xs-4 control-label">
                                                    用户名:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="mianAccountUserName" name="mianAccountUserName"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="mianAccountUserName" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountPassword" class="col-xs-4 control-label">
                                                    密码:</label>
                                                <div class="col-xs-8">
                                                    <input type="password" class="form-control" id="mianAccountPassword" name="mianAccountPassword"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="mianAccountPassword" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountPasswordAgain" class="col-xs-4 control-label">
                                                    确认密码:</label>
                                                <div class="col-xs-8">
                                                    <input type="password" class="form-control" id="mianAccountPasswordAgain" name="mianAccountPasswordAgain"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="mianAccountPasswordAgain" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountMobilePhone" class="col-xs-4 control-label">
                                                    手机号:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="mianAccountMobilePhone" name="mianAccountMobilePhone"
                                                        maxlength="16" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="mianAccountMobilePhone" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountEmial" class="col-xs-4 control-label">
                                                    邮箱:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="mianAccountEmial" name="mianAccountEmial"
                                                        maxlength="64" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="mianAccountEmial" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountRealName" class="col-xs-4 control-label">
                                                    真实姓名:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="mianAccountRealName" name="mianAccountRealName"
                                                        maxlength="32" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountPhone" class="col-xs-4 control-label">
                                                    固定电话:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="mianAccountPhone" name="mianAccountPhone"
                                                        maxlength="14" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-xs-8 col-xs-offset-4">
                                                    <label for="mianAccountPhone" class="error">
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="mianAccountAddr" class="col-xs-4 control-label">
                                                    地址:</label>
                                                <div class="col-xs-8">
                                                    <input type="text" class="form-control" id="mianAccountAddr" name="mianAccountAddr"
                                                        maxlength="512" />
                                                </div>
                                            </div>
                                            </form>
                                        </div>
                                        <div class="col-xs-7">
                                            <form class="form-horizontal" role="form">
                                            <div class="form-group">
                                                <label class="col-xs-3 col-xs-offset-4 control-label">
                                                    安全信息:</label>
                                            </div>
                                            <div class="form-group">
                                                <div class="radio col-xs-5 col-xs-offset-1">
                                                    <label class="radio-inline">
                                                        <input type="radio" onclick="ChangeToEver()" name="inlineRadioOptions" id="inlineRadio_Evertime"
                                                            value="option1" />
                                                        永久有效
                                                    </label>
                                                    <label class="radio-inline" style="margin-left: 40px;">
                                                        <input type="radio" onclick="ChangeToEnd()" name="inlineRadioOptions" id="inlineRadio_Endtime"
                                                            value="option2" />
                                                        截止
                                                    </label>
                                                </div>
                                                <div class="col-xs-6" style="padding-top: 14px; margin-left: -43px;">
                                                    <label>
                                                        <input class="Wdate" readonly="readonly" type="text" id="ValidEndtime" style="width: 160px;"
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
                                                        <input type="checkbox" id="inlineCheckbox1" value="option1" name="chkPrimaryCustomer" />
                                                        手机客户端
                                                    </label>
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" id="inlineCheckbox2" value="option2" name="chkPrimaryCustomer" />
                                                        B/S客户端
                                                    </label>
                                                    <label class="checkbox-inline">
                                                        <input type="checkbox" id="inlineCheckbox3" value="option3" name="chkPrimaryCustomer" />
                                                        C/S客户端
                                                    </label>
                                                </div>
                                            </div>
                                            </form>
                                        </div>
                                    </div>
                                    <div class="col-xs-12" style="text-align: center;">
                                        <button id="SaveMianAccoutInfo_Btn" class="btn btn-default" style="width: 120px;"
                                            onclick="javascript:$('#mianAccountFrom').submit();return false;">
                                            保存
                                        </button>
                                        <p id="SaveChildAccoutInfo_Text" style="color: #30c5fc; display: none;">
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12" id="primaryAccountImglist" style="display: none; position: relative;
                margin-bottom: 30px;">
            </div>
        </div>
    </div>
    <!-- 账户图片列表项 -->
    <div style="display: none">
        <div id="template-account-img-list-item">
            <div style="display: inline-block; margin-left: 128px; position: relative;">
                <img onclick="btnShowImgToInfo(this)" class="img-circle" src="" style="width: 75px;
                    height: 75px; cursor: pointer" onerror="errorImg(this)" />
                <i></i>
                <div style="text-align: center; height: auto;">
                    <center style="-o-text-overflow: ellipsis; text-overflow: ellipsis; width: 75px;
                        overflow: hidden; white-space: nowrap;">
                    </center>
                </div>
            </div>
        </div>
    </div>
    <!-- 账户图片列表 -->
    <div id="template-account-img-list" style="display: none;">
        <div style="width: 980px; margin-top: 12px;">
        </div>
    </div>
    <!-- 账户图片列表项对应的详细信息 -->
    <div id="template-account-img-info" style="display: none">
        <div style="background: url(../Images/icons/Bg_ProfileOpen.png) no-repeat; height: 16px;
            width: 110px; text-align: center;">
        </div>
        <div style="height: 460px; background-color: #f1f1f1; text-align: center; position: relative;
            top: 5px; margin-left: -15px; margin-right: -15px;">
            <div style="float: right; margin-right: 20px; padding-top: 3px;">
                <a id="editCurrentChildAccountInfo_a" href="#" onclick="EditCurrentChildAccountInfo(this);return false;">
                    <img src="../Images/icons/icon_Edit_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_Edit_Hover.png'"
                        title="修改" onmouseout="this.src='../Images/icons/icon_Edit_Normal.png'" /></a>
                <a href="#" onclick="FreezeChildAccount(this);return false;">
                    <img src="../Images/icons/icon_DisableUser_Normal.png" width="24px" height="24px"
                        onmouseover="this.src='../Images/icons/icon_DisableUser_Hover.png'" title="冻结"
                        onmouseout="this.src='../Images/icons/icon_DisableUser_Normal.png'" /></a>
                <a id="showChildHistoryInfo_a" href="#" onclick="ShowChildHistoryInfo();return false;">
                    <img src="../Images/icons/icon_History_Normal.png" width="24px" height="24px" onmouseover="this.src='../Images/icons/icon_History_Hover.png'"
                        title="历史登录信息" onmouseout="this.src='../Images/icons/icon_History_Normal.png'" />
                </a><a href="#" onclick="DeleteChildAccoutByModelOne();return false;">
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
    <!--主账户列表展示模板 -->
    <div style="display: none">
        <div id="template-panel-type">
            <div class="panel panel-default">
                <div class="panel-heading" role="tab">
                    <div class="col-md-8">
                        <%-- <input type="checkbox">--%>
                        <%--<label class="control-label">
                        </label>--%>
                        <a data-toggle="collapse" href="javascript:void(0)" onclick="OnShowMianAccoutPanelBody(this);"
                            aria-expanded="true" aria-controls="collapseOne"></a>
                        <label class="control-label" style="margin-left: 40px;">
                        </label>
                    </div>
                    <div class="btn-group">
                        <button type="button" onclick="DeleteMainAccout(this);return false;" class="btn btn-primary"
                            data-toggle="collapse" data-target="#panelcollapsetemp" aria-expanded="true"
                            aria-controls="collapseOne">
                            删除
                        </button>
                        <button type="button" onclick="FreezeOrThawPrimaryCustomer(this);return false;" class="btn btn-primary"
                            data-toggle="collapse" data-target="#panelcollapsetemp" aria-expanded="true"
                            aria-controls="collapseOne">
                            冻结
                        </button>
                        <button type="button" onclick="AuthorizeAccount(this);" class="btn btn-primary" data-toggle="collapse"
                            data-target="#panelcollapsetemp" aria-expanded="true" aria-controls="collapseOne">
                            授权
                        </button>
                    </div>
                </div>
                <div class="panel-collapse collapse" role="tabpanel" aria-labelledby="panelheadingtemp">
                    <div class="panel-body">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- 账户历史登陆信息页面 -->
    <div class="modal fade bs-example-modal-sm" id="myModal" tabindex="-1" role="dialog"
        aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm" style="width: 680px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="myModalLabel">
                        历史登陆信息</h4>
                </div>
                <div class="modal-body">
                    <table class="table table-bordered" id="logtable">
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
                    <%--<button type="button" class="btn btn-primary" data-dismiss="modal">
                        返回</button>--%>
                </div>
            </div>
        </div>
    </div>
    <!-- 图片上传 -->
    <div class="modal fade bs-example-modal-sm" id="modelUpfileWin" tabindex="-1" role="dialog"
        aria-labelledby="mySmallModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="H1">
                        图片选择</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <input type="file" id="InputFile" name="InputFile" accept="image/*" size="12" style="float: left;
                            height: 22px; width: 275px; line-height: 20px;" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn" data-dismiss="modal" onclick="btnUpdateImage();return false;">
                        上传</button>
                </div>
            </div>
        </div>
    </div>
    <!-- 重置密码 -->
    <div class="modal fade bs-example-modal-sm" id="EditAccontPassWdModal" tabindex="-1"
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
                    <form class="form-horizontal" id="FromAccountPassWd" role="form">
                    <div class="form-group">
                        <label for="OriginalPassWd" class="col-md-3 control-label">
                            原始密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="OriginalPassWd" name="OriginalPassWd"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="OriginalPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="NewPassWd" class="col-md-3 control-label">
                            新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="NewPassWd" name="NewPassWd" maxlength="32"
                                value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="NewPassWd" class="error">
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="ConfirmPassword" class="col-md-3 control-label">
                            确认新密码:</label>
                        <div class="col-md-8">
                            <input type="password" class="form-control" id="ConfirmPassword" name="ConfirmPassword"
                                maxlength="32" value="" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-xs-8 col-xs-offset-3">
                            <label for="ConfirmPassword" class="error">
                            </label>
                        </div>
                    </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="javascript:$('#FromAccountPassWd').submit();;return false;">
                        保存</button>
                </div>
            </div>
        </div>
    </div>
    <!-- 权限配置模板 -->
    <div id="childAuthorizeConfigMode" style="display: none">
        <div class="subAuthorizeDiv">
            <div class="col-xs-12 subAuthorizeTableHead">
                <div class="subAuthorizeTableHead configDiv">
                    <a href="#" style="color: #0a90e2" onclick="ShowEditChildAuthorize();return false;">
                        编辑权限</a>
                </div>
            </div>
            <div class="col-xs-12 subAuthorizeTableDiv">
                <table class="subAuthorizeTable">
                </table>
            </div>
            <div id="anthorizeHandleDiv" class="col-xs-4 col-xs-offset-4" style="margin-top: 10px;">
                <button type="button" onclick="btnSaveChildAuthorizeInfo();return false;" class="btn btn-default">
                    保存
                </button>
                <button type="button" onclick="CancleSaveChildAuthorizeInfo();return false;" class="btn btn-default">
                    取消
                </button>
            </div>
            <div id="anthorizeHandleMessageDiv" class="col-xs-4 col-xs-offset-4" style="margin-top: 10px;">
                <p style="color: #30c5fc;">
                    保存成功</p>
            </div>
        </div>
    </div>
    <div style="display: none">
        <input type="hidden" id="hidCustomerPhone" />
        <input type="hidden" id="hidCustomerEmail" />
        <input type="hidden" id="hidImageBase64Code" />
        <input type="hidden" id="hideImgExt" />
        <input type="hidden" id="hidPrimaryCustomerImgBase64Code" />
        <input type="hidden" id="hidPrimaryCustomerImgExt" />
        <input type="hidden" id="hideSelectMainAccountID" />
    </div>
    <script src="../Scripts/jquery.md5.js" type="text/javascript"></script>
    <script src="../Scripts/ajaxfileupload.js" type="text/javascript"></script>
    <script type="text/javascript">
        var treeObj;
        var strOrgLoginTerminal;
        var AccountListStyle = 1; // 1-代表图片模式，2-代表列表模式
        var CurrentLoginCostomerInfo_Validator = null;
        $(function () {
            //初始化加载当前登陆账户的基本信息 
            LoadCurrentAccountDiv();
            $("#userImg").unbind("click", ShowUpdateFilePage);
            LoadPrimaryAccountImageListInfo("");
        });

        //显示创建主账户界面
        function ShowCollapseNewAccoutPanelBody() {
            if ($("#MianAccountCollapsePanel").hasClass('in')) {
                $("#MianAccountCollapsePanel").collapse('hide');
            }
            else {
                $("#MianAccountCollapsePanel").collapse('show');
                InitAccoutPanelBody();
            }
        }

        //初始化新建主账户信息
        function InitAccoutPanelBody() {
            $("#mianAccountFrom input").val("");
            $("#inlineRadio_Endtime").prop("checked", true);
            var endtime = new Date();
            endtime.setMonth(endtime.getMonth() + 3);
            var dateTime = endtime.Format("yyyy-MM-dd hh:mm:ss");
            $("#ValidEndtime").val(dateTime);
            if (typeof ($("#ValidEndtime").attr("disabled")) != "undefined") {
                $("#ValidEndtime").removeAttr("disabled");
            }
            $("[name ='chkPrimaryCustomer']").prop("checked", true);
            easyuiMainAccountValidate();
        }

        //显示当前登陆账户的基本信息 
        function LoadCurrentAccountDiv() {
            EnableOrDisableCurrentAccountInfo(false);
            getCustomerInfo();
        }

        //启用或者禁用账户的安全信息
        function EnableOrBanCurrentAccountSafeInfo(obj) {
            $("#savePromptMessage").hide();
            $("#saveCurrentAccountInfo_divId").hide();
            if (obj) {
                $("[name='chkLoginWay']").removeAttr("disabled");
            }
            else {
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
                $("#CurrentCustomerInfoDiv").hide();
                $("#CurrentCustomerSafeInfoDiv").show();
                $("[name='chkLoginWay']").attr("disabled", "disabled");
                $("#cancleEditAccountSafeInfo").hide();
                $("#editAccountSafeInfo_a").text("修改").css("color", "#808080");
            }
        }


        //显示当前登陆账户的安全信息
        function LoadCurrentAccountSafeDiv() {
            EnableOrBanCurrentAccountSafeInfo(false);
            getCurrentAccountSafeInfo();
        }

        //获取当前登陆账户的安全信息
        function getCurrentAccountSafeInfo() {
            var result = BsrCloudServer.Customer.getCustomerInfo(0);
            if (result && result.Code == 0) {
                if (null == result.customerReponse.ValidTime || "" == result.customerReponse.ValidTime) {
                    $("#accontValid").text("永久有效");
                } else {
                    var endtime = new Date(result.customerReponse.ValidTime);
                    var dateTime = endtime.Format("yyyy-MM-dd hh:mm:ss");
                    $("#accontValid").text("截止" + dateTime);
                }
                $("[name='chkLoginWay']").prop("checked", false);
                if (result.customerReponse.LoginTypes.indexOf("3") >= 0 || result.customerReponse.LoginTypes.indexOf("4") >= 0) {
                    $("#PhoneClient").prop("checked", true);
                }
                if (result.customerReponse.LoginTypes.indexOf("1") >= 0) {
                    $("#BSPhoneClient").prop("checked", true);
                }
                if (result.customerReponse.LoginTypes.indexOf("2") >= 0) {
                    $("#CSClient").prop("checked", true);
                }
            }
            else {
                $("#savePromptMessage").show(500, function () {
                    $("#savePromptMessage").text("获取安全信息失败");
                });
            }
        }

        //账户密码验证
        function easyuiCustomerPassWdValidate() {
            var FromAccountPassWd_Validator = $("#FromAccountPassWd").validate({
                onkeyup: false,
                rules: {
                    OriginalPassWd: {
                        required: true,
                        password: true
                    },
                    NewPassWd: {
                        required: true,
                        password: true
                    },
                    ConfirmPassword: {
                        required: true,
                        equalTo: "#NewPassWd"
                    }
                },
                messages: {
                    OriginalPassWd: { required: "请输入密码" },
                    NewPassWd: { required: "请输入密码" },
                    ConfirmPassword: {
                        required: "请重复输入密码",
                        equalTo: "密码不一致"
                    }
                },
                submitHandler: function () {
                    btnSaveAccountPassWd();
                }
            });
            FromAccountPassWd_Validator.resetForm();
        }

        //重置账户密码 
        function ResetAccountPassWd() {
            $("#EditAccontPassWdModal").modal('show');
            $("#OriginalPassWd").val("");
            $("#NewPassWd").val("");
            $("#ConfirmPassword").val("");
            easyuiCustomerPassWdValidate();
        }


        //保存账户密码 
        function btnSaveAccountPassWd() {
            var result = SaveAccountPassWord();
            if (result && result.Code == 0) {
                jSuccess("密码修改成功", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                $("#EditAccontPassWdModal").modal('hide');
            } else {
                jError("密码修改失败", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                $("#OriginalPassWd").val("").focus();
                $("#NewPassWd").val("");
                $("#ConfirmPassword").val("");
            }
        }
        //保存密码 
        function SaveAccountPassWord() {
            var userPassWd = new Object();
            userPassWd.OldPassWord = $.md5($.trim($("#OriginalPassWd").val()));
            userPassWd.NewPassWord = $.md5($.trim($("#NewPassWd").val()));
            return BsrCloudServer.Customer.SaveAccountPassWord(userPassWd);
        }

        //取消修改当前登陆账户的安全信息
        function CancelSaveAccountSafeInfo() {
            EnableOrBanCurrentAccountSafeInfo(false);
            getCurrentAccountSafeInfo();
            $("#editAccountSafeInfo_a").text("修改").css("color", "#808080");
            $("#cancleEditAccountSafeInfo").hide();
        }

        //修改账户的安全信息 
        function EditAccountSafeInfo(obj) {
            if (obj) {
                if ($.trim($(obj).text()) == "修改") {
                    EnableOrBanCurrentAccountSafeInfo(true);
                    $(obj).text("保存").css("color", "");
                    $("#cancleEditAccountSafeInfo").show();
                    $("#cancleEditAccountSafeInfo").css("color", "");
                }
                else {
                    SaveAccountSafeInfo(obj);
                }
            }
        }
        //保存用户的安全信息 登陆方式 3,4-手机客户端 1-BS端登陆(WEB) 2-CS端登陆
        function SaveAccountSafeInfo(obj) {
            var loginway = "";
            if ($("#PhoneClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "3,4";
                }
                else {
                    loginway += ",3,4";
                }
            }
            if ($("#BSPhoneClient").is(":checked")) {
                if ("" == loginway) {
                    loginway = "1";
                }
                else {
                    loginway += ",1";
                }
            }
            if ($("#CSClient").is(":checked")) {
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
                EnableOrBanCurrentAccountSafeInfo(false);
                $("#savePromptMessage").show(500, function () {
                    $("#savePromptMessage").text("保存成功");
                });
            }
            else {
                $("#savePromptMessage").show(500, function () {
                    $("#savePromptMessage").text("保存失败");
                });
            }
        }

        //获取用户选择的终端
        function GetAccoutSelectLoginTerminal() {
            var loginway = "";
            $("[name='chkLoginWay']").each(function () {
                if ($(this)[0].checked) {
                    if ("" == loginway) {
                        if ($(this).attr("id") == "PhoneClient") {
                            loginway = "3,4";
                        }
                        else if ($(this).attr("id") == "BSPhoneClient") {
                            loginway = "1";
                        }
                        else {
                            loginway = "2";
                        }
                    }
                    else {
                        if ($(this).attr("id") == "PhoneClient") {
                            loginway += ",3,4";
                        }
                        else if ($(this).attr("id") == "BSPhoneClient") {
                            loginway += ",1";
                        }
                        else {
                            loginway += ",2";
                        }
                    }
                }
            });
            return loginway;
        }

        /*--创建主账户基本信息验证--*/
        function easyuiMainAccountValidate() {
            var mianAccountFrom_Validator = $("#mianAccountFrom").validate({
                onkeyup: false,
                rules: {
                    mianAccountUserName: {
                        required: true,
                        string: true,
                        alnum: true,
                        isExistedLogname: true
                    },
                    mianAccountPassword: {
                        required: true,
                        password: true
                    },
                    mianAccountPasswordAgain: {
                        required: true,
                        equalTo: "#mianAccountPassword"
                    },
                    mianAccountMobilePhone: { required: true, mobilephone: true, isExistedLogname: true },
                    mianAccountEmial: { email: true, isExistedLogname: true },
                    mianAccountPhone: { telephone: true }
                },
                messages: {
                    mianAccountUserName: { required: "请输入用户名" },
                    mianAccountPassword: { required: "请输入密码" },
                    mianAccountPasswordAgain: {
                        required: "请重复输入密码",
                        equalTo: "密码不一致"
                    },
                    mianAccountMobilePhone: { required: "请输入手机号" },
                    mianAccountEmial: { email: "邮箱格式不正确" }
                },
                submitHandler: function () {
                    SaveMianAccoutInfo();
                }
            });
            mianAccountFrom_Validator.resetForm();
        }

        //        /*--创建主账户基本信息验证--*/
        //        function easyuiMainAccountValidate() {
        //            //用户名
        //            $('#mianAccountUserName').validatebox({
        //                required: true,
        //                missingMessage: '不能为空.',
        //                validType: ["account", "remoteRest['ServiceCustomer.svc/GetCustomerName','CustomerName']"]
        //            });
        //            //密码
        //            $('#mianAccountPassword').validatebox({
        //                required: true,
        //                validType: 'safepass',
        //                missingMessage: '不能为空.'
        //            });
        //            //重复密码
        //            $("#mianAccountPasswordAgain").validatebox({
        //                required: true,
        //                missingMessage: "不能为空",
        //                validType: "equalTo['#mianAccountPassword']",
        //                invalidMessage: "密码不一致."
        //            });
        //            //手机
        //            $("#mianAccountMobilePhone").validatebox({
        //                required: true,
        //                missingMessage: '不能为空.',
        //                validType: ["mobile", "remoteRest['ServiceCustomer.svc/GetReceiverPhone','ReceiverCellPhone']"],
        //                invalidMessage: ["手机号码不正确.", "已经注册."]
        //            });
        //            //固话
        //            $("#mianAccountPhone").validatebox({
        //                validType: "telphone"
        //            });
        //            //邮箱
        //            $("#mianAccountEmial").validatebox({
        //                invalidMessage: "邮箱格式不正确或已经注册.",
        //                validType: ["email", "remoteRest['ServiceCustomer.svc/GetReceiverEmail','ReceiverEmail']"]
        //            });
        //            //地址
        //            $("#mianAccountAddr").validatebox({
        //                validType: "maxLength[512]",
        //                invalidMessage: "最多512字."
        //            });
        //        }

        /*------创建主账户-------*/
        function SaveMianAccoutInfo() {
            var loginway = "";
            if ($("#inlineCheckbox1").is(":checked")) {
                if ("" == loginway) {
                    loginway = "3,4";
                }
                else {
                    loginway += ",3,4";
                }
            }
            if ($("#inlineCheckbox2").is(":checked")) {
                if ("" == loginway) {
                    loginway = "1";
                }
                else {
                    loginway += ",1";
                }
            }
            if ($("#inlineCheckbox3").is(":checked")) {
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
            var userinfo = new Object();
            userinfo.CustomerName = $('#mianAccountUserName').val();
            userinfo.Password = $.md5($.trim($("#mianAccountPassword").val()));
            userinfo.ReceiverName = $.trim($("#mianAccountRealName").val());
            userinfo.ReceiverEmail = $.trim($("#mianAccountEmial").val());
            userinfo.ReceiverCellPhone = $.trim($("#mianAccountMobilePhone").val());
            userinfo.AccountTelephone = $.trim($("#mianAccountPhone").val());
            userinfo.AccountCompanyAddress = $.trim($("#mianAccountAddr").val());
            userinfo.ImageByteBase64 = "";
            userinfo.ExtName = "";
            if ($("input:radio[name='inlineRadioOptions']:checked")[0].id == "inlineRadio_Evertime") {
                userinfo.ValidTime = null;
            }
            else {
                userinfo.ValidTime = $('#ValidEndtime').val();
            }
            userinfo.LoginTypes = loginway;
            var result = BsrCloudServer.Customer.AddPrimaryCustomerByManagerAccount(userinfo);
            if (result && result.Code == 0) {
                InitAccoutPanelBody();
                LoadPrimaryAccountImageListInfo("");
                $("#SaveChildAccoutInfo_Text").show(500, function () {
                    $("#SaveChildAccoutInfo_Text").text("保存成功");
                });
            }
            else {
                $("#SaveChildAccoutInfo_Text").show(500, function () {
                    $("#SaveChildAccoutInfo_Text").text("保存失败");
                });
            }
        }

        /*------添加验证-------*/
        function easyuiValidate() {
            CurrentLoginCostomerInfo_Validator = $("#CurrentLoginCostomerInfo").validate({
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
                    SaveCurrentAccountInfo();
                }
            });
        }

        /*------提交-------*/
        function easyuiSubmit() {
            var result = updateCustomer();
            if (result && result.Code == 0) {
                $("#userImg").show();
                $("#userImg").unbind("click", ShowUpdateFilePage);
                $("#saveCurrentAccountInfo_divId").hide(500, function () {
                    $("#savePromptMessage").show();
                    $("#savePromptMessage").text("保存成功");
                });

                //当前登陆账户信息禁用
                $("#accontmobilePhone").attr('disabled', "true");
                $("#accountEmail").attr('disabled', "true");
                $("#accountRealName").attr('disabled', "true");
                $("#accountTelphone").attr('disabled', "true");
                $("#accountAddress").attr('disabled', "true");
            }
            else {
                $("#savePromptMessage").show();
                $("#savePromptMessage").text("保存失败");
            }
        }

        function updateCustomer() {
            var result = false;
            var user = new Object();
            user.ReceiverCellPhone = $.trim($("#accontmobilePhone").val());
            user.ReceiverEmail = $.trim($.trim($("#accountEmail").val()));
            user.ReceiverName = $.trim($("#accountRealName").val());
            user.AccountTelephone = $.trim($("#accountTelphone").val());
            user.AccountCompanyAddress = $.trim($("#accountAddress").val());
            user.ImageByteBase64 = "";
            user.ExtName = "";
            var userJson = JSON.stringify(user);
            return BsrCloudServer.Customer.updateCustomer(userJson);
        }

        //获取当前登陆用户的信息
        function getCustomerInfo() {
            var result = BsrCloudServer.Customer.getCustomerInfo();
            if (result && result.Code == 0) {
                $("#accontmobilePhone").val(result.customerReponse.ReceiverCellPhone);
                $("#accountEmail").val(result.customerReponse.ReceiverEmail);
                $("#accountRealName").val(result.customerReponse.ReceiverName);
                $("#accountTelphone").val(result.customerReponse.AccountTelephone);
                $("#accountAddress").val(result.customerReponse.AccountCompanyAddress);
                var path = "../" + result.customerReponse.ImagePath;
                $("#userImg").attr("src", path);
                $("#hidCustomerPhone").val(result.customerReponse.ReceiverCellPhone);
                $("#hidCustomerEmail").val(result.customerReponse.ReceiverEmail);
            }
            else {
                $("#savePromptMessage").show(500, function () {
                    $("#savePromptMessage").text("获取登陆信息失败");
                });
            }
        }

        //主界面中的主账户查询事件
        function SearchMainAccounts() {
            LoadPrimaryAccountImageListInfo($("#MainAccountsKeyWd").val());
            //            //$("#accordion").show();
            //            $("#hideSelectMainAccountID").val("");
            //            $("#accordion").children().not("#panelOne").remove();
            //            var result = BsrCloudServer.Customer.SerachPrimaryCustomer($("#MainAccountsKeyWd").val());
            //            var settings = $.extend({
            //                callback: null,
            //                isShowTip: true
            //            }, {});
            //            if (settings.isShowTip) {
            //                jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            //            }
            //            if (result && result.Code == 0) {
            //                if (result.customerReponseList.length > 0) {
            //                    for (var i = 0; i < result.customerReponseList.length; i++) {
            //                        if (0 == i) {
            //                            $("#hideSelectMainAccountID").val(result.customerReponseList[i].CustomerId);
            //                        }
            //                        $("#template-panel-type .panel .panel-heading div:eq(0) a").text(result.customerReponseList[i].CustomerName);
            //                        $("#template-panel-type .panel .panel-heading").attr("id", result.customerReponseList[i].CustomerId);
            //                        $("#template-panel-type .panel .panel-heading div:eq(0) label:eq(0)").text("");
            //                        $("#template-panel-type").find(".btn-group").children().eq(1).text("冻结");
            //                        if (result.customerReponseList[i].IsEnable == 0) {
            //                            if (result.customerReponseList[i].ForbiddenTime != null && result.customerReponseList[i].ForbiddenTime != "") {
            //                                $("#template-panel-type .panel .panel-heading div:eq(0) label:eq(0)").text("冻结日期:" + result.customerReponseList[i].ForbiddenTime).css({
            //                                    "color": "red", "font-family": "Arial"
            //                                });
            //                            }
            //                            $("#template-panel-type").find(".btn-group").children().eq(1).text("解冻");
            //                        }
            //                        $("#template-panel-type").children().clone().appendTo($("#accordion"));
            //                    }
            //                    $("#accordion").children().not("#panelOne").eq(0).find(".panel-heading").next().collapse('show');
            //                    $.ajax({
            //                        contentType: "charset=UTF-8",
            //                        type: "post",
            //                        cache: false,
            //                        url: "AccountManagement_Child.aspx",
            //                        success: function (data) {
            //                            $("#accordion").children().not("#panelOne").eq(0).find(".panel-body").html(data);
            //                        },
            //                        error: function (e) { alert("error") }
            //                    })

            //                }
            //                if ($.jNotify) {
            //                    $.jNotify._close();
            //                }
            //                if (settings.callback) {
            //                    settings.callback();
            //                }
            //            } else {
            //                alert("查询主用户失败!");
            //            }
        }

        function ShowHistoryInfo() {
            //$("#myModalLabel").html($("#spCustomerRealName").text() + " 历史登陆信息");
            $('#myModal').modal('show');
            $("#logtable tr:gt(0)").empty();
            var result = BsrCloudServer.Customer.GetSelfLoginInfo();
            if (result && result.Code == 0) {
                if (result.operaterLogList.length > 0) {
                    for (var i = 0; i < result.operaterLogList.length; i++) {
                        var tr = "<tr><td>" + result.operaterLogList[i].OperaterTime.toString() + "</td><td>" + result.operaterLogList[i].AgentType +
                         "</td><td>" + result.operaterLogList[i].AgentVersion + "</td></tr>";
                        $("#logtable").append(tr);
                    }
                }
            }
        }
        //修改用户信息
        function EditCurrentAccountInfo() {
            EnableOrDisableCurrentAccountInfo(true);
            $("#saveCurrentAccountInfo_divId").show();
            easyuiValidate();
        }

        //保存当前登陆账户的基本信息 
        function SaveCurrentAccountInfo() {
            easyuiSubmit();
        }

        //取消保存当前登陆账户的基本信息 
        function CancelSaveCurrentAccountInfo() {
            if (CurrentLoginCostomerInfo_Validator) {
                CurrentLoginCostomerInfo_Validator.resetForm();
            }
            EnableOrDisableCurrentAccountInfo(false);
            $("#saveCurrentAccountInfo_divId").hide();
            getCustomerInfo();
        }

        //启用或者禁用前台管理员的基本信息
        function EnableOrDisableCurrentAccountInfo(obj) {
            $("#savePromptMessage").hide();
            $("#saveCurrentAccountInfo_divId").hide();
            if (obj) { //启用
                $("#CurrentLoginCostomerInfo input").removeAttr("disabled");
                $("#userImg").bind("click", ShowUpdateFilePage);
            } else {
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
                $("#CurrentCustomerInfoDiv").show();
                $("#CurrentCustomerSafeInfoDiv").hide();
                $("#CurrentLoginCostomerInfo input").attr("disabled", "disabled");
                $("#userImg").unbind("click", ShowUpdateFilePage);
            }
        }

        function ShowUpdateFilePage() {
            $('#modelUpfileWin').modal('show');
        }

        //创建主用户时
        function ShowFilePage() {
            $('#modelUpfileWin').modal('show');

        }
        function btnUpdateImage() {
            readURL($("#InputFile")[0]);
        }
        function readURL(input) {
            if ($("#InputFile").val() == "") {
                $("#savePromptMessage").show(500, function () {
                    $("#savePromptMessage").text("上传图片不能为空！");
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
                    $("#savePromptMessage").show(500, function () {
                        $("#savePromptMessage").text("上传图片格式不正确！");
                    });
                    return;
                }
                $.ajaxFileUpload({
                    url: '../PageHandler/UpLoadImage.ashx',
                    type: 'post',
                    secureuri: false, //一般设置为false
                    fileElementId: 'InputFile', // 上传文件的id、name属性名
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
                                $("#userImg").attr("src", path_upLoadImg);
                                $("#savePromptMessage").show();
                                $("#savePromptMessage").text("上传图片成功");
                            }
                            else {
                                $("#savePromptMessage").show();
                                $("#savePromptMessage").text("上传图片失败");
                            }
                        }
                        else {
                            $("#savePromptMessage").show();
                            $("#savePromptMessage").text("上传图片失败");
                        }
                    },
                    error: function (data, status, e) {
                        alert(e);
                    }
                });

            } else {
                //IE下，使用滤镜
                var docObj = document.getElementById('InputFile');
                docObj.select();
                //解决IE9下document.selection拒绝访问的错误 
                docObj.blur();
                var path = document.selection.createRange().text;
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
                    $("#savePromptMessage").show(500, function () {
                        $("#savePromptMessage").text("上传图片格式不正确！");
                    });
                    return;
                }
                imgSrc = imgSrc.replace(/\\/g, "\\\\");
                $.ajaxFileUpload({
                    url: '../PageHandler/UpLoadImage.ashx',
                    type: 'post',
                    secureuri: false, //一般设置为false
                    fileElementId: 'InputFile', // 上传文件的id、name属性名
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
                                $("#userImg").attr("src", path_upLoadImg);
                                $("#savePromptMessage").show();
                                $("#savePromptMessage").text("上传图片成功");
                            }
                            else {
                                $("#savePromptMessage").show();
                                $("#savePromptMessage").text("上传图片失败");
                            }
                        }
                        else {
                            $("#savePromptMessage").show();
                            $("#savePromptMessage").text("上传图片失败");
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

        //删除前台管理员下面的主账户 
        function DeleteMainAccout(obj) {
            var PrimaryCustomerIds = new Object();
            var myPrimaryCustomerId = [];
            myPrimaryCustomerId.push($(obj).closest(".panel-heading").attr("id"));
            PrimaryCustomerIds.PrimaryCustomerId = myPrimaryCustomerId;
            var result = BsrCloudServer.Customer.DeletePrimaryCustomer(PrimaryCustomerIds);
            if (result && result.Code == 0) {
                if ($(obj).closest(".panel-heading").next().attr("class").indexOf("in") >= 0) {
                    $(obj).closest(".panel").remove();

                    if ($("#accordion").children().not("#panelOne").length > 0) {
                        $("#accordion").children().not("#panelOne").eq(0).find(".panel-heading").next().collapse('show');
                        $("#hideSelectMainAccountID").val($("#accordion").children().not("#panelOne").eq(0).find(".panel-heading").attr("id"));
                        $.ajax({
                            contentType: "charset=UTF-8",
                            type: "post",
                            cache: false,
                            url: "AccountManagement_Child.aspx",
                            success: function (data) {
                                $("#accordion").children().not("#panelOne").eq(0).find(".panel-body").html(data);
                            },
                            error: function (e) { alert("error") }
                        })
                    }
                }
                else {
                    $(obj).closest(".panel").remove();
                }

                jError("删除成功!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            else {
                jError("删除失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        function DeleteChildAccoutByModelOne() {
            var PrimaryCustomerIds = new Object();
            var myPrimaryCustomerId = [];
            myPrimaryCustomerId.push($("#hideSelectMainAccountID").val());
            PrimaryCustomerIds.PrimaryCustomerId = myPrimaryCustomerId;
            var result = BsrCloudServer.Customer.DeletePrimaryCustomer(PrimaryCustomerIds);
            if (result && result.Code == 0) {
                LoadPrimaryAccountImageListInfo("");
                jSuccess("删除成功!", { VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            else {
                jError("删除失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
        }

        //冻结解冻主账户
        function FreezeOrThawPrimaryCustomer(obj) {
            var userinfo = new Object();
            userinfo.PrimaryCustomerId = $(obj).closest(".panel-heading").attr("id");
            if ($(obj).text() == "冻结") {
                userinfo.IsEnable = 0;
            }
            else {
                userinfo.IsEnable = 1;
            }
            var result = BsrCloudServer.Customer.EnablePrimaryCustomer(userinfo);
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
                    jError("主账户冻结失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                }
                else {
                    jError("主账户解冻失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
                }
            }
        }

        function FreezeChildAccount(obj) {
            if (obj) {
                var flag = false;
                var userinfo = new Object();
                userinfo.PrimaryCustomerId = $("#hideSelectMainAccountID").val();
                if ($.trim($(obj).find("img").attr("title")) == "冻结") {
                    userinfo.IsEnable = 0;
                    flag = true;
                }
                else {
                    userinfo.IsEnable = 1;
                    flag = false;
                }
                var result = BsrCloudServer.Customer.EnablePrimaryCustomer(userinfo);
                if (result && result.Code == 0) {
                    if (flag) {
                        $(obj).find("img").attr("title", "解冻");
                        $("#" + $("#hideSelectMainAccountID").val()).next().addClass("ImgOn");
                    }
                    else {
                        $(obj).find("img").attr("title", "冻结");
                        $("#" + $("#hideSelectMainAccountID").val()).next().removeClass("ImgOn");
                    }
                }
                else {

                }
            }
        }

        //主账户授权 
        function AuthorizeAccount(obj) {
            ShowOnePanel(obj);
            $(obj).closest(".panel-heading").next().children(".panel-body").children().remove();
            createTree(obj);
            var AuthorizeBtn = $("<div class='col-md-1 col-md-offset-10' style='padding-top:8px;'>  <button type='button' class='btn btn-sm btn-primary' data-dismiss='modal' onclick='btnSaveAuthorizePrimary();return false;'> 保存</button> </div>");
            var AuthorizeTree = $("<div class='col-md-12'><ul id='myAuthorizeTree' class='ztree'></ul></div>");
            var AuthorizeNodes = [];
            var result = BsrCloudServer.Customer.GetAuthorizeOtherPrimaryResponseDto($(obj).closest(".panel-heading").attr("id"));

            if (result.authorizeCustomerResponseList != null && result.authorizeCustomerResponseList.length > 0) {
                for (var i = 0; i < result.authorizeCustomerResponseList.length; i++) {
                    var flag = false;
                    for (var j = 0; j < result.authorizeCustomerResponseList[i].authorizeDeviceResponseList.length; j++) {
                        if (result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].IsEnable == 1) {
                            flag = true;
                            AuthorizeNodes.push({ "id": result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceId, "pId": result.authorizeCustomerResponseList[i].CustomerId, "name": result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName, "checked": true });
                        }
                        else {
                            AuthorizeNodes.push({ "id": result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceId, "pId": result.authorizeCustomerResponseList[i].CustomerId, "name": result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName, "checked": false });
                        }
                    }
                    if (result.authorizeCustomerResponseList[i].DeviceCount > 0) {
                        AuthorizeNodes.push({ "id": result.authorizeCustomerResponseList[i].CustomerId, "pId": 0, "name": result.authorizeCustomerResponseList[i].CustomerName, isParent: true, open: true, "checked": flag });
                    }
                    else {
                        AuthorizeNodes.push({ "id": result.authorizeCustomerResponseList[i].CustomerId, "pId": 0, "name": result.authorizeCustomerResponseList[i].CustomerName, isParent: true, open: true, "checked": flag });
                    }
                }
            }
            $.fn.zTree.init($(AuthorizeTree).children(), setting, AuthorizeNodes);
            treeObj = $.fn.zTree.getZTreeObj("myAuthorizeTree");
            AuthorizeBtn.appendTo($(obj).closest(".panel-heading").next().children(".panel-body"));
            AuthorizeTree.appendTo($(obj).closest(".panel-heading").next().children(".panel-body"));
            if ($(obj).text() == "授权") {
                $(obj).text("取消授权");
            } else {
                $(obj).text("授权");
            }
        }

        //创建树 
        var setting;
        function createTree(obj) {
            //树设置
            setting = {
                data: {
                    simpleData: {
                        enable: true
                    },
                    keep: {
                        parent: false,
                        leaf: true
                    }
                },
                check: {
                    enable: true,
                    chkStyle: "checkbox",
                    chkboxType: { "Y": "ps", "N": "ps" }
                },
                view: {
                    showIcon: true,
                    showLine: false,
                    showTitle: false,
                    selectedMulti: false
                },
                treeNode: {
                    nocheck: false,
                    checked: true
                }
            };
        }


        //当选择永久有效时，禁用时间控件并清空
        function ChangeToEver() {
            $("#ValidEndtime").attr("disabled", "disabled");
            $("#ValidEndtime").val("");
        }

        //当选择截止有效时，启用时间控件
        function ChangeToEnd() {
            $("#ValidEndtime").removeAttr("disabled");
            var endtime = new Date();
            endtime.setMonth(endtime.getMonth() + 3);
            var dateTime = endtime.Format("yyyy-MM-dd hh:mm:ss");
            $("#ValidEndtime").val(dateTime);
        }

        //同一时间只显示一个TAB页
        function ShowOnePanel(obj) {
            //$(obj).closest(".panel-heading").next().collapse("hide");
            $(obj).closest(".panel-heading").next().collapse('show');
            if ($(obj).closest(".panel-heading").next().attr("class").indexOf("in") >= 0) {
                $("#hideSelectMainAccountID").val($(obj).closest(".panel-heading").attr("id"));

                $("#accordion").children().not("#panelOne").each(function () {
                    if ($(this).find(".panel-heading").next().attr("class").indexOf("in") >= 0 &&
                       $(obj).closest(".panel-heading").attr("id") != $(this).find(".panel-heading").attr("id")) {
                        $(this).find(".panel-body").children().remove();
                        $(this).find(".panel-heading").next().collapse('hide');
                        $(this).find(".panel-heading").children().eq(1).children("button").eq(2).text("授权");
                    }
                })
            }
        }

        //加载子界面主账户的信息
        function OnShowMianAccoutPanelBody(obj) {
            ShowOnePanel(obj);
            $.ajax({
                contentType: "charset=UTF-8",
                type: "post",
                cache: false,
                url: "AccountManagement_Child.aspx",
                success: function (data) {
                    $(obj).closest(".panel-heading").next().children(".panel-body").html(data);
                },
                error: function (e) { alert("error") }
            })
        }

        //保存主账户的授权权限
        function btnSaveAuthorizePrimary() {
            var AuthorizePrimary = new Object();
            var authorizeCustomerResponseArray = [];
            var authorizeDeviceResponseArray = [];
            var myAuthorizeDeviceResponseList = new Object();
            if (treeObj.getCheckedNodes(true).length > 0) {
                var flag = false;
                for (var i = 0; i < treeObj.getCheckedNodes(true).length; i++) {
                    if (treeObj.getCheckedNodes(true)[i].isParent && treeObj.getCheckedNodes(true)[i].children.length > 0) {
                        flag = true;
                        var AuthorizePrimaryResponse = new Object();
                        AuthorizePrimaryResponse.CustomerId = $("#hideSelectMainAccountID").val();
                        for (var j = 0; j < treeObj.getCheckedNodes(true)[i].children.length; j++) {
                            if (treeObj.getCheckedNodes(true)[i].children[j].checked) {
                                var myAuthorizeDeviceResponse = new Object();
                                myAuthorizeDeviceResponse.DeviceId = treeObj.getCheckedNodes(true)[i].children[j].id;
                                myAuthorizeDeviceResponse.IsEnable = 1;
                                authorizeDeviceResponseArray.push(myAuthorizeDeviceResponse);
                            }
                        }
                        AuthorizePrimaryResponse.authorizeDeviceResponseList = authorizeDeviceResponseArray;
                    }
                }
                authorizeCustomerResponseArray.push(AuthorizePrimaryResponse);
                AuthorizePrimary.authorizeCustomerResponseList = authorizeCustomerResponseArray;
                if (flag) {//保存
                    var result = BsrCloudServer.Customer.AuthorizePrimaryByManagerAccount(AuthorizePrimary);
                    if (result && result.Code == 0) {
                        alert("授权成功!");
                    } else {
                        alert("授权失败!");
                    }
                }
            }
        }

        //初始化加载主账户下子账户的图片列表
        function LoadPrimaryAccountImageListInfo(key) {
            AccountListStyle = 1;
            if ($("#primaryAccountImglist").children().length > 0) {
                $("#primaryAccountImglist").children().remove();
            }
            $("#primaryAccountImglist").show();
            var settings = $.extend({
                callback: null,
                isShowTip: true
            }, {});
            if (settings.isShowTip) {
                jNotify("数据加载中...", { autoHide: false, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            var result = BsrCloudServer.Customer.SerachPrimaryCustomer(key);
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

                        //                        if (globalJS.GetStringRealLength(CustomerName) > 10) {
                        //                            CustomerName = CustomerName.substring(0, 5) + "...";
                        //                        }
                        listitem.find("center").attr("title", result.customerReponseList[i].CustomerName).css("cursor", "pointer");
                        listitem.find("center").text(CustomerName);
                        listitem.appendTo($("#template-account-img-list").children());
                        if (Math.floor(i / 4) < Math.floor(result.customerReponseList.length / 4) && 0 == (i + 1) % 4) {
                            $("#template-account-img-list").children().clone().css("height", "106px").appendTo($("#primaryAccountImglist"));
                            $("#template-account-img-list").children().children().remove();

                        }
                        else if (Math.floor(i / 4) == Math.floor(result.customerReponseList.length / 4) && i == (result.customerReponseList.length - 1)) {
                            $("#template-account-img-list").children().clone().css("height", "106px").appendTo($("#primaryAccountImglist"));
                            $("#template-account-img-list").children().children().remove();
                        }
                    }
                }
            } else {
                jError("加载主用户信息失败!", { autoHide: true, VerticalPosition: 'top', HorizontalPosition: 'center', ShowOverlay: false });
            }
            if ($.jNotify) {
                $.jNotify._close();
            }
            if (settings.callback) {
                settings.callback();
            }
        }

        function btnShowImgToInfo(obj) {
            if (obj) {
                if ($(obj).parents().eq(1).next("[name='ImgListInfo']").length > 0 &&
                    ($(obj).parent().position().left + 105).toFixed(0) + "px" == $(obj).parents().eq(1).next().css("left")) {
                    $("[name='ImgListInfo']").remove();
                }
                else {
                    $("[name='ImgListInfo']").remove();
                    var cloneImgInfo = $("#template-account-img-info").children().clone();
                    //cloneImgInfo.find(".imageSpanBanner").css("left",cloneImgInfo.find("#img-info-title").children().eq(0).position().left);
                    cloneImgInfo.attr("name", "ImgListInfo");
                    cloneImgInfo.eq(1).children().first().find("img").eq(1).attr("title", $(obj).attr("isenable"));
                    cloneImgInfo.eq(1).children().last().attr("class", "imageInfo row");
                    $(obj).parents().eq(1).after(cloneImgInfo);
                    var left = ($(obj).parent().position().left + 105).toFixed(0);
                    $(obj).parents().eq(1).next().css({ "position": "absolute", "left": left + "px", "top": (96 + $(obj).parent().position().top) + "px" });
                }
                if ($(obj).attr("id")) {
                    $("#hideSelectMainAccountID").val($(obj).attr("id"));
                    IsBaseInfoOrSafeInfo = true;
                    $("#editCurrentChildAccountInfo_a").show();
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "AccountManagement_Child.aspx",
                        success: function (data) {
                            $(".imageInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
            }
        }

        //在相应操作下显示下划线图标并显示相应信息
        function OnShowBanner(obj) {
            if (obj) {
                $(obj).parent().next().css({ "left": ($(obj).position().left + 0).toFixed(0) + "px", "top": ($(obj).position().top + 62).toFixed(0) + "px" });
                if ($.trim($(obj).text()) == "基本信息") {
                    //LoadCurrentChildAccountDiv();
                    IsBaseInfoOrSafeInfo = true;
                    $("#editCurrentChildAccountInfo_a").show();
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "AccountManagement_Child.aspx",
                        success: function (data) {
                            $(".imageInfo").html(data);
                        },
                        error: function (e) { alert("error") }
                    })
                }
                else if ($.trim($(obj).text()) == "账户安全") {
                    $("#editCurrentChildAccountInfo_a").hide();
                    IsBaseInfoOrSafeInfo = false;
                    $.ajax({
                        contentType: "charset=UTF-8",
                        type: "post",
                        cache: false,
                        url: "AccountManagement_Child.aspx",
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
                    $("#anthorizeHandleDiv").hide();
                    $("#anthorizeHandleMessageDiv").hide();
                    LoadChildAccountAuthorizeInfo(true);
                    var clone = $("#childAuthorizeConfigMode").children().clone();
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

        function OnChangeVedioStatus(obj) {
            if (obj) {
                if ($(obj).is(":checked")) {
                    $(obj).closest("tr").find("[name='Device-" + $(obj).attr("id") + "']").prop("checked", true);
                }
                else {
                    $(obj).closest("tr").find("[name='Device-" + $(obj).attr("id") + "']").prop("checked", false);
                }
            }
        }

        function OnChangeAccountStatus(obj) {
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

        //加载子账户的权限信息 ReadOnlyOrConfig--只读或者可编辑
        function LoadChildAccountAuthorizeInfo(ReadOnlyOrConfig) {
            $(".subAuthorizeTable").html("");
            var result = BsrCloudServer.Customer.GetAuthorizeOtherPrimaryResponseDto($("#hideSelectMainAccountID").val());
            if (result && result.Code == 0) {
                if (result.authorizeCustomerResponseList != null && result.authorizeCustomerResponseList.length > 0) {
                    var maxCol = 0;
                    for (var k = 0; k < result.authorizeCustomerResponseList.length; k++) {
                        if (result.authorizeCustomerResponseList[k].DeviceCount > maxCol) {
                            maxCol = result.authorizeCustomerResponseList[k].DeviceCount;
                        }
                    }
                    for (var i = 0; i < result.authorizeCustomerResponseList.length; i++) {
                        var flag = true;
                        var tr = $("<tr></tr>");
                        var td_Accout = $("<td style='color:#000000;' CustomerId='" + result.authorizeCustomerResponseList[i].CustomerId + "'></td>");
                        for (var j = 0; j < result.authorizeCustomerResponseList[i].authorizeDeviceResponseList.length; j++) {
                            var td_Device = $("<td></td>");
                            var td_Device_Div = $("<div style='line-height: 19px; text-align: left;word-break:break-all;'></div>");
                            var td_Device_Div_Lable = null;
                            var td_Device_Div_Lable_Img = null;
                            var td_Device_Div_Lable_Input = null;
                            var DeviceName = result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName;
                            //                            if (DeviceName.length > 7) {
                            //                                DeviceName = DeviceName.substring(0, 7) + "...";
                            //                            }
                            if (result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].IsEnable == 1) {
                                if (ReadOnlyOrConfig) {//只读
                                    td_Device_Div_Lable = $("<a class='text-overflow-ellipsis' title='" + result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName + "'>" + DeviceName + "</a>");
                                    td_Device_Div_Lable_Img = $("<img src='../Images/icons/icon_Selected.png' width='16px' height='16px' style='vertical-align: middle;margin-right:6px;' />");
                                    td_Device_Div_Lable.prepend(td_Device_Div_Lable_Img);
                                    td_Device_Div.append(td_Device_Div_Lable);
                                } else {
                                    td_Device_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName + "'></label>");
                                    td_Device_Div_Lable_Input = $("<input onclick='OnChangeAccountStatus(this)' id='" + result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceId + "' name='Device-" + result.authorizeCustomerResponseList[i].CustomerId + "' type='checkbox' style='vertical-align: -1px;' checked='checked'>" + DeviceName + "</input>");
                                    td_Device_Div_Lable.append(td_Device_Div_Lable_Input);
                                    td_Device_Div.append(td_Device_Div_Lable);
                                }
                            }
                            else {
                                flag = false;
                                if (!ReadOnlyOrConfig) {
                                    td_Device_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName + "'></label>");
                                    td_Device_Div_Lable_Input = $("<input onclick='OnChangeAccountStatus(this)' id='" + result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceId + "' name='Device-" + result.authorizeCustomerResponseList[i].CustomerId + "' type='checkbox' style='vertical-align: -1px;'>" + DeviceName + " </input>");
                                    td_Device_Div_Lable.append(td_Device_Div_Lable_Input);
                                    td_Device_Div.append(td_Device_Div_Lable);
                                }
                                else {
                                    td_Device_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' title='" + result.authorizeCustomerResponseList[i].authorizeDeviceResponseList[j].DeviceName + "'>" + DeviceName + "</label>");
                                    td_Device_Div.append(td_Device_Div_Lable);
                                }
                            }
                            td_Device.append(td_Device_Div);
                            tr.append(td_Device);
                        }
                        if (result.authorizeCustomerResponseList[i].authorizeDeviceResponseList.length == 0) {
                            flag = false;
                        }
                        if (maxCol <= 8) {
                            for (var k = result.authorizeCustomerResponseList[i].authorizeDeviceResponseList.length; k < 8; k++) {
                                var td_Device_Add = $("<td></td>");
                                tr.append(td_Device_Add);
                            }
                        }
                        else {
                            for (var k = result.authorizeCustomerResponseList[i].authorizeDeviceResponseList.length; k < maxCol; k++) {
                                var td_Device_Add = $("<td></td>");
                                tr.append(td_Device_Add);
                            }
                        }

                        var td_Accout_Div = $("<div style='line-height: 19px; text-align: left;word-break:break-all;cursor:pointer;'></div>");
                        var td_Accout_Div_Lable = null;
                        var td_Accout_Div_Lable_Img = null;
                        var td_Accout_Div_Lable_Input = null;
                        var CustomerName = result.authorizeCustomerResponseList[i].CustomerName;
                        //                        if (result.authorizeCustomerResponseList[i].CustomerName.length > 7) {
                        //                            CustomerName = result.authorizeCustomerResponseList[i].CustomerName.substring(0, 7) + "...";
                        //                        }
                        if (ReadOnlyOrConfig) {//只读
                            if (flag) {
                                td_Accout_Div_Lable = $("<a class='text-overflow-ellipsis' style='display:inline-block' title='" + result.authorizeCustomerResponseList[i].CustomerName + "'>" + CustomerName + "</a>");
                                td_Accout_Div_Lable_Img = $("<img src='../Images/icons/icon_Selected.png' width='16px' height='16px' style='vertical-align: middle;margin-right:6px;' />");
                                td_Accout_Div_Lable.prepend(td_Accout_Div_Lable_Img);
                                td_Accout_Div.append(td_Accout_Div_Lable);
                            }
                            else {

                                td_Accout_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' title='" + result.authorizeCustomerResponseList[i].CustomerName + "'>" + CustomerName + "</label>");
                                td_Accout_Div.append(td_Accout_Div_Lable);
                            }
                            td_Accout.append(td_Accout_Div);
                        }
                        else {
                            if (flag) {
                                td_Accout_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeCustomerResponseList[i].CustomerName + "'></label>");
                                td_Accout_Div_Lable_Input = $("<input onclick='OnChangeVedioStatus(this);' id='" + result.authorizeCustomerResponseList[i].CustomerId + "' type='checkbox' style='vertical-align: -1px;' checked='checked'>" + CustomerName + "</input>");
                                td_Accout_Div_Lable.append(td_Accout_Div_Lable_Input);
                                td_Accout_Div.append(td_Accout_Div_Lable);

                            }
                            else {
                                td_Accout_Div_Lable = $("<label class='text-overflow-ellipsis' style='cursor:pointer;' class='checkbox-inline' title='" + result.authorizeCustomerResponseList[i].CustomerName + "'></label>");
                                td_Accout_Div_Lable_Input = $("<input onclick='OnChangeVedioStatus(this)' id='" + result.authorizeCustomerResponseList[i].CustomerId + "' type='checkbox' style='vertical-align: -1px;'>" + CustomerName + "</input>");
                                td_Accout_Div_Lable.append(td_Accout_Div_Lable_Input);
                                td_Accout_Div.append(td_Accout_Div_Lable);
                            }
                            td_Accout.append(td_Accout_Div);
                        }
                        tr.prepend(td_Accout);
                        $(".subAuthorizeTable").append(tr);
                    }
                }
            }
            else {

            }
        }

        //编辑子账户的权限
        function ShowEditChildAuthorize() {
            $("#anthorizeHandleDiv").show();
            LoadChildAccountAuthorizeInfo(false);
        }

        function CancleSaveChildAuthorizeInfo() {
            $("#anthorizeHandleDiv").hide();
            $("#anthorizeHandleMessageDiv").hide();
            LoadChildAccountAuthorizeInfo(true);
        }

        //保存当前选中子账户的权限
        function btnSaveChildAuthorizeInfo() {
            if ($(".imageInfo").find("tr").length > 0) {
                var AuthorizePrimary = new Object();
                var authorizeCustomerResponseArray = [];
                var authorizeDeviceResponseArray = [];
                var myAuthorizeDeviceResponseList = new Object();
                for (var i = 0; i < $(".imageInfo").find("tr").length; i++) {
                    var AuthorizePrimaryResponse = new Object();
                    AuthorizePrimaryResponse.CustomerId = $("#hideSelectMainAccountID").val();
                    for (var j = 0; j < $(".imageInfo").find("tr").eq(i).find("td").length; j++) {
                        if (j > 0) {
                            if ($(".imageInfo").find("tr").eq(i).find("td").eq(j).find("input").is(":checked")) {
                                var myAuthorizeDeviceResponse = new Object();
                                myAuthorizeDeviceResponse.DeviceId = $(".imageInfo").find("tr").eq(i).find("td").eq(j).find("input").attr("id");
                                myAuthorizeDeviceResponse.IsEnable = 1;
                                authorizeDeviceResponseArray.push(myAuthorizeDeviceResponse);
                            }
                        }
                    }
                    AuthorizePrimaryResponse.authorizeDeviceResponseList = authorizeDeviceResponseArray;
                }
                authorizeCustomerResponseArray.push(AuthorizePrimaryResponse);
                AuthorizePrimary.authorizeCustomerResponseList = authorizeCustomerResponseArray;

                var result = BsrCloudServer.Customer.AuthorizePrimaryByManagerAccount(AuthorizePrimary);
                if (result && result.Code == 0) {
                    $("#anthorizeHandleDiv").hide();
                    $("#anthorizeHandleMessageDiv").show(500, function () {
                        $("#anthorizeHandleMessageDiv").find("p").text("保存成功");
                    });
                    LoadChildAccountAuthorizeInfo(true);
                } else {
                    $("#anthorizeHandleMessageDiv").show(500, function () {
                        $("#anthorizeHandleMessageDiv").find("p").text("保存失败");
                    });
                }
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
