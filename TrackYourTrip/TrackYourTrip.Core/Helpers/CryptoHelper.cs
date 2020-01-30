using System.Security.Cryptography;
using System.Text;

namespace TrackYourTrip.Core.Helpers
{
    public static class CryptoHelper
    {
        public static string CalculateMD5Hash(byte[] input)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(input);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
