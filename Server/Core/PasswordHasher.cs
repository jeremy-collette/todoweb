
namespace todoweb.Server.Core
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    using todoweb.Server.Core.Contract;

    public class PasswordHasher : IPasswordHasher
    {
        private RNGCryptoServiceProvider rngCryptoServiceProvider_;

        public PasswordHasher()
        {
            this.rngCryptoServiceProvider_ = new RNGCryptoServiceProvider();
        }

        public byte[] GetHash(string password)
        {
            var salt = new byte[16];
            this.rngCryptoServiceProvider_.GetBytes(salt);

            var hashGenerator = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] passwordHash = hashGenerator.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(passwordHash, 0, hashBytes, 16, 20);
            return hashBytes;
        }

        public bool Verify(string password, byte[] hash)
        {
            byte[] salt = new byte[16];
            Array.Copy(hash, 0, salt, 0, 16);

            var hashGenerator = new Rfc2898DeriveBytes(password, salt, 10000);
            var passwordHash = hashGenerator.GetBytes(20);

            return hash.Skip(16).SequenceEqual(passwordHash);
        }
    }
}
