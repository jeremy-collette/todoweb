namespace todoweb.Server.IntegrationTest
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using todoweb.Client;
    using Client = Client.Models;

    public class UserResourceClient : ResourceClient<Client.User>
    {
        private IUserClient innerClient_;

        public UserResourceClient(IUserClient innerClient)
            : base(innerClient)
        {
            this.innerClient_ = innerClient;
        }

        public override async Task<Client.User> CreateAsync(Client.User resource)
        {
            await this.innerClient_.CreateAsync(resource);
            return await this.innerClient_.LoginAsync(resource);
        }
    }
}
