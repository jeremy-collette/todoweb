using FluentAssertions;
using FluentAssertions.Equivalency;
using System;
using System.Linq;
using todoweb.Client.Models.Contract;

namespace todoweb.Server.IntegrationTest
{
    public static class ResourceLifecycleIntegrationTest
    {
        public async static void TestResource<TClientResource>(IResourceClient<TClientResource> client, Func<TClientResource> createFactory, Func<TClientResource, TClientResource> updateFactory, Func<EquivalencyAssertionOptions<TClientResource>, EquivalencyAssertionOptions<TClientResource>> options)
            where TClientResource : IClientResource
        {
            // Should be empty
            (await client.GetAllAsync()).Should().BeEmpty();

            // Add resource
            var addModel = createFactory.Invoke();
            var created = await client.CreateAsync(addModel);
            created.Should().BeEquivalentTo(addModel, options);
            (await client.GetAllAsync()).First().Should().BeEquivalentTo(created, options);

            // Update resource
            var updatedModel = updateFactory.Invoke(created);
            (await client.CreateOrUpdateAsync(updatedModel.Id, updatedModel)).Should().BeEquivalentTo(updatedModel, options);
            (await client.GetAsync(updatedModel.Id)).Should().BeEquivalentTo(updatedModel, options);

            // Delete resource
            await client.DeleteAsync(updatedModel.Id);
            (await client.GetAllAsync()).Should().BeEmpty();
        }
    }
}
