using DALLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using DALLib.Contracts;

namespace DALLib.Repos
{
    public class DbPermissionRepository : BaseRepository<DbPermission>
    {
        public DbPermissionRepository(MsSqlMonitorEntities context) : base(context) { }

        public IEnumerable<DbPermission> GetPrincipalPermissions(int principalId)
        {
            return table.Where(g => g.Principal.Select(p => p.Id).Contains(principalId));
        }
    }
}
