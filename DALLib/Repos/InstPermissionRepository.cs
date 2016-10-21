using DALLib.Models;
using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using DALLib.Contracts;

namespace DALLib.Repos
{
    public class InstPermissionRepository : BaseRepository<InstPermission>
    {
        public InstPermissionRepository(MsSqlMonitorEntities context) : base(context) { }

        public IEnumerable<InstPermission> GetPrincipalPermissions(int principalId)
        {
            return table.Where(g => g.Principals.Select(p => p.Id).Contains(principalId));
        }
    }
}
