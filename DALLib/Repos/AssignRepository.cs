using DALLib.EF;
using DALLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace DALLib.Repos
{
    public class AssignRepository : BaseRepository<Assign>
    {
        public AssignRepository(MsSqlMonitorEntities context) : base(context) { }

        public IEnumerable<Assign> GetUserAssigns(int userId)
        {
            return table.Where(g => g.UserId == userId);
        }

        public IEnumerable<Assign> GetInstanceAssings(int instanceId)
        {
            return table.Where(g => g.InstanceId == instanceId);
        }
    }
}
