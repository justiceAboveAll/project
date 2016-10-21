using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    [Table("InstPermissions")]
    public class InstPermission
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(128)]
        public string State { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        [ForeignKey("InstPrincipalId")]
        public ICollection<InstPrincipal> Principals { get; set; } = new HashSet<InstPrincipal>();
    }
}
