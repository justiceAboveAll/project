using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating.Entities
{
    public class DatabaseInfo : BaseInfo<Database>
    {
        public List<DbUserInfo> Users { get; set; } = new List<DbUserInfo>();
        public List<DbRoleInfo> Roles { get; set; } = new List<DbRoleInfo>();
        public List<PermissionInfo> Permissions { get; set; } = new List<PermissionInfo>();
    }
}
