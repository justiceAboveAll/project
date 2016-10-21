using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class DbUser
    {
        public void RefuseAllRoles()
        {
            Roles.ToList().ForEach(r => r.Users.Remove(this));
        }
    }
}
