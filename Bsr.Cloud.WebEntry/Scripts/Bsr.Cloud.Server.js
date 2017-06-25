/*------星际云服务-------*/
var BsrCloudServer = {
    host: window.location.host,
    token: function () {
        var token = globalJS.getUrlParam("token");
        if (token) {
            return token;
        } else {
            return "";
        }
    },
    //保持心跳
    authToken: function () {
        var token = globalJS.getUrlParam("token");
        if (token) {
            setInterval(function () {
                $.ajax({
                    type: "post",
                    url: "http://" + window.location.host + "/ServiceCustomer.svc/Timing",
                    headers: { "BstarCloud-User-Token": token },
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (msg) {
                    },
                    error: function (e) {
                    }
                });
            }, 30000);
        } else { window.location.href = "../Login.aspx" }
    },
    //星际云服务获取rest服务统一方法
    ajax: function (url, options) {
        var token = this.token();
        var settings = $.extend({
            type: "post",
            async: true,
            headers: { "BstarCloud-User-Token": token },
            url: "http://" + this.host + "/" + url,
            data: "",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: ajaxSuccess,
            error: ajaxError
        }, options || {});
        var result;
        function ajaxSuccess(msg) {
            result = msg;
        }
        function ajaxError(e) {
            alert("数据请求错误\n请求方法：" + url + "\n准备状态：" + e.readyState + "\n请求状态：" + e.status + "\n状态说明：" + e.statusText);
        }
        $.ajax(settings);
        return result;
    }
};
(function (BsrCloudServer) {
    /*----------用户服务-------------*/
    BsrCloudServer.Customer = {
        //登陆 zhangmz
        login: function (username, password, success) {
            var user = new Object();
            user.LoginName = username;
            user.Password = password;
            user.LoginType = 1;
            user.AgentVersion = globalJS.getBrowser();
            BsrCloudServer.ajax("ServiceCustomer.svc/Login", { data: JSON.stringify(user), async: true, success: success });
        },
        //注销 zhangmz
        logout:function(){
        
        },
        //获取当前用户访问权限
        getSelfPermission:function(success){
            BsrCloudServer.ajax("ServiceCustomer.svc/GetSelfPermission", {async: false,success:success });
        },
        //检查帐号有没有注册
        isRegister:function(loginName,success){
            var obj=new Object();
            obj.LoginName=loginName;
            BsrCloudServer.ajax("ServiceCustomer.svc/IsRegisterByLoginName", { data: JSON.stringify(obj), async: false,success:success });
            //return result;
        },
        //注册主账号 zhangmz
        addPrimaryCustomer: function (customer) {
            var result = BsrCloudServer.ajax("ServiceCustomer.svc/AddPrimaryCustomer", { data: JSON.stringify(customer), async: false });
            return result;
        },
        //发送验证码至用户手机(注册)
        sendVerifyKeyRigister: function (phoneNum, success) {
            var obj = new Object();
            obj.PhoneNum = phoneNum;
            BsrCloudServer.ajax("ServiceCustomer.svc/SendRegisterUserWithSum", { data: JSON.stringify(obj), async: true, success: success });
        },
        //发送验证码至用户手机(密码找回)
        sendVerifyKeyFindPwd: function (loginName, success) {
            var obj = new Object();
            obj.LoginName = loginName;
            BsrCloudServer.ajax("ServiceCustomer.svc/SendFindPasswordVerifyKey", { data: JSON.stringify(obj), async: true, success: success });
        },
        //校验验证码(注册)
        checkVerfyKeyRigister: function (phoneNum, verifyKey, success) {
            var obj = new Object();
            obj.PhoneNum = phoneNum;
            obj.VerifyKey = verifyKey;
            BsrCloudServer.ajax("ServiceCustomer.svc/CheckRegisterUserWithSum", { data: JSON.stringify(obj), async: true, success: success });
        },
        //校验验证码(密码找回)
        checkVerfyKeyFindPwd: function (loginName, verifyKey, success) {
            var obj = new Object();
            obj.LoginName = loginName;
            obj.VerifyKey = verifyKey
            BsrCloudServer.ajax("ServiceCustomer.svc/CheckFindPasswordVerfyKey", { data: JSON.stringify(obj), async: true, success: success });
        },
        //密码找回,修改密码
        updateCustomerPassWord: function (newPassWord, verifyToken, success) {
            var obj = new Object();
            obj.NewPassWord = newPassWord;
            obj.VerifyToken = verifyToken
            BsrCloudServer.ajax("ServiceCustomer.svc/UpdateCustomerPassWord", { data: JSON.stringify(obj), async: true, success: success });
        },
        //获取当前在线用户数量
        getCustomerOnlineTotal: function (success) {
            BsrCloudServer.ajax("ServiceCustomer.svc/GetCustomerOnlineTotal", { async: true, success: success });
        },
        //更新用户信息 jiangb
        updateCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdateSelfCustomer", { data: json, async: false });
        },
        //获取当前前台管理员下的主账号信息 jiangb
        SerachPrimaryCustomer: function (keyword) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/SearchPrimaryCustomer", { data: "{ \"KeyWord\":\"" + keyword + "\" }", async: false });
        },
        //获取当前登陆账户的基本信息 jiangb,update zhangmz20150203
        getCustomerInfo: function () {
            return BsrCloudServer.ajax("ServiceCustomer.svc/GetSelfCustomer", { async: false });
        },
        //保存账户密码 jiangb
        SaveAccountPassWord: function (userPassWd) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdateSelfCustomerPassWord", { data: JSON.stringify(userPassWd), async: false });
        },
        //前台管理员重置主用户的密码
        UpdatePrimaryPassWordByManagerAccount: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdatePrimaryPassWordByManagerAccount", { data: JSON.stringify(json), async: false });
        },
        //获取当前登陆用户的日志信息
        GetSelfLoginInfo: function () {
            var userlog = new Object();
            userlog.RequestCount = 5;
            userlog.StartCount = 0;
            return BsrCloudServer.ajax("OperaterLog.svc/GetSelfLoginInfo", { data: JSON.stringify(userlog), async: false });
        },
        //前台管理员查看主用户的登陆信息
        GetPrimaryCustomerLoginInfo: function (AccountID) {
            var userlog = new Object();
            userlog.PrimaryCustomerId = AccountID;
            userlog.RequestCount = 5;
            userlog.StartCount = 0;
            return BsrCloudServer.ajax("OperaterLog.svc/GetPrimaryCustomerLoginInfo", { data: JSON.stringify(userlog), async: false });
        },
        UpdateCustomerSafeInfo: function (loginway) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdateSelfSafeCustomer", { data: "{ \"LoginTypes\":\"" + loginway + "\" }", async: false });
        },
        //前台管理获取选中的主账户信息 jiangb
        GetSinglePrimaryCustomer: function (CurrentID) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/GetSinglePrimaryCustomer", { data: "{ \"PrimaryCustomerId\":\"" + CurrentID + "\" }", async: false });
        },
        //主账户获取单一子账户的信息
        GetSingleSubCustomer: function(CurrentID){
            return BsrCloudServer.ajax("ServiceCustomer.svc/GetSingleSubCustomer", { data: "{ \"SubCustomerId\":\"" + CurrentID + "\" }", async: false });
        },
        //获取其他主用户所有设备授权信息
        GetAuthorizeOtherPrimaryResponseDto: function (CurrentID) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/GetAuthorizeOtherPrimary", { data: "{ \"PrimaryCustomerId\":\"" + CurrentID + "\" }", async: false });
        },
        //获取前台管理员主账户的授权 jiangb
        AuthorizePrimaryByManagerAccount: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/AuthorizePrimaryByManagerAccount", { data: JSON.stringify(json), async: false });
        },
        //前台管理员修改主用户的安全信息 jiangb
        UpdatePrimarySafeByManagerAccount: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdatePrimarySafeByManagerAccount", { data: JSON.stringify(json), async: false });
        },
        //前台管理员修改主用户的基本信息
        UpdatePrimaryCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdatePrimaryCustomer", { data: JSON.stringify(json), async: false });
        },
        //由前台管理注册主账号 jiangb
        AddPrimaryCustomerByManagerAccount: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/AddPrimaryCustomerByManagerAccount", { data: JSON.stringify(json), async: false });
        },
        //前台管理员冻结与解除冻结主用户
        EnablePrimaryCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/EnablePrimaryCustomer", { data: JSON.stringify(json), async: false });
        },
        //前台管理员批量删除主用户
        DeletePrimaryCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/DeletePrimaryCustomer", { data: JSON.stringify(json), async: false });
        },
        //检索当前主用户下的所有子用户
        SearchSubCustomers: function (keyword) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/SearchSubCustomer", { data: "{ \"KeyWord\":\"" + keyword + "\" }", async: false });
        },
        //主账户修改子账户的基本信息
        UpdateSubSafeByPrimary: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdateSubSafeByPrimary", { data: JSON.stringify(json), async: false });
        },
        //主账户重置子用户密码
        UpdateSubPassWordByPrimary: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdateSubPassWordByPrimary", { data: JSON.stringify(json), async: false });
        },
        //主账户更新子用户的基本信息
        UpdateSubCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/UpdateSubCustomer", { data: JSON.stringify(json), async: false });
        },
        //主账户获取子用户的授权信息
        GetAuthorizeSubCustomer: function (CurrentID) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/GetAuthorizeSubCustomer", { data: "{ \"SubCustomerId\":\"" + CurrentID + "\" }", async: false });
        },
        //主账户查看子用户的登陆信息
        GetSubCustomerLoginInfo: function (SubAccountID) {
            var userlog = new Object();
            userlog.SubCustomerId = SubAccountID;
            userlog.RequestCount = 5;
            userlog.StartCount = 0;
            return BsrCloudServer.ajax("OperaterLog.svc/GetSubCustomerLoginInfo", { data: JSON.stringify(userlog), async: false });
        },
        //主账户冻结与解除冻结子用户
        EnableSubCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/EnableCustomer", { data: JSON.stringify(json), async: false });
        },
        //主用户删除子用户
        DeleteSubCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/DeleteSubCustomer", { data: JSON.stringify(json), async: false });
        },
        //主用户将权限设定给子用户 jiangb
        AuthorizeSubByPrimary: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/AuthorizeSubByPrimary", { data: JSON.stringify(json), async: false });
        },
        //创建子用户 jiangb
        AddSubCustomer: function (json) {
            return BsrCloudServer.ajax("ServiceCustomer.svc/AddSubCustomer", { data: JSON.stringify(json), async: false });
        },
        //*******
        CheckUpdate: function () {
            var CheckUpdateResponse = new Object();
            CheckUpdateResponse.AgentType = 3;
            CheckUpdateResponse.SoftwareId = "BlueStarCloud";
            CheckUpdateResponse.MajorVersion = "V1.0.0.1";
            CheckUpdateResponse.MinorVersion = "1.0.0";
            CheckUpdateResponse.BuildVersion = "T01";
            return BsrCloudServer.ajax("ServiceCommon.svc/CheckUpdate", { data: JSON.stringify(CheckUpdateResponse), async: false });
        }
    },
    /*---------组服务-----------*/
    BsrCloudServer.ResourceGroup = {
        //获取分组_根据当前登录用户 zhangmz
        getChannelGroup: function (success) {
            BsrCloudServer.ajax("ResourceGroup.svc/GetResourceGroup", { async: true, success: success });
        },
        //获取通道_根据分组ID zhangmz
        getChannelListByGroupId: function (groupId, success) {
            var group = new Object();
            group.ResourceGroupId = groupId;
            BsrCloudServer.ajax("ResourceGroup.svc/GetChannelByResourceGroupIdList", { data: JSON.stringify(group), async: true, success: success });
        },
        //添加分组 zhangmz
        addRGourpByName: function (resourceGroupName, parentResourceGroupId, success) {
            var group = new Object();
            group.ResourceGroupName = resourceGroupName;
            group.ParentResourceGroupId = parentResourceGroupId;
            return BsrCloudServer.ajax("ResourceGroup.svc/AddGroupByName", { data: JSON.stringify(group), async: false });
        },
        //移动分组
        moveResourceGroup: function (resourceGroupId, parentResourceGroupId) {
            var group = new Object();
            group.ResourceGroupId = resourceGroupId;
            group.ParentResourceGroupId = parentResourceGroupId;
            return BsrCloudServer.ajax("ResourceGroup.svc/MoveResourceGroup", { data: JSON.stringify(group), async: false });
        },
        //重命名组
        updateResourceGroupName: function (resourceGroupId, resourceGroupName, success) {
            var group = new Object();
            group.ResourceGroupId = resourceGroupId;
            group.ResourceGroupName = resourceGroupName;
            BsrCloudServer.ajax("ResourceGroup.svc/UpdateResourceGroupName", { data: JSON.stringify(group), async: true, success: success });
        },
        //删除组
        deleteResourceGroup: function (resourceGroupId, success) {
            var group = new Object();
            group.ResourceGroupId = resourceGroupId;
            return BsrCloudServer.ajax("ResourceGroup.svc/DeleteResourceGroup", { data: JSON.stringify(group), async: false });
        },
        //移动通道到组
        moveChannelListByResourceGroupId: function (resourceGroupId, channelIdList) {
            var groupTree = new Object();
            groupTree.ResourceGroupId = resourceGroupId;
            groupTree.Channel = channelIdList;
            return BsrCloudServer.ajax("ResourceGroup.svc/MoveChannelListByResourceGroupId", { data: JSON.stringify(groupTree), async: false });
        },
        //删除一个组下的单个通道
        deleteGroupChanne: function (channelId, resourceGroupId) {
            var groupChannel = new Object();
            groupChannel.ChannelId = channelId;
            groupChannel.ResourceGroupId = resourceGroupId;
            return BsrCloudServer.ajax("ResourceGroup.svc/DeleteGroupChannel", { data: JSON.stringify(groupChannel), async: false });
        }
    },
    /*-----------设备服务------------*/
    BsrCloudServer.Device = {
        //添加设备 zhangmz
        addDevice: function (deviceSN, success) {
            var device = new Object();
            device.DeviceName = "我的设备";
            device.SerialNumber = deviceSN;
            BsrCloudServer.ajax("Device.svc/AddDevice", { data: JSON.stringify(device), async: true, success: success });
        },
        //获取当前登陆用户所属设备 zhangmz
        getDevice: function (success) {
            BsrCloudServer.ajax("Device.svc/GetDeviceBySelf", { async: true, success: success });
        },
        //根据设备ID获取通道
        getChannelByDeviceId: function (deviceId) {
            var channelList = [];
            var device = new Object();
            device.DeviceId = deviceId;
            var msg = BsrCloudServer.ajax("Device.svc/GetChannelByDeviceId", { data: JSON.stringify(device), async: false });
            if (msg.Code == 0) {
                channelList = msg.channelList;
            }
            else {
                alert(msg.Message);
            }
            return channelList;
        },
        //根据设备ID删除
        delteDevice: function (deviceId, callback) {
            var device = new Object();
            device.Deviceid = deviceId;
            BsrCloudServer.ajax("Device.svc/DeleteDevice", { data: JSON.stringify(device), async: true, success: callback });
        },
        //更新设备名称
        updateDeviceName: function (deviceId, newDeviceName, callback) {
            var device = new Object();
            device.DeviceId = deviceId;
            device.NewDeviceName = newDeviceName;
            BsrCloudServer.ajax("Device.svc/UpdateDeviceName", { data: JSON.stringify(device), async: true, success: callback });
        },
        //获取设备在线状态
        getServerDeviceState: function (deviceIdList, callback) {
            var object = new Object();
            object.DeviceIdList = deviceIdList;
            BsrCloudServer.ajax("Device.svc/GetServerDeviceState", { data: JSON.stringify(object), async: true, success: callback });
        },
        //流媒体播放时需要的参数
        getStreamerParameter: function (channelId, success) {
            var object = new Object();
            object.ChannelId = channelId;
            BsrCloudServer.ajax("Device.svc/GetStreamerParameter", { data: JSON.stringify(object), async: true, success: success });
        },
        //查询设备SN是否已存在
        isExistSN: function (serialNumber, success) {
            var object = new Object();
            object.SerialNumber = serialNumber;
            BsrCloudServer.ajax("Device.svc/IsExistSN", { data: JSON.stringify(object), async: true, success: success });
        },
        //搜索模块
        searchDevice: function (keyWord, success) {
            var object = new Object();
            object.KeyWord = keyWord;
            BsrCloudServer.ajax("Device.svc/SearchDevice", { data: JSON.stringify(object), async: true, success: success });
        }
    },
    /*-----------通道服务------------*/
    BsrCloudServer.Channel = {
        //修改通道名字
        updateChannelName: function (channelId, channelName, success) {
            var channel = new Object();
            channel.ChannelId = channelId;
            channel.NewChannelName = channelName;
            BsrCloudServer.ajax("Channel.svc/UpdateChannelName", { data: JSON.stringify(channel), async: true, success: success });
        },
        //通道禁用/启用
        enableChannel: function (channelId, isEnable, success) {
            var channel = new Object();
            channel.ChannelId = channelId;
            channel.IsEnable = isEnable;
            BsrCloudServer.ajax("Channel.svc/EnableChannel", { data: JSON.stringify(channel), async: false, success: success });
        },
        //通道分页
        getChannelByPage: function (requestCount, startCount, success, options) {
            var settings = $.extend({
                keyWord: "",
                isGroup: 0,
                resouceGroupId: 0
            }, options || {});
            var channel = new Object();
            channel.RequestCount = requestCount;
            channel.StartCount = startCount;
            channel.KeyWord = settings.keyWord;
            channel.IsGroup = settings.isGroup;
            channel.ResouceGroupId = settings.resouceGroupId;
            BsrCloudServer.ajax("Channel.svc/SearchChannelByPage", { data: JSON.stringify(channel), async: true, success: success });
        },
        //抓取截图
        UpLoadChannelImage: function (channelId, imageByteBase64, extName, success) {
            var obj = new Object();
            obj.ChannelId = channelId;
            obj.ImageByteBase64 = imageByteBase64;
            obj.extName = extName;
            BsrCloudServer.ajax("Channel.svc/UpLoadChannelImage", { data: JSON.stringify(obj), async: true, success: success });
        },
        //设置通道码流,1高清 2均衡 3流畅
        updateChannelEncoderInfo:function(channelId,streamType,success){
            var obj = new Object();
            obj.ChannelId = channelId;
            obj.StreamType = streamType;
            BsrCloudServer.ajax("Channel.svc/UpdateChannelEncoderInfo", { data: JSON.stringify(obj), async: true, success: success });
        }
    }
})(BsrCloudServer)