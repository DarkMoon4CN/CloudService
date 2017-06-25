using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class Channel
    {
        public virtual int ChannelId { get; set; }
        public virtual string ChannelName { get; set; }
        public virtual int ChannelNumber { get; set; }       // 该通道所属设备的插槽号，如果是IPCamera，则为1
        public virtual int DeviceId { get; set; }
        public virtual string ImagePath { get; set; }        // 该通道的封面图片名，含目录
        public virtual int IsEnable { get; set; }            // 0 表示该通道被冻结
        public virtual int BPServerChannelId { get; set; }   // BP4Server的通道id
    }
}
