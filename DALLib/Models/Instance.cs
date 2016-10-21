using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALLib.Models
{
    public enum InstanceStatus : byte { Online= 0, Offline = 1, Unknown = 2 }

    public enum AuthenticationType : byte { Windows = 0, Sql = 1};

    [Table("Instances")]
    public partial class Instance
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(15), Required]
        public string ServerName { get; set; }

        [StringLength(16), Required]
        public string InstanceName { get; set; }

        [StringLength(128), Required]
        public string Login { get; set; }

        [StringLength(128), Required]
        public string Password { get; set; }

        [Required, EnumDataType(typeof(InstanceStatus))]
        public InstanceStatus Status { get; set; } = InstanceStatus.Unknown;

        [Required, EnumDataType(typeof(AuthenticationType))]
        public AuthenticationType Authentication { get; set; }

        [StringLength(50)]
        public string OSVersion { get; set; }

        public byte? CpuCount { get; set; }

        public int? Memory { get; set; }

        public bool IsOK { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public virtual InstanceVersion Version { get; set; }

        public virtual ICollection<Assign> Assigns { get; set; }

        public virtual ICollection<Database> Databases { get; set; }

        [ForeignKey("InstanceId")]
        public virtual ICollection<InstRole> Roles { get; set; } = new HashSet<InstRole>();

        [ForeignKey("InstanceId")]
        public virtual ICollection<InstLogin> Logins { get; set; } = new HashSet<InstLogin>();


       // [ForeignKey("InstanceId")]
       // public virtual ICollection<InstanceUpdateJob> UpdateJobs { get; set; } = new HashSet<InstanceUpdateJob>();
    }
}
