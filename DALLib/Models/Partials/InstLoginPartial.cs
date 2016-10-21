using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class InstLogin 
    {
        public void RefuseAllRoles()
        {
            Roles.ToList().ForEach(ir => ir.Logins.Remove(this));
        }
    }
}
