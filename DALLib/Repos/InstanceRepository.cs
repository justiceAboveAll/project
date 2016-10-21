using DALLib.Contracts;
using DALLib.EF;
using DALLib.Models;
using System.Collections.Generic;
using System.Linq;

namespace DALLib.Repos
{
    public class InstanceRepository : BaseRepository<Instance>
    {
        public InstanceRepository(MsSqlMonitorEntities context) : base(context) { }

        public bool IsInstanceExist(string serverName, string instanceName)
        {
            return table.Where(g => g.InstanceName == instanceName && g.ServerName == serverName).Count() > 0;
        }

        public IEnumerable<Instance> GetAssignedInstances(int userId)
        {
            List<int> assignedInstanceId = new AssignRepository(context).GetUserAssigns(userId)
                    .Select(g => g.InstanceId).ToList();
            return table.Where(g => assignedInstanceId.Contains(g.Id));
        }

        public InstanceVersion GetVersionByInstanceId(int instanceId)
        {
            return context.InstanceVersions.Find(instanceId);
        }

        public IEnumerable<InstPrincipal> GetInstLoginsByInstanceId(int instanceId)
        {
            return table.FirstOrDefault(g => g.Id == instanceId).Logins;
        }

        public IEnumerable<InstPrincipal> GetInstRolesByInstanceId(int instanceId)
        {
            return table.FirstOrDefault(g => g.Id == instanceId).Roles;
        }
    }
}
