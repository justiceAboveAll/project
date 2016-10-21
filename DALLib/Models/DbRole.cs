using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class DbRole : DbPrincipal
    {
        public virtual ICollection<DbUser> Users { get; set; } = new HashSet<DbUser>();
    }
}
