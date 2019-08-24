namespace todoweb.Server.UnitTest
{
    using System;
    using System.Text;

    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    using todoweb.Server.Core.Contract;
    using todoweb.Server.Models;
    using todoweb.Server.Core;

    public class ResourceStoreTests
    {
        [Fact]
        public void TodoDatabaseResourceStoreTest()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext<Todo>>()
                .UseInMemoryDatabase(databaseName: "testdb");

            var resourceStore = new DatabaseResourceManager<Todo>(new DatabaseContext<Todo>(contextOptionsBuilder.Options));
            var newTodo = resourceStore.Add(
                new Todo
                {
                    Id = Guid.NewGuid().ToString(),
                    Owner = "foo@bar.com",
                    Title = "Test Todo",
                    Done = false
                });
            resourceStore.Get(newTodo.Id).Should().BeEquivalentTo(newTodo);

            resourceStore.Delete(newTodo.Id).Should().BeTrue();

            resourceStore.Get(newTodo.Id).Should().BeNull();
        }

        [Fact]
        public void UserDatabaseResourceStoreTest()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<DatabaseContext<User>>()
                .UseInMemoryDatabase(databaseName: "testdb");

            var resourceStore = new DatabaseResourceManager<User>(new DatabaseContext<User>(contextOptionsBuilder.Options));

            var newUser = resourceStore.Add(
                new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Owner = "foo@bar.com",
                    Email = "foo@bar.com",
                    PasswordHash = Encoding.UTF8.GetBytes("foobar")
                });
            resourceStore.Get(newUser.Id).Should().BeEquivalentTo(newUser);
            resourceStore.Delete(newUser.Id).Should().BeTrue();
            resourceStore.Get(newUser.Id).Should().BeNull();
        }
    }
}
