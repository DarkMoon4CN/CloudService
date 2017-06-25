using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bsr.Cloud.BLogic
{
    interface ServerInterface
    {
        bool AddSingleDataEntity<T>(T entity) where T : class,new();
    }
}
