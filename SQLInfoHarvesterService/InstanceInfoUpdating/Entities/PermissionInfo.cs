using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating.Entities
{
    public class PermissionInfo : BaseInfoBinded<DbPermission>
    {
        public string Name { get; set; }
        public string State { get; set; }
    }
}
