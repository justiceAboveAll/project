using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectionService.InstanceInfoUpdating.Entities
{
    public class InstLoginInfo : BaseInfo<InstLogin> { }

    public class InstRoleInfo : BaseInfoBinded<InstRole> { }

    public class DbUserInfo : BaseInfo<DbUser> { }

    public class DbRoleInfo : BaseInfoBinded<DbRole> { }
}
