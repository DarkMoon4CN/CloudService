using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    public class Customer
    {
        public virtual int CustomerId { get; set; }
        public virtual string CustomerName { get; set; } // 用户名
        public virtual string Password { get; set; }     // 登录密码（md5）
        public virtual int SignInType { get; set; } // 1前台管理员，2主账号，3子用户

        public virtual string ReceiverName { get; set; }
        public virtual string ReceiverEmail { get; set; } // 用户登录邮箱，如存在 则必须唯一
        public virtual string ReceiverCellPhone { get; set; } // 用户登录手机号，如存在 则必须唯一
        public virtual int ParentId { get; set; }

        public virtual string AccountIDNumber { get; set; }
        public virtual string AccountTelephone { get; set; }
        public virtual string AccountCompany { get; set; }
        public virtual string AccountCompanyAddress { get; set; }
        public virtual int IsEnable { get; set; } // 0 表示被冻结

        public virtual string AccountHomeAddress { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual DateTime ValidTime { get; set; }     // 用户有效期时间
        public virtual DateTime ForbiddenTime { get; set; } // 用户被冻结的时间
        public virtual string LoginTypes { get; set; } // 使用哪些终端能被允许登录，格式为："1,2,3,4"


        

    }
}
