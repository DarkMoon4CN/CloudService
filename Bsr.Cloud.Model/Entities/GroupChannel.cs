using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class GroupChannel
    {
        public virtual int GroupChannelId { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual ResourceGroup resourceGroup { get; set; }
        public virtual Channel channel { get; set; }
    }
}
