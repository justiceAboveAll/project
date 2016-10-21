using System.Linq;
using DALLib.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DALLib.EF;
using DALLib.Contracts;

namespace DALLib.Repos
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(MsSqlMonitorEntities context) : base(context) { }


        public User GetByCredential(string login, string password)
        {
            return table.FirstOrDefault(g => g.Login == login && g.Password == password);
        }

        public bool IsLoginExist(string login)
        {
            return table.Where(g => g.Login == login).Count() > 0;
        }
    }
}