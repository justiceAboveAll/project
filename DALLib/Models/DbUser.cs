using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class DbUser : DbPrincipal
    {
        public virtual ICollection<DbRole> Roles { get; set; } = new HashSet<DbRole>();
    }
}
