using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALLib.Models
{
    [Table("InstanceUpdateJob")]
    public class InstanceUpdateJob
    {

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        public JobType.UpdateInfoType JobTypeId { get; set; }

        [Required]
        public int InstanceId { get; set; }

        [ForeignKey("InstanceId")]
        public virtual Instance Instance { get; set; }



        [ForeignKey("JobTypeId")]
        public virtual JobType JobType { get; set; }
    }
}
