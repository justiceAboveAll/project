using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class InstRole
    {
        public void RefuseAllLogins()
        {
            Logins.ToList().ForEach(il => il.Roles.Remove(this));
        }
    }
}
