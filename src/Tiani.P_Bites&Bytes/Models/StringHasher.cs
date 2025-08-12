using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;

namespace Tiani.P_Bites_Bytes.Models
{
    public class StringHasher
    {
        public static string SHA1(string text)
        {
            var algorithm = new SHA1Managed();
            var result = GenerateHashString(algorithm, text);
            return result;
        }

        public static string SHA256(string text)
        {
            var algorithm = new SHA256Managed();
            var result = GenerateHashString(algorithm, text);
            return result;
        }

        public static string MD5(string text)
        {
            var algorithm = new MD5CryptoServiceProvider();
            var result = GenerateHashString(algorithm, text);
            return result;
        }

        private static string GenerateHashString(HashAlgorithm algorithm, string text)
        {
            algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
            var result = algorithm.Hash;
            return string.Join(string.Empty, result.Select(x=>x.ToString("x2")));
        }
    }
}