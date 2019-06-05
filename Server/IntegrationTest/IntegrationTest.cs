namespace todoweb.Server.IntegrationTest
{
    using System;

    using FluentAssertions;
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
            todos.Should().BeEmpty();

            var todo = new ClientTodo
            {
                Title = "Foo"
            };
            var result = client.CreateAsync(todo).Result;
            result.Should().BeEquivalentTo(todo);
        }
    }
}
