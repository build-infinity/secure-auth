using System.Security.Cryptography;
using System.Text;

namespace SecureAuth.Application
{
    public static class Helper
    {
        public static string Hash(string data)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            byte[] hashBytes = SHA256.HashData(dataBytes);

            return Convert.ToBase64String(hashBytes);
        }
        
        public static bool Verify(string data, string storedDataHash)
        {
            byte[] previousHash = Convert.FromBase64String(storedDataHash);
            byte[] hash = Convert.FromBase64String(Hash(data));

            return CryptographicOperations.FixedTimeEquals(hash, previousHash);
        }
    }
}