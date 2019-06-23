namespace todoweb.Server.IntegrationTest
{
    using System.Threading.Tasks;
    using todoweb.Client;
    using Client = todoweb.Client.Models;

    public class TodoAuthenticator : IAuthenticator
    {
        private IUserClient userClient_;

        public TodoAuthenticator(IUserClient userClient)
        {
            this.userClient_ = userClient;
        }

        public async Task<bool> Authenticate()
        {
            var user = new Client.User
            {
                Email = "foo@bar.com",
                Password = "test1234"
            };
            var createdUser = await this.userClient_.CreateAsync(user);
            await userClient_.LoginAsync(user);
            return true;
        }

        public async Task<bool> Unauthenticate()
        {
            return await userClient_.LogoutAsync();
        }
    }
}
