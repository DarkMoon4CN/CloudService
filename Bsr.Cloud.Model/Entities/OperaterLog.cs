using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class OperaterLog
    {
        public virtual int OperaterId { get; set; }
        public virtual int CustomerId { get; set; } // 哪个用户来操作
        public virtual string Action { get; set; } // 操作动作
        public virtual int TargetId { get; set; }
        public virtual int TargetType { get; set; } // 见 enum OperaterLogEnum 
        public virtual string AgentType { get; set; } // 产生该日志时，用户使用的客户端类型
        public virtual string AgentVersion { get; set; } // 同上，客户端版本
        public virtual int Result { get; set; } // 0成功 非0失败
        public virtual DateTime OperaterTime { get; set; }
        public virtual string Remarks { get; set; } // 该日志详细信息
        public virtual string AgentAddress { get; set; } // 产生该日志时，web端的IP 或者 移动端手机号
    }
}
