using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DALLib.Models
{
    public enum UserRole { Admin = 0, User = 1, Guest = 2 }

    [Table("Users")]
    public partial class User
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50), Required]
        public string Login { get; set; }

        [StringLength(50), Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [EnumDataType(typeof(UserRole)), Required]
        public UserRole Role { get; set; }

        public virtual ICollection<Assign> Assigns { get; set; }

        public virtual ICollection<Credential> Credentials { get; set; }
    }
}
