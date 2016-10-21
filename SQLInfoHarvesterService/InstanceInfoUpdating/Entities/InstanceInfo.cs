using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating.Entities
{
    public class InstanceInfo : BaseInfo
    {
        public InstanceInfo(int originalInstanceId)
        {
            NativeId = originalInstanceId;
        }
        
        public InstanceStatus Status { get; set; }
        public string OsVersion { get; set; }
        public byte CpuCount { get; set; }
        public int Memory { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Build { get; set; }
        public int Revision { get; set; }
        public List<InstLoginInfo> Logins { get; set; } = new List<InstLoginInfo>(20);
        public List<InstRoleInfo> Roles { get; set; } = new List<InstRoleInfo>(20);
        public List<PermissionInfo> Permissions { get; set; } = new List<PermissionInfo>(20);
        public List<DatabaseInfo> Databases { get; private set; } = new List<DatabaseInfo>(10);
    }
}
