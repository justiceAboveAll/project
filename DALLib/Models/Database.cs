using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALLib.Models
{
    [Table("Databases")]
    public partial class Database
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int InstanceId { get; set; }

        [Required,StringLength(128)]
        public string Name { get; set; }

        [Required]
        public decimal Size { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [ForeignKey("InstanceId")]
        public virtual Instance Instance { get; set; }

        [ForeignKey("DatabaseId")]
        public virtual ICollection<DbUser> Users { get; set; } = new HashSet<DbUser>();

        [ForeignKey("DatabaseId")]
        public virtual ICollection<DbRole> Roles { get; set; } = new HashSet<DbRole>();
    }
}
