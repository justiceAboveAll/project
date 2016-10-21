using DALLib.Models;
using System.Collections.Generic;
using System.Linq;
using DALLib.EF;
using DALLib.Contracts;

namespace DALLib.Repos
{
    public class DatabaseRepository : BaseRepository<Database>
    {
        public DatabaseRepository(MsSqlMonitorEntities context) : base(context)
        {
            table = context.Databases;
        }

        public IEnumerable<Database> GetDatabasesByInstanceId(int instanceId)
        {
            return table.Where(g => g.InstanceId == instanceId);
        }

        public IEnumerable<DbPrincipal> GetDbUsersByDatabaseId(int databaseId)
        {
            return table.FirstOrDefault(g => g.Id == databaseId).Users;
        }

        public IEnumerable<DbPrincipal> GetDbRolesByDatabaseId(int databaseId)
        {
            return table.FirstOrDefault(g => g.Id == databaseId).Roles;
        }
    }
}
