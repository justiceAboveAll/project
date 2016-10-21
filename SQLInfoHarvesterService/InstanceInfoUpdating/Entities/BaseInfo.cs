using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating.Entities
{
    public abstract class BaseInfo
    {
        public int NativeId { get; set; }
    }

    public class BaseInfo<T> : BaseInfo where T : class, new()
    {
        public T Entity { get; set; }
    }
}
