using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Курсовая_по_ОП_2
{
    internal class Hash
    {
        public static string GenMD5Hash(string user0, string password0)
        { //получение хэша пароля
            byte[] passwordBytes = Encoding.UTF8.GetBytes(user0.ToUpper() + password0);
            byte[] res = new MD5CryptoServiceProvider().ComputeHash(passwordBytes);
            return Convert.ToBase64String(res);
        }
    }
}
