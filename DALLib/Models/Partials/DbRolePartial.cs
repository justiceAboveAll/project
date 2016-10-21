using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class DbRole
    {
        public void RefuseAllUsers()
        {
            this.Users.ToList().ToList().ForEach(u => u.Roles.Remove(this));
        }
    }
}
