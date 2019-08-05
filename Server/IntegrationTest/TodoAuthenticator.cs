namespace todoweb.Server.IntegrationTest
{
    using System.Threading.Tasks;

    using todoweb.Client;

    using Client = todoweb.Client.Models;

    public class TodoAuthenticator : IAuthenticator
    {
        private IUserClient userClient_;
        private Client.User createdUser_;

        public TodoAuthenticator(IUserClient userClient)
        {
            this.userClient_ = userClient;
        }

        public async Task<bool> Authenticate()
        {
            var newUser = new Client.User
            {
                Email = "foo@bar.com",
                Password = "test1234"
            };
            this.createdUser_ = await this.userClient_.CreateAsync(newUser);
            return (await this.userClient_.LoginAsync(this.createdUser_)) != null;
        }

        public async Task<bool> Unauthenticate()
        {
            return await this.userClient_.DeleteAsync(this.createdUser_.Id);
        }
    }
}
