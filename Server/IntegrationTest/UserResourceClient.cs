namespace todoweb.Server.IntegrationTest
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using todoweb.Client;
    using Client = Client.Models;

    public class UserResourceClient : ResourceClient<Client.User>
    {

        public UserResourceClient(IUserClient innerClient)
            : base(innerClient)
        {
        }

        public override async Task<Client.User> CreateAsync(Client.User resource)
        {
            await ((IUserClient)this.resourceClient_).CreateAsync(resource);
            return await ((IUserClient)this.resourceClient_).LoginAsync(resource);
        }
    }
}
