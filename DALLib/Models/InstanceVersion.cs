using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALLib.Models
{
    [Table("InstanceVersions")]
    public class InstanceVersion
    {
        [Required]
        public int Major { get; set; }

        [Required]
        public int Minor { get; set; }

        public int Build { get; set; }

        public int Revision { get; set; }

        [ForeignKey("Instance"),Key,Required]
        public int InstanceId { get; set; }

        public virtual Instance Instance { get; set; }
    }
}
