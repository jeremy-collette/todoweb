namespace todoweb.Server.IntegrationTest
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    using todoweb.Client;
    using todoweb.Client.Models.Contract;
    using todoweb.Server;
    using Client = todoweb.Client.Models;
    using todoweb.Client.Models;
    using FluentAssertions;

    public class TodowebIntegrationTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory_;

        public TodowebIntegrationTest(WebApplicationFactory<Program> factory)
        {
            this.factory_ = factory;
        }

        internal class ResourceClient<TClientResource>
            : IResourceClient<TClientResource>
            where TClientResource : IClientResource
        {
            private Type clientType_;
            private object resourceClient_;

            public ResourceClient(Type clientType, HttpClient httpClient)
            {
                this.clientType_ = clientType;
                this.resourceClient_ = Activator.CreateInstance(clientType, httpClient);
            }

            public async Task<TClientResource> CreateAsync(TClientResource resource)
            {
                var methodInfo = this.clientType_.GetMethod("CreateAsync", new Type[] { typeof(TClientResource) });
                return await (Task<TClientResource>)methodInfo.Invoke(this.resourceClient_, new object[]{ resource });
            }

            public async Task<TClientResource> CreateOrUpdateAsync(Guid id, TClientResource resource)
            {
                var methodInfo = this.clientType_.GetMethod("CreateOrUpdateAsync", new Type[] { typeof(Guid), typeof(TClientResource) });
                return await (Task<TClientResource>)methodInfo.Invoke(this.resourceClient_, new object[] { id, resource });
            }

            public async Task<bool> DeleteAsync(Guid id)
            {
                var methodInfo = this.clientType_.GetMethod("DeleteAsync", new Type[] { typeof(Guid) });
                return await (Task<bool>)methodInfo.Invoke(this.resourceClient_, new object[] { id });
            }

            public async Task<TClientResource> GetAsync(Guid id)
            {
                var methodInfo = this.clientType_.GetMethod("GetAsync", new Type[] { typeof(Guid) });
                return await (Task<TClientResource>)methodInfo.Invoke(this.resourceClient_, new object[] { id });

            }

            public async Task<IEnumerable<TClientResource>> GetAllAsync()
            {
                var methodInfo = this.clientType_.GetMethod("GetAllAsync", new Type[] { });
                return await (Task<ICollection<TClientResource>>)methodInfo.Invoke(this.resourceClient_, null);
            }
        }

        [Fact]
        public async void TodoLifecycle()
        {
            var httpClient = this.factory_.CreateClient();

            var userClient = new UserClient(httpClient);
            var user = new User
            {
                Email = "foo@bar.com",
                Password = "test1234"
            };
            var createdUser = await userClient.CreateAsync(user);
            await userClient.LoginAsync(user);

            var client = new ResourceClient<Client.Todo>(typeof(TodoClient), httpClient);
            await ResourceLifecycleIntegrationTest.TestResource(
                client,
                createFactory: () => new Client.Todo
                {
                    Title = "Test todo!"
                },
                updateFactory: (Client.Todo todo) =>
                {
                    todo.Title = "Updated title!";
                    return todo;
                },
                options: (o) => o.Excluding(t => t.Id));

            await userClient.DeleteAsync(createdUser.Id);
            await userClient.LogoutAsync();
        }

        [Fact]
        public async void UserLifecycle()
        {
            var httpClient = this.factory_.CreateClient();
            var client = new ResourceClient<Client.User>(typeof(UserClient), httpClient);

            await ResourceLifecycleIntegrationTest.TestResource(
                client,
                createFactory: () => new Client.User
                {
                    Email = "foo@bar.com",
                    Password = "testpassword"
                },
                updateFactory: (Client.User user) =>
                {
                    user.Email = "bar@baz.com";
                    return user;
                },
                options: (o) => o.Excluding(u => u.Id).Excluding(u => u.Password));
        }
    }
}
