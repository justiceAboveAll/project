using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DALLib.Models
{
    public partial class Instance
    {
        [NotMapped]
        public string DataSource => $@"{ServerName}\{InstanceName}";

        public override string ToString()
        {
            return DataSource;
        }
    }
}
