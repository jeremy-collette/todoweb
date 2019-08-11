namespace todoweb.Server.Core
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;

    using todoweb.Server.Core.Contract;

    public class PasswordHasher : IPasswordHasher
    {
        private static int SaltBytes = 16;
        private static int HashBytes = 20;
        private static int SaltHashLength = SaltBytes + HashBytes;
        private static int HashIterations = 1000;

        private RNGCryptoServiceProvider rngCryptoServiceProvider_;

        public PasswordHasher()
        {
            this.rngCryptoServiceProvider_ = new RNGCryptoServiceProvider();
        }

        public byte[] GetHash(string password)
        {
            var salt = new byte[PasswordHasher.SaltBytes];
            this.rngCryptoServiceProvider_.GetBytes(salt);

            var hashGenerator = new Rfc2898DeriveBytes(password, salt, PasswordHasher.HashIterations);
            var passwordHash = hashGenerator.GetBytes(PasswordHasher.HashBytes);

            var hashBytes = new byte[SaltHashLength];
            Array.Copy(salt, 0, hashBytes, 0, SaltBytes);
            Array.Copy(passwordHash, 0, hashBytes, SaltBytes, HashBytes);
            return hashBytes;
        }

        public bool Verify(string password, byte[] hash)
        {
            var salt = new byte[PasswordHasher.SaltBytes];
            Array.Copy(hash, 0, salt, 0, PasswordHasher.SaltBytes);

            var hashGenerator = new Rfc2898DeriveBytes(password, salt, PasswordHasher.HashIterations);
            var passwordHash = hashGenerator.GetBytes(PasswordHasher.HashBytes);

            return hash.Skip(PasswordHasher.SaltBytes).SequenceEqual(passwordHash);
        }
    }
}
