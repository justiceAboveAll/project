using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALLib.Models
{
    [Table("JobType")]
    public class JobType
    {
        public enum UpdateInfoType : byte { Full = 0, CheckStatus = 1, }


        [Key, Required, EnumDataType(typeof(UpdateInfoType))]
        public UpdateInfoType Type { get; set; }

        // How many create  parallel threads for reading data
        [Required]
        public int MaxParallelReads { get; set; } = Environment.ProcessorCount;

        //haw many information portions write in one transaction
        [Required]
        public int MaxMutualWrites { get; set; } = Environment.ProcessorCount;


        [Required]
        public int RepeatTimeSec { get; set; }  //Repeat time in seconds
    }
}
