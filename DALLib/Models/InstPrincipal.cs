using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    [Table("InstPrincipals")]
    public abstract partial class InstPrincipal
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int InstanceId { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        [Required, StringLength(128)]
        public string Type { get; set; }

        [ForeignKey("InstanceId")]
        public virtual Instance Instance { get; set; }

        [ForeignKey("InstPrincipalId")]
        public virtual ICollection<InstPermission> Permissions { get; set; } = new HashSet<InstPermission>();
    }
}
