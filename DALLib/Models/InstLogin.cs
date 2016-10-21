using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    public partial class InstLogin : InstPrincipal
    {
        [Required]
        public bool IsDisabled { get; set; } = false;

        public virtual ICollection<InstRole> Roles { get; set; } = new HashSet<InstRole>();
    }
}
