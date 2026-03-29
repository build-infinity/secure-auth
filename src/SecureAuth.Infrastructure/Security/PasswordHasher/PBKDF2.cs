using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using SecureAuth.Application.Abstractions;

namespace SecureAuth.Infrastructure.Security.PasswordHasher
{
    internal sealed class PBKDF2 : IPasswordHasher
    {
        private readonly PasswordOptions _passwordOptions;

        public PBKDF2(IOptions<PasswordOptions> passwordOptions)
        {
            _passwordOptions = passwordOptions.Value;
        }

        public string Hash(string password)
        {            
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            HashAlgorithmName hashAlgorithm = _passwordOptions.HashAlgorithm switch
            {
                Algorithm.SHA256 => HashAlgorithmName.SHA256,
                Algorithm.SHA512 => HashAlgorithmName.SHA512,

                _=> throw new InvalidOperationException("unsupported algorithm type")
            };

            byte[] salt = RandomNumberGenerator.GetBytes(_passwordOptions.SaltSize);


            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password : password,
                salt : salt,
                iterations : _passwordOptions.Iterations,
                hashAlgorithm : hashAlgorithm,
                outputLength : _passwordOptions.HashSize
            );

            string hashBase64 = Convert.ToBase64String(hash);
            string saltBase64 = Convert.ToBase64String(salt);

            return $"{hashAlgorithm}${_passwordOptions.Iterations}${_passwordOptions.HashSize}${saltBase64}${hashBase64}";
        }

        public bool Verify(string password, string storedHash)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            ArgumentException.ThrowIfNullOrWhiteSpace(storedHash);

            string[] pr = storedHash.Split('$');

            if(pr.Length != 5)
                throw new InvalidOperationException("invalid storedhash format"); // must be custom exception

            if(!Enum.TryParse(pr[0], ignoreCase : true, out Algorithm algorithm) || !int.TryParse(pr[1], out int itertions) || !int.TryParse(pr[2], out int hashSize))
                throw new InvalidOperationException("invalid storedhash format");
            
            HashAlgorithmName hashAlgorithm = algorithm switch 
            {
                Algorithm.SHA256 => HashAlgorithmName.SHA256,
                Algorithm.SHA512 => HashAlgorithmName.SHA512,

                _ => throw new InvalidOperationException("unsuported hash algorithm in storedhash")
            };

            byte[] previousHash, previousSalt;

            try 
            {
                previousSalt = Convert.FromBase64String(pr[3]);
                previousHash = Convert.FromBase64String(pr[4]);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password : password,
                salt : previousSalt,
                iterations : itertions,
                hashAlgorithm : hashAlgorithm,
                outputLength : hashSize
            );     

            return CryptographicOperations.FixedTimeEquals(hash, previousHash);
        }
    }
}