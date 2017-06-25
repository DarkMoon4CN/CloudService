using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Bsr.Cloud.Model.Entities
{
    //通道的分组
    public class ResourceGroup
    {
        public virtual int ResourceGroupId { get; set; }
        public virtual string ResourceGroupName { get; set; }
        public virtual int ParentResourceGroupId { get; set; }
        public virtual int CustomerId { get; set; }
    }
}
