namespace todoweb.Server.IntegrationTest
{
    using System;

    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    using todoweb.Client;
    using todoweb.Server;
    using ClientTodo = todoweb.Client.Todo;

    public class TodowebIntegrationTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory_;

        public TodowebIntegrationTest(WebApplicationFactory<Program> factory)
        {
            this.factory_ = factory;
        }

        [Fact]
        public void TodoLifecycle()
        {
            var httpClient = this.factory_.CreateClient();
            var client = new TodoClient(this.factory_.Server.BaseAddress.ToString(), httpClient);

            var todos = client.GetAllAsync().Result;
            Assert.Empty(todos);

            var todo = new ClientTodo
            {
                Title = "Foo"
            };
            var result = client.CreateAsync(todo).Result;
            Assert.Equal(todo.Title, result.Title);
            Assert.NotEqual(Guid.Empty, result.Id);
        }
    }
}
