using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.Model.Entities
{
    //用户收藏
    [Serializable]
    public class UserFavorite
    {
        public virtual int UserFavoriteId { get; set; }
        public virtual int CustomerId { get; set; }
        public virtual int UserFavoriteType { get; set; }
        public virtual int UserFavoriteTypeId { get; set; }
        public virtual DateTime FavoriteTime { get; set; }
        public virtual string AliasName { get; set; }
    }
}
