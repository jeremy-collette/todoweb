namespace todoweb.Server.IntegrationTest
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using todoweb.Client;
    using Client = Client.Models;

    public class UserResourceClient : ResourceClient<Client.User>
    {
        private IUserClient innerClient;

        public UserResourceClient(HttpClient httpClient)
            : base(typeof(UserClient), httpClient)
        {
            this.innerClient = new UserClient(httpClient);
        }

        public override async Task<Client.User> CreateAsync(Client.User resource)
        {
            await this.innerClient.CreateAsync(resource);
            return await this.innerClient.LoginAsync(resource);
        }
    }
}
