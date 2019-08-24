namespace todoweb.Server.IntegrationTest
{
    using System.Threading.Tasks;

    public interface IAuthenticator
    {
        Task<bool> Authenticate();

        Task<bool> Unauthenticate();
    }
}
