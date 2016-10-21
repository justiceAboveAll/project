using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Models
{
    [Table("Assigns")]
    public partial class Assign
    {
        [Key,Column(Order = 0),Required]
        public int UserId { get; set; }

        [Key,Column(Order = 1),Required]
        public int InstanceId { get; set; }

        [StringLength(50)]
        public string Alias { get; set; }

        public bool IsHidden { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("InstanceId")]
        public Instance Instance { get; set; }
    }
}
