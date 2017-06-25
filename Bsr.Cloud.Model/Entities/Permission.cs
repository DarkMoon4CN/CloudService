using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class Permission
    {
        public virtual int PermissionId { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual string PermissionName { get; set; } //权限动作名
        public virtual int NodeId { get; set; } // 资源的id,类型取决于NodeType
        public virtual int NodeType { get; set; } // 1=设备 2=通道
        public virtual int IsEnable { get; set; } // 1表示有权限，0表示没有

    }
}
