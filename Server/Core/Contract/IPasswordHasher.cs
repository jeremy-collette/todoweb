namespace todoweb.Server.Core.Contract
{
    public interface IPasswordHasher
    {
        byte[] GetHash(string password);

        public bool Verify(string password, byte[] hash);
    }
}
