using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Impersonation
{
    public class ImpersonationResult
    {
        public System.Security.Principal.WindowsImpersonationContext User { get; set; } = null;

        public bool HasError { get; set; } = false;

        public string ErrorString { get; set; } = "";

        public ImpersonationResult()
        {

        }
    }
}
