using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    [Table("DbPrincipals")]
    public abstract partial class DbPrincipal
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DatabaseId { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string Type { get; set; }

        [ForeignKey("DatabaseId")]
        public virtual Database Database { get; set; }

        [ForeignKey("DbPrincipalId")]
        public virtual ICollection<DbPermission> Permissions { get; set; } = new HashSet<DbPermission>();
    }
}
