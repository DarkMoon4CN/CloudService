using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class BPServerConfig
    {
        public virtual int BPServerConfigId { get; set; }
        public virtual string Name { get; set; }
        public virtual string BusinessLogicAddress { get; set; }
        public virtual string StreamerAddress { get; set; }
        public virtual string StreamerPublicAddress { get; set; } // 流媒体服务器对外的地址前缀，格式为 host:port
        public virtual string StorageAddress { get; set; }
        public virtual string Domain { get; set; }
    }
}
