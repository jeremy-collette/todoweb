namespace todoweb.Server.IntegrationTest
{
    using System.IO;

    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    using todoweb.Client;
    using todoweb.Server;
    using Client = Client.Models;
    using Microsoft.Extensions.Configuration;

    public class TodowebIntegrationTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory_;

        public TodowebIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.Test.json");

            this.factory_ = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile(configPath).Build();
                });
            });
        }

        [Fact]
        public async void TodoLifecycle()
        {
            var httpClient = this.factory_.CreateClient();
            var client = new ResourceClient<Client.Todo>(ClientFactory.CreateTodoClient(httpClient));

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
                options: (o) => o.Excluding(t => t.Id),
                authenticator: new TodoAuthenticator(ClientFactory.CreateUserClient(httpClient)));
        }

        [Fact]
        public async void UserLifecycle()
        {
            var httpClient = this.factory_.CreateClient();
            var client = new UserResourceClient(ClientFactory.CreateUserClient(httpClient));

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
                options: (o) => o.Excluding(u => u.Id).Excluding(u => u.Password),
                authenticator: null);
            ;
        }
    }
}
