using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLInfoCollectorService.Security
{
    public interface ICrypto
    {
        string Decrypt(string value, string salt, string key);

        string Encrypt(string value, string salt, string key);

        string GetRandomKey();
    }
}
