using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class Device
    {
        public virtual int DeviceId { get; set; }
        public virtual string DeviceName { get; set; }
        public virtual int DeviceType { get; set; } // 3 表示序列号方式添加
        public virtual int CustomerId { get; set; }
        public virtual string SerialNumber { get; set; } // 设备序列号（唯一），最关键信息
        public virtual int BPServerDeviceId { get; set; } // 该设备所属的bp4server中的id值
        public virtual int BPServerConfigId { get; set; } // 该设备在哪个bp4server中
        public virtual int HardwareType { get; set; } // 3表示ipc, 4,5,6为dvr,nvr. 取值见 Bsr.DeviceAdapter.Model中的 enum HardwareType
        public virtual string UserName { get; set; } // 登录到设备时的用户名
        public virtual string PassWord { get; set; } // 同上，密码
    }
}
