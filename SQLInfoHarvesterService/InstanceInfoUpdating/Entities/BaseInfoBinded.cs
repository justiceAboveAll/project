using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating.Entities
{
    public abstract class BaseInfoBinded<T> : BaseInfo<T> where T : class, new()
    {
        public List<int> AssociatedIds { get; set; } = new List<int>(5);
    }
}
