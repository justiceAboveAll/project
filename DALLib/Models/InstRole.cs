using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class InstRole : InstPrincipal
    {
        public virtual ICollection<InstLogin> Logins { get; set; } = new HashSet<InstLogin>();
    }
}
