namespace todoweb.Server.IntegrationTest
{
    using System.Linq;

    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Xunit;

    using todoweb.Client;
    using todoweb.Server;
    using ClientTodo = todoweb.Client.Models.Todo;
    using System.Threading.Tasks;

    public class TodowebIntegrationTest
        : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> factory_;

        public TodowebIntegrationTest(WebApplicationFactory<Program> factory)
        {
            this.factory_ = factory;
        }

        [Fact]
        public async Task TodoLifecycleAsync()
        {
            // Create client / stand-up service
            var httpClient = this.factory_.CreateClient();
            var client = new TodoClient(this.factory_.Server.BaseAddress.ToString(), httpClient);

            // Check no todos exist
            (await client.GetAllAsync()).Should().BeEmpty();

            // Create a todo
            var todo = new ClientTodo
            {
                Title = "Foo"
            };
            var result = await client.CreateAsync(todo);
            result.Should().BeEquivalentTo(todo, o => o.Excluding(r => r.Id));

            // Check it exists
            (await client.GetAllAsync()).First().Should().BeEquivalentTo(todo, o => o.Excluding(r => r.Id));

            // Update it
            todo.Title = "Bar";
            result = await client.CreateOrUpdateAsync(result.Id, todo);
            result.Should().BeEquivalentTo(todo, o => o.Excluding(r => r.Id));

            // Check it's updated
            (await client.GetAsync(result.Id)).Should().BeEquivalentTo(todo, o => o.Excluding(r => r.Id));

            // Delete it
            await client.DeleteAsync(result.Id);

            // Check it's gone
            (await client.GetAllAsync()).Should().BeEmpty();
        }
    }
}
